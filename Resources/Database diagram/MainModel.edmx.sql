
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/23/2018 20:53:46
-- Generated from EDMX file: C:\Users\David\source\repos\DBModel\DBModel\MainModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [NewTestDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AddressCounty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Address] DROP CONSTRAINT [FK_AddressCounty];
GO
IF OBJECT_ID(N'[dbo].[FK_GenderPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person] DROP CONSTRAINT [FK_GenderPerson];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonAddress]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person] DROP CONSTRAINT [FK_PersonAddress];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientPersonStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Patients] DROP CONSTRAINT [FK_PatientPersonStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_RHPatient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Patients] DROP CONSTRAINT [FK_RHPatient];
GO
IF OBJECT_ID(N'[dbo].[FK_BloodTypePatient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Patients] DROP CONSTRAINT [FK_BloodTypePatient];
GO
IF OBJECT_ID(N'[dbo].[FK_DoctorPatient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Patients] DROP CONSTRAINT [FK_DoctorPatient];
GO
IF OBJECT_ID(N'[dbo].[FK_DonationStatusDonor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DonationStatus] DROP CONSTRAINT [FK_DonationStatusDonor];
GO
IF OBJECT_ID(N'[dbo].[FK_Patients_inherits_Persons]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Patients] DROP CONSTRAINT [FK_Patients_inherits_Persons];
GO
IF OBJECT_ID(N'[dbo].[FK_Doctors_inherits_Persons]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Doctors] DROP CONSTRAINT [FK_Doctors_inherits_Persons];
GO
IF OBJECT_ID(N'[dbo].[FK_Donors_inherits_Persons]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Donors] DROP CONSTRAINT [FK_Donors_inherits_Persons];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Account];
GO
IF OBJECT_ID(N'[dbo].[County]', 'U') IS NOT NULL
    DROP TABLE [dbo].[County];
GO
IF OBJECT_ID(N'[dbo].[Address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Address];
GO
IF OBJECT_ID(N'[dbo].[Person]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person];
GO
IF OBJECT_ID(N'[dbo].[Gender]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Gender];
GO
IF OBJECT_ID(N'[dbo].[RH]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RH];
GO
IF OBJECT_ID(N'[dbo].[BloodType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BloodType];
GO
IF OBJECT_ID(N'[dbo].[PersonStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonStatus];
GO
IF OBJECT_ID(N'[dbo].[DonationStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DonationStatus];
GO
IF OBJECT_ID(N'[dbo].[Person_Patients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person_Patients];
GO
IF OBJECT_ID(N'[dbo].[Person_Doctors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person_Doctors];
GO
IF OBJECT_ID(N'[dbo].[Person_Donors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person_Donors];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [AccountID] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Counties'
CREATE TABLE [dbo].[Counties] (
    [CountyID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Addresses'
CREATE TABLE [dbo].[Addresses] (
    [AddressID] int IDENTITY(1,1) NOT NULL,
    [Street] nvarchar(max)  NOT NULL,
    [StreetNo] smallint  NOT NULL,
    [City] nvarchar(max)  NOT NULL,
    [CountyID] int  NOT NULL,
    [Zipcode] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Persons'
CREATE TABLE [dbo].[Persons] (
    [PersonID] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [BirthDate] datetime  NOT NULL,
    [GenderID] int  NOT NULL,
    [Nin] nvarchar(max)  NOT NULL,
    [PhoneNo] nvarchar(max)  NOT NULL,
    [AddressID] int  NOT NULL
);
GO

-- Creating table 'Genders'
CREATE TABLE [dbo].[Genders] (
    [GenderID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RHs'
CREATE TABLE [dbo].[RHs] (
    [RHID] int IDENTITY(1,1) NOT NULL,
    [Value] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'BloodTypes'
CREATE TABLE [dbo].[BloodTypes] (
    [BloodTypeID] int IDENTITY(1,1) NOT NULL,
    [Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PersonStatuses'
CREATE TABLE [dbo].[PersonStatuses] (
    [PersonStatusID] int IDENTITY(1,1) NOT NULL,
    [Status] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DonationStatuses'
CREATE TABLE [dbo].[DonationStatuses] (
    [DonationStatusID] int IDENTITY(1,1) NOT NULL,
    [Weight] decimal(18,0)  NOT NULL,
    [Pulse] smallint  NOT NULL,
    [BloodPressure] decimal(18,0)  NOT NULL,
    [HasPastSurgeries] bit  NOT NULL,
    [HasAlcoholConsumption] bit  NOT NULL,
    [HasFatConsumption] bit  NOT NULL,
    [IsInTreatment] bit  NOT NULL,
    [HasDiseases] bit  NOT NULL,
    [Donor_PersonID] int  NOT NULL
);
GO

-- Creating table 'Persons_Patient'
CREATE TABLE [dbo].[Persons_Patient] (
    [PersonStatusID] int  NOT NULL,
    [RHID] int  NOT NULL,
    [BloodTypeID] int  NOT NULL,
    [DoctorID] int  NOT NULL,
    [PersonID] int  NOT NULL
);
GO

-- Creating table 'Persons_Doctor'
CREATE TABLE [dbo].[Persons_Doctor] (
    [IsActive] bit  NOT NULL,
    [PersonID] int  NOT NULL
);
GO

-- Creating table 'Persons_Donor'
CREATE TABLE [dbo].[Persons_Donor] (
    [PersonID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [AccountID] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([AccountID] ASC);
GO

-- Creating primary key on [CountyID] in table 'Counties'
ALTER TABLE [dbo].[Counties]
ADD CONSTRAINT [PK_Counties]
    PRIMARY KEY CLUSTERED ([CountyID] ASC);
GO

-- Creating primary key on [AddressID] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [PK_Addresses]
    PRIMARY KEY CLUSTERED ([AddressID] ASC);
GO

-- Creating primary key on [PersonID] in table 'Persons'
ALTER TABLE [dbo].[Persons]
ADD CONSTRAINT [PK_Persons]
    PRIMARY KEY CLUSTERED ([PersonID] ASC);
GO

-- Creating primary key on [GenderID] in table 'Genders'
ALTER TABLE [dbo].[Genders]
ADD CONSTRAINT [PK_Genders]
    PRIMARY KEY CLUSTERED ([GenderID] ASC);
GO

-- Creating primary key on [RHID] in table 'RHs'
ALTER TABLE [dbo].[RHs]
ADD CONSTRAINT [PK_RHs]
    PRIMARY KEY CLUSTERED ([RHID] ASC);
GO

-- Creating primary key on [BloodTypeID] in table 'BloodTypes'
ALTER TABLE [dbo].[BloodTypes]
ADD CONSTRAINT [PK_BloodTypes]
    PRIMARY KEY CLUSTERED ([BloodTypeID] ASC);
GO

-- Creating primary key on [PersonStatusID] in table 'PersonStatuses'
ALTER TABLE [dbo].[PersonStatuses]
ADD CONSTRAINT [PK_PersonStatuses]
    PRIMARY KEY CLUSTERED ([PersonStatusID] ASC);
GO

-- Creating primary key on [DonationStatusID] in table 'DonationStatuses'
ALTER TABLE [dbo].[DonationStatuses]
ADD CONSTRAINT [PK_DonationStatuses]
    PRIMARY KEY CLUSTERED ([DonationStatusID] ASC);
GO

-- Creating primary key on [PersonID] in table 'Persons_Patient'
ALTER TABLE [dbo].[Persons_Patient]
ADD CONSTRAINT [PK_Persons_Patient]
    PRIMARY KEY CLUSTERED ([PersonID] ASC);
GO

-- Creating primary key on [PersonID] in table 'Persons_Doctor'
ALTER TABLE [dbo].[Persons_Doctor]
ADD CONSTRAINT [PK_Persons_Doctor]
    PRIMARY KEY CLUSTERED ([PersonID] ASC);
GO

-- Creating primary key on [PersonID] in table 'Persons_Donor'
ALTER TABLE [dbo].[Persons_Donor]
ADD CONSTRAINT [PK_Persons_Donor]
    PRIMARY KEY CLUSTERED ([PersonID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CountyID] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [FK_AddressCounty]
    FOREIGN KEY ([CountyID])
    REFERENCES [dbo].[Counties]
        ([CountyID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressCounty'
CREATE INDEX [IX_FK_AddressCounty]
ON [dbo].[Addresses]
    ([CountyID]);
GO

-- Creating foreign key on [GenderID] in table 'Persons'
ALTER TABLE [dbo].[Persons]
ADD CONSTRAINT [FK_GenderPerson]
    FOREIGN KEY ([GenderID])
    REFERENCES [dbo].[Genders]
        ([GenderID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GenderPerson'
CREATE INDEX [IX_FK_GenderPerson]
ON [dbo].[Persons]
    ([GenderID]);
GO

-- Creating foreign key on [AddressID] in table 'Persons'
ALTER TABLE [dbo].[Persons]
ADD CONSTRAINT [FK_PersonAddress]
    FOREIGN KEY ([AddressID])
    REFERENCES [dbo].[Addresses]
        ([AddressID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonAddress'
CREATE INDEX [IX_FK_PersonAddress]
ON [dbo].[Persons]
    ([AddressID]);
GO

-- Creating foreign key on [PersonStatusID] in table 'Persons_Patient'
ALTER TABLE [dbo].[Persons_Patient]
ADD CONSTRAINT [FK_PatientPersonStatus]
    FOREIGN KEY ([PersonStatusID])
    REFERENCES [dbo].[PersonStatuses]
        ([PersonStatusID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientPersonStatus'
CREATE INDEX [IX_FK_PatientPersonStatus]
ON [dbo].[Persons_Patient]
    ([PersonStatusID]);
GO

-- Creating foreign key on [RHID] in table 'Persons_Patient'
ALTER TABLE [dbo].[Persons_Patient]
ADD CONSTRAINT [FK_RHPatient]
    FOREIGN KEY ([RHID])
    REFERENCES [dbo].[RHs]
        ([RHID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RHPatient'
CREATE INDEX [IX_FK_RHPatient]
ON [dbo].[Persons_Patient]
    ([RHID]);
GO

-- Creating foreign key on [BloodTypeID] in table 'Persons_Patient'
ALTER TABLE [dbo].[Persons_Patient]
ADD CONSTRAINT [FK_BloodTypePatient]
    FOREIGN KEY ([BloodTypeID])
    REFERENCES [dbo].[BloodTypes]
        ([BloodTypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BloodTypePatient'
CREATE INDEX [IX_FK_BloodTypePatient]
ON [dbo].[Persons_Patient]
    ([BloodTypeID]);
GO

-- Creating foreign key on [DoctorID] in table 'Persons_Patient'
ALTER TABLE [dbo].[Persons_Patient]
ADD CONSTRAINT [FK_DoctorPatient]
    FOREIGN KEY ([DoctorID])
    REFERENCES [dbo].[Persons_Doctor]
        ([PersonID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DoctorPatient'
CREATE INDEX [IX_FK_DoctorPatient]
ON [dbo].[Persons_Patient]
    ([DoctorID]);
GO

-- Creating foreign key on [Donor_PersonID] in table 'DonationStatuses'
ALTER TABLE [dbo].[DonationStatuses]
ADD CONSTRAINT [FK_DonationStatusDonor]
    FOREIGN KEY ([Donor_PersonID])
    REFERENCES [dbo].[Persons_Donor]
        ([PersonID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DonationStatusDonor'
CREATE INDEX [IX_FK_DonationStatusDonor]
ON [dbo].[DonationStatuses]
    ([Donor_PersonID]);
GO

-- Creating foreign key on [PersonID] in table 'Persons_Patient'
ALTER TABLE [dbo].[Persons_Patient]
ADD CONSTRAINT [FK_Patient_inherits_Person]
    FOREIGN KEY ([PersonID])
    REFERENCES [dbo].[Persons]
        ([PersonID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PersonID] in table 'Persons_Doctor'
ALTER TABLE [dbo].[Persons_Doctor]
ADD CONSTRAINT [FK_Doctor_inherits_Person]
    FOREIGN KEY ([PersonID])
    REFERENCES [dbo].[Persons]
        ([PersonID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PersonID] in table 'Persons_Donor'
ALTER TABLE [dbo].[Persons_Donor]
ADD CONSTRAINT [FK_Donor_inherits_Person]
    FOREIGN KEY ([PersonID])
    REFERENCES [dbo].[Persons]
        ([PersonID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------