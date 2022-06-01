# Chimera.ScoreSystem

## Tecnologies
- Framework: **ASP.Net Core 3.1**
- API Documentation: **Swagger**
- Data Storage: **Elasticsearch**
- Data Storage Client: **Nest**

## Component Choices
The solution was not been developed in TDD because the requirements are mainly based on the tecnology handling and the core logic is absent.
The Architecture is a REST API that stores data on the repository that uses Nest client to communicate with Elastic for storaging data.

### Database Choice
I choose **elastycsearch** because of performance question made in the requirements related to possible future improvements. "Can we handle 1,000 users? What about 1,000,000?".
In my first architecture I thout to use SQL Server for storaging data because of my familiarity with it, but it is not a scalable solution and it allows 32K of maximum open connections.
SQL Server is a wrong technology if we need to allow 1M of open connections, it will simply crash and the cost for made the solution scalable could be very expansive.


Elasticsearch instead is a very scalable solution and does not have limitations on number of connections. So it basically is perfect for handle a traficated service.
For simplicity, the provided solution has only one node, and at the moment a high number of requests probably will be closed with a timeout error in case of 1M requests per second, but the power of Elastycsearch is that it can be scaled later at a cheap coast.

### ASP.NET Core 3.1
I choose this version of .NET core and not .NET 6 because of my familiarity with it and I prefered to move on something I know for reduce the risks seeing the ammount of time for the project.

## Technical Documentation
API are documented with swagger that expose a handy UI for testing all endpoints.
