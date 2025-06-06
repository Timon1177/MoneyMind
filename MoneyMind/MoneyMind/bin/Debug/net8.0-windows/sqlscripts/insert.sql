DELETE FROM Income
WHERE fk_userID = (SELECT id FROM users WHERE Username = 'TestUser');

DELETE FROM Expenses
WHERE fk_userID = (SELECT id FROM users WHERE Username = 'TestUser');

DELETE FROM SavingGoals
WHERE fk_userID = (SELECT id FROM users WHERE Username = 'TestUser');

DELETE FROM Feedback
WHERE Email = 'test@example.com';

DELETE FROM users WHERE Username = 'TestUser';

INSERT INTO users (firstname, lastname, Username, Password, Balance, email) 
VALUES ('Max', 'Mustermann', 'TestUser', 'TestPW', 0, 'test@test.ch');

INSERT INTO Income (fk_userID, Amount, Category)
VALUES (
    (SELECT id FROM users WHERE Username = 'TestUser'),
    500, 'Salary'
);

INSERT INTO Expenses (fk_userID, Amount, Type, Category)
VALUES (
    (SELECT id FROM users WHERE Username = 'TestUser'),
    200, 'Fixed', 'Rent'
);

INSERT INTO SavingGoals (GoalName, TargetAmount, DeadLine, fk_userID)
VALUES (
    'Vacation', 1500, '2025-12-31',
    (SELECT id FROM users WHERE Username = 'TestUser')
);

INSERT INTO Feedback (Title, Message, Email)
VALUES 
('Test Feedback', 'This is a test feedback message.', 'test@example.com');