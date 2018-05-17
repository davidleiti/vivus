namespace Vivus.Core.Configurations
{
    using Newtonsoft.Json;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Represents a collection of configurations methods.
    /// </summary>
    public static class Configurations
    {
        #region Public Methods

        /// <summary>
        /// Returns a connection string based on the name of the confguration.
        /// </summary>
        /// <param name="path">The path to the json file.</param>
        /// <param name="configurationName">The name of the configuration.</param>
        /// <returns></returns>
        public static string GetConnectionString(string path, string configurationName)
        {
            return JsonConvert.DeserializeObject<JsonConfigurations>(File.ReadAllText(path))
                .Configurations
                .Single(configuration => configuration.Name == configurationName)
                .Fields.Select(field => $"{ field.Name }={ field.Value }")?.Aggregate((field1, field2) => $"{ field1 };{ field2 }");
        }

        #endregion

        #region Private Classes

        /// <summary>
        /// Represents a JSON array of configurations.
        /// </summary>
        private class JsonConfigurations
        {
            /// <summary>
            /// Gets or sets the array of configurations.
            /// </summary>
            public Configuration[] Configurations { get; set; }
        }

        /// <summary>
        /// Represents a configuration.
        /// </summary>
        private class Configuration
        {
            /// <summary>
            /// Gets or sets the name of the configuration.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the fields of the configuration.
            /// </summary>
            public Field[] Fields { get; set; }
        }

        /// <summary>
        /// Represents a field inside a configuration.
        /// </summary>
        private class Field
        {
            /// <summary>
            /// Gets or sets the name of the field.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the value of the field.
            /// </summary>
            public string Value { get; set; }
        }

        #endregion
    }
}
