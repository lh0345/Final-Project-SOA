# Azure Deployment Guide

## Prerequisites

- Azure subscription
- Azure CLI installed (`az`)
- .NET 10.0 SDK

## Step 1: Create Azure Resources

```powershell
# Login to Azure
az login

# Create resource group
az group create --name TaskFlowRG --location westeurope

# Create SQL Server
az sql server create --name taskflow-sql-server `
    --resource-group TaskFlowRG `
    --location westeurope `
    --admin-user taskflowadmin `
    --admin-password "YourStrong!Passw0rd"

# Configure firewall to allow Azure services
az sql server firewall-rule create --resource-group TaskFlowRG `
    --server taskflow-sql-server `
    --name AllowAzureServices `
    --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

# Create SQL Database
az sql db create --resource-group TaskFlowRG `
    --server taskflow-sql-server `
    --name TaskFlowDb `
    --service-objective S0

# Create App Service plan
az appservice plan create --name TaskFlowPlan `
    --resource-group TaskFlowRG `
    --sku B1 --is-linux

# Create Web App
az webapp create --name taskflow-api `
    --resource-group TaskFlowRG `
    --plan TaskFlowPlan `
    --runtime "DOTNET:10"

# Get connection string
$connString = "Server=tcp:taskflow-sql-server.database.windows.net,1433;Database=TaskFlowDb;User ID=taskflowadmin;Password=YourStrong!Passw0rd;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

# Configure app settings
az webapp config connection-string set --resource-group TaskFlowRG `
    --name taskflow-api `
    --settings DefaultConnection="$connString" `
    --connection-string-type SQLAzure

az webapp config appsettings set --resource-group TaskFlowRG `
    --name taskflow-api `
    --settings Jwt:Key="Your-Production-Secret-Key-That-Is-At-Least-32-Characters!" `
    Jwt:Issuer="TaskFlowAPI" `
    Jwt:Audience="TaskFlowClient" `
    AdminCredentials:Email="admin@taskflow.com" `
    AdminCredentials:Password="Admin@123456"
```

## Step 2: Publish and Deploy

```powershell
# Publish the API
dotnet publish TaskFlow.API/TaskFlow.API.csproj -c Release -o ./publish

# Deploy via zip
cd publish
Compress-Archive -Path * -DestinationPath ../deploy.zip -Force
cd ..

az webapp deploy --resource-group TaskFlowRG `
    --name taskflow-api `
    --src-path ./deploy.zip `
    --type zip
```

## Step 3: Verify

Navigate to `https://taskflow-api.azurewebsites.net/swagger` to verify the deployment.
