CREATE TABLE question_type (
    type_id int PRIMARY KEY,
    type_name nvarchar(255) NOT NULL
);

CREATE TABLE question (
    question_id int IDENTITY (1, 1) PRIMARY KEY,
    question_text nvarchar(255) NOT NULL,
    question_order int NOT NULL,
    type_id int NOT NULL FOREIGN KEY REFERENCES question_type (type_id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE star_question (
    question_id int PRIMARY KEY FOREIGN KEY REFERENCES question (question_id) ON DELETE CASCADE ON UPDATE CASCADE,
    num_of_stars int NOT NULL
);

CREATE TABLE smiley_question (
    question_id int PRIMARY KEY FOREIGN KEY REFERENCES question (question_id) ON DELETE CASCADE ON UPDATE CASCADE,
    num_of_faces int NOT NULL
);

CREATE TABLE slider_question (
    question_id int PRIMARY KEY FOREIGN KEY REFERENCES question (question_id) ON DELETE CASCADE ON UPDATE CASCADE,
    start_value int NOT NULL,
    end_value int Not NULL,
    start_value_caption nvarchar(255) NOT NULL,
    end_value_caption nvarchar(255) NOT NULL
);