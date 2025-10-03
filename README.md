# IDV Demo System - Ekwantu Consulting

**Identity Verification Demonstration System for Prudential Insurance**

## Project Overview

A full-stack Identity Verification demonstration system showcasing seamless integration between external ID sources and insurance product registration, with emphasis on modern UI/UX and real-time data verification capabilities.

## Technology Stack

### Backend
- .NET 8.0 Web API
- Entity Framework Core 8.0
- PostgreSQL Database
- JWT Authentication
- Swagger/OpenAPI documentation

### Frontend
- React 18+ with TypeScript
- Tailwind CSS + shadcn/ui components
- React Query for data fetching
- React Router for navigation

### DevOps
- Docker & Docker Compose
- Git version control

## Quick Start

### Prerequisites
- .NET 8.0 SDK
- Node.js 18+
- Docker Desktop (optional)
- PostgreSQL (if running without Docker)

### Backend Setup
```bash
cd backend
dotnet restore
dotnet ef database update
dotnet run --project IDV.API
```

### Frontend Setup
```bash
cd frontend/idv-demo-ui
npm install
npm start
```

### Docker Setup
```bash
docker-compose up --build
```

## Project Structure

```
IDV/
├── backend/
│   ├── IDV.API/          # Web API project
│   ├── IDV.Core/         # Domain models and interfaces
│   ├── IDV.Application/  # Business logic and DTOs
│   └── IDV.Infrastructure/ # Data access and repositories
├── frontend/
│   └── idv-demo-ui/      # React TypeScript application
├── docker-compose.yml
└── README.md
```

## Demo Features

- **ID Verification**: Real-time verification from mock external sources
- **Client Registration**: Seamless registration with product attachment
- **Product Management**: Comprehensive insurance product catalog
- **Reporting & Analytics**: Export capabilities and dashboard statistics
- **Audit Trail**: Complete user action logging

## Demo Users

- **Admin**: admin@ekwantu.com / Admin@123
- **Agent**: agent@ekwantu.com / Agent@123
- **Viewer**: viewer@ekwantu.com / Viewer@123

## API Documentation

Once the backend is running, visit:
- Swagger UI: https://localhost:5001/swagger
- API Base URL: https://localhost:5001/api

## Development Timeline

- **Phase 1**: Foundation & Setup (Days 1-2)
- **Phase 2**: Backend API Development (Days 3-7)
- **Phase 3**: Frontend Development (Days 8-13)
- **Phase 4**: Integration & Testing (Days 14-15)
- **Phase 5**: Deployment & Documentation (Days 16-17)

## Support

For questions or support, contact Ekwantu Consulting.

---

**Version**: 1.0  
**Last Updated**: October 2025  
**Status**: In Development