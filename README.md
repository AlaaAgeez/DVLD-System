<body>

<h1>🚗 DVLD Management System</h1>
<p><strong>Windows Desktop Application – .NET | SQL Server</strong></p>

<hr>

<h2>📌 Overview</h2>
<p>
The DVLD Management System is a full-scale Windows Desktop Application that simulates real-world operations of a Driving License & Vehicle Department.
</p>

<p>
The system is designed as a production-level administrative application, focusing on:
</p>

<ul>
  <li>Clean and scalable architecture</li>
  <li>Strong separation of concerns</li>
  <li>Robust validation and data integrity</li>
  <li>Maintainability and long-term scalability</li>
</ul>

<p><strong>This is not a simple CRUD project.</strong></p>
<p>
It represents a real governmental-style desktop system with complex workflows and strict business rules.
</p>

<hr>

<h2>❓ Problem Statement</h2>
<p>Many desktop applications suffer from:</p>

<ul>
  <li>Mixing UI logic with business logic and database access</li>
  <li>Weak validation and poor error handling</li>
  <li>Difficult maintenance and scalability</li>
  <li>Lack of workflow control and traceability</li>
  <li>Absence of logging and monitoring</li>
</ul>

<p>
This project was built to solve these problems by applying proper layered architecture and clean software engineering principles from the ground up.
</p>

<hr>

<h2>🧱 System Architecture</h2>

<h3>🔹 3-Layered Architecture Diagram</h3>

<h3>🧱 Layer Responsibilities</h3>

<h4>🔹 Presentation Layer</h4>
<ul>
  <li>Windows Forms user interface</li>
  <li>Handles user interaction and input only</li>
  <li>No business logic</li>
  <li>No direct database access</li>
</ul>

<h4>🔹 Business Layer</h4>
<ul>
  <li>Core business rules</li>
  <li>Centralized validation logic</li>
  <li>License lifecycle management</li>
  <li>Application and workflow processing</li>
</ul>

<h4>🔹 Data Access Layer</h4>
<ul>
  <li>SQL Server access using ADO.NET</li>
  <li>Parameterized queries</li>
  <li>Transaction-based operations (Commit / Rollback)</li>
  <li>Secure and centralized data handling</li>
</ul>

<hr>

<h2>📁 Project Structure</h2>

<pre>
DVLD/
│
├── PresentationLayer/
│   │
│   ├── Applications/
│   │   ├── Application Types/
│   │   │   ├── frmEditApplicationType.cs
│   │   │   └── frmListApplicationTypes.cs
│   │   │
│   │   ├── Controllers/
│   │   │   ├── ctrlApplicationBasicInfo.cs
│   │   │   └── ctrlDrivingLicenseApplicationInfo.cs
│   │   │
│   │   ├── Local Driving License/
│   │   │   ├── frmAddUpdateLocalDrivingLicenseApplication.cs
│   │   │   ├── frmListLocalDrivingLicenseApplications.cs
│   │   │   ├── frmLocalDrivingLicenseApplicationInfo.cs
│   │   │   ├── frmRenewLocalDrivingLicenseApplication.cs
│   │   │   └── frmReplaceLostOrDamagedLicenseApplication.cs
│   │   │
│   │   └── International License/
│   │       ├── frmListInternationalLicenseApplications.cs
│   │       └── frmNewInternationalLicenseApplication.cs
│   │
│   ├── Drivers/
│   │   └── frmListDrivers.cs
│   │
│   ├── Global Classes/
│   │   ├── clsGlobal.cs
│   │   └── clsValidation.cs
│   │
│   ├── Licenses/
│   │   ├── Controllers/
│   │   │   ├── ctrlDriverInternationalLicenseInfo.cs
│   │   │   ├── ctrlDriverLicenseInfo.cs
│   │   │   ├── ctrlDriverLicenseInfoWithFilter.cs
│   │   │   └── ctrlDriverLicenses.cs
│   │   │
│   │   ├── Detain License/
│   │   │   ├── frmDetainLicenseApplication.cs
│   │   │   ├── frmListDetainedLicenses.cs
│   │   │   └── frmReleaseDetainedLicenseApplication.cs
│   │   │
│   │   └── Local Licenses/
│   │       ├── frmShowLicenseInfo.cs
│   │       ├── frmIssueDriverLicenseFirstTime.cs
│   │       ├── frmShowInternationalLicenseInfo.cs
│   │       └── frmShowPersonLicenseHistory.cs
│   │
│   ├── Login/
│   │   └── frmLogin.cs
│   │
│   ├── People/
│   │   ├── UserControllers/
│   │   │   ├── ctrlPersonCardWithFilter.cs
│   │   │   └── ctrlShowPersonInfo.cs
│   │   ├── FrmAddUpdateNewPerson.cs
│   │   ├── frmFindPerson.cs
│   │   ├── FrmManagePeople.cs
│   │   └── FrmShowPersonDetails.cs
│   │
│   ├── Tests/
│   │   ├── Controllers/
│   │   │   ├── ctrlScheduleTest.cs
│   │   │   └── ctrlScheduledTest.cs
│   │   │
│   │   ├── Test Types/
│   │   │   ├── frmEditTestType.cs
│   │   │   └── frmListTestTypes.cs
│   │   │
│   │   ├── frmListTestAppointments.cs
│   │   ├── frmScheduleTest.cs
│   │   └── frmTakeTest.cs
│   │
│   ├── Users/
│   │   ├── ctrlUserCard.cs
│   │   ├── frmAddUpdateUser.cs
│   │   ├── frmChangePassword.cs
│   │   ├── frmListUsers.cs
│   │   └── frmUserInfo.cs
│   │
│   ├── FrmMain.cs
│   ├── Program.cs
│   ├── App.config
│   └── app.manifest
│
├── BusinessLayer/
│   ├── clsApplication.cs
│   ├── clsApplicationType.cs
│   ├── clsCountry.cs
│   ├── clsDetainedLicense.cs
│   ├── clsDriver.cs
│   ├── clsGlobalBusiness.cs
│   ├── clsInternationalLicense.cs
│   ├── clsLicense.cs
│   ├── clsLicenseClass.cs
│   ├── clsLocalDrivingLicenseApplication.cs
│   ├── clsLogger.cs
│   ├── clsPerson.cs
│   ├── clsTest.cs
│   ├── clsTestAppointment.cs
│   ├── clsTestType.cs
│   └── clsUser.cs
│
└── DataAccessLayer/
    ├── clsApplicationData.cs
    ├── clsApplicationTypeData.cs
    ├── clsCountryData.cs
    ├── clsDataAccessSettings.cs
    ├── clsDetainedLicenseData.cs
    ├── clsDriverData.cs
    ├── clsInternationalLicenseData.cs
    ├── clsLicenseClassData.cs
    ├── clsLicenseData.cs
    ├── clsLocalDrivingLicenseApplicationData.cs
    ├── clsLoggerData.cs
    ├── clsPersonData.cs
    ├── clsTestAppointmentData.cs
    ├── clsTestData.cs
    ├── clsTestTypeData.cs
    └── clsUserData.cs
</pre>

<hr>

<h2>🧼 Code Quality & Design</h2>
<ul>
  <li>Clean Code principles</li>
  <li>Separation of Concerns</li>
  <li>Single Responsibility Principle (SRP)</li>
  <li>Reusable and maintainable components</li>
  <li>Clear and readable structure</li>
</ul>

<hr>

<h2>🔐 Security & Data Integrity</h2>
<ul>
  <li>SQL Injection prevention using parameterized queries</li>
  <li>SQL Server transactions (Commit / Rollback)</li>
  <li>Centralized validation across layers</li>
  <li>Secure handling of sensitive data</li>
</ul>

<hr>

<h2>👥 Core Features</h2>
<ul>
  <li>People & Drivers Management</li>
  <li>Local & International License Management</li>
  <li>First-time License Issuing</li>
  <li>License Renewal & Replacement</li>
  <li>Detained License Handling (Detain / Release)</li>
  <li>Driving Tests Scheduling and Execution</li>
  <li>Application workflow tracking</li>
  <li>Centralized Validation & Logging</li>
</ul>

<hr>

<h2>🧾 Logging & Monitoring</h2>
<ul>
  <li>Windows Event Logger integration</li>
  <li>Error, warning, and activity logging</li>
  <li>Improved debugging, auditing, and reliability</li>
</ul>

<hr>

<h2>🪟 Windows Integration</h2>
<ul>
  <li>Windows Forms UI</li>
  <li>Windows Registry for configuration</li>
  <li>Desktop shortcut support</li>
  <li>Native Windows Event Log usage</li>
</ul>

<hr>

<h2>⚙ Technology Stack</h2>
<ul>
  <li>C# (.NET)</li>
  <li>Windows Forms</li>
  <li>ADO.NET</li>
  <li>Microsoft SQL Server</li>
  <li>SSMS</li>
  <li>Visual Studio 2022</li>
  <li>Git & GitHub</li>
</ul>

<hr>

<h2>▶ How to Run the Project</h2>

<h3>1️⃣ Requirements</h3>
<ul>
  <li>Windows OS</li>
  <li>Visual Studio 2022</li>
  <li>.NET Framework / .NET</li>
  <li>SQL Server</li>
  <li>SSMS</li>
</ul>

<h3>2️⃣ Database Setup</h3>
<ul>
  <li>Create database: DVLD_DB</li>
  <li>Execute SQL scripts</li>
  <li>Verify tables and relations</li>
</ul>

<h3>3️⃣ Configuration</h3>
<ul>
  <li>Open solution in Visual Studio</li>
  <li>Update App.config connection string</li>
</ul>

<h3>4️⃣ Run</h3>
<ul>
  <li>Build solution</li>
  <li>Run application</li>
</ul>

<hr>

<h2>🎯 Project Goal</h2>
<p>
To demonstrate how to build a professional, secure, and maintainable Windows desktop system using correct architecture and real-world workflows.
</p>

<hr>

<h2>🚀 Final Note</h2>
<p>
This project is not about writing code that only works,
but about building a system that is clean, secure, maintainable,
and ready for real production environments.
</p>

</body>
</html>
