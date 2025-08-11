# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . . 

# Substitua pelo caminho correto do seu projeto .csproj
RUN dotnet publish "BoraEncontros.Workers/BoraEncontros.Workers.csproj" -c Release -o /app/publish

# Etapa final com imagem base de Azure Functions .NET Isolated
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated9.0 AS runtime
WORKDIR /home/site/wwwroot

COPY --from=build /app/publish . 

# Porta padrão do host de funções
#ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
#    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

EXPOSE 9090

# O ENTRYPOINT é definido automaticamente pela imagem do Azure Functions
