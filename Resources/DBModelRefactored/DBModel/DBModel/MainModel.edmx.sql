
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/05/2018 14:10:57
-- Generated from EDMX file: E:\Year 2 Semester 2\Software Engineering\vivus\Resources\DBModelRefactored\DBModel\DBModel\MainModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Vivus];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AddressCounty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Addresses] DROP CONSTRAINT [FK_AddressCounty];
GO
IF OBJECT_ID(N'[dbo].[FK_GenderPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons] DROP CONSTRAINT [FK_GenderPerson];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonAddress]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons] DROP CONSTRAINT [FK_PersonAddress];
GO
IF OBJECT_ID(N'[dbo].[FK_PatientPersonStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Patient] DROP CONSTRAINT [FK_PatientPersonStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_RHPatient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Patient] DROP CONSTRAINT [FK_RHPatient];
GO
IF OBJECT_ID(N'[dbo].[FK_BloodTypePatient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Patient] DROP CONSTRAINT [FK_BloodTypePatient];
GO
IF OBJECT_ID(N'[dbo].[FK_DoctorPatient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Patient] DROP CONSTRAINT [FK_DoctorPatient];
GO
IF OBJECT_ID(N'[dbo].[FK_DonationStatusDonor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DonationStatuses] DROP CONSTRAINT [FK_DonationStatusDonor];
GO
IF OBJECT_ID(N'[dbo].[FK_BloodContainerRH]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BloodContainers] DROP CONSTRAINT [FK_BloodContainerRH];
GO
IF OBJECT_ID(N'[dbo].[FK_DonationCenterAddress]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DonationCenters] DROP CONSTRAINT [FK_DonationCenterAddress];
GO
IF OBJECT_ID(N'[dbo].[FK_ContainerTypeBloodContainer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BloodContainers] DROP CONSTRAINT [FK_ContainerTypeBloodContainer];
GO
IF OBJECT_ID(N'[dbo].[FK_BloodRequestRequestPriority]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BloodRequests] DROP CONSTRAINT [FK_BloodRequestRequestPriority];
GO
IF OBJECT_ID(N'[dbo].[FK_BloodRequestPatient]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BloodRequests] DROP CONSTRAINT [FK_BloodRequestPatient];
GO
IF OBJECT_ID(N'[dbo].[FK_BloodRequestDoctor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BloodRequests] DROP CONSTRAINT [FK_BloodRequestDoctor];
GO
IF OBJECT_ID(N'[dbo].[FK_DonationCenterBloodContainer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BloodContainers] DROP CONSTRAINT [FK_DonationCenterBloodContainer];
GO
IF OBJECT_ID(N'[dbo].[FK_BloodRequestDonationCenter_BloodRequest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BloodRequestDonationCenter] DROP CONSTRAINT [FK_BloodRequestDonationCenter_BloodRequest];
GO
IF OBJECT_ID(N'[dbo].[FK_BloodRequestDonationCenter_DonationCenter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BloodRequestDonationCenter] DROP CONSTRAINT [FK_BloodRequestDonationCenter_DonationCenter];
GO
IF OBJECT_ID(N'[dbo].[FK_BloodRequestBloodContainer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BloodContainers] DROP CONSTRAINT [FK_BloodRequestBloodContainer];
GO
IF OBJECT_ID(N'[dbo].[FK_DCPersonnelDonationCenter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_DCPersonnel] DROP CONSTRAINT [FK_DCPersonnelDonationCenter];
GO
IF OBJECT_ID(N'[dbo].[FK_DonationFormDCPersonnel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DonationForms] DROP CONSTRAINT [FK_DonationFormDCPersonnel];
GO
IF OBJECT_ID(N'[dbo].[FK_DonationFormDonor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DonationForms] DROP CONSTRAINT [FK_DonationFormDonor];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonMessage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_PersonMessage];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonMessage1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_PersonMessage1];
GO
IF OBJECT_ID(N'[dbo].[FK_WomenInfoDonationStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WomenInfos] DROP CONSTRAINT [FK_WomenInfoDonationStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_Patient_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Patient] DROP CONSTRAINT [FK_Patient_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Doctor_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Doctor] DROP CONSTRAINT [FK_Doctor_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_Donor_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_Donor] DROP CONSTRAINT [FK_Donor_inherits_Person];
GO
IF OBJECT_ID(N'[dbo].[FK_DCPersonnel_inherits_Person]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Persons_DCPersonnel] DROP CONSTRAINT [FK_DCPersonnel_inherits_Person];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Accounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accounts];
GO
IF OBJECT_ID(N'[dbo].[Counties]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Counties];
GO
IF OBJECT_ID(N'[dbo].[Addresses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Addresses];
GO
IF OBJECT_ID(N'[dbo].[Persons]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Persons];
GO
IF OBJECT_ID(N'[dbo].[Genders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Genders];
GO
IF OBJECT_ID(N'[dbo].[RHs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RHs];
GO
IF OBJECT_ID(N'[dbo].[BloodTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BloodTypes];
GO
IF OBJECT_ID(N'[dbo].[PersonStatuses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PersonStatuses];
GO
IF OBJECT_ID(N'[dbo].[DonationStatuses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DonationStatuses];
GO
IF OBJECT_ID(N'[dbo].[WomenInfos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WomenInfos];
GO
IF OBJECT_ID(N'[dbo].[DonationCenters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DonationCenters];
GO
IF OBJECT_ID(N'[dbo].[BloodContainers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BloodContainers];
GO
IF OBJECT_ID(N'[dbo].[ContainerTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContainerTypes];
GO
IF OBJECT_ID(N'[dbo].[BloodRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BloodRequests];
GO
IF OBJECT_ID(N'[dbo].[RequestPriorities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RequestPriorities];
GO
IF OBJECT_ID(N'[dbo].[DonationForms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DonationForms];
GO
IF OBJECT_ID(N'[dbo].[Messages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Messages];
GO
IF OBJECT_ID(N'[dbo].[Administrators]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Administrators];
GO
IF OBJECT_ID(N'[dbo].[Owners]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Owners];
GO
IF OBJECT_ID(N'[dbo].[Persons_Patient]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Persons_Patient];
GO
IF OBJECT_ID(N'[dbo].[Persons_Doctor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Persons_Doctor];
GO
IF OBJECT_ID(N'[dbo].[Persons_Donor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Persons_Donor];
GO
IF OBJECT_ID(N'[dbo].[Persons_DCPersonnel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Persons_DCPersonnel];
GO
IF OBJECT_ID(N'[dbo].[BloodRequestDonationCenter]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BloodRequestDonationCenter];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [AccountID] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [DonorPersonID] int  NOT NULL
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
    [Zipcode] nvarchar(max)  NOT NULL,
    [Donor_PersonID] int  NOT NULL
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

-- Creating table 'WomenInfos'
CREATE TABLE [dbo].[WomenInfos] (
    [WomenInfoID] int IDENTITY(1,1) NOT NULL,
    [IsPregnant] bit  NOT NULL,
    [PostBirth] bit  NOT NULL,
    [Menstruating] bit  NOT NULL,
    [DonationStatus_DonationStatusID] int  NOT NULL
);
GO

-- Creating table 'DonationCenters'
CREATE TABLE [dbo].[DonationCenters] (
    [DonationCenterID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [AddressID] int  NOT NULL
);
GO

-- Creating table 'BloodContainers'
CREATE TABLE [dbo].[BloodContainers] (
    [BloodContainerID] int IDENTITY(1,1) NOT NULL,
    [HarvestDate] datetime  NOT NULL,
    [RHID] int  NOT NULL,
    [ContainerCode] nvarchar(max)  NOT NULL,
    [ContainerTypeID] int  NOT NULL,
    [DonationCenterID] int  NOT NULL,
    [IsExpired] bit  NOT NULL,
    [BloodRequestBloodRequestID] int  NOT NULL
);
GO

-- Creating table 'ContainerTypes'
CREATE TABLE [dbo].[ContainerTypes] (
    [ContainerTypeID] int IDENTITY(1,1) NOT NULL,
    [RedCell] int  NOT NULL,
    [Plasma] int  NOT NULL,
    [Thrombocyte] int  NOT NULL
);
GO

-- Creating table 'BloodRequests'
CREATE TABLE [dbo].[BloodRequests] (
    [BloodRequestID] int IDENTITY(1,1) NOT NULL,
    [ThrombocytesQuantity] int  NOT NULL,
    [RedCellsQuantity] int  NOT NULL,
    [PlasmaQuantity] int  NOT NULL,
    [RequestPriorityID] int  NOT NULL,
    [PatientID] int  NOT NULL,
    [DoctorID] int  NOT NULL
);
GO

-- Creating table 'RequestPriorities'
CREATE TABLE [dbo].[RequestPriorities] (
    [RequestPriorityID] int IDENTITY(1,1) NOT NULL,
    [Priority] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DonationForms'
CREATE TABLE [dbo].[DonationForms] (
    [DonationFormID] int IDENTITY(1,1) NOT NULL,
    [TravelStatus] nvarchar(max)  NOT NULL,
    [DonationStatus] bit  NOT NULL,
    [DCPersonnelID] int  NOT NULL,
    [DonorID] int  NOT NULL
);
GO

-- Creating table 'Messages'
CREATE TABLE [dbo].[Messages] (
    [MessageID] int IDENTITY(1,1) NOT NULL,
    [SenderID] int  NOT NULL,
    [RecieverID] int  NOT NULL
);
GO

-- Creating table 'Administrators'
CREATE TABLE [dbo].[Administrators] (
    [AdministratorID] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Owners'
CREATE TABLE [dbo].[Owners] (
    [OwnerID] int IDENTITY(1,1) NOT NULL
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
    [WorkAddressID] int  NOT NULL,
    [PersonID] int  NOT NULL,
    [Account_AccountID] int  NOT NULL
);
GO

-- Creating table 'Persons_Donor'
CREATE TABLE [dbo].[Persons_Donor] (
    [PersonID] int  NOT NULL
);
GO

-- Creating table 'Persons_DCPersonnel'
CREATE TABLE [dbo].[Persons_DCPersonnel] (
    [Disabled] bit  NOT NULL,
    [DonationCenterID] int  NOT NULL,
    [PersonID] int  NOT NULL,
    [Account_AccountID] int  NOT NULL
);
GO

-- Creating table 'BloodRequestDonationCenter'
CREATE TABLE [dbo].[BloodRequestDonationCenter] (
    [BloodRequest_BloodRequestID] int  NOT NULL,
    [DonationCenter_DonationCenterID] int  NOT NULL
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

-- Creating primary key on [WomenInfoID] in table 'WomenInfos'
ALTER TABLE [dbo].[WomenInfos]
ADD CONSTRAINT [PK_WomenInfos]
    PRIMARY KEY CLUSTERED ([WomenInfoID] ASC);
GO

-- Creating primary key on [DonationCenterID] in table 'DonationCenters'
ALTER TABLE [dbo].[DonationCenters]
ADD CONSTRAINT [PK_DonationCenters]
    PRIMARY KEY CLUSTERED ([DonationCenterID] ASC);
GO

-- Creating primary key on [BloodContainerID] in table 'BloodContainers'
ALTER TABLE [dbo].[BloodContainers]
ADD CONSTRAINT [PK_BloodContainers]
    PRIMARY KEY CLUSTERED ([BloodContainerID] ASC);
GO

-- Creating primary key on [ContainerTypeID] in table 'ContainerTypes'
ALTER TABLE [dbo].[ContainerTypes]
ADD CONSTRAINT [PK_ContainerTypes]
    PRIMARY KEY CLUSTERED ([ContainerTypeID] ASC);
GO

-- Creating primary key on [BloodRequestID] in table 'BloodRequests'
ALTER TABLE [dbo].[BloodRequests]
ADD CONSTRAINT [PK_BloodRequests]
    PRIMARY KEY CLUSTERED ([BloodRequestID] ASC);
GO

-- Creating primary key on [RequestPriorityID] in table 'RequestPriorities'
ALTER TABLE [dbo].[RequestPriorities]
ADD CONSTRAINT [PK_RequestPriorities]
    PRIMARY KEY CLUSTERED ([RequestPriorityID] ASC);
GO

-- Creating primary key on [DonationFormID] in table 'DonationForms'
ALTER TABLE [dbo].[DonationForms]
ADD CONSTRAINT [PK_DonationForms]
    PRIMARY KEY CLUSTERED ([DonationFormID] ASC);
GO

-- Creating primary key on [MessageID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [PK_Messages]
    PRIMARY KEY CLUSTERED ([MessageID] ASC);
GO

-- Creating primary key on [AdministratorID] in table 'Administrators'
ALTER TABLE [dbo].[Administrators]
ADD CONSTRAINT [PK_Administrators]
    PRIMARY KEY CLUSTERED ([AdministratorID] ASC);
GO

-- Creating primary key on [OwnerID] in table 'Owners'
ALTER TABLE [dbo].[Owners]
ADD CONSTRAINT [PK_Owners]
    PRIMARY KEY CLUSTERED ([OwnerID] ASC);
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

-- Creating primary key on [PersonID] in table 'Persons_DCPersonnel'
ALTER TABLE [dbo].[Persons_DCPersonnel]
ADD CONSTRAINT [PK_Persons_DCPersonnel]
    PRIMARY KEY CLUSTERED ([PersonID] ASC);
GO

-- Creating primary key on [BloodRequest_BloodRequestID], [DonationCenter_DonationCenterID] in table 'BloodRequestDonationCenter'
ALTER TABLE [dbo].[BloodRequestDonationCenter]
ADD CONSTRAINT [PK_BloodRequestDonationCenter]
    PRIMARY KEY CLUSTERED ([BloodRequest_BloodRequestID], [DonationCenter_DonationCenterID] ASC);
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

-- Creating foreign key on [RHID] in table 'BloodContainers'
ALTER TABLE [dbo].[BloodContainers]
ADD CONSTRAINT [FK_BloodContainerRH]
    FOREIGN KEY ([RHID])
    REFERENCES [dbo].[RHs]
        ([RHID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BloodContainerRH'
CREATE INDEX [IX_FK_BloodContainerRH]
ON [dbo].[BloodContainers]
    ([RHID]);
GO

-- Creating foreign key on [AddressID] in table 'DonationCenters'
ALTER TABLE [dbo].[DonationCenters]
ADD CONSTRAINT [FK_DonationCenterAddress]
    FOREIGN KEY ([AddressID])
    REFERENCES [dbo].[Addresses]
        ([AddressID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DonationCenterAddress'
CREATE INDEX [IX_FK_DonationCenterAddress]
ON [dbo].[DonationCenters]
    ([AddressID]);
GO

-- Creating foreign key on [ContainerTypeID] in table 'BloodContainers'
ALTER TABLE [dbo].[BloodContainers]
ADD CONSTRAINT [FK_ContainerTypeBloodContainer]
    FOREIGN KEY ([ContainerTypeID])
    REFERENCES [dbo].[ContainerTypes]
        ([ContainerTypeID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ContainerTypeBloodContainer'
CREATE INDEX [IX_FK_ContainerTypeBloodContainer]
ON [dbo].[BloodContainers]
    ([ContainerTypeID]);
GO

-- Creating foreign key on [RequestPriorityID] in table 'BloodRequests'
ALTER TABLE [dbo].[BloodRequests]
ADD CONSTRAINT [FK_BloodRequestRequestPriority]
    FOREIGN KEY ([RequestPriorityID])
    REFERENCES [dbo].[RequestPriorities]
        ([RequestPriorityID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BloodRequestRequestPriority'
CREATE INDEX [IX_FK_BloodRequestRequestPriority]
ON [dbo].[BloodRequests]
    ([RequestPriorityID]);
GO

-- Creating foreign key on [PatientID] in table 'BloodRequests'
ALTER TABLE [dbo].[BloodRequests]
ADD CONSTRAINT [FK_BloodRequestPatient]
    FOREIGN KEY ([PatientID])
    REFERENCES [dbo].[Persons_Patient]
        ([PersonID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BloodRequestPatient'
CREATE INDEX [IX_FK_BloodRequestPatient]
ON [dbo].[BloodRequests]
    ([PatientID]);
GO

-- Creating foreign key on [DoctorID] in table 'BloodRequests'
ALTER TABLE [dbo].[BloodRequests]
ADD CONSTRAINT [FK_BloodRequestDoctor]
    FOREIGN KEY ([DoctorID])
    REFERENCES [dbo].[Persons_Doctor]
        ([PersonID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BloodRequestDoctor'
CREATE INDEX [IX_FK_BloodRequestDoctor]
ON [dbo].[BloodRequests]
    ([DoctorID]);
GO

-- Creating foreign key on [DonationCenterID] in table 'BloodContainers'
ALTER TABLE [dbo].[BloodContainers]
ADD CONSTRAINT [FK_DonationCenterBloodContainer]
    FOREIGN KEY ([DonationCenterID])
    REFERENCES [dbo].[DonationCenters]
        ([DonationCenterID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DonationCenterBloodContainer'
CREATE INDEX [IX_FK_DonationCenterBloodContainer]
ON [dbo].[BloodContainers]
    ([DonationCenterID]);
GO

-- Creating foreign key on [BloodRequest_BloodRequestID] in table 'BloodRequestDonationCenter'
ALTER TABLE [dbo].[BloodRequestDonationCenter]
ADD CONSTRAINT [FK_BloodRequestDonationCenter_BloodRequest]
    FOREIGN KEY ([BloodRequest_BloodRequestID])
    REFERENCES [dbo].[BloodRequests]
        ([BloodRequestID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [DonationCenter_DonationCenterID] in table 'BloodRequestDonationCenter'
ALTER TABLE [dbo].[BloodRequestDonationCenter]
ADD CONSTRAINT [FK_BloodRequestDonationCenter_DonationCenter]
    FOREIGN KEY ([DonationCenter_DonationCenterID])
    REFERENCES [dbo].[DonationCenters]
        ([DonationCenterID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BloodRequestDonationCenter_DonationCenter'
CREATE INDEX [IX_FK_BloodRequestDonationCenter_DonationCenter]
ON [dbo].[BloodRequestDonationCenter]
    ([DonationCenter_DonationCenterID]);
GO

-- Creating foreign key on [BloodRequestBloodRequestID] in table 'BloodContainers'
ALTER TABLE [dbo].[BloodContainers]
ADD CONSTRAINT [FK_BloodRequestBloodContainer]
    FOREIGN KEY ([BloodRequestBloodRequestID])
    REFERENCES [dbo].[BloodRequests]
        ([BloodRequestID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BloodRequestBloodContainer'
CREATE INDEX [IX_FK_BloodRequestBloodContainer]
ON [dbo].[BloodContainers]
    ([BloodRequestBloodRequestID]);
GO

-- Creating foreign key on [DonationCenterID] in table 'Persons_DCPersonnel'
ALTER TABLE [dbo].[Persons_DCPersonnel]
ADD CONSTRAINT [FK_DCPersonnelDonationCenter]
    FOREIGN KEY ([DonationCenterID])
    REFERENCES [dbo].[DonationCenters]
        ([DonationCenterID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCPersonnelDonationCenter'
CREATE INDEX [IX_FK_DCPersonnelDonationCenter]
ON [dbo].[Persons_DCPersonnel]
    ([DonationCenterID]);
GO

-- Creating foreign key on [DCPersonnelID] in table 'DonationForms'
ALTER TABLE [dbo].[DonationForms]
ADD CONSTRAINT [FK_DonationFormDCPersonnel]
    FOREIGN KEY ([DCPersonnelID])
    REFERENCES [dbo].[Persons_DCPersonnel]
        ([PersonID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DonationFormDCPersonnel'
CREATE INDEX [IX_FK_DonationFormDCPersonnel]
ON [dbo].[DonationForms]
    ([DCPersonnelID]);
GO

-- Creating foreign key on [DonorID] in table 'DonationForms'
ALTER TABLE [dbo].[DonationForms]
ADD CONSTRAINT [FK_DonationFormDonor]
    FOREIGN KEY ([DonorID])
    REFERENCES [dbo].[Persons_Donor]
        ([PersonID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DonationFormDonor'
CREATE INDEX [IX_FK_DonationFormDonor]
ON [dbo].[DonationForms]
    ([DonorID]);
GO

-- Creating foreign key on [SenderID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_PersonMessage]
    FOREIGN KEY ([SenderID])
    REFERENCES [dbo].[Persons]
        ([PersonID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonMessage'
CREATE INDEX [IX_FK_PersonMessage]
ON [dbo].[Messages]
    ([SenderID]);
GO

-- Creating foreign key on [RecieverID] in table 'Messages'
ALTER TABLE [dbo].[Messages]
ADD CONSTRAINT [FK_PersonMessage1]
    FOREIGN KEY ([RecieverID])
    REFERENCES [dbo].[Persons]
        ([PersonID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonMessage1'
CREATE INDEX [IX_FK_PersonMessage1]
ON [dbo].[Messages]
    ([RecieverID]);
GO

-- Creating foreign key on [DonationStatus_DonationStatusID] in table 'WomenInfos'
ALTER TABLE [dbo].[WomenInfos]
ADD CONSTRAINT [FK_WomenInfoDonationStatus]
    FOREIGN KEY ([DonationStatus_DonationStatusID])
    REFERENCES [dbo].[DonationStatuses]
        ([DonationStatusID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WomenInfoDonationStatus'
CREATE INDEX [IX_FK_WomenInfoDonationStatus]
ON [dbo].[WomenInfos]
    ([DonationStatus_DonationStatusID]);
GO

-- Creating foreign key on [Donor_PersonID] in table 'Addresses'
ALTER TABLE [dbo].[Addresses]
ADD CONSTRAINT [FK_AddressDonor]
    FOREIGN KEY ([Donor_PersonID])
    REFERENCES [dbo].[Persons_Donor]
        ([PersonID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AddressDonor'
CREATE INDEX [IX_FK_AddressDonor]
ON [dbo].[Addresses]
    ([Donor_PersonID]);
GO

-- Creating foreign key on [Account_AccountID] in table 'Persons_Doctor'
ALTER TABLE [dbo].[Persons_Doctor]
ADD CONSTRAINT [FK_DoctorAccount]
    FOREIGN KEY ([Account_AccountID])
    REFERENCES [dbo].[Accounts]
        ([AccountID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DoctorAccount'
CREATE INDEX [IX_FK_DoctorAccount]
ON [dbo].[Persons_Doctor]
    ([Account_AccountID]);
GO

-- Creating foreign key on [Account_AccountID] in table 'Persons_DCPersonnel'
ALTER TABLE [dbo].[Persons_DCPersonnel]
ADD CONSTRAINT [FK_DCPersonnelAccount]
    FOREIGN KEY ([Account_AccountID])
    REFERENCES [dbo].[Accounts]
        ([AccountID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DCPersonnelAccount'
CREATE INDEX [IX_FK_DCPersonnelAccount]
ON [dbo].[Persons_DCPersonnel]
    ([Account_AccountID]);
GO

-- Creating foreign key on [DonorPersonID] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_DonorAccount]
    FOREIGN KEY ([DonorPersonID])
    REFERENCES [dbo].[Persons_Donor]
        ([PersonID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DonorAccount'
CREATE INDEX [IX_FK_DonorAccount]
ON [dbo].[Accounts]
    ([DonorPersonID]);
GO

-- Creating foreign key on [WorkAddressID] in table 'Persons_Doctor'
ALTER TABLE [dbo].[Persons_Doctor]
ADD CONSTRAINT [FK_DoctorAddress]
    FOREIGN KEY ([WorkAddressID])
    REFERENCES [dbo].[Addresses]
        ([AddressID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DoctorAddress'
CREATE INDEX [IX_FK_DoctorAddress]
ON [dbo].[Persons_Doctor]
    ([WorkAddressID]);
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

-- Creating foreign key on [PersonID] in table 'Persons_DCPersonnel'
ALTER TABLE [dbo].[Persons_DCPersonnel]
ADD CONSTRAINT [FK_DCPersonnel_inherits_Person]
    FOREIGN KEY ([PersonID])
    REFERENCES [dbo].[Persons]
        ([PersonID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------