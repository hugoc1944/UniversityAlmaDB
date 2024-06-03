-- Apply indexes to optimize search
CREATE INDEX IX_vwCourseDetails_Title
ON UniversityAlma.vwCourseDetails (Title);
GO
CREATE INDEX IX_vwCourseDetails_Description
ON UniversityAlma.vwCourseDetails (Description);
GO