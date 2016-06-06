if not exists (select * from sys.databases where name = 'Omega')
begin
CREATE DATABASE Omega;
end;
GO

USE Omega;
GO

if exists(select * from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'dbo' and TABLE_NAME = 'tMusicList') 
begin
	drop table dbo.tMusicList;
end;

Create table dbo.tMusicList(
	id varchar(30) primary key,
	danceability varchar(30),
	energy varchar(30),
	loudness varchar(30),
	mode varchar(30),
	speechiness varchar(30),
	acousticness varchar(30),
	instrumentalness varchar(30),
	liveness varchar(30),
	valence varchar(30),
	tempo varchar(30),
	[type] varchar(30),
);