Schedulink is a C# Windows Forms application designed to simplify the process of scheduling and managing appointments between students and professors. The application features a dynamic calendar interface, role-based access, and persistent data storage.

🚀 Features
Role-Based Access: Specialized interfaces for both Students and Professors identified during login (e.g., 's' for student, 'p' for professor).

Interactive Calendar: A custom grid-based calendar system that allows students to navigate months and select specific dates for appointments.

Appointment Management:

Students: Can browse a list of professors, select a date/time, and categorize the appointment by reason (e.g., Thesis Writing, Regrading, General Talk).

Professors: Can view a consolidated list of all their scheduled appointments in a dedicated data grid.

Prioritization System: Appointments are assigned priority levels based on the reason for the visit (e.g., "Thesis Writing" is higher priority than "General Talk").

Persistence: Uses SQLite to store and manage appointment data, allowing for creation and deletion of records.

🛠️ Technical Stack
Language: C#.

Framework: .NET 8.0 Windows Forms.

Database: SQLite (via System.Data.SQLite NuGet package).

Development Environment: Visual Studio.

📂 Project Structure
Models:

User.cs: Base class for shared user attributes like credentials and contact info.

Student.cs & Professor.cs: Inherited classes for role-specific data.

Appointment.cs: Represents the meeting details (date, time, reason).

Logic & UI:

BaseHandler.cs: Handles core business logic and database interactions.

Form1.cs: The initial login screen and UI generation.

Hub.cs: The main dashboard containing the calendar and appointment tables.

ucDays.cs: A custom UserControl representing individual day tiles in the calendar.

🔧 Installation & Setup
Clone the Repository:

Bash
git clone https://github.com/yourusername/Schedulink.git
Prerequisites: Ensure you have the .NET 8.0 SDK installed.

Database Configuration: The application requires appointments.db located in the output directory.

Build and Run:

Open Schedulink.sln in Visual Studio.

Restore NuGet packages (specifically System.Data.SQLite).

Press F5 or use dotnet run to launch the application.

📝 Usage
Login: Enter your credentials in the login form. The system redirects you to the Student or Professor hub based on your account type.

For Students:

Select a professor from the menu.

Use the calendar to find an available date and click "Make Appointment".

For Professors:

Click "See Appointments" to view your upcoming schedule.

Manage or delete appointments as needed.
