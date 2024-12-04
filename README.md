# ⏰ Time Management Application

## 📜 Project Description
This project is a **Time Management Tool** developed in three phases to help students effectively allocate study time for their modules. Initially created as a standalone desktop application, it evolves into a web-based application for greater accessibility. The software calculates the hours needed for self-study per week and tracks time spent on each module.

## 📚 Project Parts
The project consists of three parts:

### 1️⃣ Part 1 - Basic Application
**Features:**
- **Add Modules**: Users can add modules with the following details:
  - 📘 Module code (e.g., PROG6212)
  - 📗 Module name (e.g., Programming 2B)
  - 📊 Number of credits
  - ⏳ Weekly class hours
- **Set Semester Details**: Enter the number of weeks in the semester and the start date.
- **Self-Study Calculation**: Calculates and displays weekly self-study hours for each module.
- **Track Study Hours**: Record the time spent on each module and display remaining hours for the week.
- **Session-only Data Storage**: Data is stored in memory and cleared when the application closes.

**Technical Details:**
- **Technologies**: C#, WPF
- **Data Management**: LINQ for data manipulation
- **Architecture**: Custom class library for data and calculations

**Instructions:**
1. Clone the repository.
2. Open the solution in Visual Studio.
3. Build and run the project.

---

### 2️⃣ Part 2 - Persisting the Data
**Features:**
- **Data Persistence**: Stores user data in a SQL database.
- **User  Registration and Authentication**:
  - Users can register with a username and password.
  - 🔒 Passwords are securely stored as hashes.
  - Users log in to access their own data.
- **Enhanced Data Retrieval**: User-specific data only.

**Technical Details:**
- **Technologies**: WPF, SQL, ADO.NET/Entity Framework Core
- **Performance**: Multi-threading ensures a responsive UI during database operations.

**Instructions:**
1. Clone the repository.
2. Open the solution in Visual Studio.
3. Run the SQL script provided to set up the database.
4. Build and run the project.
5. Use the login credentials to access your data.

---

### 3️⃣ Part 3 - Web Application
**Features:**
- **Web Accessibility**: Users can access their data from any device with a browser.
- **Graphical View of Study Hours (Option 1)**: Displays a graph showing time spent on each module weekly.
- **Weekly Reminders (Option 2)**: Displays reminders for specific modules planned for each day.

**Technical Details:**
- **Technologies**: ASP.NET Core, SQL
- **Architecture**: Reuses the custom class library from Part 2.
- **Database**: Utilizes SQL with updated scripts for web integration.

**Instructions:**
1. Clone the repository.
2. Open the solution in Visual Studio.
3. Run the SQL script provided to set up the database.
4. Configure the web server settings in `appsettings.json`.
5. Build and run the project.
6. Access the application via the browser.

---

## 📋 Requirements
**Software:**
- Visual Studio 2022 or later
- SQL Server
- .NET Core SDK

**Database:**
- SQL script to create and configure the database (provided in the `DatabaseScripts` folder).

## 📊 UML Class Diagrams
Class diagrams for all three parts are available in the `UML_Diagrams` folder.

## 📜 Change Log
A detailed change log is available in the `ChangeLog.txt` file, outlining improvements and bug fixes after Part 1.

## 📖 User Manual
A comprehensive user manual with screenshots is available in `User Manual.pdf`.

## 🙏 Acknowledgments
- Sipho and Lerato for inspiring the project idea.
- [University Name] for providing resources and support.
