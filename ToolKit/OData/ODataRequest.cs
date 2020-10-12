using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ToolKit.OData
{
    /// <summary>
    /// An OData Request Object.
    /// </summary>
    public class ODataRequest
    {
        private readonly List<string> _properties;

        private readonly List<string> _sortby;

        private int _skip;

        private int _take;

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataRequest" /> class.
        /// </summary>
        public ODataRequest()
        {
            _properties = new List<string>();
            _sortby = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ODataRequest" /> class.
        /// </summary>
        /// <param name="url">The URL of the OData feed.</param>
        [SuppressMessage(
            "Design",
            "CA1054:URI-like parameters should not be strings",
            Justification = "Just don't need the additional overhead of the Uri class.")]
        public ODataRequest(string url)
        {
            _properties = new List<string>();
            _sortby = new List<string>();
            Url = url;
        }

        /// <summary>
        /// Gets or sets the OData entity to be requested.
        /// </summary>
        public string Entity { get; set; }

        /// <summary>
        /// Gets or sets the filter to be used.
        /// </summary>
        public ODataFilter Filter { get; set; }

        /// <summary>
        /// Gets or sets the URL of the OData feed.
        /// </summary>
        [SuppressMessage(
            "Design",
            "CA1056:URI-like properties should not be strings",
            Justification = "This property doesn't need the additional overhead of the Uri class.")]
        public string Url { get; set; }

        /// <summary>
        /// Adds a property to retrieve from the OData service.
        /// </summary>
        /// <param name="property">The property name.</param>
        public void AddProperty(string property)
        {
            if (!_properties.Contains(property))
            {
                _properties.Add(property);
            }
        }

        /// <summary>
        /// Adds sorting to retrieve from the OData service.
        /// </summary>
        /// <param name="property">The property name.</param>
        /// <param name="descending"><c>true</c> to sort descending; otherwise <c>false</c>.</param>
        public void AddSorting(string property, bool descending = false)
        {
            var sortby = property;

            if (descending)
            {
                sortby += " desc";
            }

            if (!_sortby.Contains(sortby))
            {
                _sortby.Add(sortby);
            }
        }

        /// <summary>
        /// Skip the specified number of entities.
        /// </summary>
        /// <param name="number">The number of entities to skip.</param>
        public void Skip(int number) => _skip = number;

        /// <summary>
        /// Take the specified number of entities.
        /// </summary>
        /// <param name="number">The number of entities to take.</param>
        public void Take(int number) => _take = number;

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            var select = string.Join(",", _properties);

            if (!string.IsNullOrWhiteSpace(select))
            {
                select = $"$select={select}&";
            }

            var filter = string.Empty;

            if (Filter != null)
            {
                filter = $"$filter={Uri.EscapeUriString(Filter.ToString())}&";
            }

            var sortby = string.Join(",", _sortby);

            if (!string.IsNullOrWhiteSpace(sortby))
            {
                sortby = $"$orderby={sortby}&";
            }

            var take = string.Empty;

            if (_take > 0)
            {
                take = $"$top={_take}&";
            }

            var skip = string.Empty;

            if (_skip > 0)
            {
                skip = $"$skip={_skip}&";
            }

            var uri = $"{Url}{Entity}?{select}{filter}{sortby}{take}{skip}";

            uri = uri.EndsWith("?", StringComparison.Ordinal) ? uri.Substring(0, uri.Length - 1) : uri;
            uri = uri.EndsWith("&", StringComparison.Ordinal) ? uri.Substring(0, uri.Length - 1) : uri;

            return uri;
        }
    }
}
