# Billing system - backend
> Web application to billing customers in your language school. 

## Table of contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Features](#features)
* [Status](#status)
* [Sreenshots](#screenshots)

## General info
Application to invoice customers.
That one is built in Clean Architecture and CQRS pattern.

## Technologies
* .NET Core 8.0
* WEB API
* ASP.NET
* EntityFramework
* Depedency Injection
* LINQ
* Fluent Validation
* MediatR
* Serilog

## Features
* Customers managment - CRUD operations
* Invoices managment - CRUD operations
* Corrections managment - CRUD operations
* Interest Notes - CRUD operations
* Readings (data to invoice) managment - CRUD operations
* Print documents
* Users authetications

## Screenshots
### Swagger
![Swagger](BillingSystem.WebApi/Src/Swagger_1.png)
![Swagger](BillingSystem.WebApi/Src/Swagger_2.png)
![Swagger](BillingSystem.WebApi/Src/Swagger_3.png)

Screenshots from Frontend:
https://github.com/GrzegorzSitkowski/BillingSystem-Frontend

## Status
WORK IN PROGRESS
I work for options to insert payments of customers and load data from file. 

Database: https://github.com/GrzegorzSitkowski/BillingSystem-Database
