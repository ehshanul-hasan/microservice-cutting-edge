This is serving as the microservice backend for e demo TourPackage Booking system app Which is built using cutting edge technologies like

1. .NET6
2. Consul [Service Discovery]
3. Ocelot [API gateway]
4. RabbitMQ [Message Bus]
5. SQL Server [Database]
6. Entity Framework [ORM]
7. Unit Of Work with Generic Repository Patter.
8. Redis [cache]
9. Serilog [Logging library]
10. Elastic search, Logstash, Kibana [ELK centralized logging]
11. Open Telemetry with zipkin [Tracing]
12. Health Check UI [Services Health check]
13. Consul Key Vault [Centralized Configuration Management]

This back end is serving microfronted https://github.com/ehshanul-hasan/Micro_Frontend_Angular.git which is bult using module federation.


![image](https://user-images.githubusercontent.com/77856935/155855477-c27168b5-6959-4306-8ec9-fb34053107ea.png)


NB: This project is a demo just for technology show down </br>
Catalog service is full functional with sql server connection with unit of work and repository pattern.</br>
Cart service is only connected to redis and rabbitmqto get set value. Ideally it there hould be a repository pattern as well which is not included in this project.</br>
Resrvation service is capable to consume and process messages from RabbitMQ. Ideally DB operation of creating reservation will be present here but not implemented right now.
