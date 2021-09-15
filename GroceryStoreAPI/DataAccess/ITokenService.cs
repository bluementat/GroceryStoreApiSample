using GroceryStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.DataAccess
{
    public interface ITokenService
    {
        public string BuildToken(string key, string issuer, ApiUserDTO user);
        public bool IsTokenValid(string key, string issuer, string token);
     
    }
}
