using GroceryStoreAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace GroceryStoreAPI.DataAccess
{
    public class UserRepositoryInMemory : IUserRepository
    {
        private readonly List<ApiUser> ApiUserDb = new List<ApiUser>();

        public UserRepositoryInMemory()
        {
            ApiUserDb.Add(new ApiUser
            {
                Name = "John",
                Password = "password1"
            });
            ApiUserDb.Add(new ApiUser
            {
                Name = "Jane",
                Password = "password2"
            });
        }

        public ApiUserDTO Getuser(ApiUser user)
        {
            ApiUserDTO result = new ApiUserDTO() { Name = string.Empty };

            var FoundUser = ApiUserDb.Where(x => x.Name.ToLower() == user.Name.ToLower() && x.Password == user.Password).FirstOrDefault();
            if (FoundUser != null)
            {
                result.Name = FoundUser.Name;
            }

            return result;
        }
    }
}
