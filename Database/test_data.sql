INSERT INTO
    question(question_text, question_order, type_id)
Values
    ('Question Number 1', 1, 1),
    ('Question Number 2', 2, 2),
    ('Question Number 3', 3, 3),
    ('Question Number 4', 1, 2),
    ('Question Number 5', 1, 3);


INSERT INTO
    smiley_question
Values
    (1, 3);


INSERT INTO
    star_question
Values
    (3, 3),
    (5, 5);


INSERT INTO
    slider_question (
        question_id,
        start_value,
        end_value,
        start_value_caption,
        end_value_caption
    )
VALUES
    (2, 0, 100, 'Q2 start', 'Q2 end'),
    (4, 10, 50, 'Q4 start', 'Q4 end');