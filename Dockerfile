FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Boards.WebApp/*.csproj ./Boards.WebApp/
COPY Boards.DAL/*.csproj ./Boards.DAL/
COPY Boards.DTO/*.csproj ./Boards.DTO/
COPY Boards.Tests/*.csproj ./Boards.Test/
RUN dotnet restore

# copy everything else and build app
COPY Boards.WebApp/. ./Boards.WebApp/
COPY Boards.DAL/. ./Boards.DAL/
COPY Boards.DTO/. ./Boards.DTO/
COPY Boards.Tests/. ./Boards.Tests/

WORKDIR /app/Boards.WebApp
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/Boards.WebApp/out ./
ENTRYPOINT ["dotnet", "Boards.WebApp.dll"]