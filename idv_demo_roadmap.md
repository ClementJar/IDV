# IDV Demo System - Development Roadmap
**Ekwantu Consulting - Prudential Insurance Demo**

## Executive Summary
Build a full-stack Identity Verification demonstration system showcasing seamless integration between external ID sources and insurance product registration, with emphasis on modern UI/UX and real-time data verification capabilities.

---

## 🎯 Project Objectives
1. Demonstrate ID verification capability from external sources
2. Showcase product attachment and client management
3. Illustrate reporting and data export functionality
4. Present modern, professional UI/UX aligned with enterprise standards
5. Complete development in **2-3 weeks** with 1 developer

---

## 📋 System Architecture Overview

### Technology Stack
**Backend:**
- .NET 8.0 Web API
- Entity Framework Core 8.0
- SQL Server / PostgreSQL
- JWT Authentication
- Swagger/OpenAPI documentation

**Frontend:**
- React 18+ with TypeScript
- Tailwind CSS + shadcn/ui components
- React Query for data fetching
- React Router for navigation
- Recharts for data visualization
- Lucide React for icons

**DevOps:**
- Docker & Docker Compose
- Git version control
- Postman collection for API testing

---

## 🏗️ Phase 1: Foundation & Setup (Days 1-2)

### 1.1 Project Initialization
**Backend Setup:**
```bash
# Create solution structure
dotnet new sln -n IDV
dotnet new webapi -n IDV.API
dotnet new classlib -n IDV.Core
dotnet new classlib -n IDV.Infrastructure
dotnet new classlib -n IDV.Application
```

**Required NuGet Packages:**
- Microsoft.EntityFrameworkCore.SqlServer (v8.0.x)
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Authentication.JwtBearer
- AutoMapper.Extensions.Microsoft.DependencyInjection
- FluentValidation.AspNetCore
- Serilog.AspNetCore
- Swashbuckle.AspNetCore
- EPPlus (for Excel export)
- iTextSharp / QuestPDF (for PDF export)

**Frontend Setup:**
```bash
npx create-react-app idv-demo-ui --template typescript
# Install dependencies
npm install @tanstack/react-query axios react-router-dom
npm install -D tailwindcss postcss autoprefixer
npm install lucide-react recharts date-fns
npm install @radix-ui/react-* (for shadcn components)
```

### 1.2 Database Design

**Tables to Create:**

**Users Table:**
```sql
- UserId (PK, GUID)
- Username (unique, nvarchar(50))
- Email (unique, nvarchar(100))
- PasswordHash (nvarchar(500))
- FullName (nvarchar(200))
- Role (nvarchar(50)) - Admin, Agent, Viewer
- IsActive (bit)
- CreatedAt (datetime2)
- LastLoginAt (datetime2)
```

**IDSourceClients Table (Mock External Source):**
```sql
- ClientId (PK, GUID)
- IDType (nvarchar(50)) - NationalID, Passport, DriversLicense
- IDNumber (unique, nvarchar(50))
- FullName (nvarchar(200))
- DateOfBirth (date)
- Gender (nvarchar(20))
- MobileNumber (nvarchar(20))
- Province (nvarchar(100))
- District (nvarchar(100))
- PostalCode (nvarchar(20))
- Source (nvarchar(100)) - INRIS, ZRA, MNO
- IsVerified (bit)
- CreatedAt (datetime2)
```

**RegisteredClients Table:**
```sql
- RegistrationId (PK, GUID)
- ClientId (FK to IDSourceClients)
- IDNumber (nvarchar(50))
- FullName (nvarchar(200))
- DateOfBirth (date)
- Gender (nvarchar(20))
- MobileNumber (nvarchar(20))
- Email (nvarchar(100))
- Province (nvarchar(100))
- District (nvarchar(100))
- PostalCode (nvarchar(20))
- RegisteredBy (FK to Users)
- RegistrationDate (datetime2)
- Status (nvarchar(50)) - Active, Pending, Suspended
- Notes (nvarchar(max))
```

**Products Table:**
```sql
- ProductId (PK, GUID)
- ProductCode (unique, nvarchar(50))
- ProductName (nvarchar(200))
- Category (nvarchar(100)) - Life, Health, Savings, Pension, Asset
- Description (nvarchar(max))
- PremiumAmount (decimal(18,2))
- Currency (nvarchar(10))
- IsActive (bit)
- CreatedAt (datetime2)
```

**ClientProducts Table (Junction):**
```sql
- ClientProductId (PK, GUID)
- RegistrationId (FK to RegisteredClients)
- ProductId (FK to Products)
- EnrollmentDate (datetime2)
- Status (nvarchar(50)) - Active, Lapsed, Cancelled
- PremiumAmount (decimal(18,2))
- PolicyNumber (nvarchar(100))
- StartDate (date)
- EndDate (date, nullable)
- Notes (nvarchar(max))
```

**AuditLog Table:**
```sql
- AuditId (PK, GUID)
- UserId (FK to Users)
- Action (nvarchar(100)) - Search, Register, Update, Export
- EntityType (nvarchar(100))
- EntityId (GUID)
- Details (nvarchar(max))
- IPAddress (nvarchar(50))
- Timestamp (datetime2)
```

**VerificationAttempts Table:**
```sql
- AttemptId (PK, GUID)
- UserId (FK to Users)
- IDNumber (nvarchar(50))
- SearchTimestamp (datetime2)
- ResultStatus (nvarchar(50)) - Found, NotFound, Multiple, Error
- ResultCount (int)
- ResponseTime (int) - milliseconds
- SourceSystem (nvarchar(100))
```

### 1.3 Seed Data Preparation

**Create 50+ Mock Client Records (Zambian context):**
- Mix of ID types: National ID (80%), Passport (15%), Driver's License (5%)
- Diverse provinces: Lusaka, Copperbelt, Eastern, Western, Southern, Northern, etc.
- Realistic Zambian names and phone numbers (+260)
- Various age groups and genders

**Create Product Catalog (20-25 products):**

**Life Insurance (6 products):**
1. Term Life Cover - Basic death benefit
2. Whole Life Assurance - Lifetime coverage with cash value
3. Endowment Policy - Savings + insurance
4. Unit-Linked Life Plan - Investment-linked
5. Family Protection Plan - Multiple beneficiaries
6. Funeral Cover - Immediate payout

**Health & Protection (5 products):**
1. Critical Illness Cover - Cancer, heart attack, stroke
2. Personal Accident Plan - Disability/death from accidents
3. Income Protection - Salary replacement
4. Hospital Cash Plan - Daily hospital allowance
5. Comprehensive Medical Cover - Full hospitalization

**Savings & Investment (5 products):**
1. Smart Save Plan - Guaranteed returns + bonus
2. Education Endowment - Child education fund
3. Balanced Investment Fund - Mixed portfolio
4. Equity Growth Fund - Stock market exposure
5. Fixed Income Plan - Stable returns

**Pensions/Retirement (4 products):**
1. Individual Retirement Account - Personal pension
2. Group Pension Scheme - Employer-sponsored
3. Immediate Annuity - Convert lump sum to income
4. Deferred Annuity - Future retirement income

**Asset Management (3 products):**
1. Property Investment Fund - Real estate portfolio
2. Money Market Fund - Short-term investments
3. Aggressive Growth Portfolio - High-risk/high-return

**Demo Users:**
- Admin: admin@ekwantu.com / Admin@123
- Agent: agent@ekwantu.com / Agent@123
- Viewer: viewer@ekwantu.com / Viewer@123

---

## 🔧 Phase 2: Backend API Development (Days 3-7)

### 2.1 Core Models & DTOs (Day 3)

**Create Domain Models in IDV.Core:**
- User.cs
- IDSourceClient.cs
- RegisteredClient.cs
- Product.cs
- ClientProduct.cs
- AuditLog.cs
- VerificationAttempt.cs

**Create DTOs in IDV.Application:**

**Authentication:**
- LoginRequestDto
- LoginResponseDto
- TokenDto

**ID Verification:**
- IDVerificationRequestDto
- IDVerificationResponseDto (matching document format)
- ClientSearchResultDto
- MultipleRecordsResponseDto

**Client Registration:**
- RegisterClientRequestDto
- RegisterClientResponseDto
- ClientDetailsDto
- UpdateClientDto

**Product Management:**
- ProductDto
- AttachProductRequestDto
- ClientProductDto

**Reporting:**
- ClientReportDto
- ExportRequestDto
- ExportResponseDto

### 2.2 Repository Pattern Implementation (Day 3-4)

**Interfaces (IDV.Core/Interfaces):**
- IUserRepository
- IIDSourceRepository
- IRegisteredClientRepository
- IProductRepository
- IClientProductRepository
- IAuditLogRepository
- IUnitOfWork

**Implementations (IDV.Infrastructure/Repositories):**
- Implement all repository interfaces
- Generic Repository<T> base class
- UnitOfWork implementation

### 2.3 Business Logic Layer (Day 4-5)

**Services (IDV.Application/Services):**

**IAuthenticationService:**
- Login(LoginRequestDto)
- ValidateToken(string token)
- RefreshToken(string refreshToken)
- GetCurrentUser()

**IIDVerificationService:**
- VerifyIDNumber(string idNumber) - Returns IDVerificationResponseDto
- SearchMultipleSources(string idNumber)
- LogVerificationAttempt()
- Simulate delay (200-500ms) for realism

**IClientRegistrationService:**
- RegisterNewClient(RegisterClientRequestDto)
- GetClientDetails(Guid registrationId)
- UpdateClientInfo(UpdateClientDto)
- GetAllRegisteredClients(filters, pagination)
- SearchRegisteredClients(string searchTerm)

**IProductService:**
- GetAllProducts(string category)
- GetProductById(Guid productId)
- AttachProductToClient(AttachProductRequestDto)
- RemoveProductFromClient(Guid clientProductId)
- GetClientProducts(Guid registrationId)
- UpdateClientProduct(UpdateClientProductDto)

**IReportingService:**
- GenerateClientReport(filters)
- ExportToExcel(exportRequest)
- ExportToPDF(exportRequest)
- GetDashboardStatistics()

**IAuditService:**
- LogAction(userId, action, details)
- GetAuditTrail(filters)

### 2.4 API Controllers (Day 5-6)

**Controllers to Create:**

**AuthController:**
```
POST /api/auth/login
POST /api/auth/refresh
GET /api/auth/me
POST /api/auth/logout
```

**IDVerificationController:**
```
GET /api/verification/{idNumber}
POST /api/verification/batch
GET /api/verification/history
```

**ClientsController:**
```
POST /api/clients/register
GET /api/clients
GET /api/clients/{id}
PUT /api/clients/{id}
DELETE /api/clients/{id}
GET /api/clients/search?q={term}
```

**ProductsController:**
```
GET /api/products
GET /api/products/{id}
GET /api/products/categories
POST /api/clients/{clientId}/products
DELETE /api/clients/{clientId}/products/{productId}
GET /api/clients/{clientId}/products
```

**ReportsController:**
```
GET /api/reports/clients
POST /api/reports/export/excel
POST /api/reports/export/pdf
GET /api/reports/statistics
GET /api/reports/audit-trail
```

### 2.5 Security & Middleware (Day 6-7)

**Implement:**
- JWT token generation and validation
- Role-based authorization [Authorize(Roles = "Admin")]
- Request/Response logging middleware
- Exception handling middleware
- CORS configuration
- Rate limiting (optional but impressive)
- API versioning

**appsettings.json Configuration:**
```json
{
  "JwtSettings": {
    "SecretKey": "Your-256-bit-secret-key-here",
    "Issuer": "IDV",
    "Audience": "IDV.Client",
    "ExpirationMinutes": 60
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=IDV;..."
  },
  "AllowedOrigins": ["http://localhost:3000"]
}
```

### 2.6 API Testing & Documentation (Day 7)

**Create:**
- Swagger configuration with JWT authorization
- Postman collection with all endpoints
- Sample requests/responses for each endpoint
- API documentation markdown

---

## 🎨 Phase 3: Frontend Development (Days 8-13)

### 3.1 Project Structure & Setup (Day 8)

**Folder Structure:**
```
src/
├── assets/
├── components/
│   ├── common/
│   │   ├── Button.tsx
│   │   ├── Input.tsx
│   │   ├── Card.tsx
│   │   ├── Modal.tsx
│   │   ├── Table.tsx
│   │   ├── Loader.tsx
│   │   └── Badge.tsx
│   ├── layout/
│   │   ├── Navbar.tsx
│   │   ├── Sidebar.tsx
│   │   └── Layout.tsx
│   ├── auth/
│   │   └── LoginForm.tsx
│   ├── verification/
│   │   ├── SearchBar.tsx
│   │   └── ResultCard.tsx
│   ├── clients/
│   │   ├── ClientList.tsx
│   │   ├── ClientDetails.tsx
│   │   ├── RegisterClient.tsx
│   │   └── ClientCard.tsx
│   ├── products/
│   │   ├── ProductList.tsx
│   │   ├── ProductCard.tsx
│   │   └── AttachProduct.tsx
│   └── reports/
│       ├── ReportFilters.tsx
│       ├── StatisticsCards.tsx
│       └── ExportButtons.tsx
├── services/
│   ├── api.ts
│   ├── authService.ts
│   ├── verificationService.ts
│   ├── clientService.ts
│   ├── productService.ts
│   └── reportService.ts
├── hooks/
│   ├── useAuth.ts
│   ├── useClients.ts
│   └── useProducts.ts
├── contexts/
│   └── AuthContext.tsx
├── types/
│   └── index.ts
├── utils/
│   ├── formatters.ts
│   └── validators.ts
├── pages/
│   ├── Login.tsx
│   ├── Dashboard.tsx
│   ├── Verification.tsx
│   ├── ClientRegistration.tsx
│   ├── ClientManagement.tsx
│   └── Reports.tsx
└── App.tsx
```

**Install & Configure shadcn/ui:**
```bash
npx shadcn-ui@latest init
npx shadcn-ui@latest add button input card table badge dialog
npx shadcn-ui@latest add select dropdown-menu avatar
```

### 3.2 Authentication & Layout (Day 8-9)

**Create Authentication Flow:**
- Login page with modern design
- JWT token storage in memory (NOT localStorage)
- AuthContext for user state management
- Protected routes wrapper
- Automatic token refresh
- Logout functionality

**Design System Setup:**
- Define color palette (professional blues, greens)
- Typography scale
- Spacing system
- Shadow utilities
- Animation configurations

**Layout Components:**
- Responsive sidebar with navigation
- Top navbar with user profile
- Breadcrumb navigation
- Notification system (toast messages)

### 3.3 ID Verification Interface (Day 9-10)

**Verification Page Features:**
- Prominent search bar with ID type selector
- Real-time validation as user types
- Loading states with skeleton screens
- Result display cards matching API response
- Error states with helpful messages
- "Register Client" quick action button

**Design Elements:**
- Animated search icon
- Smooth transitions
- Color-coded result status (green = found, red = not found, yellow = multiple)
- Responsive card layouts
- Copy-to-clipboard for details

### 3.4 Client Registration (Day 10-11)

**Registration Form:**
- Multi-step wizard or single-page form
- Auto-populate from verification results
- Form validation with instant feedback
- Product selection with checkboxes/cards
- Image placeholders for product icons
- Premium amount display
- Confirmation modal before submission
- Success animation after registration

**Form Fields:**
- All IDSourceClient fields (read-only if from verification)
- Email address
- Additional contact info
- Product selection with search/filter
- Policy start date picker
- Notes/comments field

**UX Enhancements:**
- Progress indicator for multi-step
- Inline error messages
- Auto-save to session state
- "Save as draft" option
- Clear/reset functionality

### 3.5 Client Management (Day 11-12)

**Client List View:**
- Responsive data table with pagination
- Search/filter by name, ID number, province
- Sort by registration date, name, status
- Status badges (Active, Pending, Suspended)
- Quick actions (View, Edit, Delete)
- Bulk actions (Export selected)
- Empty state with illustration

**Client Details View:**
- Profile header with client info
- Product cards showing attached products
- Policy details expandable sections
- Timeline of activities
- Edit mode toggle
- Add/remove products
- Audit trail section

**Design Features:**
- Card-based layout
- Gradient backgrounds
- Icons from Lucide React
- Hover effects and transitions
- Modal dialogs for confirmations
- Success/error toast notifications

### 3.6 Product Management (Day 12)

**Product Display:**
- Grid layout of product cards
- Category filtering tabs
- Search functionality
- Product details modal
- Pricing information
- Feature highlights
- "Attach to Client" action

**Product Card Design:**
- Category icon
- Product name and code
- Short description
- Premium amount
- Status indicator
- Hover animations

### 3.7 Reports & Analytics (Day 12-13)

**Dashboard/Reports Page:**
- Statistics cards (Total Clients, Active Policies, Revenue)
- Charts using Recharts:
  - Registrations over time (Line chart)
  - Products by category (Pie chart)
  - Top provinces (Bar chart)
- Recent activity feed
- Quick filters (date range, province, product category)

**Export Functionality:**
- Export buttons (Excel, PDF)
- Loading state during export
- Download progress indicator
- Success confirmation
- Custom report filters modal

**Visual Polish:**
- Animated number counters
- Gradient cards for statistics
- Responsive chart sizing
- Interactive tooltips
- Professional color schemes

### 3.8 Responsive Design & Polish (Day 13)

**Ensure:**
- Mobile-first responsive design
- Tablet breakpoint optimization
- Touch-friendly buttons and inputs
- Hamburger menu for mobile sidebar
- Optimized images and assets
- Loading states everywhere
- Error boundaries
- 404 page
- Smooth page transitions

---

## 🧪 Phase 4: Integration & Testing (Days 14-15)

### 4.1 Backend Testing (Day 14)

**Unit Tests:**
- Service layer tests for all business logic
- Repository tests with in-memory database
- Validator tests
- DTO mapping tests

**Integration Tests:**
- API endpoint tests
- Database integration tests
- Authentication flow tests

### 4.2 Frontend Testing (Day 14)

**Component Tests:**
- Key component rendering tests
- Form validation tests
- User interaction tests

**E2E Testing (Optional but Recommended):**
- Login flow
- Complete registration workflow
- Search and view flow
- Export functionality

### 4.3 Integration Testing (Day 15)

**End-to-End Scenarios:**
1. User login → Search ID → Register client → Attach products → View details
2. User login → View all clients → Export report
3. Multiple record handling
4. Error scenarios (not found, server error)

**Performance Testing:**
- API response times
- Frontend load times
- Large dataset handling
- Export performance

---

## 📦 Phase 5: Deployment & Documentation (Days 16-17)

### 5.1 Docker Configuration (Day 16)

**Create Dockerfile for API:**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Build steps
```

**Create Dockerfile for Frontend:**
```dockerfile
FROM node:18-alpine AS build
FROM nginx:alpine
# Build and serve steps
```

**Docker Compose:**
```yaml
version: '3.8'
services:
  db:
    image: postgres:15
  api:
    build: ./backend
    ports: ["5000:80"]
  web:
    build: ./frontend
    ports: ["3000:80"]
```

### 5.2 Documentation (Day 16-17)

**Create:**
1. **README.md** - Project overview, setup instructions
2. **API_DOCUMENTATION.md** - Endpoint details, examples
3. **USER_GUIDE.md** - How to use the demo
4. **ARCHITECTURE.md** - System design, diagrams
5. **DEPLOYMENT.md** - Deployment steps
6. **DEMO_SCRIPT.md** - Walkthrough for presentation

**Demo Data Script:**
- SQL scripts for seed data
- Instructions to reset demo data
- Sample IDs for testing

### 5.3 Demo Preparation (Day 17)

**Prepare:**
- Clean, seeded database
- Test all workflows
- Prepare sample search IDs
- Create demo user accounts
- Test on different browsers
- Prepare backup plan for live demo
- Screen recording of full workflow

---

## 📊 Success Criteria Checklist

### Backend:
- [ ] All API endpoints functional
- [ ] JWT authentication working
- [ ] Database relationships correct
- [ ] Seed data populated
- [ ] Swagger documentation complete
- [ ] Error handling robust
- [ ] Logging implemented
- [ ] Export functions working

### Frontend:
- [ ] Responsive on all screen sizes
- [ ] Modern, professional design
- [ ] All pages functional
- [ ] Smooth animations and transitions
- [ ] Loading states everywhere
- [ ] Error handling with user-friendly messages
- [ ] Form validations working
- [ ] Search functionality fast and accurate
- [ ] Export downloads working

### Integration:
- [ ] Frontend-Backend communication seamless
- [ ] Authentication flow complete
- [ ] Data flow correct (Search → Register → View)
- [ ] Product attachment working
- [ ] Reports generating correctly
- [ ] Audit trail capturing actions

---

## 🚀 Quick Start Commands

### Backend:
```bash
cd IDV.API
dotnet ef database update
dotnet run
# API runs on https://localhost:5001
```

### Frontend:
```bash
cd idv-demo-ui
npm install
npm start
# UI runs on http://localhost:3000
```

### Docker:
```bash
docker-compose up --build
```

---

## 🎯 Demo Flow Script

**5-Minute Demo Walkthrough:**

1. **Login** (30 seconds)
   - Show secure authentication
   - Explain role-based access

2. **ID Verification** (1 minute)
   - Search for national ID: 150585/10/1 (Zambian NRC format)
   - Show instant verification result
   - Highlight data source integration

3. **Client Registration** (1.5 minutes)
   - Register client from verified data
   - Attach 2-3 products (Life + Health + Pension)
   - Show premium calculations
   - Complete registration

4. **Client Management** (1 minute)
   - Navigate to client list
   - Show newly registered client
   - View detailed client profile
   - Display attached products

5. **Reporting** (1 minute)
   - Show dashboard statistics
   - Export client report to Excel
   - Download and open file

**Key Talking Points:**
- Integration with external ID sources (INRIS, ZRA, MNO)
- Real-time verification (< 500ms response)
- Scalable microservice architecture
- Comprehensive audit trail
- Modern, intuitive user interface
- Multi-product support
- Flexible deployment (cloud/on-premise)

---

## 📞 Support & Contacts

**Developer Resources:**
- .NET Documentation: https://docs.microsoft.com/dotnet
- React Documentation: https://react.dev
- Tailwind CSS: https://tailwindcss.com
- shadcn/ui: https://ui.shadcn.com

**Project Timeline:** 17 working days (3.4 weeks)
**Recommended Team:** 1 Full-stack Developer + 1 Designer (optional)

---

## 🔄 Optional Enhancements (If Time Permits)

- Real-time notifications (SignalR)
- Advanced analytics dashboard
- Biometric integration mockup
- Multi-language support
- Dark mode toggle
- Progressive Web App (PWA) features
- Offline capability
- WebSocket for real-time updates
- Advanced filtering and sorting
- Bulk import functionality
- Email notifications mockup
- SMS alerts integration mockup

---

**Document Version:** 1.0  
**Last Updated:** June 2025  
**Prepared By:** Ekwantu Consulting  
**Status:** Ready for Development