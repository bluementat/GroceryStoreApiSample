using GroceryStoreAPI.DataAccess;
using GroceryStoreAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]    
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly IGroceryStoreRepository repository;
        private readonly ILogger<CustomersController> logger;

        public CustomersController(IGroceryStoreRepository Repository, ILogger<CustomersController> Logger)
        {
            repository = Repository;
            logger = Logger;
        }


        /// <summary>
        /// Get a list of all customers
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/customers        
        ///
        /// </remarks>        
        /// <returns>Returns a list of all customers</returns>
        /// <response code="200">Returns a list of all customers</response>        
        [HttpGet]
        [ProducesResponseType(typeof(List<Customer>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetAllCustomers()
        {
            var result = await repository.GetAll();

            return Ok(result);
        }


        /// <summary>
        /// Get a customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/v1/customers/[id]        
        ///
        /// </remarks>        
        /// <returns>A customer</returns>
        /// <response code="200">Returns a customer with the ID = id</response>
        /// <response code="404">If the customer is not found</response>   
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetCustomer(int id)
        {
            var result = await repository.FindById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }


        /// <summary>
        /// Add a new customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT api/v1/customers
        ///     {        
        ///         "name": "CustomerName"
        ///     }
        ///
        /// </remarks>        
        /// <returns>A customer</returns>
        /// <response code="200">The customer already exists in database</response>   
        /// <response code="201">The customer was added</response>
        [HttpPut]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> AddCustomer(CustomerDTO newCustomer)
        {
            var CheckForDuplicate = await repository.FindByName(newCustomer.name);
            if (CheckForDuplicate != null)
            {
                return Ok(CheckForDuplicate);
            }

            var customer = new Customer() { id = 0, name = newCustomer.name };
            var result = await repository.AddCustomer(customer);

            string ResourceUri = CreateUriStringForResourceId(result.id);

            return Created(new Uri(ResourceUri), result);
        }


        /// <summary>
        /// Update a customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/v1/customers
        ///     {        
        ///         "id": "1"
        ///         "name": "CustomerName"
        ///     }
        ///
        /// </remarks>        
        /// <returns>A customer</returns>         
        /// <response code="201">The customer was updated</response>        
        /// <response code="404">The customer does not exist</response>  
        /// <response code="503">Unable to add customer to the database</response>  
        [HttpPost]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> UpdateCustomer(Customer updateCustomer)
        {
            if (await repository.FindById(updateCustomer.id) == null)
            {
                return NotFound();
            }

            if (await repository.UpdateCustomer(updateCustomer))
            {
                string ResourceUri = CreateUriStringForResourceId(updateCustomer.id);

                return Created(new Uri(ResourceUri), updateCustomer);
            }
            else
            {
                return StatusCode(503);
            }
        }

        
        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE api/v1/customers/[id]
        ///
        /// </remarks>              
        /// <response code="204">The customer has been deleted</response>        
        /// <response code="404">The customer does not exist</response>  
        /// <response code="503">Unable to delete customer from the database</response>  
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            if (await repository.FindById(id) == null)
            {
                return NotFound();
            }

            if (await repository.DeleteCustomer(id))
            {
                return NoContent();
            }
            else
            {
                return StatusCode(503);
            }
        }

        private string CreateUriStringForResourceId(int Id)
        {
            return $"https://"
                            + HttpContext.Request.Host.ToString()
                            + HttpContext.Request.Path.ToString()
                            + "/" + Id;
        }
    }
}
