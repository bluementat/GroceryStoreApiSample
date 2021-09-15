using GroceryStoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStoreAPI.DataAccess
{
    public interface IUserRepository
    {
        public ApiUserDTO Getuser(ApiUser user);
    }
}
