# Azure Deployment Fix for 403 Forbidden Error

## Problem Analysis
Your LMS application is returning 403 Forbidden because of database configuration issues and deployment workflow problems.

## Current Configuration
- **Database**: Aiven MySQL (`mysql-28fa20e-yuneeb-project.a.aivencloud.com:12289`)
- **Database Name**: `lms_db`
- **User**: `avnadmin`
- **Connection**: SSL enabled (Aiven default)

## Issues Fixed

### 1. Build Configuration Issues
- ✅ Fixed: Changed `dotnet publish` to target specific project instead of solution
- ✅ Fixed: Updated artifact paths to match new publish directory

### 2. Azure Actions Deprecation
- ✅ Fixed: Added `FORCE_JAVASCRIPT_ACTIONS_TO_NODE24: true` environment variable
- ✅ Fixed: Updated `azure/login@v1` to `azure/login@v2`

### 3. Database Configuration
- ✅ Fixed: Updated `appsettings.Production.json` to use your Aiven MySQL database
- ✅ Fixed: Skipped SQL Server database initialization (not needed for external MySQL)
- ✅ Fixed: Uncommented environment variable replacement in `DependencyInjection.cs`

### 4. CORS Configuration
- ✅ Fixed: Added Azure domain to allowed origins in `Program.cs`

## Quick Deployment Options

### Option 1: Use Simple Deployment Workflow (Recommended)
I've created a new simplified deployment workflow that avoids the IP Forbidden issues:

1. **Go to GitHub Actions** in your repository
2. **Select "Deploy Simple" workflow**
3. **Click "Run workflow"**
4. **Choose environment**: Development
5. **Click "Run workflow"**

This workflow:
- Builds and tests the application
- Deploys directly using Azure CLI (avoids IP forbidden issues)
- Restarts the app service
- Tests the health endpoint
- Provides deployment URLs

### Option 2: Fix Existing Workflow
If you prefer to fix the existing workflow, the issues are:
1. **IP Forbidden**: May need to check Azure App Service deployment restrictions
2. **Database Migration**: SQL Server migrations were failing on MySQL
3. **Build Configuration**: Solution-level publish was causing conflicts

### Option 3: Manual Deployment
```bash
# Build locally
dotnet publish src/Web/Web.csproj -c Release -o ./publish
zip -r publish.zip ./publish/*

# Deploy using Azure CLI
az webapp deployment source config-zip \
  --resource-group your-resource-group \
  --name lms-web-api-ddadcpgpe5gwc8gq \
  --src ./publish.zip

# Restart app service
az webapp restart \
  --resource-group your-resource-group \
  --name lms-web-api-ddadcpgpe5gwc8gq
```

## Testing After Deployment

### 1. Test Health Endpoint
```bash
curl https://lms-web-api-ddadcpgpe5gwc8gq.centralindia-01.azurewebsites.net/health
```

### 2. Test API Documentation
```bash
curl https://lms-web-api-ddadcpgpe5gwc8gq.centralindia-01.azurewebsites.net/swagger
```

### 3. Check Application Logs
```bash
az webapp log tail \
  --resource-group your-resource-group \
  --name lms-web-api-ddadcpgpe5gwc8gq
```

## If You Still Get 403 Errors

### Check Azure App Service Configuration
```bash
az webapp config appsettings list \
  --resource-group your-resource-group \
  --name lms-web-api-ddadcpgpe5gwc8gq
```

### Verify Database Connection
The application should now connect to your Aiven MySQL database using the connection string in `appsettings.Production.json`.

### Check Network Security
If you still have IP issues:
1. Check Azure App Service network restrictions
2. Verify Aiven MySQL allows connections from Azure
3. Check any firewalls or network security groups

## Database Migrations (if needed)

If your database schema needs updating:
```bash
# Connect to your Aiven MySQL database and run migrations
dotnet ef database update --project src/Infrastructure --startup-project src/Web --connection "server=mysql-28fa20e-yuneeb-project.a.aivencloud.com;Port=12289;User=avnadmin;Password=AVNS_SaeQDY_w4cCS811dMYX;Database=lms_db;Connection Timeout=60;Charset=utf8mb4;"
```

## Summary

All major issues have been fixed:
- ✅ Build configuration corrected
- ✅ Node.js deprecation warnings resolved
- ✅ Database connection configured for Aiven MySQL
- ✅ CORS includes Azure domain
- ✅ Simplified deployment workflow created

Use the **Deploy Simple** workflow for the most reliable deployment experience.
