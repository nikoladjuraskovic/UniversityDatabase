CREATE TABLE [dbo].[Students] (
    [StudentID] INT           IDENTITY (1, 1) NOT NULL,
    [LastName]  NVARCHAR (40) NOT NULL,
    [FirstName] NVARCHAR (20) NOT NULL,
    [Year]      INT           NOT NULL,
    CONSTRAINT [PK_dbo.Students] PRIMARY KEY CLUSTERED ([StudentID] ASC)
);

    CREATE TABLE [dbo].[Courses] (
    [CourseID] INT            IDENTITY (1, 1) NOT NULL,
    [Title]    NVARCHAR (MAX) NULL,
    [Points]   INT            NOT NULL,
    CONSTRAINT [PK_dbo.Courses] PRIMARY KEY CLUSTERED ([CourseID] ASC)
);

    CREATE TABLE [dbo].[Exams] (
    [ExamID]    INT        IDENTITY (1, 1) NOT NULL,
    [CourseID]  INT        NOT NULL,
    [StudentID] INT        NOT NULL,
    [Grade]     FLOAT (53) NULL,
    CONSTRAINT [PK_dbo.Exams] PRIMARY KEY CLUSTERED ([ExamID] ASC),
    CONSTRAINT [FK_dbo.Exams_dbo.Courses_CourseID] FOREIGN KEY ([CourseID]) REFERENCES [dbo].[Courses] ([CourseID]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.Exams_dbo.Students_StudentID] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Students] ([StudentID]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_CourseID]
    ON [dbo].[Exams]([CourseID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StudentID]
    ON [dbo].[Exams]([StudentID] ASC);

     
INSERT INTO Students (LastName, FirstName, Year)
        VALUES ('Carson', 'Alexander', 1);

INSERT INTO Students (LastName, FirstName, Year)
        VALUES ('Meredith', 'Alonso', 1);

INSERT INTO Students (LastName, FirstName, Year)
        VALUES ('Arturo', 'Anand', 2);

INSERT INTO Students (LastName, FirstName, Year)
        VALUES ('Gytis', 'Barzdukas', 2);

INSERT INTO Students (LastName, FirstName, Year)
        VALUES ('Yan', 'Li', 3);

INSERT INTO Students (LastName, FirstName, Year)
        VALUES ('Peggy', 'Justice', 3);

INSERT INTO Students (LastName, FirstName, Year)
        VALUES ('Laura', 'Norman', 4);

INSERT INTO Students (LastName, FirstName, Year)
        VALUES ('Nino', 'Olivetto', 4);



INSERT INTO Courses (Title, Points)
        VALUES ('Chemistry', 3);

INSERT INTO Courses (Title, Points)
        VALUES ('Microeconomics', 3);

INSERT INTO Courses (Title, Points)
        VALUES ('Macroeconomics', 3);

INSERT INTO Courses (Title, Points)
        VALUES ('Calculus', 4);

INSERT INTO Courses (Title, Points)
        VALUES ('Trigonometry', 4);

INSERT INTO Courses (Title, Points)
        VALUES ('Composition', 3);

INSERT INTO Courses (Title, Points)
        VALUES ('Literature', 4);


INSERT INTO Exams (StudentID, CourseID, Grade)
	VALUES (1,1,1);

INSERT INTO Exams (StudentID, CourseID, Grade)
	VALUES (1,2,3);

INSERT INTO Exams (StudentID, CourseID, Grade)
	VALUES (1,3,1);

INSERT INTO Exams (StudentID, CourseID, Grade)
	VALUES (2,4,2);

INSERT INTO Exams (StudentID, CourseID, Grade)
	VALUES (2,5,4);

INSERT INTO Exams (StudentID, CourseID, Grade)
	VALUES (2,6,4);

INSERT INTO Exams (StudentID, CourseID)
	VALUES (3,1);

INSERT INTO Exams (StudentID, CourseID)
	VALUES (4,1);

INSERT INTO Exams (StudentID, CourseID, Grade)
	VALUES (4,2,4);

INSERT INTO Exams (StudentID, CourseID, Grade)
	VALUES (5,3,3);

INSERT INTO Exams (StudentID, CourseID)
	VALUES (6,4);

INSERT INTO Exams (StudentID, CourseID, Grade)
	VALUES (7,5,2);