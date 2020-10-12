#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=OpenCover&version=4.7.922"
#tool "nuget:?package=ReportGenerator&version=4.5.0"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");

if ((target == "Default") && (TeamCity.IsRunningOnTeamCity)) {
    target = "TeamCity";
    configuration = "Release";
}

if ((target == "Default") && (AppVeyor.IsRunningOnAppVeyor)) {
    target = "AppVeyor";
    configuration = "Release";
}

///////////////////////////////////////////////////////////////////////////////

var baseDirectory = MakeAbsolute(Directory("."));
var buildDirectory = baseDirectory + "/.build";
var outputDirectory = buildDirectory + "/output";
var version = "0.0.0";

var msbuildSettings = new MSBuildSettings {
    ArgumentCustomization = args => args.Append("/consoleloggerparameters:ErrorsOnly"),
    Configuration = configuration,
    ToolVersion = MSBuildToolVersion.Default,
    NodeReuse = false,
    WarningsAsError = true
}.WithProperty("OutDir", outputDirectory);

var dotNetCoreBuildSettings = new DotNetCoreBuildSettings {
    Configuration = configuration,
    OutputDirectory = outputDirectory,
    MSBuildSettings = new DotNetCoreMSBuildSettings {
        TreatAllWarningsAs = MSBuildTreatAllWarningsAs.Error,
        Verbosity = DotNetCoreVerbosity.Normal
    },
    NoDependencies = true,
    NoIncremental = true,
    NoRestore = true
};

var restoreSettings = new DotNetCoreRestoreSettings { NoDependencies = true };

///////////////////////////////////////////////////////////////////////////////

Setup(setupContext =>
{
    if (setupContext.TargetTask.Name == "Package")
    {
        Information("Switching to Release Configuration for packaging...");
        configuration = "Release";

        msbuildSettings.Configuration = "Release";
        dotNetCoreBuildSettings.Configuration = "Release";
    }

    IEnumerable<string> stdout;
    StartProcess ("git", new ProcessSettings {
        Arguments = "describe --tags --abbrev=0",
        RedirectStandardOutput = true,
    }, out stdout);
    List<String> result = new List<string>(stdout);
    version = String.IsNullOrEmpty(result[0]) ? "0.0.0" : result[0];

    StartProcess ("git", new ProcessSettings {
        Arguments = "rev-parse --short=8 HEAD",
        RedirectStandardOutput = true,
    }, out stdout);
    result = new List<string>(stdout);
    var packageId = String.IsNullOrEmpty(result[0]) ? "unknown" : result[0];

    var branch = "unknown";
    if (AppVeyor.IsRunningOnAppVeyor) {
        branch = EnvironmentVariable("APPVEYOR_REPO_BRANCH");
    } else {
        StartProcess ("git", new ProcessSettings {
            Arguments = "symbolic-ref --short HEAD",
            RedirectStandardOutput = true,
        }, out stdout);
        result = new List<string>(stdout);
        branch = String.IsNullOrEmpty(result[0]) ? "unknown" : result[0];
    }

    if (branch != "master") {
        version = $"{version}-{branch}.{packageId}";
    }

    if (AppVeyor.IsRunningOnAppVeyor) {
        AppVeyor.UpdateBuildVersion(version);
    }

    if (TeamCity.IsRunningOnTeamCity) {
        TeamCity.SetBuildNumber(version);
    }

    Information($"Package ID is '{packageId}' on branch '{branch}'");

});

TaskSetup(setupContext =>
{
    if (TeamCity.IsRunningOnTeamCity)
    {
        TeamCity.WriteStartBuildBlock(setupContext.Task.Description ?? setupContext.Task.Name);
        TeamCity.WriteStartProgress(setupContext.Task.Description ?? setupContext.Task.Name);
    }
});

TaskTeardown(teardownContext =>
{
    if (TeamCity.IsRunningOnTeamCity)
    {
        TeamCity.WriteEndBuildBlock(teardownContext.Task.Description ?? teardownContext.Task.Name);
        TeamCity.WriteEndProgress(teardownContext.Task.Description ?? teardownContext.Task.Name);
    }
});

Task("Default")
    .IsDependentOn("Compile");

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(buildDirectory);
        CleanDirectories(baseDirectory + "/**/bin");
        CleanDirectories(baseDirectory + "/**/obj");
    });

Task("Init")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        CreateDirectory(buildDirectory);
        CreateDirectory(outputDirectory);
    });

Task("Version")
    .IsDependentOn("Init")
    .Does(() =>
    {
        Information("Marking this build as version: " + version);

        var assemblyVersion = version.Contains('-') ? "0.0.0" : version;

        CreateAssemblyInfo(buildDirectory + "/CommonAssemblyInfo.cs", new AssemblyInfoSettings {
            Version = assemblyVersion,
            FileVersion = assemblyVersion,
            InformationalVersion = version,
            Copyright = String.Format("(c) Julian Easterling {0}", DateTime.Now.Year),
            Company = String.Empty,
            Configuration = configuration
        });
    });

Task("Compile")
    .IsDependentOn("Compile.ToolKit")
    .IsDependentOn("Compile.ToolKit-Windows")
    .IsDependentOn("Compile.ToolKit.Data.EFCore")
    .IsDependentOn("Compile.ToolKit.Data.EntityFramework")
    .IsDependentOn("Compile.ToolKit.Data.MongoDb")
    .IsDependentOn("Compile.ToolKit.Data.Nhibernate")
    .IsDependentOn("Compile.ToolKit.Data.Nhibernate.UnitTests")
    .IsDependentOn("Compile.ToolKit.WebApi")
    .IsDependentOn("Compile.ToolKit.Wpf");

Task("Compile.ToolKit")
    .IsDependentOn("Init")
    .IsDependentOn("Version")
    .Does(() =>
    {
        var settings = dotNetCoreBuildSettings;
        settings.MSBuildSettings.AddFileLogger(
            new MSBuildFileLoggerSettings {
                LogFile = buildDirectory + "/msbuild-ToolKit.log" });

        DotNetCoreRestore("ToolKit/ToolKit.csproj", restoreSettings);
        DotNetCoreBuild("ToolKit/ToolKit.csproj", settings);
    });

Task("Compile.ToolKit-Windows")
    .IsDependentOn("Compile.ToolKit")
    .Does(() =>
    {
        var buildSettings = msbuildSettings.WithTarget("ReBuild");
        buildSettings.AddFileLogger(
                new MSBuildFileLogger {
                    LogFile = buildDirectory + "/msbuild-ToolKit-Windows.log" });

        NuGetRestore("ToolKit-Windows/ToolKit-Windows.csproj");
        MSBuild("ToolKit-Windows/ToolKit-Windows.csproj", buildSettings);
    });

Task("Compile.ToolKit.WebApi")
    .IsDependentOn("Version")
    .Does(() =>
    {
        var settings = dotNetCoreBuildSettings;
        settings.MSBuildSettings.AddFileLogger(
            new MSBuildFileLoggerSettings {
                LogFile = buildDirectory + "/msbuild-ToolKit.WebApi.log" });

        DotNetCoreRestore("ToolKit.WebApi/ToolKit.WebApi.csproj", restoreSettings);
        DotNetCoreBuild("ToolKit.WebApi/ToolKit.WebApi.csproj", settings);
    });

Task("Compile.ToolKit.Wpf")
    .IsDependentOn("Version")
    .Does(() =>
    {
        var buildSettings = msbuildSettings.WithTarget("ReBuild");
        buildSettings.AddFileLogger(
                new MSBuildFileLogger {
                    LogFile = buildDirectory + "/msbuild-ToolKit.Wpf.log" });

        NuGetRestore("ToolKit.Wpf/ToolKit.Wpf.csproj");
        MSBuild("ToolKit.Wpf/ToolKit.Wpf.csproj", buildSettings);
    });

Task("Compile.ToolKit.Data.EFCore")
    .IsDependentOn("Compile.ToolKit")
    .Does(() =>
    {
        var settings = dotNetCoreBuildSettings;
        settings.MSBuildSettings.AddFileLogger(
            new MSBuildFileLoggerSettings {
                LogFile = buildDirectory + "/msbuild-ToolKit.Data.EFCore.log" });

        DotNetCoreRestore("ToolKit.Data.EFCore/ToolKit.Data.EFCore.csproj", restoreSettings);
        DotNetCoreBuild("ToolKit.Data.EFCore/ToolKit.Data.EFCore.csproj", settings);
    });

Task("Compile.ToolKit.Data.EntityFramework")
    .IsDependentOn("Compile.ToolKit")
    .Does(() =>
    {
        var buildSettings = msbuildSettings.WithTarget("ReBuild");
        buildSettings.AddFileLogger(
                new MSBuildFileLogger {
                    LogFile = buildDirectory + "/msbuild-ToolKit.Data.EntityFramework.log" });

        NuGetRestore("ToolKit.Data.EntityFramework/ToolKit.Data.EntityFramework.csproj");
        MSBuild("ToolKit.Data.EntityFramework/ToolKit.Data.EntityFramework.csproj", buildSettings);
    });

Task("Compile.ToolKit.Data.MongoDb")
    .IsDependentOn("Compile.ToolKit")
    .Does(() =>
    {
        var settings = dotNetCoreBuildSettings;
        settings.MSBuildSettings.AddFileLogger(
            new MSBuildFileLoggerSettings {
                LogFile = buildDirectory + "/msbuild-ToolKit.Data.MongoDb.log" });

        DotNetCoreRestore("ToolKit.Data.MongoDb/ToolKit.Data.MongoDb.csproj", restoreSettings);
        DotNetCoreBuild("ToolKit.Data.MongoDb/ToolKit.Data.MongoDb.csproj", settings);
    });

Task("Compile.ToolKit.Data.NHibernate")
    .IsDependentOn("Compile.ToolKit")
    .Does(() =>
    {
        var settings = dotNetCoreBuildSettings;
        settings.MSBuildSettings.AddFileLogger(
            new MSBuildFileLoggerSettings {
                LogFile = buildDirectory + "/msbuild-ToolKit.Data.NHibernate.log" });

        DotNetCoreRestore("ToolKit.Data.NHibernate/ToolKit.Data.NHibernate.csproj", restoreSettings);
        DotNetCoreBuild("ToolKit.Data.NHibernate/ToolKit.Data.NHibernate.csproj", settings);
    });

Task("Compile.ToolKit.Data.NHibernate.UnitTests")
    .IsDependentOn("Compile.ToolKit.Data.NHibernate")
    .Does(() =>
    {
        var settings = dotNetCoreBuildSettings;
        settings.MSBuildSettings.AddFileLogger(
            new MSBuildFileLoggerSettings {
                LogFile = buildDirectory + "/msbuild-ToolKit.Data.NHibernate.UnitTests.log" });

        DotNetCoreRestore("ToolKit.Data.NHibernate.UnitTests/ToolKit.Data.NHibernate.UnitTests.csproj", restoreSettings);
        DotNetCoreBuild("ToolKit.Data.NHibernate.UnitTests/ToolKit.Data.NHibernate.UnitTests.csproj", settings);
    });

Task("Compile.UnitTests")
    .IsDependentOn("Compile")
    .Does(() =>
    {
        var buildSettings = msbuildSettings.WithTarget("ReBuild");
        buildSettings.AddFileLogger(
                new MSBuildFileLogger {
                    LogFile = buildDirectory + "/msbuild-UnitTests.log" });

        NuGetRestore("UnitTests/UnitTests.csproj");
        MSBuild("UnitTests/UnitTests.csproj", buildSettings);
    });

Task("Test")
    .IsDependentOn("UnitTest");

Task("UnitTest")
    .IsDependentOn("Compile.UnitTests")
    .Does(() =>
    {
        XUnit2(outputDirectory + "/UnitTests.dll",
            new XUnit2Settings {
                Parallelism = ParallelismOption.All,
                ShadowCopy = false
            });
    });

Task("Coverage")
    .IsDependentOn("Compile.UnitTests")
    .Does(() =>
    {
        CreateDirectory(buildDirectory + "/coverage");

        OpenCover(tool => {
            tool.XUnit2(outputDirectory + "/UnitTests.dll",
                new XUnit2Settings {
                    Parallelism = ParallelismOption.All,

                    ShadowCopy = false });
            },
            new FilePath(buildDirectory + "/coverage/coverage.xml"),
            new OpenCoverSettings() { Register = "user" }
                .WithFilter(@"+[*]*")
                .WithFilter(@"-[UnitTests]*")
                .WithFilter(@"-[xunit.*]*")
                .WithFilter(@"-[Common.*]*")
                .ExcludeByAttribute("System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute")
                .ExcludeByFile("*/*Designer.cs;*/*.g.cs;*.*.g.i.cs"));
    });

Task("Coverage.Report")
    .IsDependentOn("Coverage")
    .Does(() =>
    {
        ReportGenerator(buildDirectory + "/coverage/coverage.xml", buildDirectory + "/coverage");
    });

Task("TeamCity")
    .Does(() =>
    {
        if (DirectoryExists(baseDirectory + "\\UnitTests")) {
            RunTarget("coverage");

            // Write class coverage
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageAbsCCovered' value='{0}']",
                XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@visitedClasses")));
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageAbsCTotal' value='{0}']",
                XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@numClasses")));
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageC' value='{0:N2}']",
                (
                    Convert.ToDouble(XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@visitedClasses")) /
                    Convert.ToDouble(XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@numClasses"))
                ) * 100));

            // Report method coverage
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageAbsMCovered' value='{0}']",
                XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@visitedMethods")));
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageAbsMTotal' value='{0}']",
                XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@numMethods")));
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageM' value='{0:N2}']",
                (
                    Convert.ToDouble(XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@visitedMethods")) /
                    Convert.ToDouble(XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@numMethods"))
                ) * 100));

            // Report branch coverage
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageAbsBCovered' value='{0}']",
                XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@visitedBranchPoints")));
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageAbsBTotal' value='{0}']",
                XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@numBranchPoints")));
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageB' value='{0}']",
                XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@branchCoverage")));

            // Report statement coverage
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageAbsSCovered' value='{0}']",
                XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@visitedSequencePoints")));
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageAbsSTotal' value='{0}']",
                XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@numSequencePoints")));
            Information(String.Format(
                "##teamcity[buildStatisticValue key='CodeCoverageS' value='{0}']",
                XmlPeek(buildDirectory + "\\coverage\\coverage.xml", "/CoverageSession/Summary/@sequenceCoverage")));
        } else {
            RunTarget("default");
        }
    });

Task("Package")
    .IsDependentOn("Test")
    .Does(() =>
    {
        CreateDirectory(buildDirectory + "\\packages");

        var nuGetPackSettings = new NuGetPackSettings {
            Version = version.Replace('/', '.'),
            OutputDirectory = buildDirectory + "\\packages"
        };

        var nuspecFiles = GetFiles(baseDirectory + "\\*.nuspec");

        NuGetPack(nuspecFiles, nuGetPackSettings);
    });

Task("AppVeyor")
    .IsDependentOn("Package")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
    {
        CopyFiles(buildDirectory + "\\packages\\*.nupkg", MakeAbsolute(Directory("./")), false);

        GetFiles(baseDirectory + "\\*.nupkg")
            .ToList()
            .ForEach(f => AppVeyor.UploadArtifact(f, new AppVeyorUploadArtifactsSettings { DeploymentName = "packages" }));
    });

///////////////////////////////////////////////////////////////////////////////

RunTarget(target);
