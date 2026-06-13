# Smart Cybersecurity Threat Visualizer

A production-ready, full-stack cybersecurity threat intelligence web application
built with ASP.NET Core MVC on .NET 10.

---

## Tech Stack

| Layer          | Technology                          |
|----------------|-------------------------------------|
| Backend        | ASP.NET Core MVC, C#, .NET 10       |
| Database       | SQL Server, Entity Framework Core   |
| Authentication | ASP.NET Core Identity               |
| Frontend       | Bootstrap 5, HTML, CSS              |
| Charts         | Chart.js 4                          |
| Maps           | Leaflet.js, OpenStreetMap/CartoDB   |
| APIs           | AbuseIPDB, VirusTotal, HaveIBeenPwned, IPStack |

---

## Features

- **Cyber Attack Map** — Interactive Leaflet.js world map with animated threat markers
- **IP Threat Analysis** — AbuseIPDB reputation check with geo-location mini map
- **Phishing URL Checker** — VirusTotal 70+ engine scan with detection ratio bar
- **Password Analyzer** — k-Anonymity HIBP breach check + local strength scoring
- **Security Dashboard** — Chart.js doughnut and bar charts with stat cards
- **Threat Intelligence Feed** — Aggregated view of all threat activity
- **About Cybersecurity** — Educational content on threat types and CIA Triad
- **Role-based Auth** — ASP.NET Core Identity with Admin and User roles

---

## Prerequisites

- Visual Studio 2022 (any edition)
- .NET 10 SDK (`dotnet --version` should show `10.x.x`)
- SQL Server or SQL Server LocalDB (included with VS 2022)

---

## Setup Instructions

### 1. Clone or Download the Project

```bash
git clone https://github.com/YOUR_USERNAME/SmartCyberViz.git
cd SmartCyberViz
```

Or download the ZIP and extract it.

### 2. Open in Visual Studio 2022

Double-click `SmartCyberViz.sln` to open the solution.

### 3. Configure the Database

Open `appsettings.json` and update the connection string if needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SmartCyberVizDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

For a full SQL Server instance:
```json
"DefaultConnection": "Server=YOUR_SERVER;Database=SmartCyberVizDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
```

### 4. Get Free API Keys

#### AbuseIPDB (IP Reputation)
1. Go to https://www.abuseipdb.com/register
2. Create a free account
3. Go to https://www.abuseipdb.com/account/api
4. Copy your API key

#### VirusTotal (URL Scanner)
1. Go to https://www.virustotal.com/gui/join-us
2. Create a free account
3. Go to your profile → API Key
4. Copy your API key

#### HaveIBeenPwned (Password Breach)
1. Go to https://haveibeenpwned.com/API/Key
2. Purchase a key (starts at $3.50/month)
3. **OR** skip this — the password strength analyzer still works locally without it
4. Copy your API key

#### IPStack (Geo-Location)
1. Go to https://ipstack.com/signup/free
2. Create a free account (10,000 requests/month)
3. Copy your API key from the dashboard

### 5. Add API Keys to appsettings.json

```json
"ApiKeys": {
  "AbuseIPDB":       "YOUR_ABUSEIPDB_KEY_HERE",
  "VirusTotal":      "YOUR_VIRUSTOTAL_KEY_HERE",
  "HaveIBeenPwned":  "YOUR_HIBP_KEY_HERE",
  "IPStack":         "YOUR_IPSTACK_KEY_HERE"
}
```

### 6. Run Database Migrations

Open **Tools → NuGet Package Manager → Package Manager Console** and run:

```powershell
Update-Database
```

### 7. Run the Application

Press **F5** in Visual Studio or:

```bash
dotnet run
```

The app will open at `https://localhost:7xxx`

---

## Project Structure
SmartCyberViz/
├── Controllers/          # MVC Controllers for each page
├── Data/                 # ApplicationDbContext
├── Models/               # Entity models (ThreatLog, IPReport, etc.)
├── ViewModels/           # View-specific data transfer objects
├── Views/                # Razor views (.cshtml) for each page
├── Services/             # API integration service classes
│   └── Interfaces/       # Service interfaces
├── Repositories/         # Data access layer
│   └── Interfaces/       # Repository interfaces
└── wwwroot/              # Static files (CSS, JS)

---

## Architecture
Browser
│
▼
Controller  ──►  Service Layer  ──►  External APIs
│                                 (AbuseIPDB, VirusTotal,
│                                  HIBP, IPStack)
▼
Repository Layer
│
▼
Entity Framework Core
│
▼
SQL Server Database

---

## Database Tables

| Table           | Purpose                              |
|-----------------|--------------------------------------|
| AspNetUsers     | Identity user accounts               |
| AspNetRoles     | Admin / User roles                   |
| ThreatLogs      | Attack map threat entries            |
| IPReports       | AbuseIPDB check results              |
| PhishingChecks  | VirusTotal URL scan results          |
| PasswordChecks  | HIBP + strength check results        |

---

## Default Roles

| Role  | Access                          |
|-------|---------------------------------|
| Admin | Full access to all features     |
| User  | Access to all tools and reports |

Roles are seeded automatically on first run.

---

## API Free Tier Limits

| API            | Free Limit                    |
|----------------|-------------------------------|
| AbuseIPDB      | 1,000 checks/day              |
| VirusTotal     | 4 requests/minute, 500/day    |
| HaveIBeenPwned | Paid (from $3.50/month)       |
| IPStack        | 10,000 requests/month         |

---

## Screenshots

> Add screenshots of your running app here after launch.

---

## Author

**Nidhi Varade**
MSc Computer Science — K.K. Wagh College, Nashik
GitHub: https://github.com/enveeee
LinkedIn: https://linkedin.com/in/varades

---

## License

This project is built for educational and portfolio purposes.