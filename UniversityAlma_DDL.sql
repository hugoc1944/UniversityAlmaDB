-- INSERT THE FOLLOWING DATA:
-- 8 User profiles where 1 is an admin, 2 is a mentor and 5 are regular users
-- Insert one certificate to a mentor profile
-- Insert three categories
-- Insert courses into the categories
-- Insert sessions into categories
-- Insert favourites between the courses
-- Insert an audit to an admin action (Change description of a course)
-- Insert a notification to the mentor informing that its course had the description changed by an admin

-- Insert profiles
INSERT INTO UniversityAlma.Profile (Name, Password, Email, Username, PhoneNumber, Gender, ProfilePic, Birthday, MailList) VALUES
('John', '12345', 'admin@gmail.com', 'john23', '910910910', 'Male', NULL, '1995-01-02', 0),
('Peter', '54321', 'mentor@gmail.com', 'peter_mentor', '920920920', 'Male', NULL, '1990-05-01', 0),
('Mary', '321321', 'mary1@gmail.com', 'mary91', '930930930', 'Female', NULL, '1991-03-01', 0),
('Martin', '12345', 'martin9@gmail.com', 'martin_pete', '910900923', 'Male', NULL, '2001-05-11', 0),
('Alice', '12345', 'alice@gmail.com', 'alice1990', '910910111', 'Female', NULL, '1990-08-10', 0),
('Bob', '12345', 'bob@gmail.com', 'bob1992', '910910112', 'Male', NULL, '1992-06-15', 0),
('Charlie', '12345', 'charlie@gmail.com', 'charlie1994', '910910113', 'Male', NULL, '1994-11-20', 0),
('Diana', '12345', 'diana@gmail.com', 'diana1993', '910910114', 'Female', NULL, '1993-03-25', 0);
GO

-- Insert 1 admin
INSERT INTO UniversityAlma.Admin (UserId) VALUES (1);
GO

-- Insert mentors
INSERT INTO UniversityAlma.Mentor (UserId, Experience, Verified) VALUES
(2, '5 years of mentoring experience', 1),
(5, '3 years of mentoring experience', 1);
GO

-- Insert mentor certificate
INSERT INTO UniversityAlma.Certificates (MentorId, Title) VALUES
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
('Sleeping Audios', 'Audios to calm your mind. Listen in a quiet and relaxing ambient.', 2, 1),
('Mindfulness Meditation', 'Guided mindfulness meditation sessions.', 1, 1),
('Stress Relief', 'Techniques and sessions for relieving stress.', 1, 1),
('Peaceful Music', 'Soothing music for peace of mind.', 1, 2), -- Adjusted MentorId
('Yoga Nidra', 'Sessions for deep relaxation and better sleep.', 1, 2), -- Adjusted MentorId
('Deep Sleep', 'Music and sounds to promote deep sleep.', 2, 1),
('Dreamscapes', 'Soundscapes to enhance your dreams.', 2, 2), -- Adjusted MentorId
('Focus Music', 'Music to help you focus.', 3, 1),
('Productive Beats', 'Upbeat music to boost productivity.', 3, 1),
('Concentration Boost', 'Sessions designed to enhance concentration.', 3, 2), -- Adjusted MentorId
('Study Aid', 'Sounds and music to aid in studying.', 3, 2); -- Adjusted MentorId
GO

-- Insert sessions
INSERT INTO UniversityAlma.Session (CourseId, Title, Media, Duration) VALUES
(1, 'Ocean Sounds', 'ocean_sounds.mp3', 93),
(1, 'Heavy Raining', 'raining.mp3', 60),
(2, 'Whale Sounds', 'whales.mp3', 89),
(2, 'Waves', 'waves.mp3', 90),
(2, 'Windy Hills', 'wind.mp3', 105),
(3, 'Mindfulness Basics', 'mindfulness_basics.mp3', 45),
(3, 'Body Scan', 'body_scan.mp3', 50),
(4, 'Breathing Techniques', 'breathing_techniques.mp3', 30),
(4, 'Progressive Muscle Relaxation', 'muscle_relaxation.mp3', 40),
(5, 'Ambient Music', 'ambient_music.mp3', 60),
(5, 'Nature Sounds', 'nature_sounds.mp3', 55),
(6, 'Yoga Nidra Introduction', 'yoga_nidra_intro.mp3', 70),
(6, 'Deep Relaxation', 'deep_relaxation.mp3', 65),
(6, 'Guided Visualization', 'guided_visualization.mp3', 60),
(7, 'Calming Waves', 'calming_waves.mp3', 80),
(7, 'Night Rain', 'night_rain.mp3', 75),
(7, 'Crickets at Night', 'crickets_night.mp3', 60),
(8, 'Forest Dreams', 'forest_dreams.mp3', 90),
(8, 'Mountain Streams', 'mountain_streams.mp3', 85),
(9, 'Focus and Flow', 'focus_flow.mp3', 65),
(9, 'Deep Work', 'deep_work.mp3', 60),
(10, 'Productive Morning', 'productive_morning.mp3', 70),
(10, 'Energetic Afternoon', 'energetic_afternoon.mp3', 75),
(11, 'Intense Concentration', 'intense_concentration.mp3', 55),
(11, 'Calm Focus', 'calm_focus.mp3', 50),
(12, 'Study Session 1', 'study_session_1.mp3', 80),
(12, 'Study Session 2', 'study_session_2.mp3', 85),
(12, 'Exam Prep', 'exam_prep.mp3', 90);
GO

-- Insert favorites
INSERT INTO UniversityAlma.Favorites(ProfileId, CourseId) VALUES
(3, 1),
(1, 2),
(1, 1),
(2, 1),
(2, 2),
(3, 2),
(1, 3),
(2, 3),
(3, 3),
(5, 3),
(1, 4),
(2, 4),
(3, 4),
(5, 4),
(1, 5),
(5, 5),
(1, 6),
(2, 6),
(3, 6),
(4, 6),
(2, 7),
(3, 7),
(1, 8),
(2, 8),
(3, 8),
(4, 8),
(5, 8),
(3, 9),
(4, 9),
(5, 9),
(3, 10),
(4, 10),
(5, 10),
(1, 11),
(2, 11),
(1, 12),
(2, 12),
(5, 12);
GO

-- Insert history
INSERT INTO UniversityAlma.History(ProfileId, CourseId, SessionNumber, ElapsedTime) VALUES
(3, 1, 2, 50);
GO

-- Insert audit types
INSERT INTO UniversityAlma.AuditTypes (AuditTypeID, AuditTypeName) VALUES
(1, 'Course Update'),
(2, 'Mentor Verification'),
(3, 'User Moderation'),
(4, 'Course Deleted');
GO

-- Insert audit
INSERT INTO UniversityAlma.Audits (AdminId, UserId, CourseId, AuditTypeId, Date) VALUES
(1, 1, 2, 1, '2024-05-20');
GO
