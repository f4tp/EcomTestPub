# EcomTest - ECommerce API
A brief base for an Ecommerce web API, for Order Creation only

## Notes

This is an attempt at an Ecommerce API, for creating Orders only. There is plenty I have not considered within the amount of time I have spent on it. The following gives a summary of future considerations to make it an effective codebase:
-	There is some behaviour on CreateOrder in the domain service not working currently when committing a DB transaction, I didn't have time to investigate but will need to be fixed
-	Mappings - all DTOs in same namespace for my auto automapper mapper routine, need to change this
-	All DTOs need to have public setters on them atm so the HTTP request can have the JSON serialization set the DTO propeties. I was hoping to use  [JsonPropertyName("propertyName")] attributes but they seemed not to work, will be one to fix in the future.
-	Concurrency – added TimeStamp but run out of time to implement this
-	Scope is set at (mostly) public, need to go through and rethink
-	Not every prop on each entity has a column attribute tag on to tell them db migration what datatype to create
-	OrderController made into CreateOrderController as logic in there was gettign heavy, better to seperate things out like this. This likely would appliy to the Application and Domain services when the application gets larger
-	Not had chance to add documentation tags to all methods
-	Some specific things - search for FUTURE CONSIDERATION: for things I would do differently given more time
-	Other general considerations that need to be taken on board in future:
-	  CORS
-	  Auth
-	  Localisation
-	  Testing
-	  Caching
-	  Monitoring 



[![Build Status](https://travis-ci.org/username/repo.svg?branch=main)](https://travis-ci.org/username/repo)
[![Coverage Status](https://coveralls.io/repos/github/username/repo/badge.svg?branch=main)](https://coveralls.io/github/username/repo?branch=main)
![GitHub version](https://img.shields.io/badge/version-1.0-blue.svg)

## Table of Contents

- [Description](#description)
- [Installation](#installation)
- [Usage](#usage)
- [Configuration](#configuration)
- [Contributing](#contributing)
- [Testing](#testing)
- [Licence](#licence)
- [Acknowledgments](#acknowledgments)
- [Documentation](#documentation)
- [Contact Information](#contact-information)

## Description

EcomTest is an e-commerce API that provides a set of endpoints for managing orders, products, and customers.

## Installation

To install EcomTest, follow these steps:

## Usage
..
## Configuration
..
## Contributing
..
## Testing
..
## Licence
--
## Acknowledgements
..
## Documentation
..
## Contact Information
