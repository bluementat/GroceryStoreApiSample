# Overview
This repository contains a simple Grocery Store REST API Solution. At this time, the API only provides endpoints for access to Customer data which only contains ID's and Names. This data is provided by an in-memory database abstracted with a repository design pattern. For later development, an IGroceryStoreRepository interface is provided to allow other data sources as well as an expansion of the data provided.

The API implements JWT Authentication. Consumers can create bearer tokens by first authenticating against an in-memory user database. Once more, this database id abstracted with user repository to allow other implementations. The user database contains two items
- User 1
  - Name: John
  - Password: password1
- User 2 
  - Name: Jane
  - Password: password2

Consuming the /ap1/v1/Authenticate/login endpoint with a user name and password returns a token that can be used to access other API endpoints described below. The solution also provides a Swagger Document for more detailed endpoint descriptions and testing.

GET /api​/v1​/Customers - Get a list of all customers

PUT /api​/v1​/Customers - Add a new customer

POST /api​/v1​/Customers - Update a customer

GET /api​/v1​/Customers​/{id} - Get a customer

DELETE /api​/v1​/Customers​/{id} - Delete a customer


## Technologies
- .NET 5
- ASP.NET Core Authentication
- XUnit
- OpenAPI/Swagger

