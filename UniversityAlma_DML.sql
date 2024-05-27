CREATE SCHEMA UniversityAlma;
GO

-- Profile Table
CREATE TABLE UniversityAlma.Profile(
	ProfileId INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(100),
	Password VARCHAR(128),
	Email VARCHAR(100),
	Username VARCHAR(50) UNIQUE ,
	PhoneNumber VARCHAR(15),
	Gender VARCHAR(10),
	ProfilePic VARBINARY(255),
	Birthday DATE,
	MailList BIT
);
GO

-- User Table
CREATE TABLE UniversityAlma.[User](
	UserId INT IDENTITY(1,1) PRIMARY KEY,
	ProfileId INT,
	CONSTRAINT FK_User_Profile FOREIGN KEY (ProfileId) REFERENCES UniversityAlma.Profile(ProfileId)
);
GO

-- Favorites Table
CREATE TABLE UniversityAlma.Favorites(
	ProfileId INT,
	CourseId INT,

	CONSTRAINT PK_Favorites PRIMARY KEY (ProfileId, CourseId),
	CONSTRAINT FK_Favorites_Profile FOREIGN KEY (ProfileId) REFERENCES UniversityAlma.Profile(ProfileId),
);
GO


-- Admin Table
CREATE TABLE UniversityAlma.Admin(
	AdminId INT PRIMARY KEY,
	UserId INT,

	CONSTRAINT FK_Admin_User FOREIGN KEY (UserId) REFERENCES UniversityAlma.[User](UserId)
);
GO

-- Mentor Table
CREATE TABLE UniversityAlma.Mentor(
	MentorId INT PRIMARY KEY,
	UserId INT,
	Experience VARCHAR(MAX),
	Verified BIT,

	CONSTRAINT FK_Mentor_User FOREIGN KEY (UserId) REFERENCES UniversityAlma.[User](UserId)
);
GO

-- Category Table
CREATE TABLE UniversityAlma.Category(
	CategoryId INT PRIMARY KEY,
	Type VARCHAR(100)
);
GO

-- Course Table
CREATE TABLE UniversityAlma.Course(
	CourseId INT PRIMARY KEY,
	Title VARCHAR(100),
	Description VARCHAR(500),
	CategoryId INT,
	MentorId INT,
	FavCount INT DEFAULT 0,

	CONSTRAINT  FK_Course_Category FOREIGN KEY (CategoryId) REFERENCES UniversityAlma.Category(CategoryId),
	CONSTRAINT FK_Course_Mentor FOREIGN KEY (MentorId) REFERENCES UniversityAlma.Mentor(MentorId)
);
GO

-- Audit Types
CREATE TABLE UniversityAlma.AuditTypes(
	AuditTypeID INT PRIMARY KEY,
	AuditTypeName VARCHAR(150)
);
GO

-- Audits Table
CREATE TABLE UniversityAlma.Audits(
	AuditId INT PRIMARY KEY,
	AdminId INT,
	UserId INT,
	CourseId INT,
	AuditTypeId INT,
	Date DATE,
	
	CONSTRAINT FK_Audits_Admin FOREIGN KEY (AdminId) REFERENCES UniversityAlma.Admin(AdminId),
	CONSTRAINT FK_Audits_User FOREIGN KEY (UserId) REFERENCES UniversityAlma.[User](UserId),
	CONSTRAINT FK_Audits_Course FOREIGN KEY (CourseId) REFERENCES UniversityAlma.Course(CourseId),
	CONSTRAINT FK_Audits_AuditTypes FOREIGN KEY (AuditTypeId) REFERENCES UniversityAlma.AuditTypes(AuditTypeId),
);
GO

-- Session Table
CREATE TABLE UniversityAlma.Session(
	SessionId INT PRIMARY KEY,
	CourseId INT,
	Number INT,
	Title VARCHAR(100),
	Media VARCHAR(MAX),
	Duration INT,

	CONSTRAINT FK_SESSION_COURSE FOREIGN KEY (CourseId) REFERENCES UniversityAlma.Course(CourseId)
);
GO


-- History Table
CREATE TABLE UniversityAlma.History(
	ProfileId INT PRIMARY KEY,
	CourseId INT,
	SessionNumber INT,
	ElapsedTime INT,

	CONSTRAINT FK_History_Profile FOREIGN KEY (ProfileId) REFERENCES UniversityAlma.Profile(ProfileId),
	CONSTRAINT FK_History_Course FOREIGN KEY (CourseId) REFERENCES UniversityAlma.Course(CourseId)
);
GO

-- Notification Table
CREATE TABLE UniversityAlma.Notification(
	NotificationId INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT,
	Title VARCHAR(100),
	Info VARCHAR(300),
	Icon VARCHAR(255)
);
GO

-- Certificate Table
CREATE TABLE UniversityAlma.Certificates (
	CertificateId INT PRIMARY KEY,
	MentorId INT,
	Title VARCHAR(100),

	CONSTRAINT FK_Certificates_Mentor FOREIGN KEY (MentorId) REFERENCES UniversityAlma.Mentor(MentorId)
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

-- Triggers

-- Trigger to increment FavCount entry in Course table according to Favorites table
-- Insert (Increment FavCount)
CREATE TRIGGER trigIncrementFavCount
ON UniversityAlma.Favorites
AFTER INSERT
AS
BEGIN
	UPDATE UniversityAlma.Course
	SET FavCount = FavCount + 1
	WHERE CourseId IN (SELECT CourseId FROM inserted);
END;
GO
-- Delete (Decrement FavCount)
CREATE TRIGGER trigDecrementFavCount
ON UniversityAlma.Favorites
AFTER DELETE
AS
BEGIN
    UPDATE UniversityAlma.Course
    SET FavCount = FavCount - 1
    WHERE CourseId IN (SELECT CourseId FROM deleted);
END;
GO


-- Trigger for audit notification
CREATE TRIGGER TrigAuditNotification
ON UniversityAlma.Audits
AFTER INSERT
AS
BEGIN
	-- Insert notification for userId referenced by the audit
	INSERT INTO UniversityAlma.Notification(UserId, Title, Info, Icon)
	SELECT i.UserId, at.AuditTypeName, '', 'alert.png'
	FROM inserted i
	JOIN UniversityAlma.AuditTypes at ON i.AuditTypeId = at.AuditTypeId
	WHERE i.UserId IS NOT NULL;
END;
GO

-- Trigger to insert user o profile create
CREATE TRIGGER trigInsertUser
ON UniversityAlma.Profile
AFTER INSERT
AS
BEGIN
	INSERT INTO UniversityAlma.[User] (ProfileId)
	SELECT ProfileId
	FROM inserted;
END;
GO