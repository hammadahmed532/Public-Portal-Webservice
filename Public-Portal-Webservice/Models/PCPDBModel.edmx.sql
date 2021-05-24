
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/24/2021 22:25:07
-- Generated from EDMX file: D:\FYP\Public-Portal-Webservice\Public-Portal-Webservice\Models\PCPDBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PCP5];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Complaint_Det_Remarks_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Complaint_Det_Remarks] DROP CONSTRAINT [FK_Complaint_Det_Remarks_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_Complaints_ToStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Complaints] DROP CONSTRAINT [FK_Complaints_ToStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_Complaints_ToUC]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Complaints] DROP CONSTRAINT [FK_Complaints_ToUC];
GO
IF OBJECT_ID(N'[dbo].[FK_PendingComplaints_ToTable]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PendingComplaints] DROP CONSTRAINT [FK_PendingComplaints_ToTable];
GO
IF OBJECT_ID(N'[dbo].[FK_SupportingComplaints_ToComplaints]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupportingComplaints] DROP CONSTRAINT [FK_SupportingComplaints_ToComplaints];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Complaint_Det_Remarks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Complaint_Det_Remarks];
GO
IF OBJECT_ID(N'[dbo].[Complaints]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Complaints];
GO
IF OBJECT_ID(N'[dbo].[PendingComplaints]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PendingComplaints];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Status]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Status];
GO
IF OBJECT_ID(N'[dbo].[SupportingComplaints]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupportingComplaints];
GO
IF OBJECT_ID(N'[dbo].[UCAreas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UCAreas];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [Account_id] int IDENTITY(1,1) NOT NULL,
    [Full_name] nvarchar(max)  NOT NULL,
    [username] nvarchar(50)  NOT NULL,
    [password] nvarchar(50)  NOT NULL,
    [isEmailVerified] bit  NULL,
    [ActivationCode] uniqueidentifier  NULL,
    [phone_number] bigint  NOT NULL,
    [address] nvarchar(max)  NOT NULL,
    [map_long] float  NULL,
    [map_lat] float  NULL,
    [email_address] nvarchar(max)  NOT NULL,
    [UC_Area_id] int  NULL,
    [Town_id] int  NULL,
    [date_created] datetime  NULL,
    [profession] nvarchar(max)  NULL,
    [gender] nvarchar(max)  NULL,
    [department_id] int  NULL,
    [Role] int  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [category_id] int  NOT NULL,
    [Category1] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Complaint_Det_Remarks'
CREATE TABLE [dbo].[Complaint_Det_Remarks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Complaint_id] int  NOT NULL,
    [Remarks_Field_Off] nvarchar(max)  NULL,
    [Remarks_Member] nvarchar(max)  NULL,
    [Remarks_Coordinator] nvarchar(max)  NULL,
    [Remarks_Nazim] nvarchar(max)  NULL,
    [updated_Image1] nvarchar(max)  NULL,
    [updated_Image2] nvarchar(max)  NULL,
    [updated_Image3] nvarchar(max)  NULL,
    [severity_lvl] int  NULL,
    [budgetYear] int  NULL
);
GO

-- Creating table 'Complaints'
CREATE TABLE [dbo].[Complaints] (
    [complaint_Id] int IDENTITY(1,1) NOT NULL,
    [c_title] nvarchar(max)  NULL,
    [account_id] int  NULL,
    [description] nvarchar(max)  NULL,
    [UC_area_id] int  NULL,
    [map_long] float  NULL,
    [map_lat] float  NULL,
    [date_creation] datetime  NOT NULL,
    [date_stage_2] datetime  NULL,
    [date_stage_3] datetime  NULL,
    [date_stage_4] datetime  NULL,
    [date_stage_6] datetime  NULL,
    [image1] nvarchar(max)  NULL,
    [image2] nvarchar(max)  NULL,
    [image3] nvarchar(max)  NULL,
    [stage_id] int  NOT NULL,
    [date_stage_5] datetime  NULL,
    [category_id] int  NULL,
    [view_public] nvarchar(10)  NULL,
    [admin_confirm] bit  NULL,
    [date_last_modified] datetime  NOT NULL,
    [Forwarded_By_Account_id] int  NULL,
    [complaint_Type] nvarchar(max)  NULL,
    [is_Notified] bit  NULL,
    [Expected_amount] float  NULL
);
GO

-- Creating table 'Feedbacks'
CREATE TABLE [dbo].[Feedbacks] (
    [Feedback_id] int IDENTITY(1,1) NOT NULL,
    [complaint_id] int  NULL,
    [FeedbackMessage] nvarchar(max)  NULL,
    [Rating] int  NULL,
    [time_created] datetime  NULL
);
GO

-- Creating table 'PendingComplaints'
CREATE TABLE [dbo].[PendingComplaints] (
    [p_id] int IDENTITY(1,1) NOT NULL,
    [complaint_id] int  NOT NULL,
    [severity_Lvl] int  NULL,
    [Year] int  NULL
);
GO

-- Creating table 'Status'
CREATE TABLE [dbo].[Status] (
    [stage_id] int  NOT NULL,
    [stage_description] nvarchar(max)  NULL,
    [Status_Percent] int  NULL,
    [Status_first] nvarchar(max)  NULL,
    [Status_Budget] nvarchar(max)  NULL,
    [Status_Bogus] nvarchar(max)  NULL,
    [Status_Already_Done] nvarchar(max)  NULL
);
GO

-- Creating table 'SupportingComplaints'
CREATE TABLE [dbo].[SupportingComplaints] (
    [S_id] int IDENTITY(1,1) NOT NULL,
    [Complaint_id] int  NOT NULL,
    [account_id] int  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [time_created] datetime  NULL
);
GO

-- Creating table 'Towns'
CREATE TABLE [dbo].[Towns] (
    [town_id] int  NOT NULL,
    [town_name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UCAreas'
CREATE TABLE [dbo].[UCAreas] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UCName] nvarchar(max)  NULL,
    [TownID] int  NULL
);
GO

-- Creating table 'Votings'
CREATE TABLE [dbo].[Votings] (
    [vote_id] int IDENTITY(1,1) NOT NULL,
    [complaint_id] int  NOT NULL,
    [account_id] int  NOT NULL,
    [time_created] datetime  NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Role_Id] int  NOT NULL,
    [Role_Name] nvarchar(50)  NULL,
    [Role_Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Announcement1'
CREATE TABLE [dbo].[Announcement1] (
    [Announcement_id] int  NOT NULL,
    [title] varchar(50)  NULL,
    [account_id] int  NULL,
    [description] varchar(max)  NULL,
    [UC_Area_id] int  NULL,
    [map_long] float  NULL,
    [map_lat] float  NULL,
    [date_creation] datetime  NOT NULL,
    [date_stage_2] datetime  NULL,
    [date_stage_3] datetime  NULL,
    [date_stage_4] datetime  NULL,
    [date_stage_5] datetime  NULL,
    [date_stage_6] datetime  NULL,
    [image_file_1] varchar(max)  NULL,
    [image_file_2] varchar(max)  NULL,
    [image_file_3] varchar(max)  NULL,
    [stage_id] int  NULL,
    [category_id] int  NULL,
    [admin_confirm] bit  NULL,
    [date_last_modified] datetime  NOT NULL,
    [forwarded_by_account_id] int  NULL,
    [complaint_type] varchar(50)  NULL,
    [is_notified] bit  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Account_id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([Account_id] ASC);
GO

-- Creating primary key on [category_id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([category_id] ASC);
GO

-- Creating primary key on [Id] in table 'Complaint_Det_Remarks'
ALTER TABLE [dbo].[Complaint_Det_Remarks]
ADD CONSTRAINT [PK_Complaint_Det_Remarks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [complaint_Id] in table 'Complaints'
ALTER TABLE [dbo].[Complaints]
ADD CONSTRAINT [PK_Complaints]
    PRIMARY KEY CLUSTERED ([complaint_Id] ASC);
GO

-- Creating primary key on [Feedback_id] in table 'Feedbacks'
ALTER TABLE [dbo].[Feedbacks]
ADD CONSTRAINT [PK_Feedbacks]
    PRIMARY KEY CLUSTERED ([Feedback_id] ASC);
GO

-- Creating primary key on [p_id] in table 'PendingComplaints'
ALTER TABLE [dbo].[PendingComplaints]
ADD CONSTRAINT [PK_PendingComplaints]
    PRIMARY KEY CLUSTERED ([p_id] ASC);
GO

-- Creating primary key on [stage_id] in table 'Status'
ALTER TABLE [dbo].[Status]
ADD CONSTRAINT [PK_Status]
    PRIMARY KEY CLUSTERED ([stage_id] ASC);
GO

-- Creating primary key on [S_id] in table 'SupportingComplaints'
ALTER TABLE [dbo].[SupportingComplaints]
ADD CONSTRAINT [PK_SupportingComplaints]
    PRIMARY KEY CLUSTERED ([S_id] ASC);
GO

-- Creating primary key on [town_id] in table 'Towns'
ALTER TABLE [dbo].[Towns]
ADD CONSTRAINT [PK_Towns]
    PRIMARY KEY CLUSTERED ([town_id] ASC);
GO

-- Creating primary key on [ID] in table 'UCAreas'
ALTER TABLE [dbo].[UCAreas]
ADD CONSTRAINT [PK_UCAreas]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [vote_id] in table 'Votings'
ALTER TABLE [dbo].[Votings]
ADD CONSTRAINT [PK_Votings]
    PRIMARY KEY CLUSTERED ([vote_id] ASC);
GO

-- Creating primary key on [Role_Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Role_Id] ASC);
GO

-- Creating primary key on [Announcement_id] in table 'Announcement1'
ALTER TABLE [dbo].[Announcement1]
ADD CONSTRAINT [PK_Announcement1]
    PRIMARY KEY CLUSTERED ([Announcement_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [department_id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_Account_ToCategory]
    FOREIGN KEY ([department_id])
    REFERENCES [dbo].[Categories]
        ([category_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Account_ToCategory'
CREATE INDEX [IX_FK_Account_ToCategory]
ON [dbo].[Accounts]
    ([department_id]);
GO

-- Creating foreign key on [Town_id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_Account_Town]
    FOREIGN KEY ([Town_id])
    REFERENCES [dbo].[Towns]
        ([town_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Account_Town'
CREATE INDEX [IX_FK_Account_Town]
ON [dbo].[Accounts]
    ([Town_id]);
GO

-- Creating foreign key on [UC_Area_id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_Accounts_ToUC]
    FOREIGN KEY ([UC_Area_id])
    REFERENCES [dbo].[UCAreas]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Accounts_ToUC'
CREATE INDEX [IX_FK_Accounts_ToUC]
ON [dbo].[Accounts]
    ([UC_Area_id]);
GO

-- Creating foreign key on [account_id] in table 'Complaints'
ALTER TABLE [dbo].[Complaints]
ADD CONSTRAINT [FK_Complaints_ToAccount]
    FOREIGN KEY ([account_id])
    REFERENCES [dbo].[Accounts]
        ([Account_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Complaints_ToAccount'
CREATE INDEX [IX_FK_Complaints_ToAccount]
ON [dbo].[Complaints]
    ([account_id]);
GO

-- Creating foreign key on [Forwarded_By_Account_id] in table 'Complaints'
ALTER TABLE [dbo].[Complaints]
ADD CONSTRAINT [FK_Complaints_ToForward_Admin_id]
    FOREIGN KEY ([Forwarded_By_Account_id])
    REFERENCES [dbo].[Accounts]
        ([Account_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Complaints_ToForward_Admin_id'
CREATE INDEX [IX_FK_Complaints_ToForward_Admin_id]
ON [dbo].[Complaints]
    ([Forwarded_By_Account_id]);
GO

-- Creating foreign key on [account_id] in table 'SupportingComplaints'
ALTER TABLE [dbo].[SupportingComplaints]
ADD CONSTRAINT [FK_SupportingComplaints_ToTable]
    FOREIGN KEY ([account_id])
    REFERENCES [dbo].[Accounts]
        ([Account_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupportingComplaints_ToTable'
CREATE INDEX [IX_FK_SupportingComplaints_ToTable]
ON [dbo].[SupportingComplaints]
    ([account_id]);
GO

-- Creating foreign key on [account_id] in table 'Votings'
ALTER TABLE [dbo].[Votings]
ADD CONSTRAINT [FK_Voting_ToAccount]
    FOREIGN KEY ([account_id])
    REFERENCES [dbo].[Accounts]
        ([Account_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Voting_ToAccount'
CREATE INDEX [IX_FK_Voting_ToAccount]
ON [dbo].[Votings]
    ([account_id]);
GO

-- Creating foreign key on [category_id] in table 'Complaints'
ALTER TABLE [dbo].[Complaints]
ADD CONSTRAINT [FK_Complaints_Category]
    FOREIGN KEY ([category_id])
    REFERENCES [dbo].[Categories]
        ([category_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Complaints_Category'
CREATE INDEX [IX_FK_Complaints_Category]
ON [dbo].[Complaints]
    ([category_id]);
GO

-- Creating foreign key on [Complaint_id] in table 'Complaint_Det_Remarks'
ALTER TABLE [dbo].[Complaint_Det_Remarks]
ADD CONSTRAINT [FK_Complaint_Det_Remarks_ToTable]
    FOREIGN KEY ([Complaint_id])
    REFERENCES [dbo].[Complaints]
        ([complaint_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Complaint_Det_Remarks_ToTable'
CREATE INDEX [IX_FK_Complaint_Det_Remarks_ToTable]
ON [dbo].[Complaint_Det_Remarks]
    ([Complaint_id]);
GO

-- Creating foreign key on [stage_id] in table 'Complaints'
ALTER TABLE [dbo].[Complaints]
ADD CONSTRAINT [FK_Complaints_ToStatus]
    FOREIGN KEY ([stage_id])
    REFERENCES [dbo].[Status]
        ([stage_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Complaints_ToStatus'
CREATE INDEX [IX_FK_Complaints_ToStatus]
ON [dbo].[Complaints]
    ([stage_id]);
GO

-- Creating foreign key on [UC_area_id] in table 'Complaints'
ALTER TABLE [dbo].[Complaints]
ADD CONSTRAINT [FK_Complaints_ToUC]
    FOREIGN KEY ([UC_area_id])
    REFERENCES [dbo].[UCAreas]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Complaints_ToUC'
CREATE INDEX [IX_FK_Complaints_ToUC]
ON [dbo].[Complaints]
    ([UC_area_id]);
GO

-- Creating foreign key on [complaint_id] in table 'Feedbacks'
ALTER TABLE [dbo].[Feedbacks]
ADD CONSTRAINT [FK_Feedback_ToComplaint]
    FOREIGN KEY ([complaint_id])
    REFERENCES [dbo].[Complaints]
        ([complaint_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Feedback_ToComplaint'
CREATE INDEX [IX_FK_Feedback_ToComplaint]
ON [dbo].[Feedbacks]
    ([complaint_id]);
GO

-- Creating foreign key on [complaint_id] in table 'PendingComplaints'
ALTER TABLE [dbo].[PendingComplaints]
ADD CONSTRAINT [FK_PendingComplaints_ToTable]
    FOREIGN KEY ([complaint_id])
    REFERENCES [dbo].[Complaints]
        ([complaint_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PendingComplaints_ToTable'
CREATE INDEX [IX_FK_PendingComplaints_ToTable]
ON [dbo].[PendingComplaints]
    ([complaint_id]);
GO

-- Creating foreign key on [Complaint_id] in table 'SupportingComplaints'
ALTER TABLE [dbo].[SupportingComplaints]
ADD CONSTRAINT [FK_SupportingComplaints_ToComplaints]
    FOREIGN KEY ([Complaint_id])
    REFERENCES [dbo].[Complaints]
        ([complaint_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupportingComplaints_ToComplaints'
CREATE INDEX [IX_FK_SupportingComplaints_ToComplaints]
ON [dbo].[SupportingComplaints]
    ([Complaint_id]);
GO

-- Creating foreign key on [complaint_id] in table 'Votings'
ALTER TABLE [dbo].[Votings]
ADD CONSTRAINT [FK_Voting_ToComplaints]
    FOREIGN KEY ([complaint_id])
    REFERENCES [dbo].[Complaints]
        ([complaint_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Voting_ToComplaints'
CREATE INDEX [IX_FK_Voting_ToComplaints]
ON [dbo].[Votings]
    ([complaint_id]);
GO

-- Creating foreign key on [Role] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_Account_ToRoles]
    FOREIGN KEY ([Role])
    REFERENCES [dbo].[Roles]
        ([Role_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Account_ToRoles'
CREATE INDEX [IX_FK_Account_ToRoles]
ON [dbo].[Accounts]
    ([Role]);
GO

-- Creating foreign key on [account_id] in table 'Announcement1'
ALTER TABLE [dbo].[Announcement1]
ADD CONSTRAINT [FK_Announcement_ToAccount]
    FOREIGN KEY ([account_id])
    REFERENCES [dbo].[Accounts]
        ([Account_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Announcement_ToAccount'
CREATE INDEX [IX_FK_Announcement_ToAccount]
ON [dbo].[Announcement1]
    ([account_id]);
GO

-- Creating foreign key on [forwarded_by_account_id] in table 'Announcement1'
ALTER TABLE [dbo].[Announcement1]
ADD CONSTRAINT [FK_Announcement_ToForwardedBy]
    FOREIGN KEY ([forwarded_by_account_id])
    REFERENCES [dbo].[Accounts]
        ([Account_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Announcement_ToForwardedBy'
CREATE INDEX [IX_FK_Announcement_ToForwardedBy]
ON [dbo].[Announcement1]
    ([forwarded_by_account_id]);
GO

-- Creating foreign key on [category_id] in table 'Announcement1'
ALTER TABLE [dbo].[Announcement1]
ADD CONSTRAINT [FK_Announcement_ToCategory]
    FOREIGN KEY ([category_id])
    REFERENCES [dbo].[Categories]
        ([category_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Announcement_ToCategory'
CREATE INDEX [IX_FK_Announcement_ToCategory]
ON [dbo].[Announcement1]
    ([category_id]);
GO

-- Creating foreign key on [stage_id] in table 'Announcement1'
ALTER TABLE [dbo].[Announcement1]
ADD CONSTRAINT [FK_Announcement_ToStatus]
    FOREIGN KEY ([stage_id])
    REFERENCES [dbo].[Status]
        ([stage_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Announcement_ToStatus'
CREATE INDEX [IX_FK_Announcement_ToStatus]
ON [dbo].[Announcement1]
    ([stage_id]);
GO

-- Creating foreign key on [UC_Area_id] in table 'Announcement1'
ALTER TABLE [dbo].[Announcement1]
ADD CONSTRAINT [FK_Announcement_ToUCArea]
    FOREIGN KEY ([UC_Area_id])
    REFERENCES [dbo].[UCAreas]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Announcement_ToUCArea'
CREATE INDEX [IX_FK_Announcement_ToUCArea]
ON [dbo].[Announcement1]
    ([UC_Area_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------