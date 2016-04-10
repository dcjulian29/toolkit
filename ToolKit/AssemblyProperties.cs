using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Common.Logging;

namespace ToolKit
{
    /// <summary>
    /// This class wraps several properties that can be determined by reflection on the Assembly
    /// </summary>
    public class AssemblyProperties
    {
        private static ILog _log = LogManager.GetLogger<AssemblyProperties>();

        /// <summary>
        /// Return the build number of the first Assembly loaded in this AppDomain.
        /// </summary>
        /// <returns>a number containing the build number</returns>
        public static int BuildVersion()
        {
            return BuildVersion(null);
        }

        /// <summary>
        /// Return the build number of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a number containing the build number</returns>
        public static int BuildVersion(string assemblyFileName)
        {
            return GetAssembly(assemblyFileName).GetName().Version.Build;
        }

        /// <summary>
        /// Return the date and time the first Assembly loaded in this AppDomain was compiled on.
        /// </summary>
        /// <returns>
        /// the date and time the first Assembly loaded in this AppDomain was compiled on
        /// </returns>
        public static DateTime CompiledOn()
        {
            return CompiledOn(null);
        }

        /// <summary>
        /// Return the date and time the file containing an assembly was compiled on.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>the date and time the file containing an assembly was compiled on</returns>
        public static DateTime CompiledOn(string assemblyFileName)
        {
            // The most reliable method to get the date and time that an assembly was compiled on
            // appears to be by retrieving the linker timestamp from the PE header.
            var filePath = GetAssembly(assemblyFileName).Location;
            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.Read(buffer, 0, 2048);
            }

            var secondsSince1970 = BitConverter.ToInt32(buffer, BitConverter.ToInt32(buffer, 60) + 8);
            var linkTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(secondsSince1970);

            return TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, TimeZoneInfo.Local);
        }

        /// <summary>
        /// Returns a string representation of the compile mode for the first Assembly loaded in this AppDomain.
        /// </summary>
        /// <returns>"RELEASE" if in release mode; otherwise "DEBUG"</returns>
        public static string Configuration()
        {
            return Configuration(null);
        }

        /// <summary>
        /// Returns a string representation of the compile mode for the named assembly
        /// </summary>
        /// <param name="assemblyFileName">Filename of the assembly file.</param>
        /// <returns>"RELEASE" if in release mode; otherwise "DEBUG"</returns>
        public static string Configuration(string assemblyFileName)
        {
            if (IsDebugMode(assemblyFileName))
            {
                return "DEBUG";
            }
            else
            {
                return "RELEASE";
            }
        }

        /// <summary>
        /// Return the GUID string of the first Assembly loaded in this AppDomain.
        /// </summary>
        /// <returns>a string of the Assembly GUID</returns>
        public static string Guid()
        {
            return Guid(null);
        }

        /// <summary>
        /// Return the GUID string of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a string of the Assembly GUID</returns>
        public static string Guid(string assemblyFileName)
        {
            var assembly = GetAssembly(assemblyFileName);
            var attributes = assembly.GetCustomAttributes(typeof(GuidAttribute), false);

            return attributes.Length == 0 ? String.Empty : ((GuidAttribute)attributes[0]).Value;
        }

        /// <summary>
        /// Determines whether the first Assembly loaded in this AppDomain is in debug mode.
        /// </summary>
        /// <returns><c>true</c> if in debug mode; otherwise, <c>false</c>.</returns>
        public static bool IsDebugMode()
        {
            return IsDebugMode(null);
        }

        /// <summary>
        /// Determines whether the file containing an assembly is in debug mode.
        /// </summary>
        /// <param name="assemblyFileName">Filename of the assembly file.</param>
        /// <returns><c>true</c> if in debug mode; otherwise, <c>false</c>.</returns>
        public static bool IsDebugMode(string assemblyFileName)
        {
            var assembly = GetAssembly(assemblyFileName);
            var attributes = assembly.GetCustomAttributes(false);

            return attributes.OfType<DebuggableAttribute>().Select(attribute => attribute.IsJITTrackingEnabled).FirstOrDefault();
        }

        /// <summary>
        /// Determines whether the first Assembly loaded in this AppDomain is in release mode.
        /// </summary>
        /// <returns><c>true</c> if in release mode; otherwise, <c>false</c>.</returns>
        public static bool IsReleaseMode()
        {
            return !IsDebugMode();
        }

        /// <summary>
        /// Determines whether the file containing an assembly is in release mode.
        /// </summary>
        /// <param name="assemblyFileName">Filename of the assembly file.</param>
        /// <returns><c>true</c> if in release mode; otherwise, <c>false</c>.</returns>
        public static bool IsReleaseMode(string assemblyFileName)
        {
            return !IsDebugMode(assemblyFileName);
        }

        /// <summary>
        /// Return the last time this assembly was modified.
        /// </summary>
        /// <returns>the last time this assembly was modified</returns>
        public static DateTime LastModified()
        {
            return LastModified(null);
        }

        /// <summary>
        /// Return the last time this assembly was modified.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>the last time this assembly was modified</returns>
        public static DateTime LastModified(string assemblyFileName)
        {
            return File.GetLastWriteTime(GetAssembly(assemblyFileName).Location);
        }

        /// <summary>
        /// Return the Major Version number of the first Assembly loaded in this AppDomain.
        /// </summary>
        /// <returns>a number containing the Major Version number</returns>
        public static int MajorVersion()
        {
            return MajorVersion(null);
        }

        /// <summary>
        /// Return the Major Version number of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a number containing the Major Version number</returns>
        public static int MajorVersion(string assemblyFileName)
        {
            return GetAssembly(assemblyFileName).GetName().Version.Major;
        }

        /// <summary>
        /// Return the Minor Version number of the first Assembly loaded in this AppDomain.
        /// </summary>
        /// <returns>a number containing the Minor Version number</returns>
        public static int MinorVersion()
        {
            return MinorVersion(null);
        }

        /// <summary>
        /// Return the Minor Version number of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a number containing the Minor Version number</returns>
        public static int MinorVersion(string assemblyFileName)
        {
            return GetAssembly(assemblyFileName).GetName().Version.Minor;
        }

        /// <summary>
        /// Return the Revision number of the first Assembly loaded in this AppDomain.
        /// </summary>
        /// <returns>a number containing the Revision number</returns>
        public static int RevisionVersion()
        {
            return RevisionVersion(null);
        }

        /// <summary>
        /// Return the Revision number of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a number containing the Revision number</returns>
        public static int RevisionVersion(string assemblyFileName)
        {
            return GetAssembly(assemblyFileName).GetName().Version.Revision;
        }

        /// <summary>
        /// Return the version number of the first Assembly loaded in this AppDomain.
        /// </summary>
        /// <returns>a string containing the version number</returns>
        public static string Version()
        {
            return Version(null);
        }

        /// <summary>
        /// Return the version number of the file containing an assembly.
        /// </summary>
        /// <param name="assemblyFileName">Name of the assembly file.</param>
        /// <returns>a string containing the version number</returns>
        public static string Version(string assemblyFileName)
        {
            return GetAssembly(assemblyFileName).GetName().Version.ToString();
        }

        private static Assembly GetAssembly(string fileName)
        {
            Assembly assembly;
            var assemblyProvided = !String.IsNullOrEmpty(fileName);

            if (assemblyProvided)
            {
                assembly = Assembly.LoadFile(fileName);
            }
            else
            {
                assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            }

            if (assembly == null)
            {
                throw new NullReferenceException("Unable to obtain a valid Assembly!");
            }

            return assembly;
        }
    }
}
