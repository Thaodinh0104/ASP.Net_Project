
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/03/2018 08:47:42
-- Generated from EDMX file: D:\FreeGrapProject\FreeGrab\Models\ModelFreeGrab.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [FreeGrabProject];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Comments_Customers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Customers];
GO
IF OBJECT_ID(N'[dbo].[FK_Comments_Employees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Employees];
GO
IF OBJECT_ID(N'[dbo].[FK_Comments_Grabers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Grabers];
GO
IF OBJECT_ID(N'[dbo].[FK_Comments_Newses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Newses];
GO
IF OBJECT_ID(N'[dbo].[FK_Comments_Patients]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Patients];
GO
IF OBJECT_ID(N'[dbo].[FK_Newses_Customers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Newses] DROP CONSTRAINT [FK_Newses_Customers];
GO
IF OBJECT_ID(N'[dbo].[FK_Employees_Roles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_Employees_Roles];
GO
IF OBJECT_ID(N'[dbo].[FK_HistoryPosts_Employees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HistoryPosts] DROP CONSTRAINT [FK_HistoryPosts_Employees];
GO
IF OBJECT_ID(N'[dbo].[FK_HistoryGrabers_Grapers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HistoryGrabs] DROP CONSTRAINT [FK_HistoryGrabers_Grapers];
GO
IF OBJECT_ID(N'[dbo].[FK_HistoryGrabs_Patients]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HistoryGrabs] DROP CONSTRAINT [FK_HistoryGrabs_Patients];
GO
IF OBJECT_ID(N'[dbo].[FK_HistoryPosts_Newses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HistoryPosts] DROP CONSTRAINT [FK_HistoryPosts_Newses];
GO
IF OBJECT_ID(N'[dbo].[FK_Patients_Hospitals]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Patients] DROP CONSTRAINT [FK_Patients_Hospitals];
GO
IF OBJECT_ID(N'[dbo].[FK_Blogs_Types]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Newses] DROP CONSTRAINT [FK_Blogs_Types];
GO
IF OBJECT_ID(N'[dbo].[FK_Photos_Blogs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Photos] DROP CONSTRAINT [FK_Photos_Blogs];
GO
IF OBJECT_ID(N'[dbo].[FK_Patients_PatientStatuses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Patients] DROP CONSTRAINT [FK_Patients_PatientStatuses];
GO
IF OBJECT_ID(N'[dbo].[FK_Photos_Patients]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Photos] DROP CONSTRAINT [FK_Photos_Patients];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Grabers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Grabers];
GO
IF OBJECT_ID(N'[dbo].[HistoryGrabs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HistoryGrabs];
GO
IF OBJECT_ID(N'[dbo].[HistoryPosts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HistoryPosts];
GO
IF OBJECT_ID(N'[dbo].[Hospitals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Hospitals];
GO
IF OBJECT_ID(N'[dbo].[Newses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Newses];
GO
IF OBJECT_ID(N'[dbo].[Patients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Patients];
GO
IF OBJECT_ID(N'[dbo].[PatientStatuses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PatientStatuses];
GO
IF OBJECT_ID(N'[dbo].[Photos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Photos];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[TypeNewses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TypeNewses];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentId] int  NULL,
    [Contents] varchar(max)  NOT NULL,
    [EmployeeId] int  NULL,
    [CustomerId] int  NULL,
    [NewsId] int  NULL,
    [GrabId] int  NULL,
    [PatientId] int  NULL,
    [DateCreate] datetime  NOT NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Avatar] nvarchar(250)  NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [DateOfBirth] datetime  NOT NULL,
    [Gender] tinyint  NOT NULL,
    [Email] nvarchar(70)  NOT NULL,
    [Phone] nvarchar(15)  NOT NULL,
    [Password] nvarchar(250)  NOT NULL,
    [DateCreate] datetime  NOT NULL,
    [DateUpdate] datetime  NOT NULL,
    [IsActive] bit  NULL,
    [IsEmailVerified] bit  NOT NULL,
    [ActivationCode] uniqueidentifier  NOT NULL,
    [ResetPasswordCode] nvarchar(100)  NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Avatar] nvarchar(250)  NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [DateOfBirth] datetime  NOT NULL,
    [Gender] tinyint  NOT NULL,
    [Email] nvarchar(100)  NOT NULL,
    [Phone] nvarchar(15)  NOT NULL,
    [RoleID] int  NOT NULL,
    [Password] nvarchar(250)  NOT NULL,
    [DateCreate] datetime  NOT NULL,
    [DateUpdate] datetime  NOT NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'Grabers'
CREATE TABLE [dbo].[Grabers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FullName] nvarchar(50)  NOT NULL,
    [IDCard] varchar(15)  NOT NULL,
    [Phone] varchar(15)  NOT NULL,
    [Avatar] varchar(150)  NOT NULL,
    [DateCreate] datetime  NOT NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'HistoryGrabs'
CREATE TABLE [dbo].[HistoryGrabs] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [GrabId] int  NOT NULL,
    [PatientId] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'HistoryPosts'
CREATE TABLE [dbo].[HistoryPosts] (
    [Id] int  NOT NULL,
    [NewsId] int  NOT NULL,
    [EmployeeId] int  NOT NULL,
    [DatePost] datetime  NOT NULL,
    [View] int  NULL,
    [IsActive] bit  NULL,
    [DateUpdate] datetime  NOT NULL
);
GO

-- Creating table 'Hospitals'
CREATE TABLE [dbo].[Hospitals] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(150)  NOT NULL,
    [Address] nvarchar(150)  NOT NULL,
    [Phone] varchar(15)  NOT NULL,
    [DateCreate] datetime  NOT NULL,
    [DateUpdate] datetime  NOT NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'Newses'
CREATE TABLE [dbo].[Newses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subject] nvarchar(250)  NOT NULL,
    [Contents] varchar(max)  NOT NULL,
    [TypeId] int  NOT NULL,
    [CustomerId] int  NOT NULL,
    [DateCreate] datetime  NOT NULL,
    [Dateupdate] datetime  NOT NULL,
    [IsPost] bit  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'Patients'
CREATE TABLE [dbo].[Patients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FullName] nvarchar(50)  NOT NULL,
    [Age] int  NOT NULL,
    [Gender] tinyint  NOT NULL,
    [HospitalId] int  NOT NULL,
    [DateDeparture] datetime  NOT NULL,
    [Phone] varchar(15)  NOT NULL,
    [Destination] nvarchar(150)  NOT NULL,
    [StatusId] int  NOT NULL,
    [DateCreate] datetime  NOT NULL,
    [DateUpdate] datetime  NOT NULL
);
GO

-- Creating table 'PatientStatuses'
CREATE TABLE [dbo].[PatientStatuses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(150)  NOT NULL,
    [DateCreate] datetime  NOT NULL,
    [DateUpdate] datetime  NOT NULL
);
GO

-- Creating table 'Photos'
CREATE TABLE [dbo].[Photos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Url] nvarchar(150)  NOT NULL,
    [PatientId] int  NULL,
    [NewsId] int  NULL,
    [DateCreate] datetime  NOT NULL,
    [DateUpdate] datetime  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [DateCreate] datetime  NOT NULL,
    [DateUpdate] datetime  NOT NULL
);
GO

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 'TypeNewses'
CREATE TABLE [dbo].[TypeNewses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ParentId] int  NOT NULL,
    [Type] nvarchar(50)  NOT NULL,
    [DateCreate] datetime  NOT NULL,
    [DateUpdate] datetime  NOT NULL,
    [IsActive] bit  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Grabers'
ALTER TABLE [dbo].[Grabers]
ADD CONSTRAINT [PK_Grabers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HistoryGrabs'
ALTER TABLE [dbo].[HistoryGrabs]
ADD CONSTRAINT [PK_HistoryGrabs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HistoryPosts'
ALTER TABLE [dbo].[HistoryPosts]
ADD CONSTRAINT [PK_HistoryPosts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ID] in table 'Hospitals'
ALTER TABLE [dbo].[Hospitals]
ADD CONSTRAINT [PK_Hospitals]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'Newses'
ALTER TABLE [dbo].[Newses]
ADD CONSTRAINT [PK_Newses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [PK_Patients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PatientStatuses'
ALTER TABLE [dbo].[PatientStatuses]
ADD CONSTRAINT [PK_PatientStatuses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Photos'
ALTER TABLE [dbo].[Photos]
ADD CONSTRAINT [PK_Photos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [Id] in table 'TypeNewses'
ALTER TABLE [dbo].[TypeNewses]
ADD CONSTRAINT [PK_TypeNewses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CustomerId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comments_Customers]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comments_Customers'
CREATE INDEX [IX_FK_Comments_Customers]
ON [dbo].[Comments]
    ([CustomerId]);
GO

-- Creating foreign key on [EmployeeId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comments_Employees]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comments_Employees'
CREATE INDEX [IX_FK_Comments_Employees]
ON [dbo].[Comments]
    ([EmployeeId]);
GO

-- Creating foreign key on [GrabId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comments_Grabers]
    FOREIGN KEY ([GrabId])
    REFERENCES [dbo].[Grabers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comments_Grabers'
CREATE INDEX [IX_FK_Comments_Grabers]
ON [dbo].[Comments]
    ([GrabId]);
GO

-- Creating foreign key on [NewsId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comments_Newses]
    FOREIGN KEY ([NewsId])
    REFERENCES [dbo].[Newses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comments_Newses'
CREATE INDEX [IX_FK_Comments_Newses]
ON [dbo].[Comments]
    ([NewsId]);
GO

-- Creating foreign key on [PatientId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_Comments_Patients]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[Patients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Comments_Patients'
CREATE INDEX [IX_FK_Comments_Patients]
ON [dbo].[Comments]
    ([PatientId]);
GO

-- Creating foreign key on [CustomerId] in table 'Newses'
ALTER TABLE [dbo].[Newses]
ADD CONSTRAINT [FK_Newses_Customers]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Newses_Customers'
CREATE INDEX [IX_FK_Newses_Customers]
ON [dbo].[Newses]
    ([CustomerId]);
GO

-- Creating foreign key on [RoleID] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_Employees_Roles]
    FOREIGN KEY ([RoleID])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Employees_Roles'
CREATE INDEX [IX_FK_Employees_Roles]
ON [dbo].[Employees]
    ([RoleID]);
GO

-- Creating foreign key on [EmployeeId] in table 'HistoryPosts'
ALTER TABLE [dbo].[HistoryPosts]
ADD CONSTRAINT [FK_HistoryPosts_Employees]
    FOREIGN KEY ([EmployeeId])
    REFERENCES [dbo].[Employees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HistoryPosts_Employees'
CREATE INDEX [IX_FK_HistoryPosts_Employees]
ON [dbo].[HistoryPosts]
    ([EmployeeId]);
GO

-- Creating foreign key on [GrabId] in table 'HistoryGrabs'
ALTER TABLE [dbo].[HistoryGrabs]
ADD CONSTRAINT [FK_HistoryGrabers_Grapers]
    FOREIGN KEY ([GrabId])
    REFERENCES [dbo].[Grabers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HistoryGrabers_Grapers'
CREATE INDEX [IX_FK_HistoryGrabers_Grapers]
ON [dbo].[HistoryGrabs]
    ([GrabId]);
GO

-- Creating foreign key on [PatientId] in table 'HistoryGrabs'
ALTER TABLE [dbo].[HistoryGrabs]
ADD CONSTRAINT [FK_HistoryGrabs_Patients]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[Patients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HistoryGrabs_Patients'
CREATE INDEX [IX_FK_HistoryGrabs_Patients]
ON [dbo].[HistoryGrabs]
    ([PatientId]);
GO

-- Creating foreign key on [NewsId] in table 'HistoryPosts'
ALTER TABLE [dbo].[HistoryPosts]
ADD CONSTRAINT [FK_HistoryPosts_Newses]
    FOREIGN KEY ([NewsId])
    REFERENCES [dbo].[Newses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HistoryPosts_Newses'
CREATE INDEX [IX_FK_HistoryPosts_Newses]
ON [dbo].[HistoryPosts]
    ([NewsId]);
GO

-- Creating foreign key on [HospitalId] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [FK_Patients_Hospitals]
    FOREIGN KEY ([HospitalId])
    REFERENCES [dbo].[Hospitals]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Patients_Hospitals'
CREATE INDEX [IX_FK_Patients_Hospitals]
ON [dbo].[Patients]
    ([HospitalId]);
GO

-- Creating foreign key on [TypeId] in table 'Newses'
ALTER TABLE [dbo].[Newses]
ADD CONSTRAINT [FK_Blogs_Types]
    FOREIGN KEY ([TypeId])
    REFERENCES [dbo].[TypeNewses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Blogs_Types'
CREATE INDEX [IX_FK_Blogs_Types]
ON [dbo].[Newses]
    ([TypeId]);
GO

-- Creating foreign key on [NewsId] in table 'Photos'
ALTER TABLE [dbo].[Photos]
ADD CONSTRAINT [FK_Photos_Blogs]
    FOREIGN KEY ([NewsId])
    REFERENCES [dbo].[Newses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Photos_Blogs'
CREATE INDEX [IX_FK_Photos_Blogs]
ON [dbo].[Photos]
    ([NewsId]);
GO

-- Creating foreign key on [StatusId] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [FK_Patients_PatientStatuses]
    FOREIGN KEY ([StatusId])
    REFERENCES [dbo].[PatientStatuses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Patients_PatientStatuses'
CREATE INDEX [IX_FK_Patients_PatientStatuses]
ON [dbo].[Patients]
    ([StatusId]);
GO

-- Creating foreign key on [PatientId] in table 'Photos'
ALTER TABLE [dbo].[Photos]
ADD CONSTRAINT [FK_Photos_Patients]
    FOREIGN KEY ([PatientId])
    REFERENCES [dbo].[Patients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Photos_Patients'
CREATE INDEX [IX_FK_Photos_Patients]
ON [dbo].[Photos]
    ([PatientId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------