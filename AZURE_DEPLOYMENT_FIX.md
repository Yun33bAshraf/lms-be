# Azure Deployment Fix for 403 Forbidden Error

## Problem Analysis
Your LMS application is returning 403 Forbidden because of database configuration issues. The application is now configured to use your existing Aiven MySQL database.

## Current Configuration
- **Database**: Aiven MySQL (`mysql-28fa20e-yuneeb-project.a.aivencloud.com:12289`)
- **Database Name**: `lms_db`
- **User**: `avnadmin`
- **Connection**: SSL enabled (Aiven default)

## Quick Fix - Deploy Updated Configuration

The code has been updated to use your existing Aiven MySQL database. Now deploy the changes:

### 1. Deploy Updated Code
```bash
# Build and deploy
dotnet publish src/Web/Web.csproj -c Release -o ./publish
zip -r publish.zip ./publish/*

# Deploy to Azure
az webapp deployment source config-zip \
  --resource-group your-resource-group \
  --name lms-web-api-ddadcpgpe5gwc8gq \
  --src ./publish.zip
```

### 2. Restart the App Service
```bash
az webapp restart \
  --resource-group your-resource-group \
  --name lms-web-api-ddadcpgpe5gwc8gq
```

### 3. Test the Health Endpoint
```bash
curl https://lms-web-api-ddadcpgpe5gwc8gq.centralindia-01.azurewebsites.net/health
```

### 4. Test the API
```bash
curl https://lms-web-api-ddadcpgpe5gwc8gq.centralindia-01.azurewebsites.net/swagger
```

## What Was Fixed
1. ✅ **Database Connection**: Updated `appsettings.Production.json` to use your Aiven MySQL database
2. ✅ **CORS Configuration**: Added Azure domain to allowed origins in `Program.cs`
3. ✅ **Environment Variables**: Uncommented environment variable replacement in `DependencyInjection.cs`

## Important Notes

### Aiven MySQL Security
- Aiven requires SSL connections by default
- Your connection string includes `Charset=utf8mb4` for proper Unicode support
- The database is accessible from Azure (no firewall changes needed)

### If You Still Get 403 Errors
Check the application logs:
```bash
az webapp log tail \
  --resource-group your-resource-group \
  --name lms-web-api-ddadcpgpe5gwc8gq
```

Common issues:
1. **Database Connection**: Verify Aiven database is accessible from Azure
2. **Migration Issues**: May need to run migrations if database schema is different
3. **Startup Errors**: Check logs for detailed error messages

### Run Database Migrations (if needed)
If the database schema needs updating:
```bash
# SSH into the app service
az webapp ssh \
  --resource-group your-resource-group \
  --name lms-web-api-ddadcpgpe5gwc8gq

# Once connected:
cd /home/site/wwwroot
dotnet ef database update --project src/Infrastructure --startup-project src/Web
```

## Alternative: Set Environment Variables in Azure
If you prefer to use environment variables instead of hardcoding:
```bash
az webapp config appsettings set \
  --resource-group your-resource-group \
  --name lms-web-api-ddadcpgpe5gwc8gq \
  --settings \
    "ConnectionStrings__DefaultConnection=server=mysql-28fa20e-yuneeb-project.a.aivencloud.com;Port=12289;User=avnadmin;Password=AVNS_SaeQDY_w4cCS811dMYX;Database=lms_db;Connection Timeout=60;Charset=utf8mb4;" \
    "ASPNETCORE_ENVIRONMENT=Production"
```

## Next Steps
1. Deploy the updated code
2. Restart the app service
3. Test the health endpoint
4. If needed, run database migrations

Your application should now connect to your existing Aiven MySQL database and resolve the 403 Forbidden error.
