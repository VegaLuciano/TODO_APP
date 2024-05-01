CREATE TABLE "User" (
    id SERIAL PRIMARY KEY,
    email TEXT NOT NULL,
    password TEXT NOT NULL
);

CREATE TABLE TypeTask (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL
);

INSERT INTO TypeTask (name) VALUES
    ('normal'),
    ('no rush'),
    ('urgent');
	
 CREATE TABLE Task (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    userId INTEGER,
    date DATE,
    deadline DATE,
    typeId INTEGER,
    description TEXT,
    FOREIGN KEY (userId) REFERENCES "User"(id),
    FOREIGN KEY (typeId) REFERENCES TypeTask(id)
);


