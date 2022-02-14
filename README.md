# insurance
A simple ASP.NET Core sample web application using .Net Web Api Core and PostgreSQL with Docker support.

## Prerequisites
1. [Docker]

## Steps
1. `git clone https://github.com/luanphamh/insurance.git`

2. `cd src/api`

3. `docker-compose build`

4. `docker-compose up`

5.  Navigate to:  http://localhost:8887/swagger

## API
1.  Get  http://localhost:8887/insurance: api lấy tất cả hồ sơ bảo hiểm

1.  Post http://localhost:8887/insurance: api nộp hồ sơ

2.  Post http://localhost:8887/insurance/findFraudProfile: api trả về kết quả các hồ sơ gian lận 
