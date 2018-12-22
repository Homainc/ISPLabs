USE master;
GO

IF NOT EXISTS
   (  SELECT [name] 
      FROM sys.tables
      WHERE [name] = [dbo].[Partition] 
   )
	CREATE TABLE [dbo].[Partition] (
		[Id] INT NOT NULL,
		[Name] NVARCHAR(255),
		CONSTRAINT PK_PARTITION PRIMARY KEY (Id)
	);

IF NOT EXISTS
   (  SELECT [name] 
      FROM sys.tables
      WHERE [name] = [dbo].[Role] 
   )
	CREATE TABLE [dbo].[Role] (
		[Id] INT NOT NULL,
		[Name] NVARCHAR(255),
		CONSTRAINT PK_ROLE PRIMARY KEY (id)
	);

IF NOT EXISTS
   (  SELECT [name] 
      FROM sys.tables
      WHERE [name] = [dbo].[User] 
   )
	CREATE TABLE [dbo].[User] (
		[Id] INT NOT NULL,
		[Login] NVARCHAR(255),
		[Email] NVARCHAR(255),
		[Password] NVARCHAR(255),
		[RegistrationDate] DATETIME,
		[Role_id] INT,
		CONSTRAINT PK_USER PRIMARY KEY (Id),
		CONSTRAINT FK_USER_ROLE_ID FOREIGN KEY (Role_id)
			REFERENCES [dbo].[Role](id)
	);

IF NOT EXISTS
   (  SELECT [name] 
      FROM sys.tables
      WHERE [name] = [dbo].[Category] 
   )
	CREATE TABLE [dbo].[Category] (
		[Id] INT NOT NULL,
		[Partition_id] INT,
		[Name] NVARCHAR(255),
		[Description] NVARCHAR(255),
		CONSTRAINT PK_CATEGORY PRIMARY KEY (Id),
		CONSTRAINT FK_CATEGORY_PARTITION_ID FOREIGN KEY (Partition_id)
			REFERENCES [dbo].[Partition](Id)
	);

IF NOT EXISTS
   (  SELECT [name] 
      FROM sys.tables
      WHERE [name] = [dbo].[Partition] 
   )
	CREATE TABLE [dbo].[Topic] (
		[Id] INT NOT NULL,
		[Category_id] INT,
		[User_id] INT,
		[Name] NVARCHAR(255),
		[Date] DATETIME,
		[IsClosed] BIT,
		CONSTRAINT PK_TOPIC PRIMARY KEY (Id),
		CONSTRAINT FK_TOPIC_CATEGORY_ID FOREIGN KEY (Category_id)
			REFERENCES [dbo].[Category](id),
		CONSTRAINT FK_TOPIC_USER_ID FOREIGN KEY (User_id)
			REFERENCES [dbo].[User](Id)
	);

IF NOT EXISTS
   (  SELECT [name] 
      FROM sys.tables
      WHERE [name] = [dbo].[ForumMessage] 
   )
	CREATE TABLE [dbo].[ForumMessage] (
		[Id] INT NOT NULL,
		[Text] NVARCHAR(255),
		[Topic_id] INT,
		[User_id] INT,
		[Date] DATETIME,
		CONSTRAINT PK_FORUMMESSAGE PRIMARY KEY (Id),
		CONSTRAINT FK_FORUMMESSAGE_TOPIC_ID FOREIGN KEY (Topic_id)
			REFERENCES [dbo].[Topic](Id),
		CONSTRAINT FK_FORUMMESSAGE_USER_ID FOREIGN KEY (User_id)
			REFERENCES [dbo].[User](Id)
);