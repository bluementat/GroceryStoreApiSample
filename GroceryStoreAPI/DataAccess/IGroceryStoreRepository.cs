using GroceryStoreAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryStoreAPI.DataAccess
{
    public interface IGroceryStoreRepository
    {
        public Task<List<Customer>> GetAll();

        public Task<Customer> FindById(int Id);

        public Task<Customer> FindByName(string name);

        public Task<Customer> AddCustomer(Customer customer);

        public Task<bool> UpdateCustomer(Customer customer);

        public Task<bool> DeleteCustomer(int Id);
    }
}
