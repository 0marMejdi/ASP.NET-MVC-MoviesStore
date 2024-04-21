FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /
RUN dotnet new mvc --name ASP.NET-MVC-MoviesStore
WORKDIR /ASP.NET-MVC-MoviesStore
# COPY . . 
COPY Controllers Controllers 
COPY Views Views 
COPY Services Services
COPY Models Models
COPY appsettings.json appsettings.json
COPY InitialGenres.json InitialGenres.json
COPY wwwroot wwwroot
COPY Repositories Repositories
COPY Program.cs Program.cs

RUN dotnet add package Microsoft.EntityFrameworkCore --version 7.0.0
RUN dotnet add package Microsoft.EntityFrameworkCore.sqlserver --version 7.0.0
RUN dotnet add package Microsoft.EntityFrameworkCore.design --version 7.0.0
RUN dotnet add package Microsoft.EntityFrameworkCore.tools --version 7.0.0

RUN dotnet build 
RUN dotnet tool install --global dotnet-ef --version 7.0.0
RUN export PATH="$PATH:/root/.dotnet/tools"
RUN /root/.dotnet/tools/dotnet-ef migrations add init 
RUN /root/.dotnet/tools/dotnet-ef database update 

EXPOSE 5555
CMD dotnet run --urls "http://0.0.0.0:5555"