using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosNetCoreSqlApi.Models
{
    public class Contact
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Address Address { get; set; }
        public string Email { get; set; }
    }
}
