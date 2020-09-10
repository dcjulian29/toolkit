using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Common.Logging;

namespace ToolKit
{
    /// <summary>
    /// This class holds and persists the settings used by an application as XML file.
    /// </summary>
    /// <typeparam name="T">
    /// A type that is inheriting from this class.
    /// Example: MySettings : ApplicationSettings&lt;MySettings&gt;
    /// </typeparam>
    [XmlRoot("settings")]
    public abstract class ApplicationSettings<T>
    {
        private static ILog _log = LogManager.GetLogger<ApplicationSettings<T>>();

        /// <summary>
        /// Loads the application settings.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>a de-serialized object containing the application settings.</returns>
        public T Load(string fileName)
        {
            // Check to make sure that the fileName is not a relative file and/or path name This will
            // become important when the calling process is a Windows Service or other type of
            // hosting process...
            if (!File.Exists(fileName))
            {
                var fullPath = Assembly.GetAssembly(typeof(T)).Location;
                var pathDirectory = Path.GetDirectoryName(fullPath);
                var separator = Path.DirectorySeparatorChar;
                var potentialFileName = $"{pathDirectory}{separator}{fileName}";

                if (File.Exists(potentialFileName))
                {
                    fileName = potentialFileName;
                }
            }

            if (!File.Exists(fileName))
            {
                return default(T);
            }

            _log.Debug($"Loading application settings from {fileName}");

            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(fileName))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Saves the application settings to XML file.
        /// </summary>
        /// <param name="fileName">Name of the file to store the application settings.</param>
        public void Save(string fileName)
        {
            // If the file does not exist, make sure to prepend the directory of the assembly.
            if ((!File.Exists(fileName)) && !Path.IsPathRooted(fileName))
            {
                var fullPath = Assembly.GetAssembly(typeof(T)).Location;
                var pathDirectory = Path.GetDirectoryName(fullPath);
                var separator = Path.DirectorySeparatorChar;
                fileName = String.Format("{0}{1}{2}", pathDirectory, separator, fileName);
            }

            var serializer = new XmlSerializer(typeof(T));

            _log.DebugFormat("Saving application settings to {0}", fileName);

            using (var writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, this);
            }
        }
    }
}
