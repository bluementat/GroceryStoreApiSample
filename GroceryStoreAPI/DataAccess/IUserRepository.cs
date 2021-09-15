using GroceryStoreAPI.Models;

namespace GroceryStoreAPI.DataAccess
{
    public interface IUserRepository
    {
        public ApiUserDTO Getuser(ApiUser user);
    }
}
