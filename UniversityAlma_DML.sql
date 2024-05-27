CREATE SCHEMA UniversityAlma;
GO

-- Profile Table
CREATE TABLE UniversityAlma.Profile(
	ProfileId INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	Password VARCHAR(128) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Username VARCHAR(50) UNIQUE  NOT NULL,
	PhoneNumber VARCHAR(15),
	Gender VARCHAR(10),
	ProfilePic VARBINARY(255),
	Birthday DATE NOT NULL,
	MailList BIT DEFAULT 0
);
GO

-- User Table
CREATE TABLE UniversityAlma.[User](
	UserId INT IDENTITY(1,1) PRIMARY KEY,
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
	Number INT NOT NULL,
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

--

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

--

-- Trigger to insert/delete user on profile create/delete
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

CREATE TRIGGER trigDeleteUser
ON UniversityAlma.Profile
AFTER DELETE
AS
BEGIN
	DELETE FROM UniversityAlma.[User]
	WHERE ProfileId IN (SELECT ProfileId FROM deleted);
END;
GO

--

-- Favorite Notification for the Mentor trigger
CREATE TRIGGER trigFavoriteNotification
ON UniversityAlma.Favorites
AFTER INSERT
AS
BEGIN
	-- Variables to hold notification details
	DECLARE @CourseId INT;
	DECLARE @MentorId INT;
	DECLARE @UserId INT;
	DECLARE @CourseTitle VARCHAR(100);
	DECLARE @NotificationId INT;

	-- Loop trough the inserted favorites for a specific course
	DECLARE cur CURSOR FOR
	SELECT CourseId
	FROM inserted;

	OPEN cur;
	FETCH NEXT FROM cur INTO @CourseId;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Get the mentorId and courseId for the favorited course
		SELECT @MentorId = MentorId, @CourseTitle = Title
		FROM UniversityAlma.Course
		WHERE CourseId = @CourseId;
		-- Get the userId associated to the mentor
		SELECT @UserId = UserId
		FROM UniversityAlma.Mentor
		WHERE MentorId = @MentorId;
		-- Get total number of favorites for the course
		DECLARE @TotalFavorites INT;
		SELECT @TotalFavorites = COUNT(*)
		FROM UniversityAlma.Favorites
		WHERE CourseId = @CourseId;

		-- Check if there is already a favorites notification for this mentor and course
		SELECT @NotificationId = NotificationId
		FROM UniversityAlma.Notification
		WHERE UserId = @UserId
			AND Title = 'New favorites for your course'
			AND Info LIKE '%' + @CourseTitle + '%'
			AND Checked = 0;
		
		IF @NotificationId IS NOT NULL
		BEGIN
			-- Update the existing notification
			UPDATE UniversityAlma.Notification
			SET Info = 'Your course ' + @CourseTitle + ' has ' + CAST(@TotalFavorites AS VARCHAR) + ' favorites'
			WHERE NotificationId = @NotificationId;
		END
		ELSE
		BEGIN
			-- Insert new notification if it doesn't exist
			INSERT INTO UniversityAlma.Notification(UserId, Title, Info, Icon)
			VALUES (@UserId, 'New favorites for your course', 'Your course ' + @CourseTitle + ' has ' + CAST(@TotalFavorites AS VARCHAR) + ' favorites', 'fav.png');
		END

		FETCH NEXT FROM cur INTO @CourseId;
	END

	CLOSE cur;
	DEALLOCATE cur;
END;
GO

--

-- If a notification is checked, delete the notification
CREATE TRIGGER trigResetNotification
ON UniversityAlma.Notification
AFTER UPDATE
AS
BEGIN
	DELETE FROM UniversityAlma.Notification
	WHERE Checked = 1;
END;
GO