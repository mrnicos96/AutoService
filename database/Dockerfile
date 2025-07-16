FROM mcr.microsoft.com/mssql/server:2022-latest

COPY ./migrations/ /migrations/

ENV ACCEPT_EULA=Y
ENV MSSQL_SA_PASSWORD=MyStrong@Passw0rd
ENV MSSQL_PID=Developer

EXPOSE 1433