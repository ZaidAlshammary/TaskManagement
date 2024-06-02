# #TaskManagement Project

This repository contains the #TaskManagement project, which uses Angular for the front end and .NET Core 6 for the back end.

## Prerequisites

Before you start, make sure you have the following tools installed on your system:
- .NET 6 SDK
- Node.js v20.14.0
- npm v10.7.0

## Setting up the Development Environment

### Angular Setup

1. **Install Angular CLI**:
   ```bash
   npm install -g @angular/cli@17.3.8
   ```

2. **Install project dependencies**:
   Navigate to the Angular project directory and run:
   ```bash
   npm install
   ```

### .NET Setup

1. **Install .NET Entity Framework Core tools**:
   ```bash
   dotnet tool install --global dotnet-ef
   ```

2. **Restore .NET dependencies**:
   Navigate to the .NET project directory and run:
   ```bash
   dotnet restore
   ```

3. **Build the solution**:
   ```bash
   dotnet build
   ```

## Running the Application

### Start the Backend

1. **Navigate to the .NET project directory** and run:
   ```bash
   dotnet ef database update
   ```
    This will do the migrations to the database and seed an admin user:
    
    
    Name:Password
    ```bash
    admin:admin
    ```

1. **Navigate to the .NET project directory** and run:
   ```bash
   dotnet run
   ```
   This will start the backend server on `http://localhost:7260`.

### Start the Frontend

1. **Navigate to the Angular project directory** and run:
   ```bash
   ng serve
   ```
   This will start the Angular development server and open the application in your default browser at `http://localhost:4200`.
