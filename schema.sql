-- ============================================================
--  schema.sql  –  clubs + players + transfers + leagues + league_teams
--  MySQL / XAMPP phpMyAdmin  |  .NET 8
-- ============================================================

CREATE DATABASE IF NOT EXISTS football_league
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE football_league;

SET FOREIGN_KEY_CHECKS = 0;
DROP TABLE IF EXISTS league_teams;
DROP TABLE IF EXISTS transfers;
DROP TABLE IF EXISTS players;
DROP TABLE IF EXISTS clubs;
DROP TABLE IF EXISTS leagues;
SET FOREIGN_KEY_CHECKS = 1;

-- ── clubs ──────────────────────────────────────────────────
CREATE TABLE clubs (
    ClubId    INT          NOT NULL AUTO_INCREMENT,
    Name      VARCHAR(100) NOT NULL,
    City      VARCHAR(80)  NULL,
    CreatedAt DATETIME     NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_clubs      PRIMARY KEY (ClubId),
    CONSTRAINT uq_clubs_name UNIQUE (Name)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ── leagues ────────────────────────────────────────────────
CREATE TABLE leagues (
    LeagueId INT          NOT NULL AUTO_INCREMENT,
    Name     VARCHAR(100) NOT NULL,
    Season   VARCHAR(20)  NOT NULL,
    CONSTRAINT pk_leagues        PRIMARY KEY (LeagueId),
    CONSTRAINT uq_league_season  UNIQUE (Name, Season)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ── players ────────────────────────────────────────────────
CREATE TABLE players (
    PlayerId    INT          NOT NULL AUTO_INCREMENT,
    ClubId      INT          NOT NULL,
    FullName    VARCHAR(120) NOT NULL,
    BirthDate   DATE         NOT NULL,
    Position    ENUM('GK','DF','MF','FW') NOT NULL,
    ShirtNumber TINYINT      NULL,
    Status      ENUM('Active','Injured','Suspended') NOT NULL DEFAULT 'Active',
    CONSTRAINT pk_players      PRIMARY KEY (PlayerId),
    CONSTRAINT fk_players_club FOREIGN KEY (ClubId)
        REFERENCES clubs(ClubId) ON DELETE RESTRICT ON UPDATE CASCADE,
    CONSTRAINT uq_player_shirt UNIQUE (ClubId, ShirtNumber)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ── transfers ──────────────────────────────────────────────
CREATE TABLE transfers (
    TransferId   INT           NOT NULL AUTO_INCREMENT,
    PlayerId     INT           NOT NULL,
    FromClubId   INT           NULL,
    ToClubId     INT           NOT NULL,
    TransferDate DATE          NOT NULL,
    Fee          DECIMAL(12,2) NULL,
    Note         TEXT          NULL,
    CONSTRAINT pk_transfers       PRIMARY KEY (TransferId),
    CONSTRAINT fk_transfer_player FOREIGN KEY (PlayerId)
        REFERENCES players(PlayerId) ON DELETE RESTRICT,
    CONSTRAINT fk_transfer_from   FOREIGN KEY (FromClubId)
        REFERENCES clubs(ClubId) ON DELETE RESTRICT,
    CONSTRAINT fk_transfer_to     FOREIGN KEY (ToClubId)
        REFERENCES clubs(ClubId) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ── league_teams (many-to-many) ────────────────────────────
CREATE TABLE league_teams (
    LeagueId INT NOT NULL,
    ClubId   INT NOT NULL,
    CONSTRAINT pk_league_teams    PRIMARY KEY (LeagueId, ClubId),
    CONSTRAINT fk_lt_league       FOREIGN KEY (LeagueId)
        REFERENCES leagues(LeagueId) ON DELETE RESTRICT,
    CONSTRAINT fk_lt_club         FOREIGN KEY (ClubId)
        REFERENCES clubs(ClubId) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
