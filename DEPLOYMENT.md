# Fly.io Deployment Guide

This guide will help you deploy the LMS Backend application to Fly.io.

## Prerequisites

1. Install Fly.io CLI: https://fly.io/docs/hands-on/install-flyctl/
2. Sign up for a Fly.io account: https://fly.io/app/sign-up
3. Authenticate with Fly.io: `fly auth login`

## Environment Variables

You'll need to set these environment variables in your Fly.io app:

### Database Configuration
- `MYSQL_HOST` - Your MySQL database host
- `MYSQL_USER` - Your MySQL username  
- `MYSQL_PASSWORD` - Your MySQL password
- `MYSQL_DATABASE` - Your MySQL database name (default: ts_prod_new)

### JWT Configuration
- `Jwt__SecretKey` - Your JWT secret key
- `Jwt__ExpiryMinutes` - JWT token expiry time (default: 60)

### Email Configuration (Optional)
- `ZohoLogin__smtpUsername` - SMTP username for emails
- `ZohoLogin__smtpPassword` - SMTP password for emails

### Frontend Configuration
- `AppConfig__FEBaseURL` - Frontend base URL

## Deployment Steps

### 1. Set Up Fly.io App

```bash
# Create a new app (if not already created)
fly apps create lms-be --org personal

# Set the app to the dev branch
fly deploy --build-arg GIT_BRANCH=dev
```

### 2. Configure Environment Variables

```bash
# Set database variables
fly secrets set MYSQL_HOST=your-mysql-host
fly secrets set MYSQL_USER=your-mysql-user
fly secrets set MYSQL_PASSWORD=your-mysql-password
fly secrets set MYSQL_DATABASE=ts_prod_new

# Set JWT variables
fly secrets set Jwt__SecretKey=your-jwt-secret-key
fly secrets set Jwt__ExpiryMinutes=60

# Set frontend URL
fly secrets set AppConfig__FEBaseURL=https://ts.techlign.com

# Set email variables (optional)
fly secrets set ZohoLogin__smtpUsername=your-smtp-username
fly secrets set ZohoLogin__smtpPassword=your-smtp-password
```

### 3. Deploy the Application

```bash
# Deploy from the dev branch
git checkout dev
fly deploy

# Or deploy with specific branch
fly deploy --build-arg GIT_BRANCH=dev
```

### 4. Verify Deployment

```bash
# Check app status
fly status

# View logs
fly logs

# Open the app in browser
fly open
```

## Configuration Files

### fly.toml
- Configures app name: `lms-be`
- Sets region to Amsterdam (ams)
- Internal port: 8080
- Shared CPU 1x, 256MB memory
- Health check endpoint: `/health`

### Dockerfile
- Multi-stage build for optimization
- Uses .NET 8 runtime
- Exposes port 8080 for Fly.io
- Sets development environment

## Database Setup

### Option 1: Fly.io Managed Postgres
```bash
# Create a Postgres cluster
fly postgres create --name lms-db --region ams --size shared-cpu-1x --vm-size shared-cpu-1x

# Attach to your app
fly postgres attach lms-db --app lms-be
```

### Option 2: External Database
Use your existing MySQL database by setting the appropriate environment variables.

## Troubleshooting

### Common Issues

1. **Build Failures**: Check that all .csproj files are correctly referenced
2. **Database Connection**: Verify database credentials and network access
3. **CORS Issues**: Ensure your frontend URL is in the allowed origins list
4. **Port Issues**: Make sure port 8080 is correctly configured

### Useful Commands

```bash
# View app logs
fly logs

# Check app status
fly status

# Open SSH session
fly ssh console

# Restart app
fly restart

# Scale app
fly scale count 1
```

## Monitoring

- Health checks are enabled at `/health`
- Logs are automatically collected by Fly.io
- Monitor metrics through the Fly.io dashboard

## Updates

To update your application:
1. Push changes to the dev branch
2. Run `fly deploy`
3. Monitor the deployment with `fly logs`
