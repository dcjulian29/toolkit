namespace ToolKit.Data
{
    /// <summary>
    /// Represents a Name and Value pair.
    /// </summary>
    public sealed class NameValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameValue" /> class.
        /// </summary>
        public NameValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NameValue" /> class.
        /// </summary>
        /// <param name="name">The Name of a Name and Value pair.</param>
        /// <param name="value">The Value of a Name and Value pair.</param>
        public NameValue(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the Name of a Name and Value pair.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Value of a Name and Value pair.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Retrieves a string that contains the Name and Value property values.
        /// </summary>
        /// <returns>A string that represents this instance.</returns>
        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}
