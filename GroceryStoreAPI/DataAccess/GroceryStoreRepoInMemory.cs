using GroceryStoreAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GroceryStoreAPI.DataAccess
{
    public class GroceryStoreRepoInMemory : IGroceryStoreRepository
    {
        private readonly GroceryStoreDataBaseInMemory db;
        private readonly IConfiguration configuration;
        private readonly ILogger logger;

        public GroceryStoreRepoInMemory(IConfiguration Configuration, ILogger<GroceryStoreRepoInMemory> Log)
        {
            configuration = Configuration;
            logger = Log;
            ReadInData(out db);
        }


        public async Task<List<Customer>> GetAll()
        {
            return await Task.FromResult(db.customers.Select(c => c).ToList());
        }

        public async Task<Customer> FindById(int Id)
        {
            return await Task.FromResult(db.customers.Find(c => c.id == Id));
        }

        public async Task<Customer> FindByName(string name)
        {
            return await Task.FromResult(db.customers.Find(c => c.name == name));
        }

        public async Task<Customer> AddCustomer(Customer customer)
        {
            customer.id = await FindNextAvailableCustomerId();
            db.customers.Add(customer);

            return customer;
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            var ItemToUpdate = db.customers.Find(c => c.id == customer.id);
            if (ItemToUpdate == null)
            {
                return await Task.FromResult(false);
            }

            db.customers.Where(c => c.id == customer.id).First().name = customer.name;
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteCustomer(int Id)
        {
            var ItemToDelete = db.customers.Find(c => c.id == Id);
            if (ItemToDelete == null)
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(db.customers.Remove(ItemToDelete));
        }


        private bool ReadInData(out GroceryStoreDataBaseInMemory Db)
        {           
            try
            {
                string DatabaseAsJson = File.ReadAllText(configuration["DatabaseFile"]);
                Db = JsonSerializer.Deserialize<GroceryStoreDataBaseInMemory>(DatabaseAsJson);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                Db = new GroceryStoreDataBaseInMemory();
                Db.customers = new List<Customer>();
                return false;
            }
        }

        private async Task<int> FindNextAvailableCustomerId()
        {
            int result = db.customers.Count;
            while (db.customers.Exists(c => c.id == result))
            {
                result++;
            }

            return await Task.FromResult(result);
        }

    }
}
