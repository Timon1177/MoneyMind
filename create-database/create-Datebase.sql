use master;
go

drop database if exists MoneyMind;
go

create database MoneyMind;
go

use MoneyMind;
go

create table user(
	userID int identity(1,1) primary key,
	username varchar(255) not null,
	email varchar(255) unique not null, 
	password varchar(255) not null,
)

create table categories (
	categoryID int identity(1,1) primary key,
	category_name varchar(255) not null, 
	priority int,
)

create table income (
	incomeID int 
)