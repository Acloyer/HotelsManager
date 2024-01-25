create database HotelsDB

use HotelsDB

create table Hotels (
    [Id] int primary key identity,
    [Name] nvarchar(100),
    [Price] money,
);