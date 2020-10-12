using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using ToolKit.Validation;

namespace ToolKit
{
    /// <summary>
    /// This class wraps several properties that can be determined by reflection on the Assembly.
    /// </summary>
    public static class AssemblyProperties
    {
        /// <summary>
        /// Return the build number of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a number containing the build number.</returns>
        public static int BuildVersion(string assemblyFileName)
        {
            return BuildVersion(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Return the build number of the file containing an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>a number containing the build number.</returns>
        public static int BuildVersion(Assembly assembly)
        {
            return Check.NotNull(assembly, nameof(assembly)).GetName().Version.Build;
        }

        /// <summary>
        /// Return the UTC date and time the file containing an assembly was compiled on.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>the date and time the file containing an assembly was compiled on.</returns>
        public static DateTime CompiledOn(string assemblyFileName)
        {
            return CompiledOn(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Return the UTC date and time the file containing an assembly was compiled on.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>the date and time the file containing an assembly was compiled on.</returns>
        public static DateTime CompiledOn(Assembly assembly)
        {
            // The most reliable method to get the date and time that an assembly was compiled on
            // appears to be by retrieving the linker timestamp from the PE header.
            var filePath = Check.NotNull(assembly, nameof(assembly)).Location;
            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.Read(buffer, 0, 2048);
            }

            var secondsSince1970 = BitConverter.ToInt32(buffer, BitConverter.ToInt32(buffer, 60) + 8);

            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secondsSince1970);
        }

        /// <summary>
        /// Returns a string representation of the compile mode for the named assembly.
        /// </summary>
        /// <param name="assemblyFileName">Filename of the assembly file.</param>
        /// <returns>"RELEASE" if in release mode; otherwise "DEBUG".</returns>
        public static string Configuration(string assemblyFileName)
        {
            return Configuration(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Returns a string representation of the compile mode for the named assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>"RELEASE" if in release mode; otherwise "DEBUG".</returns>
        public static string Configuration(Assembly assembly)
        {
            if (IsDebugMode(assembly))
            {
                return "DEBUG";
            }
            else
            {
                return "RELEASE";
            }
        }

        /// <summary>
        /// Return the GUID string of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a string of the Assembly GUID.</returns>
        [SuppressMessage(
            "Naming",
            "CA1720:Identifier contains type name",
            Justification = "Too Bad. It is what it is.")]
        public static string Guid(string assemblyFileName)
        {
            return Guid(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Return the GUID string of the file containing an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>a string of the Assembly GUID.</returns>
        [SuppressMessage(
            "Naming",
            "CA1720:Identifier contains type name",
            Justification = "Too Bad. It is what it is.")]
        public static string Guid(Assembly assembly)
        {
            var attributes =
                Check.NotNull(assembly, nameof(assembly)).GetCustomAttributes(typeof(GuidAttribute), false);

            return attributes.Length == 0 ? string.Empty : ((GuidAttribute)attributes[0]).Value;
        }

        /// <summary>
        /// Determines whether the file containing an assembly is in debug mode.
        /// </summary>
        /// <param name="assemblyFileName">Filename of the assembly file.</param>
        /// <returns><c>true</c> if in debug mode; otherwise, <c>false</c>.</returns>
        public static bool IsDebugMode(string assemblyFileName)
        {
            return IsDebugMode(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Determines whether the file containing an assembly is in debug mode.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns><c>true</c> if in debug mode; otherwise, <c>false</c>.</returns>
        public static bool IsDebugMode(Assembly assembly)
        {
            var attributes = Check.NotNull(assembly, nameof(assembly)).GetCustomAttributes(false);

            return attributes.OfType<DebuggableAttribute>().Select(attribute => attribute.IsJITTrackingEnabled).FirstOrDefault();
        }

        /// <summary>
        /// Determines whether the file containing an assembly is in release mode.
        /// </summary>
        /// <param name="assemblyFileName">Filename of the assembly file.</param>
        /// <returns><c>true</c> if in release mode; otherwise, <c>false</c>.</returns>
        public static bool IsReleaseMode(string assemblyFileName)
        {
            return !IsDebugMode(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Determines whether the file containing an assembly is in release mode.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns><c>true</c> if in release mode; otherwise, <c>false</c>.</returns>
        public static bool IsReleaseMode(Assembly assembly)
        {
            return !IsDebugMode(assembly);
        }

        /// <summary>
        /// Return the last time this assembly was modified.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>the last time this assembly was modified.</returns>
        public static DateTime LastModified(string assemblyFileName)
        {
            return LastModified(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Return the last time this assembly was modified.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>the last time this assembly was modified.</returns>
        public static DateTime LastModified(Assembly assembly)
        {
            return File.GetLastWriteTime(Check.NotNull(assembly, nameof(assembly)).Location);
        }

        /// <summary>
        /// Return the Major Version number of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a number containing the Major Version number.</returns>
        public static int MajorVersion(string assemblyFileName)
        {
            return MajorVersion(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Return the Major Version number of the file containing an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>a number containing the Major Version number.</returns>
        public static int MajorVersion(Assembly assembly)
        {
            return Check.NotNull(assembly, nameof(assembly)).GetName().Version.Major;
        }

        /// <summary>
        /// Return the Minor Version number of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a number containing the Minor Version number.</returns>
        public static int MinorVersion(string assemblyFileName)
        {
            return MinorVersion(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Return the Minor Version number of the file containing an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>a number containing the Minor Version number.</returns>
        public static int MinorVersion(Assembly assembly)
        {
            return Check.NotNull(assembly, nameof(assembly)).GetName().Version.Minor;
        }

        /// <summary>
        /// Return the Assembly Name of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a string containing the name of the assembly.</returns>
        public static string Name(string assemblyFileName)
        {
            return Name(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Return the Assembly Name of the file containing an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>a string containing the name of the assembly.</returns>
        public static string Name(Assembly assembly)
        {
            return Check.NotNull(assembly, nameof(assembly)).GetName().Name;
        }

        /// <summary>
        /// Return the Revision number of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a number containing the Revision number.</returns>
        public static int RevisionVersion(string assemblyFileName)
        {
            return RevisionVersion(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Return the Revision number of the file containing an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>a number containing the Revision number.</returns>
        public static int RevisionVersion(Assembly assembly)
        {
            return Check.NotNull(assembly, nameof(assembly)).GetName().Version.Revision;
        }

        /// <summary>
        /// Return the version number of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a string containing the version number.</returns>
        public static string Version(string assemblyFileName)
        {
            return Version(LoadAssembly(assemblyFileName));
        }

        /// <summary>
        /// Return the version number of the file containing an assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>a string containing the version number.</returns>
        public static string Version(Assembly assembly)
        {
            return Check.NotNull(assembly, nameof(assembly)).GetName().Version.ToString();
        }

        [SuppressMessage(
            "Major Code Smell",
            "S3885:\"Assembly.Load\" should be used",
            Justification = "Using Load() causes unit test to fail with valid assemblies, so no.")]
        private static Assembly LoadAssembly(string fileName)
        {
            var assembly = Assembly.LoadFile(fileName);

            if (assembly == null)
            {
                throw new FileLoadException("Unable to obtain a valid Assembly!");
            }

            return assembly;
        }
    }
}
