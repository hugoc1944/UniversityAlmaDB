-- View to get all course details
DROP VIEW IF EXISTS UniversityAlma.vwCourseDetails;
GO
CREATE VIEW UniversityAlma.vwCourseDetails
WITH SCHEMABINDING
AS
SELECT
	c.CourseId,
	c.Title,
	c.Description,
	c.CategoryId,
	c.MentorId,
	m.Experience AS MentorExperience,
    COUNT_BIG(s.SessionId) AS SessionCount,
	c.FavCount AS FavCount,
	COUNT_BIG(*) AS CountBigAll,  -- Required for indexed views
	c.IsDeleted
FROM 
    UniversityAlma.Course c
JOIN 
    UniversityAlma.Mentor m ON c.MentorId = m.MentorId
JOIN 
    UniversityAlma.Session s ON c.CourseId = s.CourseId
GROUP BY
    c.CourseId,
    c.Title,
    c.Description,
    c.CategoryId,
    c.MentorId,
    m.Experience,
    c.FavCount,
	c.IsDeleted;
GO
CREATE UNIQUE CLUSTERED INDEX IX_vwCourseDetails_CourseId
ON UniversityAlma.vwCourseDetails (CourseId);
GO

-- View for User Details
DROP VIEW IF EXISTS UniversityAlma.vwUserDetails;
GO
CREATE VIEW UniversityAlma.vwUserDetails
AS
SELECT
	p.ProfileId,
	p.Name,
	p.Username,
	DATEDIFF(YEAR, p.Birthday, GETDATE()) AS Age,
	p.Email,
	CASE WHEN m.UserId IS NOT NULL THEN 'True' ELSE 'False' END AS Mentor
FROM UniversityAlma.Profile p
JOIN UniversityAlma.[User] u ON p.ProfileId = u.ProfileId
LEFT JOIN UniversityAlma.Mentor m ON u.UserId = m.UserId;
GO
