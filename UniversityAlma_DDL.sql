-- INSERT THE FOLLOWING DATA:
-- 4 User profiles where 1 is an admin, 1 is a mentor and 2 are regular users
-- Insert one certificate to the mentor profile
-- Insert three categories
-- Insert two courses using two different categories
-- Insert 2 sessions to course 1 and 3 sessions to course 2
-- Insert one course to an user favorites, and one course to an admin favorites
-- Insert an audit to an admin action (Change description of a course)
-- Insert a notification to the mentor informing that its course had the description changed by an admin

-- Insert 4 Profiles - 1 admin 1 mentor 2 users
INSERT INTO UniversityAlma.Profile (Name, Password, Email, Username, PhoneNumber, Gender, ProfilePic, Birthday, MailList) VALUES
( 'John', '12345', 'admin@gmail.com', 'john23', '910910910', 'Male', NULL, '1995-01-02', 0),
( 'Peter', '54321', 'mentor@gmail.com', 'peter_mentor', '920920920', 'Male', NULL, '1990-05-01', 0),
( 'Mary', '321321', 'mary1@gmail.com', 'mary91', '930930930', 'Female', NULL, '1991-03-01', 0),
( 'Martin', 'password123', 'martin9@gmail.com', 'martin_pete', '910900923', 'Male', NULL, '2001-05-11', 0);
GO

-- Insert 1 admin
INSERT INTO UniversityAlma.Admin (UserId) VALUES
(1);
GO

-- Insert 1 mentor
INSERT INTO UniversityAlma.Mentor (UserId, Experience, Verified) VALUES
(2, '5 years of mentoring experience', 1);
GO

-- Insert mentor certificate
INSERT INTO UniversityAlma.Certificates ( MentorId, Title) VALUES
(1, 'Bachelor degree in psychology - University of Aveiro');
GO

-- Insert categories
INSERT INTO UniversityAlma.Category (CategoryId, Type) VALUES
(1, 'Calm'),
(2, 'Sleep'),
(3, 'Focus');
GO

-- Insert courses
INSERT INTO UniversityAlma.Course (Title, Description, CategoryId, MentorId) VALUES
('Listen & Relax', 'A combination of listening sessions for your mind.', 1, 1),
('Sleeping Audios', 'Audios to calm your mind. Listen in a quiet and relaxing ambient.', 2, 1);
GO

-- Insert sessions
INSERT INTO UniversityAlma.Session (CourseId, Title, Media, Duration) VALUES
(1, 'Ocean Sounds', 'ocean_sounds.mp3', 93),
(1, 'Heavy Raining', 'raining.mp3', 60),
(2, 'Whale Sounds', 'whales.mp3', 89),
(2, 'Waves', 'waves.mp3', 90),
(2, 'Windy Hills', 'wind.mp3', 105);
GO

-- Insert favorites - User1 fav course 1, Admin fav course 2
INSERT INTO UniversityAlma.Favorites(ProfileId, CourseId) VALUES
(3, 1),
(1, 2);
GO

-- Insert history - User1 has history in course 1, session 2, elapsedTime=50
INSERT INTO UniversityAlma.History(ProfileId, CourseId, SessionNumber, ElapsedTime) VALUES
(3, 1, 2, 50);
GO

-- Insert audit types
INSERT INTO UniversityAlma.AuditTypes (AuditTypeID, AuditTypeName) VALUES
(1, 'Course Update'),
(2, 'Mentor Verification'),
(3, 'User Moderation');
GO

-- Insert audit - Admin performed an action on Course 2
INSERT INTO UniversityAlma.Audits (AdminId, UserId, CourseId, AuditTypeId, Date) VALUES
(1, 1, 2, 1, '2024-05-20');
GO

-- Insert data into notification
-- Notifications should have the audit notification