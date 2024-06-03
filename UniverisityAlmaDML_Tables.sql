CREATE SCHEMA UniversityAlma;
GO

-- Profile Table
CREATE TABLE UniversityAlma.Profile(
	ProfileId INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	Password VARCHAR(120) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Username VARCHAR(50) UNIQUE  NOT NULL,
	PhoneNumber VARCHAR(15),
	Gender VARCHAR(10),
	ProfilePic VARBINARY(MAX),
	Birthday DATE NOT NULL,
	MailList BIT DEFAULT 0
);
GO

-- User Table
CREATE TABLE UniversityAlma.[User](
	UserId INT PRIMARY KEY,
	ProfileId INT NOT NULL,
	
	CONSTRAINT FK_User_Profile FOREIGN KEY (ProfileId) REFERENCES UniversityAlma.Profile(ProfileId)
);
GO

-- Admin Table
CREATE TABLE UniversityAlma.Admin(
	AdminId INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT NOT NULL,

	CONSTRAINT FK_Admin_User FOREIGN KEY (UserId) REFERENCES UniversityAlma.[User](UserId)
);
GO

-- Mentor Table
CREATE TABLE UniversityAlma.Mentor(
	MentorId INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT NOT NULL,
	Experience VARCHAR(MAX),
	Verified BIT DEFAULT 0,

	CONSTRAINT FK_Mentor_User FOREIGN KEY (UserId) REFERENCES UniversityAlma.[User](UserId)
);
GO

-- Certificate Table
CREATE TABLE UniversityAlma.Certificates (
	CertificateId INT IDENTITY(1,1) PRIMARY KEY,
	MentorId INT NOT NULL,
	Title VARCHAR(100) NOT NULL,

	CONSTRAINT FK_Certificates_Mentor FOREIGN KEY (MentorId) REFERENCES UniversityAlma.Mentor(MentorId)
);
GO

-- Category Table
CREATE TABLE UniversityAlma.Category(
	CategoryId INT PRIMARY KEY NOT NULL,
	Type VARCHAR(100) NOT NULL
);
GO

-- Course Table
CREATE TABLE UniversityAlma.Course(
	CourseId INT IDENTITY(1,1) PRIMARY KEY,
	Title VARCHAR(100) NOT NULL,
	Description VARCHAR(500) NOT NULL,
	CategoryId INT NOT NULL,
	MentorId INT NOT NULL,
	FavCount INT DEFAULT 0,
	IsDeleted BIT DEFAULT 0

	CONSTRAINT  FK_Course_Category FOREIGN KEY (CategoryId) REFERENCES UniversityAlma.Category(CategoryId),
	CONSTRAINT FK_Course_Mentor FOREIGN KEY (MentorId) REFERENCES UniversityAlma.Mentor(MentorId)
);
GO

-- Favorites Table
CREATE TABLE UniversityAlma.Favorites(
	ProfileId INT NOT NULL,
	CourseId INT NOT NULL,

	CONSTRAINT PK_Favorites PRIMARY KEY (ProfileId, CourseId),
	CONSTRAINT FK_Favorites_Profile FOREIGN KEY (ProfileId) REFERENCES UniversityAlma.Profile(ProfileId),
);
GO

-- Audit Types
CREATE TABLE UniversityAlma.AuditTypes(
	AuditTypeID INT PRIMARY KEY NOT NULL,
	AuditTypeName VARCHAR(150) NOT NULL
);
GO

-- Audits Table
CREATE TABLE UniversityAlma.Audits(
	AuditId INT IDENTITY(1,1) PRIMARY KEY,
	AdminId INT NOT NULL,
	UserId INT,
	CourseId INT,
	AuditTypeId INT NOT NULL,
	Date DATE,
	
	CONSTRAINT FK_Audits_Admin FOREIGN KEY (AdminId) REFERENCES UniversityAlma.Admin(AdminId),
	CONSTRAINT FK_Audits_User FOREIGN KEY (UserId) REFERENCES UniversityAlma.[User](UserId),
	CONSTRAINT FK_Audits_Course FOREIGN KEY (CourseId) REFERENCES UniversityAlma.Course(CourseId),
	CONSTRAINT FK_Audits_AuditTypes FOREIGN KEY (AuditTypeId) REFERENCES UniversityAlma.AuditTypes(AuditTypeId),
);
GO

-- Session Table
CREATE TABLE UniversityAlma.Session(
	SessionId INT IDENTITY(1,1) PRIMARY KEY,
	CourseId INT NOT NULL,
	Number INT,
	Title VARCHAR(100) NOT NULL,
	Media VARCHAR(MAX),
	Duration INT,

	CONSTRAINT FK_SESSION_COURSE FOREIGN KEY (CourseId) REFERENCES UniversityAlma.Course(CourseId)
);
GO

-- History Table
CREATE TABLE UniversityAlma.History(
	ProfileId INT PRIMARY KEY NOT NULL,
	CourseId INT NOT NULL,
	SessionNumber INT NOT NULL,
	ElapsedTime INT,

	CONSTRAINT FK_History_Profile FOREIGN KEY (ProfileId) REFERENCES UniversityAlma.Profile(ProfileId),
	CONSTRAINT FK_History_Course FOREIGN KEY (CourseId) REFERENCES UniversityAlma.Course(CourseId)
);
GO

-- Notification Table
CREATE TABLE UniversityAlma.Notification(
	NotificationId INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT NOT NULL,
	Title VARCHAR(100) NOT NULL,
	Info VARCHAR(300),
	Icon VARCHAR(255),
	Checked BIT DEFAULT 0
);
GO


-- Relationships
ALTER TABLE UniversityAlma.Notification
ADD CONSTRAINT FK_Notification_User FOREIGN KEY (UserId) REFERENCES UniversityAlma.[User](UserId);

ALTER TABLE UniversityAlma.History
ADD CONSTRAINT FK_History_Session FOREIGN KEY (SessionNumber) REFERENCES UniversityAlma.Session(SessionId);

ALTER TABLE UniversityAlma.Favorites
ADD CONSTRAINT FK_Favorites_Course FOREIGN KEY (CourseId) REFERENCES UniversityAlma.Course(CourseId);
GO