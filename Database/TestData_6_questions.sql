INSERT INTO
    Question(QuestionText, QuestionOrder, TypeId)
Values
    ('Smiley Question  1', 1, 1),
    ('Smiley Question  2', 2, 1),
    ('Slider Question  1', 3, 2),
    ('Slider Question  2', 3, 2),
    ('Stars Question  1', 2, 3),
    ('Stars Question  2', 1, 3);

INSERT INTO
    SmileyQuestion
Values
    (1, 2),
    (2, 2);

INSERT INTO
    StarQuestion
Values
    (5, 3),
    (6, 4);

INSERT INTO
    SliderQuestion (
        QuestionId,
        StartValue,
        EndValue,
        StartValueCaption,
        EndValueCaption
    )
VALUES
    (3, 1, 100, 'start1', 'end1'),
    (4, 10, 50, 'start2', 'end2');