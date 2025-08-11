# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY . . 

# Substitua pelo caminho correto do seu projeto .csproj
RUN dotnet publish "BoraEncontros.Workers/BoraEncontros.Workers.csproj" -c Release -o /app/publish

# Etapa final com imagem base de Azure Functions .NET Isolated
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated9.0 AS runtime
WORKDIR /home/site/wwwroot

COPY --from=build /app/publish . 

# Opcional: logs mais verbosos no console
#ENV AzureFunctionsJobHost__logging__console__isEnabled=true

EXPOSE 8080
# O ENTRYPOINT é definido automaticamente pela imagem do Azure Functions
