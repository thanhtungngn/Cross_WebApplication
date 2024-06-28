# Cross WebApplication

This repository contains the source code for both the backend and frontend of the Cross WebApplication application. Below are the setup instructions for both components.

## Prerequisites

Before starting, ensure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) for the backend.
- [MongoDB](https://www.mongodb.com/) for the database.
- [Node.js](https://nodejs.org/) (which includes npm) for the frontend Angular application.

### Optional:
- [Angular CLI](https://angular.io/cli) for Angular commands (not included in the project).

## Getting Started

### Backend Setup

This solution contains two .NET projects configured to run simultaneously for the backend services.

#### Option 1 : Using Visual Studio 
- Open SPA_WebApplication.sln file
- Right click into Solution file > Select Properties > Choose multiple startup projects > Enable both Projects.
- Build solution and restore package
- Start deug

#### Option 2 : Using .NET runtime
1. **Navigate to the backend solution directory:**
   ```bash
   cd path/Cross_WebApplication

2. **Restore dependencies:**
   ```bash
   dotnet restore

3. **Start both projects simultaneously:**
   ```bash
   dotnet run --project path/to/Project1
   dotnet run --project path/to/Project2

### Notice :
To create admmin account, can change url on frontend to `signupAdmin` or using swagger call to `signupAdmin` API
User signup normally will have default role as Reader - Admin can update role later. 
User created by Admin will have default password to log in.


