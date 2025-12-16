# AsanaFlow

A comprehensive monolithic yoga and wellness application built with ASP.NET Core, providing a complete platform for managing yoga practices, breathing exercises, music playlists, and user progress tracking.

## 📋 Overview

AsanaFlow is a full-featured yoga management system that enables users to explore yoga poses, create custom sessions, track their progress, and enhance their practice with guided breathing exercises and curated music playlists. The application features secure authentication, personalized user experiences, and a robust API for seamless integration.

## ✨ Features

- **User Management**: Secure OTP-based account creation with JWT authentication and password hashing
- **Yoga Pose Library**: Browse and explore yoga poses organized by categories
- **Custom Sessions**: Create and manage personalized yoga sessions with multiple poses
- **Progress Tracking**: Monitor your yoga journey with detailed progress metrics
- **Favorites System**: Save and organize your favorite poses and sessions
- **Breathing Exercises**: Access guided breathing techniques for meditation and relaxation
- **Music Integration**: Curated playlists to enhance your yoga practice
- **RESTful API**: 44+ endpoints for comprehensive application functionality

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core
- **ORM**: Entity Framework Core (Database-First Approach)
- **Query Language**: LINQ
- **Authentication**: JWT (JSON Web Tokens)
- **Email Service**: SMTP for OTP delivery
- **Configuration**: appsettings.json
- **Security**: Password hashing with industry-standard algorithms

## 🗄️ Database Schema

The application uses a relational database with the following tables:

- **Users**: User account information and credentials
- **Yoga_Categories**: Classification of yoga poses by type
- **Yoga_Poses**: Detailed yoga pose information and instructions
- **Sessions**: User-created yoga practice sessions
- **Session_Poses**: Many-to-many relationship between sessions and poses
- **Music_Playlists**: Curated music collections for yoga practice
- **User_Progress**: Historical tracking of user achievements and milestones
- **User_Favorites**: User bookmarked poses and sessions
- **Breathing_Exercises**: Guided breathing technique instructions

## 🚀 Getting Started

### Prerequisites

- .NET 6.0 SDK or higher
- SQL Server (or compatible database)
- SMTP server credentials for email functionality

### Installation

1. **Clone the repository**:
```bash
git clone https://github.com/yourusername/AsanaFlow.git
cd AsanaFlow
```

2. **Update the database connection string in `appsettings.json`**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=AsanaFlow;User Id=your_user;Password=your_password;"
  }
}
```

3. **Configure SMTP settings in `appsettings.json`**:
```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.your-provider.com",
    "SmtpPort": 587,
    "SmtpUsername": "your_email@example.com",
    "SmtpPassword": "your_password",
    "FromEmail": "noreply@asanaflow.com",
    "FromName": "AsanaFlow"
  }
}
```

4. **Configure JWT settings in `appsettings.json`**:
```json
{
  "JwtSettings": {
    "SecretKey": "your_secret_key_here_minimum_32_characters",
    "Issuer": "AsanaFlow",
    "Audience": "AsanaFlowUsers",
    "ExpirationMinutes": 60
  }
}
```

5. **Restore dependencies**:
```bash
dotnet restore
```

6. **Update database** (if using migrations):
```bash
dotnet ef database update
```

7. **Run the application**:
```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or the port specified in your launch settings).

## 📡 API Documentation

The application provides 44+ API endpoints organized into the following categories:

### Authentication Endpoints
- `POST /api/auth/register` - Register new user with OTP verification
- `POST /api/auth/verify-otp` - Verify OTP code
- `POST /api/auth/login` - User login with JWT token generation
- `POST /api/auth/refresh-token` - Refresh expired JWT tokens
- `POST /api/auth/forgot-password` - Request password reset
- `POST /api/auth/reset-password` - Reset password with token

### User Endpoints
- `GET /api/users/profile` - Get current user profile
- `PUT /api/users/profile` - Update user profile
- `PUT /api/users/change-password` - Change user password
- `DELETE /api/users/account` - Delete user account

### Yoga Categories Endpoints
- `GET /api/yoga-categories` - List all yoga categories
- `GET /api/yoga-categories/{id}` - Get specific category details
- `GET /api/yoga-categories/{id}/poses` - Get all poses in a category

### Yoga Poses Endpoints
- `GET /api/yoga-poses` - List all yoga poses
- `GET /api/yoga-poses/{id}` - Get specific pose details
- `GET /api/yoga-poses/category/{categoryId}` - Get poses by category
- `GET /api/yoga-poses/search?query={query}` - Search poses by name or description
- `GET /api/yoga-poses/difficulty/{level}` - Get poses by difficulty level

### Sessions Endpoints
- `GET /api/sessions` - List all user sessions
- `GET /api/sessions/{id}` - Get session details
- `POST /api/sessions` - Create new session
- `PUT /api/sessions/{id}` - Update session
- `DELETE /api/sessions/{id}` - Delete session
- `GET /api/sessions/{id}/poses` - Get all poses in a session

### Session Poses Endpoints
- `POST /api/sessions/{sessionId}/poses` - Add pose to session
- `PUT /api/sessions/{sessionId}/poses/{poseId}` - Update pose in session
- `DELETE /api/sessions/{sessionId}/poses/{poseId}` - Remove pose from session

### Music Playlists Endpoints
- `GET /api/playlists` - List all playlists
- `GET /api/playlists/{id}` - Get playlist details
- `GET /api/playlists/category/{category}` - Get playlists by category
- `GET /api/playlists/search?query={query}` - Search playlists

### User Progress Endpoints
- `GET /api/progress` - Get user progress history
- `GET /api/progress/{id}` - Get specific progress entry
- `POST /api/progress` - Log new progress entry
- `GET /api/progress/stats` - Get progress statistics
- `GET /api/progress/date-range?start={start}&end={end}` - Get progress by date range

### User Favorites Endpoints
- `GET /api/favorites` - List all user favorites
- `GET /api/favorites/poses` - List favorite poses
- `GET /api/favorites/sessions` - List favorite sessions
- `POST /api/favorites` - Add to favorites
- `DELETE /api/favorites/{id}` - Remove from favorites

### Breathing Exercises Endpoints
- `GET /api/breathing-exercises` - List all breathing exercises
- `GET /api/breathing-exercises/{id}` - Get exercise details
- `GET /api/breathing-exercises/difficulty/{level}` - Get exercises by difficulty
- `GET /api/breathing-exercises/duration?min={min}&max={max}` - Get exercises by duration

## 🔐 Authentication Flow

1. User registers with email via `POST /api/auth/register`
2. System sends OTP to user's email via SMTP
3. User verifies OTP via `POST /api/auth/verify-otp`
4. Account is activated and user can log in
5. User logs in via `POST /api/auth/login` and receives JWT access token
6. JWT token is included in Authorization header: `Bearer {token}`
7. Token can be refreshed using `POST /api/auth/refresh-token`

### Example Authentication Header
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 🔒 Security Features

- **Password Hashing**: Secure password storage using BCrypt/PBKDF2
- **JWT Authentication**: Stateless authentication with configurable expiration
- **OTP Verification**: Email-based one-time password for account activation
- **Protected Endpoints**: Authorization middleware on all protected routes
- **HTTPS Enforcement**: Secure communication in production
- **Input Validation**: Data validation on all API endpoints
- **SQL Injection Prevention**: Parameterized queries via Entity Framework Core

## 📁 Project Structure

```
AsanaFlow/
├── Controllers/           # API controllers for handling HTTP requests
├── Models/               # Entity Framework models (Database-First)
├── Data/                 # DbContext and data access layer
├── Services/             # Business logic and service layer
├── DTOs/                 # Data transfer objects for API requests/responses
├── Helpers/              # Utility classes and helper functions
├── Middleware/           # Custom middleware (JWT authentication, etc.)
├── Validators/           # Input validation logic
├── appsettings.json      # Configuration file
├── Program.cs            # Application entry point
└── Startup.cs            # Application configuration and services
```

## ⚙️ Configuration

Complete `appsettings.json` example:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AsanaFlow;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "your_super_secret_key_here_at_least_32_characters_long",
    "Issuer": "AsanaFlow",
    "Audience": "AsanaFlowUsers",
    "ExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  },
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your_email@gmail.com",
    "SmtpPassword": "your_app_password",
    "FromEmail": "noreply@asanaflow.com",
    "FromName": "AsanaFlow Support",
    "EnableSsl": true
  },
  "OtpSettings": {
    "ExpirationMinutes": 10,
    "Length": 6
  }
}
```

### Environment Variables (Recommended for Production)

Instead of storing sensitive data in `appsettings.json`, use environment variables:

```bash
export ConnectionStrings__DefaultConnection="your_connection_string"
export JwtSettings__SecretKey="your_secret_key"
export EmailSettings__SmtpPassword="your_smtp_password"
```

## 🧪 Testing

Run unit tests:
```bash
dotnet test
```

## 📦 Deployment

### Prerequisites
- SQL Server database
- SMTP service (Gmail, SendGrid, etc.)
- Web server (IIS, Azure App Service, etc.)

### Steps
1. Update `appsettings.Production.json` with production configuration
2. Build the application:
```bash
dotnet build --configuration Release
```
3. Publish the application:
```bash
dotnet publish --configuration Release --output ./publish
```
4. Deploy the published files to your web server
5. Configure HTTPS certificates
6. Set up database connection and run migrations

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Your Name**
- GitHub: [@jesuisPraful](https://github.com/jesuisPraful)
- Email: prafulsingh211@gmail.com

## 🙏 Acknowledgments

- Entity Framework Core documentation
- ASP.NET Core community
- JWT authentication best practices
- Yoga community for pose references

---

**⚠️ Important Security Note**: Never commit sensitive information like connection strings, API keys, or SMTP passwords to version control. Use environment variables or secure configuration management tools in production environments.
