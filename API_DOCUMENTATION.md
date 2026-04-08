# Library Management System API Documentation

## Overview
This document provides a comprehensive list of all APIs available in the Library Management System. The system is built with ASP.NET Core and supports multi-tenant architecture.

## Base URL
```
https://your-domain.com/api
```

## Authentication
All API endpoints (except authentication endpoints) require JWT Bearer token in the Authorization header:
```
Authorization: Bearer {your-jwt-token}
```

---

## 🔐 Authentication & Authorization APIs

### Login
```
POST   /api/auth/login
```
**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123"
}
```

### Register
```
POST   /api/auth/register
```
**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "password123",
  "firstName": "John",
  "lastName": "Doe"
}
```

### Logout
```
POST   /api/auth/logout
```

### Refresh Token
```
POST   /api/auth/refresh-token
```

### Forgot Password
```
POST   /api/auth/forgot-password
```

### Reset Password
```
POST   /api/auth/reset-password
```

### Get Profile
```
GET    /api/auth/profile
```

### Update Profile
```
PUT    /api/auth/profile
```

### Change Password
```
POST   /api/auth/change-password
```

### Get Permissions
```
GET    /api/auth/permissions
```

---

## 🏢 Tenant Management APIs

### Get All Tenants
```
GET    /api/tenants
```

### Create Tenant
```
POST   /api/tenants
```
**Request Body:**
```json
{
  "name": "City Library",
  "subdomain": "citylibrary",
  "logoUrl": "https://example.com/logo.png",
  "maxUsers": 100
}
```

### Get Tenant by ID
```
GET    /api/tenants/{id}
```

### Update Tenant
```
PUT    /api/tenants/{id}
```

### Delete Tenant
```
DELETE /api/tenants/{id}
```

### Get Tenant Settings
```
GET    /api/tenants/{id}/settings
```

### Update Tenant Settings
```
PUT    /api/tenants/{id}/settings
```

---

## 👥 User Management APIs

### Get All Users
```
GET    /api/users?page=1&pageSize=20&search=john&isActive=true
```

### Create User
```
POST   /api/users
```

### Get User by ID
```
GET    /api/users/{id}
```

### Update User
```
PUT    /api/users/{id}
```

### Delete User
```
DELETE /api/users/{id}
```

### Activate User
```
POST   /api/users/{id}/activate
```

### Deactivate User
```
POST   /api/users/{id}/deactivate
```

### Get User Profile
```
GET    /api/users/{id}/profile
```

### Update User Profile
```
PUT    /api/users/{id}/profile
```

### Get User Preferences
```
GET    /api/users/{id}/preferences
```

### Update User Preferences
```
PUT    /api/users/{id}/preferences
```

---

## 📚 Book Catalog APIs

### Get All Books
```
GET    /api/books?page=1&pageSize=20&search=title&genreId=1&authorId=1
```

### Create Book
```
POST   /api/books
```
**Request Body:**
```json
{
  "title": "The Great Book",
  "isbn": "978-0123456789",
  "description": "A great book about...",
  "publicationDate": "2023-01-01",
  "pageCount": 250,
  "language": "English",
  "format": "Hardcover",
  "publisherId": 1,
  "authorIds": [1, 2],
  "genreIds": [1, 3]
}
```

### Get Book by ID
```
GET    /api/books/{id}
```

### Update Book
```
PUT    /api/books/{id}
```

### Delete Book
```
DELETE /api/books/{id}
```

### Get Book Copies
```
GET    /api/books/{id}/copies
```

### Add Book Copy
```
POST   /api/books/{id}/copies
```
**Request Body:**
```json
{
  "barcode": "BC001",
  "condition": "Good",
  "location": "A1-B2"
}
```

### Check Book Availability
```
GET    /api/books/{id}/availability
```

### Search Books
```
GET    /api/books/search?q=javascript&genreId=1&authorId=1
```

### Get Book History
```
GET    /api/books/{id}/history
```

### Upload Book Cover
```
POST   /api/books/{id}/upload-cover
```

---

## ✍️ Author & Publisher APIs

### Get All Authors
```
GET    /api/authors?page=1&pageSize=20&search=name
```

### Create Author
```
POST   /api/authors
```
**Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "biography": "Author biography...",
  "birthDate": "1980-01-01",
  "nationality": "American"
}
```

### Get Author by ID
```
GET    /api/authors/{id}
```

### Update Author
```
PUT    /api/authors/{id}
```

### Delete Author
```
DELETE /api/authors/{id}
```

### Get All Publishers
```
GET    /api/publishers
```

### Create Publisher
```
POST   /api/publishers
```
**Request Body:**
```json
{
  "name": "Publisher Name",
  "address": "123 Main St",
  "city": "New York",
  "country": "USA",
  "phone": "+1-555-0123",
  "email": "contact@publisher.com"
}
```

### Get Publisher by ID
```
GET    /api/publishers/{id}
```

### Update Publisher
```
PUT    /api/publishers/{id}
```

### Delete Publisher
```
DELETE /api/publishers/{id}
```

### Get All Genres
```
GET    /api/genres
```

### Create Genre
```
POST   /api/genres
```
**Request Body:**
```json
{
  "name": "Science Fiction",
  "description": "Fictional stories about science and technology",
  "colorCode": "#FF5733",
  "parentGenreId": null
}
```

### Get Genre by ID
```
GET    /api/genres/{id}
```

### Update Genre
```
PUT    /api/genres/{id}
```

### Delete Genre
```
DELETE /api/genres/{id}
```

---

## 👤 Member Management APIs

### Get All Members
```
GET    /api/members?page=1&pageSize=20&search=john&membershipType=Student
```

### Create Member
```
POST   /api/members
```
**Request Body:**
```json
{
  "userId": 123,
  "membershipNumber": "MEM001",
  "membershipType": "Student",
  "membershipStartDate": "2023-01-01",
  "maxBooksAllowed": 5,
  "loanPeriodDays": 14
}
```

### Get Member by ID
```
GET    /api/members/{id}
```

### Update Member
```
PUT    /api/members/{id}
```

### Delete Member
```
DELETE /api/members/{id}
```

### Get Member Loans
```
GET    /api/members/{id}/loans
```

### Get Member Reservations
```
GET    /api/members/{id}/reservations
```

### Get Member Fines
```
GET    /api/members/{id}/fines
```

### Suspend Member
```
POST   /api/members/{id}/suspend
```

### Unsuspend Member
```
POST   /api/members/{id}/unsuspend
```

### Get Member Statistics
```
GET    /api/members/{id}/statistics
```

---

## 📖 Loan Management APIs

### Get All Loans
```
GET    /api/loans?page=1&pageSize=20&status=Active&memberId=123
```

### Checkout Book
```
POST   /api/loans/checkout
```
**Request Body:**
```json
{
  "bookCopyId": 1,
  "memberId": 123,
  "dueDate": "2023-02-15",
  "notes": "Standard checkout"
}
```

### Get Loan by ID
```
GET    /api/loans/{id}
```

### Return Book
```
PUT    /api/loans/{id}/return
```
**Request Body:**
```json
{
  "returnDate": "2023-02-10",
  "condition": "Good",
  "notes": "Returned in good condition"
}
```

### Renew Loan
```
PUT    /api/loans/{id}/renew
```

### Get Active Loans
```
GET    /api/loans/active
```

### Get Overdue Loans
```
GET    /api/loans/overdue
```

### Get Loan History
```
GET    /api/loans/history?memberId=123&startDate=2023-01-01&endDate=2023-12-31
```

### Mark Book as Damaged
```
POST   /api/loans/{id}/mark-damaged
```

### Mark Book as Lost
```
POST   /api/loans/{id}/mark-lost
```

### Get Loan Statistics
```
GET    /api/loans/statistics?period=monthly&year=2023
```

---

## 🔖 Reservation APIs

### Get All Reservations
```
GET    /api/reservations?page=1&pageSize=20&status=Active
```

### Create Reservation
```
POST   /api/reservations
```
**Request Body:**
```json
{
  "bookId": 1,
  "memberId": 123,
  "notes": "Reserve for research"
}
```

### Get Reservation by ID
```
GET    /api/reservations/{id}
```

### Cancel Reservation
```
PUT    /api/reservations/{id}/cancel
```

### Get Reservation Queue
```
GET    /api/reservations/queue/{bookId}
```

### Fulfill Reservation
```
PUT    /api/reservations/{id}/fulfill
```

### Get Active Reservations
```
GET    /api/reservations/active
```

### Get Expired Reservations
```
GET    /api/reservations/expired
```

### Get Member Reservations
```
GET    /api/reservations/member/{memberId}
```

---

## 💰 Fine Management APIs

### Get All Fines
```
GET    /api/fines?page=1&pageSize=20&status=Unpaid&memberId=123
```

### Create Fine
```
POST   /api/fines
```
**Request Body:**
```json
{
  "loanId": 1,
  "memberId": 123,
  "amount": 5.00,
  "fineType": "LateReturn",
  "description": "Late return fee"
}
```

### Get Fine by ID
```
GET    /api/fines/{id}
```

### Pay Fine
```
PUT    /api/fines/{id}/pay
```
**Request Body:**
```json
{
  "amountPaid": 5.00,
  "paymentMethod": "Cash",
  "paymentReference": "PAY001"
}
```

### Get Member Fines
```
GET    /api/fines/member/{memberId}
```

### Get Unpaid Fines
```
GET    /api/fines/unpaid
```

### Waive Fine
```
POST   /api/fines/{id}/waive
```

### Get Fine Statistics
```
GET    /api/fines/statistics?period=monthly&year=2023
```

### Get Fine Summary
```
GET    /api/fines/summary
```

---

## ⚙️ Library Settings APIs

### Get Library Settings
```
GET    /api/settings
```

### Update Library Settings
```
PUT    /api/settings
```
**Request Body:**
```json
{
  "defaultLoanPeriodDays": 14,
  "maxRenewalsAllowed": 2,
  "maxBooksPerMember": 5,
  "lateFeePerDay": 0.50,
  "maxLateFee": 10.00,
  "maxReservationsPerMember": 3,
  "sendDueDateReminders": true,
  "dueDateReminderDays": 2
}
```

### Get Loan Policies
```
GET    /api/settings/loan-policies
```

### Update Loan Policies
```
PUT    /api/settings/loan-policies
```

### Get Fine Policies
```
GET    /api/settings/fine-policies
```

### Update Fine Policies
```
PUT    /api/settings/fine-policies
```

### Get Notification Settings
```
GET    /api/settings/notification-settings
```

### Update Notification Settings
```
PUT    /api/settings/notification-settings
```

---

## 📊 Reporting & Analytics APIs

### Circulation Report
```
GET    /api/reports/circulation?startDate=2023-01-01&endDate=2023-12-31
```

### Popular Books Report
```
GET    /api/reports/popular-books?period=monthly&limit=10
```

### Member Activity Report
```
GET    /api/reports/member-activity?memberId=123&period=yearly
```

### Fine Revenue Report
```
GET    /api/reports/fine-revenue?period=monthly&year=2023
```

### Overdue Books Report
```
GET    /api/reports/overdue-books?days=30
```

### Inventory Report
```
GET    /api/reports/inventory?category=all
```

### Dashboard Analytics
```
GET    /api/analytics/dashboard
```

### Trend Analytics
```
GET    /api/analytics/trends?metric=loans&period=6months
```

---

## 📁 File Management APIs

### Upload File
```
POST   /api/files/upload
```

### Get File by ID
```
GET    /api/files/{id}
```

### Delete File
```
DELETE /api/files/{id}
```

### Get Book Covers
```
GET    /api/files/book-covers
```

### Import Books
```
POST   /api/files/import-books
```

### Export Books
```
GET    /api/files/export-books?format=csv
```

---

## 🔔 Notification APIs

### Get Notifications
```
GET    /api/notifications?page=1&pageSize=20&isRead=false
```

### Send Notification
```
POST   /api/notifications/send
```

### Mark Notification as Read
```
PUT    /api/notifications/{id}/mark-read
```

### Get Unread Notifications
```
GET    /api/notifications/unread
```

### Send Due Date Reminders
```
POST   /api/notifications/due-date-reminders
```

### Send Overdue Notices
```
POST   /api/notifications/overdue-notices
```

---

## 🔍 Search & Discovery APIs

### Search Books
```
GET    /api/search/books?q=javascript&filters=genre:Fiction,author:John&sort=title
```

### Search Authors
```
GET    /api/search/authors?q=john
```

### Search Members
```
GET    /api/search/members?q=john&field=name|email|membershipNumber
```

### Get Search Suggestions
```
GET    /api/search/suggestions?q=java&type=books
```

### Advanced Search
```
GET    /api/search/advanced
```
**Request Body:**
```json
{
  "title": "JavaScript",
  "author": "John Doe",
  "isbn": "978-0123456789",
  "genreIds": [1, 2],
  "publisherId": 1,
  "publicationYear": 2023,
  "language": "English"
}
```

### Get Search Filters
```
GET    /api/search/filters
```

---

## 📱 Mobile/External APIs

### Mobile Dashboard
```
GET    /api/mobile/dashboard
```

### Get Member Loans (Mobile)
```
GET    /api/mobile/member-loans/{memberId}
```

### Scan Book Barcode
```
POST   /api/mobile/scan-book
```
**Request Body:**
```json
{
  "barcode": "BC001234"
}
```

### Catalog Sync
```
GET    /api/external/catalog-sync
```

### Import External Data
```
POST   /api/external/import-data
```

---

## 🚨 Error Responses

All APIs return consistent error responses:

```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Invalid input data",
    "details": [
      {
        "field": "email",
        "message": "Email is required"
      }
    ]
  }
}
```

### Common HTTP Status Codes
- `200` - Success
- `201` - Created
- `400` - Bad Request
- `401` - Unauthorized
- `403` - Forbidden
- `404` - Not Found
- `409` - Conflict
- `500` - Internal Server Error

---

## 📝 Response Formats

### Success Response (Single Item)
```json
{
  "success": true,
  "data": {
    "id": 1,
    "title": "Book Title",
    "isbn": "978-0123456789"
  }
}
```

### Success Response (List)
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "title": "Book Title",
      "isbn": "978-0123456789"
    }
  ],
  "pagination": {
    "page": 1,
    "pageSize": 20,
    "totalItems": 100,
    "totalPages": 5
  }
}
```

---

## 🔄 API Versioning

The API supports versioning through URL segments:
```
/api/v1/books
/api/v2/books
```

Current version: `v1`

---

## 📚 Implementation Priority

### Phase 1 (Core Features)
1. Authentication & Authorization
2. Tenant Management
3. User Management
4. Book Catalog
5. Member Management
6. Loan Management

### Phase 2 (Essential Features)
7. Author & Publisher Management
8. Reservation System
9. Fine Management
10. Search & Discovery

### Phase 3 (Advanced Features)
11. Library Settings
12. Reporting & Analytics
13. File Management
14. Notifications
15. Mobile/External APIs

---

## 📞 Support

For API support and questions:
- Email: api-support@yourlibrary.com
- Documentation: https://docs.yourlibrary.com
- Status Page: https://status.yourlibrary.com

---

*Last Updated: April 8, 2026*
