# Library Management System (LMS)

A comprehensive, multi-tenant library management system built with ASP.NET Core 8.0, designed to streamline library operations, manage book inventories, handle member services, and provide detailed analytics.

## 🌟 Features Overview

### 🏢 Multi-Tenant Architecture
- **Tenant Isolation**: Complete data separation between libraries
- **Customizable Settings**: Each tenant can configure their own policies
- **Scalable Design**: Support for unlimited number of libraries
- **Subdomain Support**: Unique subdomains for each library

### 👥 User Management
- **Role-Based Access Control**: Admin, Librarian, Member roles
- **User Profiles**: Comprehensive user information management
- **Activity Tracking**: Monitor user activities and login attempts
- **Preferences System**: Personalized user experience

### 📚 Book Catalog Management
- **Complete Book Information**: Title, ISBN, authors, publishers, genres
- **Advanced Search**: Full-text search with filters
- **Book Copies Management**: Track individual copies with barcodes
- **Cover Image Support**: Upload and manage book covers
- **Inventory Tracking**: Real-time stock management

### 👤 Member Services
- **Membership Management**: Different membership types with privileges
- **Member Profiles**: Detailed member information and history
- **Activity Monitoring**: Track member borrowing patterns
- **Suspension System**: Manage member privileges

### 📖 Loan Management
- **Checkout/Return**: Complete loan lifecycle management
- **Due Date Tracking**: Automated due date calculations
- **Renewal System**: Flexible loan renewal options
- **Overdue Management**: Track and manage overdue books
- **Damage/Loss Tracking**: Handle book damage and loss incidents

### 🔖 Reservation System
- **Queue Management**: First-come, first-served reservation queue
- **Automatic Notifications**: Notify members when books become available
- **Priority System**: Manage reservation priorities
- **Expiration Handling**: Automatic cleanup of expired reservations

### 💰 Fine Management
- **Automated Fine Calculation**: Calculate fines based on policies
- **Payment Tracking**: Record fine payments and methods
- **Fine Types**: Support for various fine categories
- **Waiver System**: Authority to waive fines when needed

### 📊 Reporting & Analytics
- **Circulation Reports**: Track book circulation patterns
- **Popular Books Reports**: Identify most borrowed books
- **Member Activity Reports**: Analyze member engagement
- **Financial Reports**: Track fine revenue and payments
- **Dashboard Analytics**: Real-time statistics and KPIs

### 🔔 Notification System
- **Due Date Reminders**: Automated reminders before due dates
- **Overdue Notices**: Notify members of overdue books
- **Reservation Alerts**: Alert when reserved books are available
- **System Notifications**: General system announcements

### 🔍 Advanced Search & Discovery
- **Full-Text Search**: Search across books, authors, members
- **Filter System**: Advanced filtering options
- **Search Suggestions**: Auto-complete and suggestions
- **Faceted Search**: Search by multiple criteria

### 📁 File Management
- **Document Upload**: Support for various file types
- **Book Cover Management**: Upload and manage book covers
- **Bulk Import**: Import books from CSV/Excel files
- **Data Export**: Export reports and data in various formats

---

## 🏗️ Architecture Overview

### **Technology Stack**
- **Backend**: ASP.NET Core 8.0 Web API
- **Database**: MySQL with Entity Framework Core
- **Authentication**: JWT Bearer Tokens
- **Architecture**: Clean Architecture with DDD principles
- **Multi-tenancy**: Tenant-per-database isolation

### **Project Structure**
```
src/
├── Domain/              # Core business logic and entities
│   ├── Entities/        # Domain entities
│   ├── Enums/          # Domain enums
│   └── Common/         # Base classes and interfaces
├── Application/         # Application services and DTOs
│   ├── Common/         # Shared application interfaces
│   ├── Features/       # Feature-specific services
│   └── DTOs/           # Data transfer objects
├── Infrastructure/      # External concerns
│   ├── Data/           # Database context and configurations
│   ├── Identity/       # Identity management
│   └── Services/       # External service implementations
└── Web/                # Web API project
    ├── Controllers/    # API controllers
    ├── Middleware/     # Custom middleware
    └── Configuration/  # Startup configuration
```

### **Domain Model**
The system follows Domain-Driven Design (DDD) principles with the following core entities:

#### **Core Entities**
- **Tenant**: Multi-tenant support with custom settings
- **User**: System users with roles and permissions
- **UserProfile**: Extended user information
- **UserPreference**: User-specific preferences

#### **Library Management Entities**
- **Book**: Book catalog information
- **Author**: Author information and biographies
- **Publisher**: Publisher details and contact information
- **Genre**: Book categories with hierarchical support
- **BookCopy**: Individual book copies with tracking
- **Member**: Library members with membership details
- **Loan**: Book loan transactions
- **Reservation**: Book reservation queue
- **Fine**: Fine calculation and payment tracking
- **LibrarySettings**: Tenant-specific library policies

#### **System Entities**
- **ActionHistory**: Comprehensive audit trail
- **Category**: System categorization
- **FileStore**: File management system

---

## 🔐 Security Features

### **Authentication & Authorization**
- **JWT Authentication**: Secure token-based authentication
- **Role-Based Access**: Different permissions for different roles
- **Multi-Factor Authentication**: Enhanced security options
- **Session Management**: Secure session handling

### **Data Protection**
- **Tenant Isolation**: Complete data separation
- **Data Encryption**: Sensitive data encryption
- **Audit Logging**: Comprehensive activity tracking
- **Input Validation**: Protection against injection attacks

### **Compliance**
- **GDPR Compliance**: Data privacy and protection
- **Data Retention**: Configurable data retention policies
- **Access Controls**: Granular access permissions
- **Privacy Settings**: User-controlled privacy options

---

## 🚀 Key Features Explained

### **Multi-Tenancy**
The system supports multiple libraries (tenants) with complete data isolation:
- Each tenant has its own database schema
- Customizable settings and policies per tenant
- Tenant-specific branding and configuration
- Secure data separation and access control

### **Book Management**
Comprehensive book catalog management:
- **Book Information**: Title, ISBN, description, publication details
- **Author Management**: Author profiles with biographies and photos
- **Publisher Information**: Publisher details and contact information
- **Genre Classification**: Hierarchical genre system with color coding
- **Copy Tracking**: Individual copy management with barcodes and conditions

### **Loan System**
Complete loan lifecycle management:
- **Checkout Process**: Quick and efficient book checkout
- **Due Date Management**: Automatic due date calculation based on policies
- **Renewal System**: Flexible renewal options with limits
- **Return Processing**: Easy return with condition assessment
- **Overdue Tracking**: Automated overdue detection and notification

### **Member Management**
Comprehensive member services:
- **Membership Types**: Different membership categories (Student, Faculty, Staff, Public)
- **Member Profiles**: Detailed member information and preferences
- **Activity Tracking**: Monitor borrowing patterns and history
- **Privilege Management**: Configurable borrowing limits and privileges
- **Suspension System**: Manage member access and privileges

### **Reservation System**
Efficient reservation management:
- **Queue Management**: First-come, first-served reservation queue
- **Priority Handling**: Manage reservation priorities based on membership
- **Automatic Notifications**: Notify members when books become available
- **Expiration Management**: Automatic cleanup of expired reservations
- **Fulfillment Tracking**: Track reservation fulfillment process

### **Fine Management**
Automated fine calculation and management:
- **Policy-Based Calculation**: Fines calculated based on library policies
- **Multiple Fine Types**: Support for late returns, damages, lost books
- **Payment Tracking**: Record payments with various methods
- **Waiver Authority**: Authorized fine waiver capabilities
- **Reporting**: Comprehensive fine revenue and payment reports

### **Analytics & Reporting**
Powerful analytics and reporting capabilities:
- **Circulation Reports**: Track book borrowing patterns
- **Popular Books**: Identify most popular books and authors
- **Member Analytics**: Analyze member engagement and activity
- **Financial Reports**: Track fine revenue and payment statistics
- **Custom Reports**: Flexible reporting with filters and date ranges

---

## 🎯 Use Cases

### **For Library Staff**
- **Quick Checkout**: Efficient book checkout process
- **Member Management**: Easy member registration and management
- **Inventory Management**: Track book availability and conditions
- **Fine Collection**: Efficient fine calculation and collection
- **Reporting**: Generate reports for management and stakeholders

### **For Library Members**
- **Book Search**: Easy book search and discovery
- **Account Management**: View borrowing history and current loans
- **Reservations**: Place reservations for unavailable books
- **Notifications**: Receive due date reminders and availability alerts
- **Fine Payment**: View and pay outstanding fines

### **For Library Administrators**
- **System Configuration**: Configure library policies and settings
- **User Management**: Manage staff accounts and permissions
- **Multi-Location Support**: Manage multiple library locations
- **Analytics**: Monitor library performance and usage patterns
- **Compliance**: Ensure data privacy and regulatory compliance

---

## 📱 Integration Capabilities

### **API Integration**
- **RESTful API**: Complete API for external integrations
- **Webhook Support**: Real-time event notifications
- **Third-Party Integration**: Support for external systems
- **Mobile App Support**: API optimized for mobile applications

### **Data Import/Export**
- **Bulk Import**: Import books and members from CSV/Excel
- **Data Export**: Export reports and data in various formats
- **Backup/Restore**: Complete system backup and restore
- **Migration Tools**: Data migration from other systems

---

## 🔧 Configuration & Customization

### **Library Settings**
- **Loan Policies**: Configure loan periods and renewal limits
- **Fine Policies**: Set fine amounts and grace periods
- **Notification Settings**: Configure email and SMS notifications
- **User Interface**: Customize branding and appearance

### **System Configuration**
- **Database Settings**: Configure database connections and pooling
- **Security Settings**: Configure authentication and authorization
- **Performance Settings**: Optimize system performance
- **Logging Configuration**: Configure logging levels and destinations

---

## 📈 Performance & Scalability

### **Performance Optimization**
- **Database Optimization**: Optimized queries and indexing
- **Caching Strategy**: Multi-level caching for improved performance
- **Connection Pooling**: Efficient database connection management
- **Async Processing**: Asynchronous operations for better responsiveness

### **Scalability Features**
- **Horizontal Scaling**: Support for multiple server instances
- **Load Balancing**: Distribute load across multiple servers
- **Database Scaling**: Support for database replication and sharding
- **Cloud Ready**: Designed for cloud deployment

---

## 🛠️ Development & Deployment

### **Development Environment**
- **Local Development**: Easy local development setup
- **Docker Support**: Containerized development environment
- **Testing Framework**: Comprehensive unit and integration tests
- **Documentation**: Complete API documentation and guides

### **Deployment Options**
- **On-Premises**: Traditional server deployment
- **Cloud Deployment**: AWS, Azure, Google Cloud support
- **Hybrid Deployment**: Mix of on-premises and cloud
- **Managed Services**: Fully managed hosting options

---

## 📋 Requirements

### **System Requirements**
- **.NET 8.0 Runtime**: Latest .NET runtime
- **MySQL 8.0+**: Database server
- **Redis (Optional)**: For caching and session management
- **Web Server**: IIS, Nginx, or Apache

### **Hardware Requirements**
- **Minimum**: 2 CPU cores, 4GB RAM, 50GB storage
- **Recommended**: 4 CPU cores, 8GB RAM, 100GB storage
- **Enterprise**: 8+ CPU cores, 16GB+ RAM, 500GB+ storage

---

## 🚀 Getting Started

### **Prerequisites**
- .NET 8.0 SDK
- MySQL 8.0+ or access to MySQL database
- Visual Studio 2022 or VS Code

### **Installation Steps**

1. **Clone the Repository**
   ```bash
   git clone https://github.com/your-username/lms-be.git
   cd lms-be
   ```

2. **Configure Database**
   - Update connection string in `src/Web/appsettings.json`
   - Ensure MySQL server is running

3. **Apply Database Migrations**
   ```bash
   cd src/Infrastructure
   dotnet ef database update
   ```

4. **Run the Application**
   ```bash
   cd src/Web
   dotnet run
   ```

5. **Access the Application**
   - API: https://localhost:5001
   - Swagger Documentation: https://localhost:5001/swagger

### **Database Migration Commands**

#### **Create a new migration**
```bash
Add-Migration MigrationName -Project src\Infrastructure -StartupProject src\Web -OutputDir Data/Migrations
```

#### **Apply migrations**
```bash
Update-Database -Project src\Infrastructure -StartupProject src\Web
```

#### **Remove last migration**
```bash
Remove-Migration -Project src\Infrastructure -StartupProject src\Web
```

---

## 🧪 Testing

### **Run All Tests**
```bash
dotnet test
```

### **Run Specific Test Project**
```bash
dotnet test src/Domain.Tests
dotnet test src/Application.Tests
dotnet test src/Infrastructure.Tests
dotnet test src/Web.Tests
```

---

## 📚 Documentation

- **API Documentation**: [API_DOCUMENTATION.md](API_DOCUMENTATION.md)
- **Entity Documentation**: Detailed entity relationships and configurations
- **Migration Guide**: Step-by-step migration instructions
- **Deployment Guide**: Production deployment instructions

---

## 🔧 Build & Run Commands

### **Build the Solution**
```bash
dotnet build -tl
```

### **Run in Development Mode**
```bash
cd src/Web
dotnet watch run
```

### **Run in Production Mode**
```bash
cd src/Web
dotnet run --environment Production
```

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🤝 Support & Community

### **Documentation**
- **API Documentation**: Complete API reference
- **User Guide**: Comprehensive user documentation
- **Developer Guide**: Development and customization guide
- **FAQ**: Frequently asked questions

### **Support Channels**
- **Email Support**: support@libraryms.com
- **Community Forum**: community.libraryms.com
- **Issue Tracker**: GitHub Issues
- **Knowledge Base**: knowledge.libraryms.com

---

## 🔄 Version History

### **Version 1.0.0** (Current)
- ✅ Initial release with core library management features
- ✅ Multi-tenant architecture support
- ✅ Complete API documentation
- ✅ Basic reporting and analytics
- ✅ Shadow property issues resolved
- ✅ Database migrations completed

### **Planned Features**
- 🔄 Mobile Application: Native mobile apps for iOS and Android
- 🔄 Advanced Analytics: Machine learning-powered recommendations
- 🔄 Integration Hub: Pre-built integrations with popular systems
- 🔄 Workflow Automation: Automated library workflows
- 🔄 AI Assistant: AI-powered library assistant

---

## 🏆 Project Highlights

### **Technical Excellence**
- ✅ Clean Architecture with DDD principles
- ✅ Multi-tenant database isolation
- ✅ Comprehensive audit trail with ActionHistory
- ✅ Entity Framework Core with MySQL
- ✅ JWT-based authentication
- ✅ Comprehensive error handling
- ✅ Performance optimizations

### **Business Features**
- ✅ Complete library management workflow
- ✅ Multi-tenant support for multiple libraries
- ✅ Advanced search and filtering
- ✅ Automated fine calculation
- ✅ Reservation queue management
- ✅ Reporting and analytics
- ✅ File management system

### **Quality Assurance**
- ✅ Zero shadow property warnings
- ✅ Clean database schema
- ✅ Comprehensive entity relationships
- ✅ Proper foreign key constraints
- ✅ Performance indexing
- ✅ Data integrity validation

---

*Built with ❤️ for libraries worldwide*
