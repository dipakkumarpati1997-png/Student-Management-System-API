
# Student Management System API

Hi,
This is a simple backend project I built using ASP.NET Core Web API to practice real-world backend development.

The main idea of this project was to understand how APIs work, how authentication is implemented, and how to structure code properly using layers.

---

## What this project does

This API allows you to:

* Add new students
* View all students
* Update student details
* Delete students
* Login and access protected APIs
* Create a new User For Login

---

## Technologies used

* ASP.NET Core Web API
* SQL Server
* ADO.NET
* JWT Authentication
* Swagger
* Serilog

---

## Project structure

I have used a basic layered architecture:

* Controller → Handles API requests
* Service → Contains business logic
* Repository → Handles database operations
* Middleware → Handles exceptions
* Helpers → For things like password hashing

---

## Authentication

JWT authentication is implemented.

Flow is simple:

1. Login using username and password
2. Token is generated
3. Token is stored in session
4. Middleware automatically attaches token to requests
5. APIs are protected using `[Authorize]`

---

## API Endpoints

### Login

* POST /api/Student/login

### Student APIs

* GET /api/Student/StudetDetailsGet
* POST /api/Student/AddStudentDetails
* PUT /api/Student/UpdateStudentDetails
* DELETE /api/Student/DeleteStudentDetails/{id}
* POST /api/Student/AddUser
---

## Database

Two tables are used:

**Students**

* Id
* Name
* Email
* Age
* Course
* CreatedDate

**Users**

* Id
* Username
* Password (hashed)

---

## Logging

Logging is done using Serilog.

* Logs are stored in console and file
* File path: /logs/log.txt

---

## Exception Handling

I have added custom middleware for handling exceptions globally.

---

## How to run

1. Clone the project
2. Open in Visual Studio
3. Update connection string in appsettings.json
4. Run the project
5. Swagger will open

---

## Notes

* Passwords are hashed before storing
* JWT is used for security
* Code is structured for better readability



---

## Author

Dipak Kumar Pati
