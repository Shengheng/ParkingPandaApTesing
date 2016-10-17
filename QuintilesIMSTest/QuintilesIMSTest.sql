USE master;
GO
CREATE DATABASE QuintilesIMSTest;

Use QuintilesIMSTest;
GO

-- DDL
CREATE TABLE Tb_Batch(Batch_Id int  PRIMARY KEY, Requestor nvarchar(max) NOT NULL, Request_Data_Time varchar(max) NOT NULL, Request_Status nvarchar(max) NOT NULL, Request_For_System nvarchar(max) NOT NULL,);

CREATE TABLE Tb_Batch_Item(Batch_Id int, Item int PRIMARY KEY IDENTITY, Name nvarchar(max) NOT NULL, Email nvarchar(max) NOT NULL, Init_Password nvarchar(max) NOT NULL,[Role] nvarchar(max) NOT NULL, Reason_For_Access nvarchar(max) NOT NULL,
	FOREIGN KEY (Batch_Id)  REFERENCES Tb_Batch(Batch_Id));

-- DML
INSERT INTO Tb_Batch(Batch_Id,Requestor,Request_Data_Time,Request_Status,Request_For_System)
	VALUES(518, 'bmore', '10/27/2013 12:34:23 PM', 'Queued', 'ClinOp');

INSERT INTO Tb_Batch_Item(Batch_Id,Name,Email,Init_Password,Role,Reason_For_Access)
	VALUES (518, 'Susan Smith', 'ssmith@company1.com', 'susan12%#?', 'SuperUser', 'Because I am "cool", I can do whatever I want.'),
					(518, 'Alex O''Connor',  'alexoconnor@univ1.edu', 'itsuniv1','ReadOnly', 'I need to access report for budget < 1M $'),
					(518, 'John J. Peterson', 'john.p@comany2.com', 'J.Pe1234!', 'Auditor', 'Access to 1) all reports; 2)server system logs for "Audit"and [app]_Access_Log'),
					(518, N'Chen, Mei 陈梅', 'chehmei12@123.com', '<:-)>{;=0}', 'ReadOnly', N'我负责中国分公司财务');

