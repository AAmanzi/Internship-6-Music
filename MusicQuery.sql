BEGIN TRANSACTION;

CREATE TABLE Artists
(
	ArtistId int PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(30) NOT NULL,
	Nationality nvarchar(15)
);

CREATE TABLE Albums
(
	AlbumId int PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(20) NOT NULL,
	ReleaseYear char(4) NOT NULL
);

CREATE TABLE Songs
(
	SongId int PRIMARY KEY IDENTITY(1,1),
	Name nvarchar(15),
	Duration time NOT NULL
);

CREATE TABLE SongsOnAlbums
(
	SongId int FOREIGN KEY REFERENCES Songs(SongId) NOT NULL,
	AlbumId int FOREIGN KEY REFERENCES Albums(AlbumId) NOT NULL
);
COMMIT;

ALTER TABLE dbo.Songs
ALTER COLUMN Name nvarchar(30) not NULL

ALTER TABLE dbo.Albums
ADD ArtistId int FOREIGN KEY REFERENCES Artists(ArtistId) NOT NULL

INSERT INTO dbo.Albums
(
    --AlbumId - this column value is auto-generated
    Name,
    ReleaseYear,
    ArtistId
)
VALUES
(
        -- AlbumId - int
        N'Mali Gonzales', -- Name - nvarchar
        '1967', -- ReleaseYear - datetime2
        1 -- ArtistId - int
),
(
        -- AlbumId - int
        N'Zvezdice na nebu', -- Name - nvarchar
        '1965', -- ReleaseYear - datetime2
        1 -- ArtistId - int
),
(
        -- AlbumId - int
        N'Babaroga', -- Name - nvarchar
        '1991', -- ReleaseYear - datetime2
        2 -- ArtistId - int
),
(
        -- AlbumId - int
        N'Maskarada', -- Name - nvarchar
        '1997', -- ReleaseYear - datetime2
        2 -- ArtistId - int
),
(
        -- AlbumId - int
        N'CSS', -- Name - nvarchar
        '1998', -- ReleaseYear - datetime2
        3 -- ArtistId - int
),
(
        -- AlbumId - int
        N'GitHub', -- Name - nvarchar
        '2017', -- ReleaseYear - datetime2
        3 -- ArtistId - int
)

INSERT INTO dbo.Artists
(
    --ArtistId - this column value is auto-generated
    Name,
    Nationality
)
VALUES
(
        -- ArtistId - int
        N'Dušan Jakšić', -- Name - nvarchar
        N'Serbian' -- Nationality - nvarchar
),
(
        -- ArtistId - int
        N'Ceca', -- Name - nvarchar
        N'Serbian' -- Nationality - nvarchar
),
(
        -- ArtistId - int
        N'Mario Čeprnja', -- Name - nvarchar
        N'Croatian' -- Nationality - nvarchar
)

INSERT INTO dbo.Songs
(
    --SongId - this column value is auto-generated
    Name,
    Duration
)
VALUES
(
        -- SongId - int
        N'Babaroga', -- Name - nvarchar
        '00:04:43' -- Duration - time
),
(
        -- SongId - int
        N'Mokra trava', -- Name - nvarchar
        '00:03:15' -- Duration - time
),
(
        -- SongId - int
        N'Izbriši, vetre, trag', -- Name - nvarchar
        '00:03:54' -- Duration - time
),
(
        -- SongId - int
        N'Da si nekad do bola voleo', -- Name - nvarchar
        '00:04:46' -- Duration - time
),
(
        -- SongId - int
        N'Hej, vršnjaci', -- Name - nvarchar
        '00:03:40' -- Duration - time
),
(
        -- SongId - int
        N'Mokra trava', -- Name - nvarchar
        '00:03:15' -- Duration - time
),
(
        -- SongId - int
        N'Vazduh koji dišem', -- Name - nvarchar
        '00:03:08' -- Duration - time
),
(
        -- SongId - int
        N'Neću da budem ko mašina', -- Name - nvarchar
        '00:02:54' -- Duration - time
),
(
        -- SongId - int
        N'Popij me kao lek', -- Name - nvarchar
        '00:03:27' -- Duration - time
),
(
        -- SongId - int
        N'Crni sneg', -- Name - nvarchar
        '00:03:47' -- Duration - time
),
(
        -- SongId - int
        N'Ja sam simpatičan', -- Name - nvarchar
        '00:02:49' -- Duration - time
),
(
        -- SongId - int
        N'Žaklin', -- Name - nvarchar
        '00:02:27' -- Duration - time
),
(
        -- SongId - int
        N'Luna nad morem', -- Name - nvarchar
        '00:04:41' -- Duration - time
),
(
        -- SongId - int
        N'Maliziusella', -- Name - nvarchar
        '00:01:37' -- Duration - time
),
(
        -- SongId - int
        N'Sevastopoljski val', -- Name - nvarchar
        '00:03:13' -- Duration - time
),
(
        -- SongId - int
        N'Podmoskovske večeri', -- Name - nvarchar
        '00:03:10' -- Duration - time
),
(
        -- SongId - int
        N'Melodije d amour', -- Name - nvarchar
        '00:02:27' -- Duration - time
),
(
        -- SongId - int
        N'Jesenja elegija', -- Name - nvarchar
        '00:02:39' -- Duration - time
),
(
        -- SongId - int
        N'Sneg', -- Name - nvarchar
        '00:02:17' -- Duration - time
),
(
        -- SongId - int
        N'Dečak iz predgradja', -- Name - nvarchar
        '00:03:24' -- Duration - time
),
(
        -- SongId - int
        N'Numb', -- Name - nvarchar
        '00:03:08' -- Duration - time
),
(
        -- SongId - int
        N'Hurt', -- Name - nvarchar
        '00:04:03' -- Duration - time
),
(
        -- SongId - int
        N'Yesterday', -- Name - nvarchar
        '00:02:03' -- Duration - time
),
(
        -- SongId - int
        N'A kada boli', -- Name - nvarchar
        '00:04:31' -- Duration - time
),
(
        -- SongId - int
        N'Waiting around to die', -- Name - nvarchar
        '00:05:15' -- Duration - time
),
(
        -- SongId - int
        N'Pull it', -- Name - nvarchar
        '00:02:11' -- Duration - time
),
(
        -- SongId - int
        N'Push it', -- Name - nvarchar
        '00:03:28' -- Duration - time
),
(
        -- SongId - int
        N'Committed', -- Name - nvarchar
        '00:02:42' -- Duration - time
),
(
        -- SongId - int
        N'My computer is dying', -- Name - nvarchar
        '00:02:22' -- Duration - time
),
(
        -- SongId - int
        N'Bits for brunch', -- Name - nvarchar
        '00:04:03' -- Duration - time
)

INSERT INTO dbo.SongsOnAlbums
(
    SongId,
    AlbumId
)
VALUES
(
        1010, -- SongId - int
        3 -- AlbumId - int
),
(
        1011, -- SongId - int
        3 -- AlbumId - int
),
(
        1012, -- SongId - int
        3 -- AlbumId - int
),
(
        1013, -- SongId - int
        3 -- AlbumId - int
),
(
        1014, -- SongId - int
        3 -- AlbumId - int
),
(
        1015, -- SongId - int
        4 -- AlbumId - int
),
(
        1016, -- SongId - int
        4 -- AlbumId - int
),
(
        1017, -- SongId - int
        4 -- AlbumId - int
),
(
        1018, -- SongId - int
        4 -- AlbumId - int
),
(
        1019, -- SongId - int
        4 -- AlbumId - int
),
(
        1020, -- SongId - int
        1 -- AlbumId - int
),
(
        1021, -- SongId - int
        1 -- AlbumId - int
),
(
        1022, -- SongId - int
        1 -- AlbumId - int
),
(
        1023, -- SongId - int
        1 -- AlbumId - int
),
(
        1024, -- SongId - int
        1 -- AlbumId - int
),
(
        1025, -- SongId - int
        2 -- AlbumId - int
),
(
        1026, -- SongId - int
        2 -- AlbumId - int
),
(
        1027, -- SongId - int
        2 -- AlbumId - int
),
(
        1028, -- SongId - int
        2 -- AlbumId - int
),
(
        1029, -- SongId - int
        2 -- AlbumId - int
),
(
        1030, -- SongId - int
        5 -- AlbumId - int
),
(
        1031, -- SongId - int
        5 -- AlbumId - int
),
(
        1032, -- SongId - int
        5 -- AlbumId - int
),
(
        1033, -- SongId - int
        5 -- AlbumId - int
),
(
        1034, -- SongId - int
        5 -- AlbumId - int
),
(
        1035, -- SongId - int
        6 -- AlbumId - int
),
(
        1036, -- SongId - int
        6 -- AlbumId - int
),
(
        1037, -- SongId - int
        6 -- AlbumId - int
),
(
        1038, -- SongId - int
        6 -- AlbumId - int
),
(
        1039, -- SongId - int
        6 -- AlbumId - int
)