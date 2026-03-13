USE football_league;

INSERT INTO clubs (Name, City) VALUES
('CSKA',          'Sofia'),
('Levski',        'Sofia'),
('Ludogorets',    'Razgrad'),
('Botev Plovdiv', 'Plovdiv'),
('Beroe',         'Stara Zagora');

INSERT INTO leagues (Name, Season) VALUES
('First League',   '2024/2025'),
('First League',   '2025/2026'),
('Second League',  '2024/2025');

INSERT INTO players (ClubId, FullName, BirthDate, Position, ShirtNumber, Status) VALUES
(1, 'Ivan Petrov',       '1995-03-12', 'GK', 1,  'Active'),
(1, 'Georgi Ivanov',     '1998-07-24', 'DF', 4,  'Active'),
(1, 'Martin Stoyanov',   '2000-01-15', 'FW', 9,  'Active'),
(2, 'Nikolay Dimitrov',  '1993-11-02', 'GK', 1,  'Active'),
(2, 'Stefan Kolev',      '1997-05-18', 'MF', 8,  'Suspended'),
(2, 'Atanas Todorov',    '2001-09-30', 'FW', 11, 'Active'),
(3, 'Zdravko Popov',     '1994-06-07', 'GK', 1,  'Active'),
(3, 'Valentin Nachev',   '1999-04-22', 'DF', 5,  'Injured'),
(3, 'Kristiyan Marinov', '2002-02-14', 'FW', 7,  'Active'),
(4, 'Boris Yordanov',    '2000-12-03', 'MF', 6,  'Active');

-- First League 2024/2025 — 4 клуба
INSERT INTO league_teams (LeagueId, ClubId) VALUES
(1, 1), (1, 2), (1, 3), (1, 4);

-- First League 2025/2026 — 3 клуба
INSERT INTO league_teams (LeagueId, ClubId) VALUES
(2, 1), (2, 2), (2, 5);

-- Second League 2024/2025 — 2 клуба
INSERT INTO league_teams (LeagueId, ClubId) VALUES
(3, 4), (3, 5);

INSERT INTO transfers (PlayerId, FromClubId, ToClubId, TransferDate, Fee, Note) VALUES
(3, 1, 2, '2024-07-01', 500000.00, 'Summer transfer'),
(8, 3, 4, '2024-08-15', 0.00,      'Free transfer'),
(5, 2, 3, '2024-09-01', 250000.00, 'Mid-season move');

UPDATE players SET ClubId = 2 WHERE PlayerId = 3;
UPDATE players SET ClubId = 4 WHERE PlayerId = 8;
UPDATE players SET ClubId = 3 WHERE PlayerId = 5;
