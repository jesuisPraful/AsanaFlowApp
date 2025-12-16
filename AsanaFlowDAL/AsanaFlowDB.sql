USE [master]
GO

IF (EXISTS (SELECT name FROM master.dbo.sysdatabases 
WHERE ('[' + name + ']' = N'AsanaFlowDB'OR name = N'AsanaFlowDB')))
DROP DATABASE AsanaFlowDB
GO

CREATE DATABASE AsanaFlowDB
GO
USE AsanaFlowDB;
GO

-- Drop existing tables if they exist
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
IF OBJECT_ID('dbo.Yoga_Categories', 'U') IS NOT NULL DROP TABLE dbo.Yoga_Categories;
IF OBJECT_ID('dbo.Yoga_Poses', 'U') IS NOT NULL DROP TABLE dbo.Yoga_Poses;
IF OBJECT_ID('dbo.Sessions', 'U') IS NOT NULL DROP TABLE dbo.Sessions;
IF OBJECT_ID('dbo.Session_Poses', 'U') IS NOT NULL DROP TABLE dbo.Session_Poses;
IF OBJECT_ID('dbo.Music_Playlists', 'U') IS NOT NULL DROP TABLE dbo.Music_Playlists;

-- Create Users table
CREATE TABLE Users (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    name VARCHAR(100) NOT NULL,
    email VARCHAR(150) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    role VARCHAR(20) DEFAULT 'user' CHECK (role IN ('admin', 'user')),
    is_active BIT DEFAULT 1,
    email_verified BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    password_last_changed DATETIME NULL,
    last_login DATETIME NULL,
    failed_login_attempts INT DEFAULT 0,
    account_locked_until DATETIME NULL,
    mfa_enabled BIT DEFAULT 0,
    mfa_secret VARCHAR(255) NULL
);
GO

-- Create Yoga_Categories table
CREATE TABLE Yoga_Categories (
    category_id INT PRIMARY KEY IDENTITY(1,1),
    category_name VARCHAR(100) NOT NULL,
    description TEXT
);
GO

-- Create Yoga_Poses table
CREATE TABLE Yoga_Poses (
    pose_id INT PRIMARY KEY IDENTITY(1,1),
    pose_name VARCHAR(100) NOT NULL,
    category_id INT FOREIGN KEY REFERENCES Yoga_Categories(category_id),
    image_url VARCHAR(255),
    instructions TEXT,
    benefits TEXT,
    precautions TEXT,
    default_time INT -- in seconds
);
GO

-- Create Sessions table
CREATE TABLE Sessions (
    session_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT FOREIGN KEY REFERENCES Users(user_id),
    date DATETIME DEFAULT GETDATE(),
    total_duration INT
);
GO

-- Create Session_Poses table
CREATE TABLE Session_Poses (
    session_pose_id INT PRIMARY KEY IDENTITY(1,1),
    session_id INT FOREIGN KEY REFERENCES Sessions(session_id),
    pose_id INT FOREIGN KEY REFERENCES Yoga_Poses(pose_id),
    duration INT
);
GO

-- Create Music_Playlists table
CREATE TABLE Music_Playlists (
    playlist_id INT PRIMARY KEY IDENTITY(1,1),
    source VARCHAR(100), -- Spotify, YouTube, SoundCloud
    playlist_name VARCHAR(150),
    url VARCHAR(255),
    category_id INT FOREIGN KEY REFERENCES Yoga_Categories(category_id)
);
GO

-- User progress tracking
CREATE TABLE User_Progress (
    progress_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT FOREIGN KEY REFERENCES Users(user_id),
    pose_id INT FOREIGN KEY REFERENCES Yoga_Poses(pose_id),
    proficiency_level INT, -- 1-5 scale
    notes TEXT,
    last_practiced DATETIME
);
GO

-- Favorite poses
CREATE TABLE User_Favorites (
    user_id INT FOREIGN KEY REFERENCES Users(user_id),
    pose_id INT FOREIGN KEY REFERENCES Yoga_Poses(pose_id),
    PRIMARY KEY (user_id, pose_id)
);
GO

-- Breathing exercises table
CREATE TABLE Breathing_Exercises (
    exercise_id INT PRIMARY KEY IDENTITY(1,1),
    exercise_name VARCHAR(100),
    technique TEXT,
    duration INT,
    benefits TEXT
);
GO
INSERT INTO Yoga_Categories (category_name, description) VALUES
('Hatha Yoga', 'Focuses on physical postures and breathing exercises, suitable for beginners.'),
('Vinyasa Yoga', 'Dynamic flow of postures synchronized with breath, promotes flexibility and strength.'),
('Ashtanga Yoga', 'Structured and rigorous sequence of postures, ideal for building stamina and discipline.'),
('Iyengar Yoga', 'Emphasizes precise alignment and uses props like belts and blocks for support.'),
('Kundalini Yoga', 'Combines postures, breathing techniques, and meditation to awaken spiritual energy.'),
('Bikram Yoga', 'Practiced in a heated room, consists of 26 specific postures to detoxify and stretch.'),
('Restorative Yoga', 'Gentle and relaxing, uses props to support the body for deep relaxation.'),
('Power Yoga', 'Intense workout style derived from Ashtanga, improves strength and endurance.');

GO

INSERT INTO Yoga_Poses (pose_name, category_id, image_url, instructions, benefits, precautions, default_time) VALUES
('Chaturanga Dandasana', 8, 'https://example.com/chaturanga.jpg','Start in plank pose, bend elbows close to your body, lower yourself slowly until your shoulders are at elbow level.','Strengthens arms, shoulders, and core; improves endurance.','Avoid if you have wrist or shoulder injuries.',30),
('Warrior II (Virabhadrasana II)', 8, 'https://example.com/warrior2.jpg','Step one foot back, bend front knee, extend arms out to sides, gaze over front hand.','Strengthens legs, opens hips, improves focus.','Avoid if you have knee or hip injuries.',45),
('Boat Pose (Navasana)', 8, 'https://example.com/boatpose.jpg','Sit with legs extended, lift legs and torso to form a V-shape, arms parallel to the floor.','Strengthens core and spine, improves balance.','Avoid if you have lower back issues.',30),
('Crow Pose (Bakasana)', 8, 'https://example.com/crowpose.jpg','Squat down, place hands on the floor, lift feet off ground, balance on arms.','Strengthens arms, wrists, and core; improves balance.','Avoid if you have wrist injuries or are pregnant.',20),
('Side Plank (Vasisthasana)', 8, 'https://example.com/sideplank.jpg','From plank, shift weight onto one hand, stack feet, lift opposite arm up.','Strengthens arms, core, and obliques; improves balance.','Avoid if you have wrist or shoulder injuries.',40);
GO

SELECT * FROM Yoga_Categories;