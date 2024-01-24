FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
EXPOSE 5001
WORKDIR /app

COPY ./ssl /https

ENV ASPNETCORE_HTTPS_PORT=5001
ENV ASPNETCORE_URLS=https://+:5001
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=lmao
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY BankSystem.Api/*.csproj ./BankSystem.Api/
COPY BankSystem.Data/*.csproj ./BankSystem.Data/
COPY BankSystem.Services/*.csproj ./BankSystem.Services/
COPY BankSystem.Repositories/*.csproj ./BankSystem.Repositories/
RUN dotnet restore "BankSystem.Api/BankSystem.Api.csproj"
COPY . .
WORKDIR "/src/BankSystem.Api"
RUN dotnet build "BankSystem.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankSystem.Api.csproj" -c Release -o /app/publish 
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankSystem.Api.dll"]
