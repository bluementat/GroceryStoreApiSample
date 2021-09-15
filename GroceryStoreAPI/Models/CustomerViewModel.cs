using System.ComponentModel.DataAnnotations;

namespace GroceryStoreAPI.Models
{
    public class CustomerViewModel
    {
        [Required]
        public string name { get; set; }
    }
}
