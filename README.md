# Employee Management System
# Introduction
This is a full-stack Employee Management System built with:

Backend: ASP.NET Core
Frontend: React.js
Database: SQLite
The system includes the following features:

User authentication using JWT.
CRUD operations for managing employees.
Role-based access (manager and employees).

# Technologies Used
Backend:
ASP.NET Core
Entity Framework Core
JWT Authentication
Frontend:
React.js
Axios for API communication
Database:
SQLite (can be replaced with SQL Server if needed)
# Setup and Installation
Prerequisites
## Backend Requirements:
.NET 6 SDK
SQLite (or SQL Server for production)
Visual Studio or any text editor with .NET Core support
## Frontend Requirements:
Node.js (v14 or later)
npm (comes with Node.js)

## 1. Backend Setup
### 1. Clone the repository:

git clone https://github.com/Zehavas-NBS/Clalit-Home
cd <backend_project_folder>
### 2. Install dependencies:

dotnet restore

### 3.Configure database connection in appsettings.json:

{
    "ConnectionStrings": {
        "DefaultConnection": "Data Source=EmployeeManager.db"
    }
}
### 4.Apply migrations to create the database:

dotnet ef database update
### 5.Run the backend server:

dotnet run
# The backend will be available at http://localhost:5009.
2. Frontend Setup
  ## 2.1 Clone the repository:
  git clone https://github.com/Zehavas-NBS/Clalit-Home
  cd <frontend_project_folder>
  ## 2.2 Install dependencies:
  npm install
  If you want change ports in .env file in the root directory of the frontend project:
  PORT=3001
  REACT_APP_API_URL=http://localhost:5009/api
  ### Run the React application:
  npm start
  The frontend will be available at http://localhost:3001.
#### Usage
Access the frontend at http://localhost:3001.
Use the Sign Up page to create a new manager account.
Log in using the manager account.
Add, update, or delete employees.
Each API call will be authorized using the JWT returned during login.
API Endpoints
Authentication
POST /api/Auth/login

Request:
json
Copy code
{
  "email": "manager@example.com",
  "password": "your_password"
}
Response:
json
Copy code
{
  "token": "JWT_TOKEN",
  "managerData": { ... }
}
POST /api/Auth/signup

Request:
json
Copy code
{
  "email": "newmanager@example.com",
  "password": "your_password",
  "fullName": "John Doe"
}
Employees
GET /api/Employees/getEmployeesByManagerId

Headers:
Authorization: Bearer JWT_TOKEN
POST /api/Employees/add

Request:
json
Copy code
{
  "fullName": "Jane Smith",
  "email": "jane.smith@example.com",
  "password": "password123"
}
PUT /api/Employees/update

Request:
json
Copy code
{
  "id": "employee_id",
  "fullName": "Jane Smith Updated"
}
DELETE /api/Employees/delete/{id}

