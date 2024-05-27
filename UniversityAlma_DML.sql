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

-- Procedures
-- Procedure to resequence notification ids
DROP PROCEDURE IF EXISTS UniversityAlma.ReSequenceNotificationIDS;
GO

CREATE PROCEDURE UniversityAlma.ReSequenceNotificationIDS
AS
BEGIN
	SET NOCOUNT ON;
	-- Temporary table to store the resequenced rows
	CREATE TABLE #TempNotification(
		NewID INT IDENTITY(1,1),
		NotificationId INT,
		UserId INT,
		Title VARCHAR(100),
		Info VARCHAR(300),
		Icon VARCHAR(255),
		Checked BIT
	);
	-- Insert existing rows
	INSERT INTO #TempNotification(NotificationId, UserId, Title, Info, Icon, Checked)
	SELECT NotificationId, UserId, Title, Info, Icon, Checked
	FROM UniversityAlma.Notification
	ORDER BY NotificationId;
	-- Delete all existing rows of the notification table
	DELETE FROM UniversityAlma.Notification;
	-- Enable identity insert
	SET IDENTITY_INSERT UniversityAlma.Notification ON;
	-- Insert the resequenced from tempo to the notification table
	INSERT INTO UniversityAlma.Notification(NotificationId, UserId, Title, Info, Icon, Checked)
	SELECT NewId, UserId, Title, Info, Icon, Checked
	FROM #TempNotification;
	-- dISABLE identity insert
	SET IDENTITY_INSERT UniversityAlma.Notification OFF;
	-- Drop the temp table
	DROP TABLE #TempNotification;
END;
GO

-- Procedure to resequence session numbers on delete
DROP PROCEDURE IF EXISTS UniversityAlma.ReSequenceSessionNumbers;
GO

CREATE PROCEDURE UniversityAlma.ReSequenceSessionNumbers
	@CourseId INT
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #TempSession (
		NewNumber INT IDENTITY(1,1),
		SessionId INT
	);
	-- Insert existing sessions into temp table
	INSERT INTO #TempSession (SessionId)
	SELECT SessionId
	FROM UniversityAlma.Session
	WHERE CourseId = @CourseId
	ORDER BY SessionId;
	-- Update session numbers based on temp table
	UPDATE s
	SET s.Number = t.NewNumber
	FROM UniversityAlma.Session s
	JOIN #TempSession t ON s.SessionId = t.SessionId;
	-- Drop temp table
	DROP TABLE #TempSession;
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
	INSERT INTO UniversityAlma.[User] (UserId, ProfileId)
	SELECT ProfileId, ProfileId
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

--

-- NotificationId resequence trigger
CREATE TRIGGER UniversityAlma.trigAfterDeleteNotification
ON UniversityAlma.Notification
AFTER DELETE
AS
BEGIN
	EXEC UniversityAlma.ReSequenceNotificationIDS;
END;
GO

--

-- Session number trigger (On session insert)
CREATE TRIGGER UniversityAlma.trigInsertSessionNumber
ON UniversityAlma.Session
AFTER INSERT
AS
BEGIN
	DECLARE @CourseId INT;
	DECLARE @SessionId INT;
	DECLARE @SessionNumber INT;

	-- Loop through inserted rows
	DECLARE cur CURSOR FOR
	SELECT SessionId, CourseId 
	FROM inserted
	OPEN cur;
	FETCH NEXT FROM cur INTO @SessionId, @CourseId;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Get the next session number
		SELECT @SessionNumber = COUNT(*)
		FROM UniversityAlma.Session
		WHERE CourseId = @CourseId AND SessionId <= @SessionId;

		UPDATE UniversityAlma.Session
		SET Number = @SessionNumber
		WHERE SessionId = @SessionId;
		
		FETCH NEXT FROM cur INTO @SessionId, @CourseId;
	END

	CLOSE cur;
	DEALLOCATE cur;
END;
GO

-- Resequence numbers on session delete
CREATE TRIGGER UniversityAlma.trigDeleteSessionNumber
ON UniversityAlma.Session
AFTER DELETE
AS
BEGIN
	DECLARE @CourseId INT;
	DECLARE cur CURSOR FOR
	SELECT DISTINCT CourseId
	FROM deleted;

	OPEN cur;
	FETCH NEXT FROM cur INTO @CourseId;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC UniversityAlma.ReSequenceSessionNumbers @CourseId;
		FETCH NEXT FROM cur INTO @CourseId;
	END
	CLOSE cur;
	DEALLOCATE cur;
END;
GO