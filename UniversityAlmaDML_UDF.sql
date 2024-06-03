-- UDF
-- UDF for search courses functionality
CREATE FUNCTION UniversityAlma.fnSearchCourses
(
    @CategoryId INT,
    @SearchTerm VARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
	SELECT CourseId, Title, Description, SessionCount, FavCount
	FROM UniversityAlma.vwCourseDetails
	WHERE CategoryId = @CategoryId
		AND (Title LIKE '%' + @SearchTerm + '%' OR Description LIKE '%' + @SearchTerm + '%')
);
GO