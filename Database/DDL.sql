CREATE TABLE question_type (
    type_id int PRIMARY KEY,
    type_name nvarchar(255) NOT NULL
);

CREATE TABLE question (
    question_id int IDENTITY (1, 1) PRIMARY KEY,
    question_text nvarchar(255) NOT NULL,
    question_order int NOT NULL CHECK (question_order >= 1),
    type_id int NOT NULL FOREIGN KEY REFERENCES question_type (type_id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE star_question (
    question_id int PRIMARY KEY FOREIGN KEY REFERENCES question (question_id) ON DELETE CASCADE ON UPDATE CASCADE,
    num_of_stars int DEFAULT 1 CHECK (
        num_of_stars >= 1
        AND num_of_stars <= 10
    )
);

CREATE TABLE smiley_question (
    question_id int PRIMARY KEY FOREIGN KEY REFERENCES question (question_id) ON DELETE CASCADE ON UPDATE CASCADE,
    num_of_faces int DEFAULT 2 CHECK (
        num_of_faces >= 2
        AND num_of_faces <= 5
    )
);

CREATE TABLE slider_question (
    question_id int PRIMARY KEY FOREIGN KEY REFERENCES question (question_id) ON DELETE CASCADE ON UPDATE CASCADE,
    start_value int DEFAULT 0,
    end_value int DEFAULT 100,
    start_value_caption nvarchar(255) NOT NULL,
    end_value_caption nvarchar(255) NOT NULL,
    CONSTRAINT check_value CHECK (
        start_value >= 0
        AND end_value <= 100
        AND start_value < end_value
    )
);