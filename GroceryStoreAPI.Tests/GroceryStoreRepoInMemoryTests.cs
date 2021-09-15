using FluentAssertions;
using GroceryStoreAPI.DataAccess;
using GroceryStoreAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GroceryStoreAPI.Tests
{
    public class GroceryStoreRepoInMemoryTests
    {
        private GroceryStoreRepoInMemory GroceryStoreRepo;
        private readonly ILogger<GroceryStoreRepoInMemory> logger;
        private static readonly Dictionary<string, string> TestConfiguration = new Dictionary<string, string>
        {
            { "DatabaseFile", "..\\net5.0\\database.json" }
        };
        private readonly IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(TestConfiguration)
            .Build();

        private readonly List<Customer> TestCustomers = new List<Customer>
        {
            new Customer() { id = 1, name = "Bob"},
            new Customer() { id = 2, name = "Mary"},
            new Customer() { id = 3, name = "Joe"}
        };
        private readonly List<Customer> TestCustomersUpdated01 = new List<Customer>
        {
            new Customer() { id = 1, name = "Bob"},
            new Customer() { id = 2, name = "Jennifer"},
            new Customer() { id = 3, name = "Joe"}
        };

        private readonly Customer TestUpdate = new Customer() { id = 2, name = "Jennifer" };
        private readonly Customer TestUpdateWithBadId = new Customer() { id = 5, name = "Jennifer" };
        private readonly Customer TestCustomerToDelete = new Customer() { id = 1, name = "Bob" };



        [Fact]
        public async Task CanRepoRetrieveAllTestData()
        {
            GroceryStoreRepo = new GroceryStoreRepoInMemory(configuration, logger);

            List<Customer> result = await GroceryStoreRepo.GetAll();

            result.Should().BeEquivalentTo(TestCustomers);
        }

        [Fact]
        public async Task ReturnsNullIfCustomerNotFound()
        {
            GroceryStoreRepo = new GroceryStoreRepoInMemory(configuration, logger);

            var result = await GroceryStoreRepo.FindById(100);

            Assert.Null(result);
        }


        [Fact]
        public async Task CanFindACustomerById()
        {
            GroceryStoreRepo = new GroceryStoreRepoInMemory(configuration, logger);

            var result = await GroceryStoreRepo.FindById(3);

            Assert.Equal("Joe", result.name);
        }

        [Fact]
        public async Task CanFindACustomerByName()
        {
            GroceryStoreRepo = new GroceryStoreRepoInMemory(configuration, logger);

            var result = await GroceryStoreRepo.FindByName("Joe");

            Assert.Equal(3, result.id);
        }

        [Fact]
        public async Task CanAddNewCustomer()
        {
            GroceryStoreRepo = new GroceryStoreRepoInMemory(configuration, logger);

            Customer TestItem = new Customer() { id = 0, name = "Marvin" };

            var result = await GroceryStoreRepo.AddCustomer(TestItem);

            Assert.Equal(4, result.id);
            Assert.Equal("Marvin", result.name);
        }

        [Fact]
        public async Task CanUpdateACustomer()
        {
            GroceryStoreRepo = new GroceryStoreRepoInMemory(configuration, logger);

            var result01 = await GroceryStoreRepo.UpdateCustomer(TestUpdate);
            var result02 = await GroceryStoreRepo.GetAll();

            Assert.True(result01);
            result02.Should().BeEquivalentTo(TestCustomersUpdated01);
        }

        [Fact]
        public async Task CannotUpdateCustomerWithWrongID()
        {
            GroceryStoreRepo = new GroceryStoreRepoInMemory(configuration, logger);

            var result01 = await GroceryStoreRepo.UpdateCustomer(TestUpdateWithBadId);
            var result02 = await GroceryStoreRepo.GetAll();

            Assert.False(result01);
            result02.Should().BeEquivalentTo(TestCustomers);
        }

        [Fact]
        public async Task CanDeleteCustomer()
        {
            GroceryStoreRepo = new GroceryStoreRepoInMemory(configuration, logger);

            var result01 = await GroceryStoreRepo.DeleteCustomer(TestCustomerToDelete.id);
            List<Customer> result02 = await GroceryStoreRepo.GetAll();
            var result03 = await GroceryStoreRepo.FindById(TestCustomerToDelete.id);

            Assert.True(result01);
            Assert.Equal(2, result02.Count);
            Assert.Null(result03);
        }
    }
}
