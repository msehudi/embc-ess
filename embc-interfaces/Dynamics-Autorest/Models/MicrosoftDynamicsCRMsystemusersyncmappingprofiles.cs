// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Gov.Jag.Embc.Interfaces.Models
{
    using Newtonsoft.Json;
    using System.Linq; using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// systemusersyncmappingprofiles
    /// </summary>
    public partial class MicrosoftDynamicsCRMsystemusersyncmappingprofiles
    {
        /// <summary>
        /// Initializes a new instance of the
        /// MicrosoftDynamicsCRMsystemusersyncmappingprofiles class.
        /// </summary>
        public MicrosoftDynamicsCRMsystemusersyncmappingprofiles()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// MicrosoftDynamicsCRMsystemusersyncmappingprofiles class.
        /// </summary>
        public MicrosoftDynamicsCRMsystemusersyncmappingprofiles(string systemusersyncmappingprofileid = default(string), string syncattributemappingprofileid = default(string), long? versionnumber = default(long?), string systemuserid = default(string))
        {
            Systemusersyncmappingprofileid = systemusersyncmappingprofileid;
            Syncattributemappingprofileid = syncattributemappingprofileid;
            Versionnumber = versionnumber;
            Systemuserid = systemuserid;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "systemusersyncmappingprofileid")]
        public string Systemusersyncmappingprofileid { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "syncattributemappingprofileid")]
        public string Syncattributemappingprofileid { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "versionnumber")]
        public long? Versionnumber { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "systemuserid")]
        public string Systemuserid { get; set; }

    }
}
