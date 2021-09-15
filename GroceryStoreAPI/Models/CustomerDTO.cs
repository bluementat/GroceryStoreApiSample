using System.ComponentModel.DataAnnotations;

namespace GroceryStoreAPI.Models
{
    public class CustomerDTO
    {
        [Required]
        public string name { get; set; }
    }
}
