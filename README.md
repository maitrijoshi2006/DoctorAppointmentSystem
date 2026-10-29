# Doctor Appointment Booking System

ASP.NET Core MVC, .NET 8, Entity Framework Core, MySQL, ASP.NET Core Identity, Bootstrap.

## Features
- Patient registration, login, logout and role-based access
- Admin dashboard with counts and recent appointments
- Doctor CRUD and availability flag
- Patient booking and appointment cancellation
- Admin appointment status changes
- Server-side validation and basic slot conflict checks
- MySQL schema managed with EF Core migrations

## 1. Prerequisites
- .NET 8 SDK
- MySQL Server
- Ubuntu/Linux terminal

Check SDK: `dotnet --version`

## 2. Create database user
Login to MySQL as an administrator and run:

```sql
CREATE DATABASE doctor_appointment_db CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'appointment_user'@'localhost' IDENTIFIED BY 'ChangeThisPassword123!';
GRANT ALL PRIVILEGES ON doctor_appointment_db.* TO 'appointment_user'@'localhost';
FLUSH PRIVILEGES;
```

Edit `appsettings.json` to match your local MySQL username/password. Do not commit real production credentials.

## 3. Restore, migrate and run
From the project folder:

```bash
dotnet restore
dotnet tool install --global dotnet-ef --version 8.0.11
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run --urls http://localhost:5000
```

If `dotnet-ef` is already installed, use `dotnet tool update --global dotnet-ef --version 8.0.11` instead. The application also calls `Database.MigrateAsync()` at startup; creating and applying the migration above is the explicit setup path.

Open `http://localhost:5000`.

## Development administrator account
- Email: `admin@clinic.local`
- Password: `Admin123!`

The app creates this account on startup if it does not exist. Change the password and remove or replace this development bootstrap before any real deployment. Registration creates Patient accounts only; users cannot self-register as administrators.

## Booking rules in this student project
- Appointment times are offered every 30 minutes between 09:00 and 16:30.
- Past dates are rejected.
- A doctor/date/time slot with a non-cancelled appointment is rejected by the controller.
- For production, enforce slot uniqueness transactionally in the database and add timezone-aware scheduling, email verification, password reset, audit logging, and stronger operational secret management.

## Important note
Bootstrap and validation scripts are loaded from public CDNs, so internet access is needed for those assets unless you download and serve them locally.
