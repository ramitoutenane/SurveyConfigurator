--Create database named Survey
IF NOT EXISTS(
    SELECT
        *
    FROM
        sys.databases
    WHERE
        NAME = 'Survey'
) BEGIN CREATE DATABASE [Survey]
END
GO

--Use the database to add the tables to
    USE [Survey]
GO

--Add tables to database
    IF NOT EXISTS (
        SELECT
            *
        FROM
            sysobjects
        WHERE
            NAME = 'QuestionType'
            And xtype = 'U'
    ) BEGIN CREATE TABLE QuestionType (
        TypeID int PRIMARY KEY,
        TypeName nvarchar(25) NOT NULL
    );

END 
    IF NOT EXISTS (
        SELECT
            *
        FROM
            sysobjects
        WHERE
            name = 'Question'
            and xtype = 'U'
    ) BEGIN CREATE TABLE Question (
        QuestionId int IDENTITY (1, 1) PRIMARY KEY,
        QuestionText nvarchar(100) NOT NULL,
        QuestionOrder int NOT NULL CHECK (QuestionOrder >= 1),
        TypeID int NOT NULL FOREIGN KEY REFERENCES QuestionType (TypeID) ON DELETE CASCADE ON UPDATE CASCADE
    );

END 
    IF NOT EXISTS (
        SELECT
            *
        FROM
            sysobjects
        WHERE
            name = 'StarQuestion'
            and xtype = 'U'
    ) BEGIN CREATE TABLE StarQuestion (
        QuestionId int PRIMARY KEY FOREIGN KEY REFERENCES Question (QuestionId) ON DELETE CASCADE ON UPDATE CASCADE,
        NumOfStars int NOT NULL CHECK (
            NumOfStars >= 1
            AND NumOfStars <= 10
        )
    );

END 
    IF NOT EXISTS (
        SELECT
            *
        FROM
            sysobjects
        WHERE
            name = 'SmileyQuestion'
            and xtype = 'U'
    ) BEGIN CREATE TABLE SmileyQuestion (
        QuestionId int PRIMARY KEY FOREIGN KEY REFERENCES Question (QuestionId) ON DELETE CASCADE ON UPDATE CASCADE,
        NumOfFaces int NOT NULL CHECK (
            NumOfFaces >= 2
            AND NumOfFaces <= 5
        )
    );

END 
    IF NOT EXISTS (
        SELECT
            *
        FROM
            sysobjects
        WHERE
            name = 'SliderQuestion'
            and xtype = 'U'
    ) BEGIN CREATE TABLE SliderQuestion (
        QuestionId int PRIMARY KEY FOREIGN KEY REFERENCES Question (QuestionId) ON DELETE CASCADE ON UPDATE CASCADE,
        StartValue int NOT NULL,
        EndValue int NOT NULL,
        StartValueCaption nvarchar(25) NOT NULL,
        EndValueCaption nvarchar(25) NOT NULL,
        CONSTRAINT CheckValue CHECK (
            StartValue >= 0
            AND EndValue <= 100
            AND StartValue < EndValue
        )
    );

END 

--Insert initial data to database
    IF NOT EXISTS (
        SELECT
            1
        FROM
            QuestionType
    ) BEGIN
    INSERT INTO
        QuestionType
    VALUES
        (1, 'smiley'),
        (2, 'slider'),
        (3, 'stars');

END