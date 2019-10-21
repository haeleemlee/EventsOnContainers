FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /app
EXPOSE 80

COPY . .
RUN dotnet restore EventsCatalogAPI.csproj

FROM build AS publish
RUN dotnet publish "EventsCatalogAPI.csproj" -c Release -o /app

WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "EventsCatalogAPI.dll"]