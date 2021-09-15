using GroceryStoreAPI.Models;

namespace GroceryStoreAPI.Security
{
    public interface ITokenService
    {
        public string BuildToken(string key, string issuer, ApiUserDTO user);
        public bool IsTokenValid(string key, string issuer, string token);

    }
}
