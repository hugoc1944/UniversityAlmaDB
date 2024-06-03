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

DROP PROCEDURE IF EXISTS UniversityAlma.RegisterUser;
GO
-- User login and register procedures
-- Register
CREATE PROCEDURE UniversityAlma.RegisterUser
	@Name VARCHAR(100),
	@Password VARCHAR(120),
	@Email VARCHAR(100),
	@Username VARCHAR(50),
	@PhoneNumber VARCHAR(15),
	@Gender VARCHAR(10),
	@ProfilePic VARBINARY(MAX) = NULL, --Default is null
	@Birthday DATE,
	@MailList BIT,
	@UserId INT OUTPUT,
	@ReturnCode INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	-- Check if username exists
	IF EXISTS (SELECT 1 FROM UniversityAlma.Profile WHERE Username = @Username)
	BEGIN
		SET @ReturnCode = -1;
		RETURN; -- Username exists
	END
	-- Check if email exists
	IF EXISTS (SELECT 1 FROM UniversityAlma.Profile WHERE Email = @Email)
	BEGIN
		SET @ReturnCode = -2;
		RETURN; -- Email exists
	END

	-- Insert the new profile
	INSERT INTO UniversityAlma.Profile(Name, Password, Email, Username, PhoneNumber, Gender, ProfilePic, Birthday, MailList)
	VALUES(@Name, @Password, @Email, @Username, @PhoneNumber, @Gender, @ProfilePic, @Birthday, @MailList);
	-- Retrieve the new profileId
	DECLARE @ProfileId INT;
	SET @ProfileId = SCOPE_IDENTITY();
	-- Retrieve the UserId based on this new ProfileId
	SELECT @UserId = UserId FROM UniversityAlma.[User] WHERE ProfileId = @ProfileId;
	-- Return the new userId
	SET @ReturnCode = 0; -- Registration successful
END;
GO
--Login
DROP PROCEDURE IF EXISTS UniversityAlma.LoginUser;
GO
CREATE PROCEDURE UniversityAlma.LoginUser
	@Username VARCHAR(50),
	@Password VARCHAR(120),
	@UserId INT OUTPUT,
	@ReturnCode INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	SET @UserId= NULL;
	SET @ReturnCode = -1;
	-- Check if username and password match
	IF EXISTS (SELECT 1 FROM UniversityAlma.Profile WHERE Username = @Username AND Password = @Password)
	BEGIN
		-- Retrieve UserId
		SELECT @UserId = UserId
		FROM UniversityAlma.[User]
		WHERE ProfileId = (SELECT ProfileId FROM UniversityAlma.Profile WHERE Username = @Username);
	
		SET @ReturnCode = 0; -- Login successful
	END
	ELSE
	BEGIN
		-- Return null because the credentials are invalid
		SET @ReturnCode = -1; -- Login failed
	END
END;
GO

-- Add favorite
DROP PROCEDURE IF EXISTS UniversityAlma.AddFavorite;
GO
CREATE PROCEDURE UniversityAlma.AddFavorite
	@ProfileId INT,
	@CourseId INT
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT 1 FROM UniversityAlma.Favorites WHERE ProfileId = @ProfileId AND CourseId = @CourseId)
	BEGIN
		-- Insert favorite
		INSERT INTO UniversityAlma.Favorites(ProfileId, CourseId)
		VALUES (@ProfileId, @CourseId);
	END
END;
GO
-- Remove favorite
DROP PROCEDURE IF EXISTS UniversityAlma.RemoveFavorite;
GO
CREATE PROCEDURE UniversityAlma.RemoveFavorite
	@ProfileId INT,
	@CourseId INT
AS
BEGIN
	SET NOCOUNT ON

	-- Check if favorite exists
	IF EXISTS (SELECT 1 FROM UniversityAlma.Favorites WHERE ProfileId = @ProfileId AND CourseId = @CourseId)
	BEGIN
		-- Delete the favorite
		DELETE FROM UniversityAlma.Favorites
		WHERE ProfileId = @ProfileId AND CourseId = @CourseId;
	END
END;
GO