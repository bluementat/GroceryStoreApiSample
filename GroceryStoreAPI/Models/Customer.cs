using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GroceryStoreAPI.Models
{
    public class Customer
    {
        [JsonPropertyName("id")]
        public int id { get; set; }

        [JsonPropertyName("name")]
        [Required]
        public string name { get; set; }

        public Customer()
        {
            id = 0;
            name = String.Empty;
        }
    }
}
