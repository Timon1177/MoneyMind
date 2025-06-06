PRAGMA foreign_keys=off;

CREATE TABLE IF NOT EXISTS users_new (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL,
    Password TEXT NOT NULL,
    Balance DECIMAL DEFAULT 0,
    firstname TEXT,
    lastname TEXT,
    email TEXT
);

INSERT INTO users_new (id, Username, Password, Balance)
SELECT id, Username, Password, Balance FROM users;

DROP TABLE users;
ALTER TABLE users_new RENAME TO users;
