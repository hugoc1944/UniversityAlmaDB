--
--
DROP TRIGGER IF EXISTS TrigAuditNotification;
GO
-- Trigger for audit notification
CREATE TRIGGER TrigAuditNotification
ON UniversityAlma.Audits
AFTER INSERT
AS
BEGIN
	-- Insert notification for userId referenced by the audit
	INSERT INTO UniversityAlma.Notification(UserId, Title, Info, Icon)
	SELECT m.UserId, at.AuditTypeName, '', 'alert.png'
	FROM inserted i
	JOIN UniversityAlma.AuditTypes at ON i.AuditTypeId = at.AuditTypeId
	JOIN UniversityAlma.Course c ON i.CourseId = c.CourseId
	JOIN UniversityAlma.Mentor m ON c.MentorId = m.MentorId
	WHERE m.UserId IS NOT NULL;
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

-- FavCount trigger
DROP TRIGGER IF EXISTS trigAddFavorite;
GO
CREATE TRIGGER trigAddFavorite
ON UniversityAlma.Favorites
AFTER INSERT
AS
BEGIN
    UPDATE c
    SET c.FavCount = c.FavCount + i.FavCount
    FROM UniversityAlma.Course c
    JOIN (
        SELECT CourseId, COUNT(*) AS FavCount
        FROM inserted
        GROUP BY CourseId
    ) i ON c.CourseId = i.CourseId;
END;
GO

DROP TRIGGER IF EXISTS trigRemoveFavorite;
GO
CREATE TRIGGER trigRemoveFavorite
ON UniversityAlma.Favorites
AFTER DELETE
AS
BEGIN
    UPDATE c
    SET c.FavCount = c.FavCount - d.FavCount
    FROM UniversityAlma.Course c
    JOIN (
        SELECT CourseId, COUNT(*) AS FavCount
        FROM deleted
        GROUP BY CourseId
    ) d ON c.CourseId = d.CourseId;
END;
GO