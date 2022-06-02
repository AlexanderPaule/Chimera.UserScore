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
In my first architecture I thought to use SQL Server for storaging data because of my familiarity with it, but it is not a scalable solution and it allows 32K of maximum open connections.
SQL Server is a wrong technology if we need to allow 1M of open connections, it will simply crash and the cost for made the solution scalable could be very expansive.


Elasticsearch instead is a very scalable solution and does not have limitations on number of connections. So it basically is perfect for handle a traficated service.
For simplicity, the provided solution has only one node, and at the moment a high number of requests probably will be closed with a timeout error in case of 1M requests per second, but the power of Elasticsearch is that it can be scaled later at a cheap coast.

### ASP.NET Core 3.1
I choose this version of .NET core and not .NET 6 because of my familiarity with it and I prefered to move on something I know for reduce the risks seeing the ammount of time for the project.

## Technical Documentation
API are documented with swagger that expose a handy UI for testing all endpoints.

### Starting the service
You need a working Server Instance of Elastic for testing the solution and can download one from https://www.elastic.co/downloads/elasticsearch
Unzip it and open a CMD in the folder then run the next commands
```shell
bin\elasticsearch-service.bat install
bin\elasticsearch-setup-passwords.bat interactive
bin\elasticsearch-service.bat start
```
Put the registered password for elastic user in the configuration file **appsettings.json** in the project solution.
Now compile the solution and start it. A browser will be opened on swager path, so you can easilly proceed with testing.

### Endpoints
Endpoints are exposed by 2 different controllers
- UserController
  - [POST] /User/Register
- ScoreController
  - [POST] /Score/Register
  - [GET] /Score/Leaderboard

## Improvments and Limitations
The service is able to support 1K users and also 1M becuse Elastic is a scalable technology.
As said before, Elasticsearch has no limitations on the number of open connection, but the calls can go in timeout if the traffic is too large, so in this case the solution will require a change of current configuration.
For the solution only one node has been configured, but in case of the traffic increase, more nodes can be configured and the index can be distributed in that way.


Also the API service could be stressed in case of the traffic increase, and this can be solved with a multiple API nodes. In this case it will be necesary to introduce a balancer to redirect the calls to the less busy node.


The bottleneck at the moment depends on the resources dedicated to the single actors, API service or Elasticsearch service.
With a proper monitoring engine we can understand where could be the problem and reorganize the solution to support the traffic in the best way.