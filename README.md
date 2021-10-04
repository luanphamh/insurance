# insurance
A simple ASP.NET Core sample web application using .Net Web Api Core and PostgreSQL with Docker support.Both the Web Applicaiton and the Postgres DB runs in container. Added Swagger support to interact with API’s resources.

## Prerequisites
1. [Docker](https://www.docker.com/)

## Steps
1. `git clone https://github.com/luanphamh/insurance.git`

2. `cd src/api`

3. `docker-compose build`

4. `docker-compose up`

5.  Navigate to http://localhost:8887/swagger

## API
1.  Get  http://localhost:8887/insurance: get all insurance records

1.  Post http://localhost:8887/insurance: update or insert insurance records

2.  Post http://localhost:8887/insurance/findFraudProfile: find fraudulent insurance records
