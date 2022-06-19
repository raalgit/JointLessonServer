IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'JointLessonDB')
BEGIN
  CREATE DATABASE JointLessonDB;
END;