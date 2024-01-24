FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY BankSystem.Api/*.csproj ./BankSystem.Api/
COPY BankSystem.Data/*.csproj ./BankSystem.Data/
COPY BankSystem.Services/*.csproj ./BankSystem.Services/
COPY BankSystem.Repositories/*.csproj ./BankSystem.Repositories/
RUN dotnet restore ./BankSystem.Api/BankSystem.Api.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish ./BankSystem.Api/BankSystem.Api.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/BankSystem.Api/out .
ENTRYPOINT ["dotnet", "BankSystem.Api.dll"]

