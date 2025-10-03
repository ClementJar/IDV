# IDV System - Render Deployment Guide

This guide will help you deploy the IDV (Identity Verification) system to Render.com.

## Prerequisites

1. Git repository with your code: https://github.com/ClementJar/IDV.git
2. Render account: https://render.com
3. Project structure ready with Docker configurations

## Project Structure

```
IDV/
├── backend/                 # .NET Core API
│   ├── Dockerfile
│   ├── IDV.API/
│   ├── IDV.Application/
│   ├── IDV.Core/
│   └── IDV.Infrastructure/
├── frontend/               # React Frontend
│   ├── Dockerfile
│   ├── nginx.conf
│   └── idv-demo-ui/
└── render.yaml            # Render Blueprint
```

## Deployment Steps

### Step 1: Push Code to GitHub

1. Initialize git repository if not done:
```bash
cd C:\Users\CLEME\IdeaProjects\IDV
git init
git add .
git commit -m "Initial commit with Render deployment configuration"
git branch -M main
git remote add origin https://github.com/ClementJar/IDV.git
git push -u origin main
```

### Step 2: Deploy to Render

#### Option A: Using Render Blueprint (Recommended)

1. Go to https://render.com and sign in
2. Click "New" → "Blueprint"
3. Connect your GitHub repository: `https://github.com/ClementJar/IDV.git`
4. Render will automatically detect the `render.yaml` file
5. Click "Apply" to start the deployment
6. All services (Database, Backend API, Frontend) will be created automatically

#### Option B: Manual Service Creation

If you prefer to create services manually:

1. **Create PostgreSQL Database:**
   - Go to Render Dashboard → "New" → "PostgreSQL"
   - Name: `idv-postgres`
   - Database Name: `idv_database`
   - User: `idv_user`
   - Plan: Free

2. **Create Backend API Service:**
   - Go to Render Dashboard → "New" → "Web Service"
   - Connect GitHub repository
   - Runtime: Docker
   - Dockerfile Path: `./backend/Dockerfile`
   - Docker Context: `./backend`
   - Environment Variables:
     - `ASPNETCORE_ENVIRONMENT`: `Production`
     - `ASPNETCORE_URLS`: `http://+:10000`
     - `ConnectionStrings__DefaultConnection`: [Copy from PostgreSQL service]
     - `JwtSettings__SecretKey`: [Generate a secure random string]
     - `JwtSettings__Issuer`: `IDVSystem`
     - `JwtSettings__Audience`: `IDVClients`
     - `JwtSettings__ExpiryInHours`: `24`

3. **Create Frontend Service:**
   - Go to Render Dashboard → "New" → "Web Service"
   - Connect GitHub repository
   - Runtime: Docker
   - Dockerfile Path: `./frontend/Dockerfile`
   - Docker Context: `./frontend`
   - Environment Variables:
     - `REACT_APP_API_URL`: [URL of your backend service]/api
     - `NODE_ENV`: `production`

### Step 3: Configure Environment Variables

After deployment, you may need to update environment variables:

1. **Backend Service Environment Variables:**
   - `ConnectionStrings__DefaultConnection`: Should be automatically set from database
   - `JwtSettings__SecretKey`: Generate a secure 256-bit key
   - `AllowedOrigins__0`: URL of your frontend service

2. **Frontend Service Environment Variables:**
   - `REACT_APP_API_URL`: Should point to your backend service URL + `/api`

### Step 4: Test Deployment

1. **Check Backend Health:**
   - Visit: `https://your-backend-service.onrender.com/health`
   - Should return: `Healthy`

2. **Check Frontend:**
   - Visit: `https://your-frontend-service.onrender.com`
   - Should load the IDV application

3. **Test API Integration:**
   - Try logging in with default credentials: `admin` / `Admin@123`
   - Test ID verification functionality

## Configuration Files Explained

### render.yaml
- Defines all services and their relationships
- Automatically provisions database and connects services
- Sets up environment variables and health checks

### Backend Dockerfile
- Uses multi-stage builds for optimization
- Exposes port 10000 (required by Render)
- Includes health check endpoint

### Frontend Dockerfile
- Builds React app with production optimizations
- Uses Nginx to serve static files
- Configured for port 10000 with security headers

### nginx.conf
- Configured for Render's requirements
- Includes security headers
- Health check endpoint at `/health`
- Static asset caching

## Troubleshooting

### Common Issues:

1. **Build Failures:**
   - Check Dockerfile paths in render.yaml
   - Ensure all dependencies are properly defined
   - Check build logs in Render dashboard

2. **Database Connection Issues:**
   - Verify ConnectionStrings__DefaultConnection is set
   - Check if database service is running
   - Ensure database migrations run successfully

3. **CORS Issues:**
   - Update AllowedOrigins in backend configuration
   - Ensure frontend URL is whitelisted

4. **Environment Variables:**
   - Double-check all required environment variables are set
   - Ensure REACT_APP_API_URL points to correct backend URL

### Health Check URLs:
- Backend: `https://your-backend-service.onrender.com/health`
- Frontend: `https://your-frontend-service.onrender.com/health`

## Production Considerations

1. **Security:**
   - Use strong JWT secret keys
   - Enable HTTPS (handled by Render)
   - Review CORS policies

2. **Database:**
   - Consider upgrading to paid PostgreSQL plan for production
   - Set up regular backups
   - Monitor database performance

3. **Monitoring:**
   - Set up Render's monitoring features
   - Implement logging for debugging
   - Set up alerts for service health

## Support

If you encounter issues:
1. Check Render service logs
2. Verify all environment variables
3. Test locally with Docker first
4. Review Render documentation: https://render.com/docs

## Default Test Credentials

- **Admin Login:** `admin` / `Admin@123`
- **Test NRC IDs:** Available in the system for testing verification