using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosNetCoreSqlApi.Models
{
    public class Address
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
    }
}
