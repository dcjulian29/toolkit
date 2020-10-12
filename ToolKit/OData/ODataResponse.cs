using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace ToolKit.OData
{
    /// <summary>
    /// An OData Response Object.
    /// </summary>
    public class ODataResponse
    {
        /// <summary>
        /// Gets or sets the Context for the OData response.
        /// </summary>
        [JsonProperty("@odata.context")]
        public string Context { get; protected set; }

        /// <summary>
        /// Gets the HTTP Status code.
        /// </summary>
        public int StatusCode { get; internal set; }

        /// <summary>
        /// Gets or sets the collection of values returned.
        /// </summary>
        [JsonProperty("value")]
        [SuppressMessage(
            "Usage",
            "CA2227:Collection properties should be read only",
            Justification = "JSON Deserializing requires this property to be settable.")]
        public List<object> Value { get; protected set; }

        /// <summary>
        /// Gets or sets any warnings related to the OData request.
        /// </summary>
        [JsonProperty("@vsts.warnings")]
        [SuppressMessage(
            "Usage",
            "CA2227:Collection properties should be read only",
            Justification = "JSON Deserializing requires this property to be settable.")]
        public List<string> Warnings { get; protected set; }

        /// <summary>
        /// Create a new instance of the <see cref="ODataResponse" /> class.
        /// </summary>
        /// <param name="json">The string containing the JSON.</param>
        /// <returns>An object containing the data.</returns>
        public static ODataResponse Create(string json)
        {
            var response = JsonConvert.DeserializeObject<ODataResponse>(json);
            response.StatusCode = 200;

            return response;
        }
    }
}
