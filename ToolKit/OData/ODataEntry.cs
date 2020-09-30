using System.Collections.Generic;

namespace ToolKit.OData
{
    /// <summary>
    /// A container for an OData entry properties.
    /// </summary>
    public class ODataEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ODataEntry" /> class.
        /// </summary>
        public ODataEntry() => Properties = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataEntry" /> class.
        /// </summary>
        /// <param name="entry">The collection of entry properties.</param>
        public ODataEntry(IDictionary<string, object> entry)
            => Properties = new Dictionary<string, object>(entry);

        /// <summary>
        /// Gets the properties of the OData entry.
        /// </summary>
        protected Dictionary<string, object> Properties { get; }

        /// <summary>
        /// Gets the value of the specified property.
        /// </summary>
        /// <param name="key">The property name.</param>
        /// <returns>The property value.</returns>
        public object this[string key] => Properties[key];

        /// <summary>
        /// Returns OData entry properties as dictionary.
        /// </summary>
        /// <returns>A dictionary of OData entry properties.</returns>
        public IDictionary<string, object> AsDictionary() => Properties;
    }
}
