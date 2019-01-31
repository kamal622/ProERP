-- 03/09/2017
ALTER TABLE [dbo].[PreventiveReviewHistory] ADD ScheduledReviewDate DateTime NOT NULL
GO
ALTER TABLE [dbo].[PreventiveReviewHistory] ALTER COLUMN [ReviewDate] DATETIME NULL
GO
ALTER TABLE [dbo].[PreventiveReviewHistory] ALTER COLUMN [ReviewBy] INT NULL
GO

UPDATE PreventiveMaintenance SET NextReviewDate = '03/13/2017' WHERE ScheduleType = 1
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '03/13/2017' WHERE ScheduleType = 2
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '04/01/2017' WHERE ScheduleType = 3  and Interval = 1
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '05/01/2017' WHERE ScheduleType = 3  and Interval = 3 and LineId in (select LineId from Line where Name in ('P1', 'P3', 'P4', 'Kneder', 'Concetti'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '05/01/2017' WHERE ScheduleType = 3  and Interval = 3 and PlantId in (select PlantId from Plant where Name in ('Utility', 'X-Compound'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '06/01/2017' WHERE ScheduleType = 3  and Interval = 3 and LineId in (select LineId from Line where Name in ('P2', 'P5', 'P8'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '06/01/2017' WHERE ScheduleType = 3  and Interval = 3 and PlantId in (select PlantId from Plant where Name in ('Semicon'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '08/01/2017' WHERE ScheduleType = 3  and Interval = 6 and LineId in (select LineId from Line where Name in ('P1', 'P4', 'Concetti'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '08/01/2017' WHERE ScheduleType = 3  and Interval = 6 and PlantId in (select PlantId from Plant where Name in ('Utility', 'X-Compound'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '04/01/2017' WHERE ScheduleType = 3  and Interval = 6 and LineId in (select LineId from Line where Name in ('P2', 'P5', 'Concetti'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '08/01/2017' WHERE ScheduleType = 3  and Interval = 6 and LineId in (select LineId from Line where Name in ('P1', 'P4', 'Concetti'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '08/01/2017' WHERE ScheduleType = 3  and Interval = 6 and PlantId in (select PlantId from Plant where Name in ('Utility', 'X-Compound'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '08/01/2017' WHERE ScheduleType = 4  and Interval = 1 and LineId in (select LineId from Line where Name in ('P1', 'P4', 'Concetti'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '08/01/2017' WHERE ScheduleType = 4  and Interval = 1 and PlantId in (select PlantId from Plant where Name in ('Utility', 'X-Compound'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '11/01/2017' WHERE ScheduleType = 4  and Interval = 1 and LineId in (select LineId from Line where Name in ('P2', 'P5'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '10/01/2017' WHERE ScheduleType = 4  and Interval = 1 and LineId in (select LineId from Line where Name in ('P3', 'Kneder'))
GO
UPDATE PreventiveMaintenance SET NextReviewDate = '10/01/2017' WHERE ScheduleType = 4  and Interval = 1 and PlantId in (select PlantId from Plant where Name in ('Semicon'))
GO
DELETE UserAssignments where PreventiveMaintenanceId in (select Id from PreventiveMaintenance where LineId in (select Id from Line where Name in ('p1', 'p2', 'p3'))) and UserId != 7
GO
DELETE UserAssignments where PreventiveMaintenanceId in (select Id from PreventiveMaintenance where PlantId in (select Id from Plant where Name in ('Utility'))) and UserId != 7
GO
DELETE UserAssignments where PreventiveMaintenanceId in (select Id from PreventiveMaintenance where LineId in (select Id from Line where Name in ('Concetti'))) and UserId != 8
GO
DELETE UserAssignments where PreventiveMaintenanceId in (select Id from PreventiveMaintenance where PlantId in (select Id from Plant where Name in ('Semicon'))) and UserId != 8
GO
DELETE UserAssignments where PreventiveMaintenanceId in (select Id from PreventiveMaintenance where LineId in (select Id from Line where Name in ('p4', 'p5', 'Kneder', 'p8'))) and UserId != 9
GO
DELETE UserAssignments where PreventiveMaintenanceId in (select Id from PreventiveMaintenance where PlantId in (select Id from Plant where Name in ('X-Compound'))) and UserId != 9
GO

-- 03/15/2017
DELETE UserAssignments where PreventiveMaintenanceId in (select Id from PreventiveMaintenance where LineId in (select Id from Line where Name in ('RM Handling', 'FG Handling-1','FG Handling-2'))) and UserId != 7
GO
DELETE UserAssignments where PreventiveMaintenanceId in (select Id from PreventiveMaintenance where LineId in (select Id from Line where Name in ('66kv Switchyard'))) and UserId != 8
GO

-- 03/24/2017


ALTER TABLE [dbo].[Indents] DROP CONSTRAINT [FK_Indents_MaintenancePriorityType]
GO

ALTER TABLE [dbo].[IndentDetail] DROP CONSTRAINT [FK_IndentDetail_Vendors]
GO

ALTER TABLE [dbo].[IndentDetail] DROP CONSTRAINT [FK_IndentDetail_Users2]
GO

ALTER TABLE [dbo].[IndentDetail] DROP CONSTRAINT [FK_IndentDetail_Users1]
GO

ALTER TABLE [dbo].[IndentDetail] DROP CONSTRAINT [FK_IndentDetail_Users]
GO

ALTER TABLE [dbo].[IndentDetail] DROP CONSTRAINT [FK_IndentDetail_Plant]
GO

ALTER TABLE [dbo].[IndentDetail] DROP CONSTRAINT [FK_IndentDetail_Machine]
GO

ALTER TABLE [dbo].[IndentDetail] DROP CONSTRAINT [FK_IndentDetail_Line]
GO

ALTER TABLE [dbo].[IndentDetail] DROP CONSTRAINT [FK_IndentDetail_Items]
GO

ALTER TABLE [dbo].[IndentDetail] DROP CONSTRAINT [FK_IndentDetail_IndentStatus]
GO

ALTER TABLE [dbo].[IndentDetail] DROP CONSTRAINT [FK_IndentDetail_Indents]
GO

/****** Object:  Table [dbo].[Items]    Script Date: 3/24/2017 5:52:48 PM ******/
DROP TABLE [dbo].[Items]
GO

/****** Object:  Table [dbo].[IndentStatus]    Script Date: 3/24/2017 5:52:48 PM ******/
DROP TABLE [dbo].[IndentStatus]
GO

/****** Object:  Table [dbo].[Indents]    Script Date: 3/24/2017 5:52:48 PM ******/
DROP TABLE [dbo].[Indents]
GO

/****** Object:  Table [dbo].[IndentDetail]    Script Date: 3/24/2017 5:52:48 PM ******/
DROP TABLE [dbo].[IndentDetail]
GO

/****** Object:  Table [dbo].[IndentDetail]    Script Date: 3/24/2017 5:52:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[IndentDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UnitPrice] [int] NULL,
	[TotalAmount] [int] NULL,
	[PreferredVendorId] [int] NOT NULL,
	[ItemId] [int] NULL,
	[QtyNeeded] [decimal](16, 2) NOT NULL,
	[UnitOfMeasure] [varchar](70) NULL,
	[StatusId] [int] NOT NULL,
	[IsApprove] [bit] NULL,
	[ApprovedBy] [int] NULL,
	[ApprovedOn] [datetime] NULL,
	[RejectedBy] [int] NULL,
	[Rejectedon] [datetime] NULL,
	[POon] [datetime] NULL,
	[GRNon] [datetime] NULL,
	[IssuedBy] [int] NULL,
	[IssuedOn] [datetime] NULL,
	[PlantId] [int] NOT NULL,
	[LineId] [int] NULL,
	[MachineId] [int] NULL,
	[IndentId] [int] NULL,
	[EstimatePrice] [decimal](16, 2) NULL,
	[FinalPrice] [decimal](16, 2) NULL,
	[RequiredByDate] [datetime] NULL,
	[ReceivedDate] [datetime] NULL,
	[CancelReason] [varchar](max) NULL,
	[HoldReason] [varchar](max) NULL,
	[ReceivedQty] [decimal](16, 2) NULL,
	[PoDate] [datetime] NULL,
	[DeliveryDate] [datetime] NULL,
	[PoNo] [varchar](50) NULL,
	[IssuedQty] [decimal](16, 2) NULL,
 CONSTRAINT [PK_IndentDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Indents]    Script Date: 3/24/2017 5:52:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Indents](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Note] [varchar](max) NULL,
	[CreatedBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[Priority] [int] NOT NULL,
	[IndentNo] [int] NOT NULL,
	[Status] [int] NULL,
 CONSTRAINT [PK_Indents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[IndentStatus]    Script Date: 3/24/2017 5:52:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[IndentStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](max) NULL,
 CONSTRAINT [PK_IndentStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

/****** Object:  Table [dbo].[Items]    Script Date: 3/24/2017 5:52:48 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Items](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemCode] [varchar](100) NULL,
	[Name] [varchar](max) NULL,
	[Description] [varchar](max) NULL,
	[SpecificationFile] [varchar](max) NULL,
	[IsImported] [varchar](max) NULL,
	[Make] [varchar](500) NULL,
	[Model] [varchar](500) NULL,
	[Price] [decimal](16, 2) NULL,
	[TotalQty] [decimal](16, 2) NULL,
	[UnitOfMeasure] [varchar](100) NULL,
	[AvailableQty] [int] NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[IndentDetail]  WITH CHECK ADD  CONSTRAINT [FK_IndentDetail_Indents] FOREIGN KEY([IndentId])
REFERENCES [dbo].[Indents] ([Id])
GO

ALTER TABLE [dbo].[IndentDetail] CHECK CONSTRAINT [FK_IndentDetail_Indents]
GO

ALTER TABLE [dbo].[IndentDetail]  WITH CHECK ADD  CONSTRAINT [FK_IndentDetail_IndentStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[IndentStatus] ([Id])
GO

ALTER TABLE [dbo].[IndentDetail] CHECK CONSTRAINT [FK_IndentDetail_IndentStatus]
GO

ALTER TABLE [dbo].[IndentDetail]  WITH CHECK ADD  CONSTRAINT [FK_IndentDetail_Items] FOREIGN KEY([ItemId])
REFERENCES [dbo].[Items] ([Id])
GO

ALTER TABLE [dbo].[IndentDetail] CHECK CONSTRAINT [FK_IndentDetail_Items]
GO

ALTER TABLE [dbo].[IndentDetail]  WITH CHECK ADD  CONSTRAINT [FK_IndentDetail_Line] FOREIGN KEY([LineId])
REFERENCES [dbo].[Line] ([Id])
GO

ALTER TABLE [dbo].[IndentDetail] CHECK CONSTRAINT [FK_IndentDetail_Line]
GO

ALTER TABLE [dbo].[IndentDetail]  WITH CHECK ADD  CONSTRAINT [FK_IndentDetail_Machine] FOREIGN KEY([MachineId])
REFERENCES [dbo].[Machine] ([Id])
GO

ALTER TABLE [dbo].[IndentDetail] CHECK CONSTRAINT [FK_IndentDetail_Machine]
GO

ALTER TABLE [dbo].[IndentDetail]  WITH CHECK ADD  CONSTRAINT [FK_IndentDetail_Plant] FOREIGN KEY([PlantId])
REFERENCES [dbo].[Plant] ([Id])
GO

ALTER TABLE [dbo].[IndentDetail] CHECK CONSTRAINT [FK_IndentDetail_Plant]
GO

ALTER TABLE [dbo].[IndentDetail]  WITH CHECK ADD  CONSTRAINT [FK_IndentDetail_Users] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[IndentDetail] CHECK CONSTRAINT [FK_IndentDetail_Users]
GO

ALTER TABLE [dbo].[IndentDetail]  WITH CHECK ADD  CONSTRAINT [FK_IndentDetail_Users1] FOREIGN KEY([IssuedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[IndentDetail] CHECK CONSTRAINT [FK_IndentDetail_Users1]
GO

ALTER TABLE [dbo].[IndentDetail]  WITH CHECK ADD  CONSTRAINT [FK_IndentDetail_Users2] FOREIGN KEY([RejectedBy])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[IndentDetail] CHECK CONSTRAINT [FK_IndentDetail_Users2]
GO

ALTER TABLE [dbo].[IndentDetail]  WITH CHECK ADD  CONSTRAINT [FK_IndentDetail_Vendors] FOREIGN KEY([PreferredVendorId])
REFERENCES [dbo].[Vendors] ([Id])
GO

ALTER TABLE [dbo].[IndentDetail] CHECK CONSTRAINT [FK_IndentDetail_Vendors]
GO

ALTER TABLE [dbo].[Indents]  WITH CHECK ADD  CONSTRAINT [FK_Indents_MaintenancePriorityType] FOREIGN KEY([Priority])
REFERENCES [dbo].[MaintenancePriorityType] ([Id])
GO

ALTER TABLE [dbo].[Indents] CHECK CONSTRAINT [FK_Indents_MaintenancePriorityType]
GO


-- Insert


GO
SET IDENTITY_INSERT [dbo].[IndentStatus] ON 

GO
INSERT [dbo].[IndentStatus] ([Id], [Description]) VALUES (1, N'New')
GO
INSERT [dbo].[IndentStatus] ([Id], [Description]) VALUES (2, N'Approved')
GO
INSERT [dbo].[IndentStatus] ([Id], [Description]) VALUES (3, N'Rejected')
GO
INSERT [dbo].[IndentStatus] ([Id], [Description]) VALUES (4, N'PO')
GO
INSERT [dbo].[IndentStatus] ([Id], [Description]) VALUES (5, N'Issued')
GO
SET IDENTITY_INSERT [dbo].[IndentStatus] OFF
GO


CREATE TABLE [dbo].[PlantBudget](
 [Id] [int] IDENTITY(1,1) NOT NULL,
 [PlantId] [int] NULL,
 [MonthlyBudget] [decimal](16, 2) NULL,
 [EffectiveFrom] [datetime] NULL,
 [EffectiveTo] [datetime] NULL,
 CONSTRAINT [PK_PlantBudget] PRIMARY KEY CLUSTERED 
(
 [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[PlantBudget]  WITH CHECK ADD  CONSTRAINT [FK_PlantBudget_Plant] FOREIGN KEY([PlantId])
REFERENCES [dbo].[Plant] ([Id])
GO

ALTER TABLE [dbo].[PlantBudget] CHECK CONSTRAINT [FK_PlantBudget_Plant]
GO

INSERT INTO [dbo].[PlantBudget] VALUES(1, 100000, '01/01/2017', '12/31/2017');
INSERT INTO [dbo].[PlantBudget] VALUES(2, 100000, '01/01/2017', '12/31/2017');
INSERT INTO [dbo].[PlantBudget] VALUES(3, 100000, '01/01/2017', '12/31/2017');
INSERT INTO [dbo].[PlantBudget] VALUES(4, 100000, '01/01/2017', '12/31/2017');
INSERT INTO [dbo].[PlantBudget] VALUES(5, 100000, '01/01/2017', '12/31/2017');
INSERT INTO [dbo].[PlantBudget] VALUES(6, 100000, '01/01/2017', '12/31/2017');
INSERT INTO [dbo].[PlantBudget] VALUES(7, 100000, '01/01/2017', '12/31/2017');
INSERT INTO [dbo].[PlantBudget] VALUES(8, 100000, '01/01/2017', '12/31/2017');
GO

insert into Roles values('Lavel2');
GO

update BreakDown set FailureDescription = REPLACE(FailureDescription, '&nbsp;', ' ')
GO

/*
This script was created by Visual Studio on 3/28/2017 at 10:50 AM.
Run this script on DESKTOP-VFV7189\MSSQLSERVER2014.PLMM (DESKTOP-VFV7189\vatsaldesai) to make it the same as DESKTOP-VFV7189\MSSQLSERVER2014.PLMM_27032017 (DESKTOP-VFV7189\vatsaldesai).
This script performs its actions in the following order:
1. Disable foreign-key constraints.
2. Perform DELETE commands. 
3. Perform UPDATE commands.
4. Perform INSERT commands.
5. Re-enable foreign-key constraints.
Please back up your target database before running this script.
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
/*Pointer used for text / image updates. This might not be needed, but is declared here just in case*/
DECLARE @pv binary(16)
BEGIN TRANSACTION
ALTER TABLE [dbo].[UserAssignments] DROP CONSTRAINT [FK_UserAssignments_PreventiveMaintenance]
ALTER TABLE [dbo].[UserAssignments] DROP CONSTRAINT [FK_UserAssignments_Users]
ALTER TABLE [dbo].[PreventiveMaintenance] DROP CONSTRAINT [FK_PreventiveMaintenance_Machine]
ALTER TABLE [dbo].[PreventiveMaintenance] DROP CONSTRAINT [FK_PreventiveMaintenance_Plant]
ALTER TABLE [dbo].[PreventiveMaintenance] DROP CONSTRAINT [FK_PreventiveMaintenance_PreventiveScheduleType]
ALTER TABLE [dbo].[PreventiveMaintenance] DROP CONSTRAINT [FK_PreventiveMaintenance_PreventiveWorkDescription]
ALTER TABLE [dbo].[PreventiveMaintenance] DROP CONSTRAINT [FK_PreventiveMaintenance_Line]
SET IDENTITY_INSERT [dbo].[PreventiveMaintenance] ON
INSERT INTO [dbo].[PreventiveMaintenance] ([Id], [PlantId], [LineId], [MachineId], [WorkId], [Description], [Checkpoints], [ScheduleType], [Interval], [ShutdownRequired], [ScheduleStartDate], [ScheduleEndDate], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [LastReviewDate], [NextReviewDate], [PreferredVendorId]) VALUES (1507, 1, 18, 163, 11, NULL, NULL, 2, 1, 15, '20170328 10:27:31.487', NULL, NULL, '20170328 04:58:43.273', NULL, NULL, NULL, '20170327 00:00:00.000', 0)
INSERT INTO [dbo].[PreventiveMaintenance] ([Id], [PlantId], [LineId], [MachineId], [WorkId], [Description], [Checkpoints], [ScheduleType], [Interval], [ShutdownRequired], [ScheduleStartDate], [ScheduleEndDate], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [LastReviewDate], [NextReviewDate], [PreferredVendorId]) VALUES (1508, 1, 18, 163, 15, NULL, NULL, 3, 1, 15, '20170401 10:28:46.000', NULL, NULL, '20170328 04:59:34.650', NULL, NULL, NULL, '20170401 00:00:00.000', 0)
INSERT INTO [dbo].[PreventiveMaintenance] ([Id], [PlantId], [LineId], [MachineId], [WorkId], [Description], [Checkpoints], [ScheduleType], [Interval], [ShutdownRequired], [ScheduleStartDate], [ScheduleEndDate], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [LastReviewDate], [NextReviewDate], [PreferredVendorId]) VALUES (1509, 1, 18, 163, 16, NULL, NULL, 3, 1, NULL, '20170401 10:29:40.000', NULL, NULL, '20170328 05:00:25.187', NULL, NULL, NULL, '20170401 00:00:00.000', 0)
INSERT INTO [dbo].[PreventiveMaintenance] ([Id], [PlantId], [LineId], [MachineId], [WorkId], [Description], [Checkpoints], [ScheduleType], [Interval], [ShutdownRequired], [ScheduleStartDate], [ScheduleEndDate], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [LastReviewDate], [NextReviewDate], [PreferredVendorId]) VALUES (1510, 1, 18, 163, 17, NULL, NULL, 3, 1, 15, '20170401 10:30:29.000', NULL, NULL, '20170328 05:01:03.413', NULL, NULL, NULL, '20170401 00:00:00.000', 0)
INSERT INTO [dbo].[PreventiveMaintenance] ([Id], [PlantId], [LineId], [MachineId], [WorkId], [Description], [Checkpoints], [ScheduleType], [Interval], [ShutdownRequired], [ScheduleStartDate], [ScheduleEndDate], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [LastReviewDate], [NextReviewDate], [PreferredVendorId]) VALUES (1511, 1, 18, 163, 18, NULL, NULL, 4, 1, 240, '20170328 10:31:06.380', NULL, NULL, '20170328 05:01:41.097', NULL, NULL, NULL, '20171001 00:00:00.000', 0)
INSERT INTO [dbo].[PreventiveMaintenance] ([Id], [PlantId], [LineId], [MachineId], [WorkId], [Description], [Checkpoints], [ScheduleType], [Interval], [ShutdownRequired], [ScheduleStartDate], [ScheduleEndDate], [CreatedBy], [CreatedOn], [UpdatedBy], [UpdatedOn], [LastReviewDate], [NextReviewDate], [PreferredVendorId]) VALUES (1512, 1, 18, 163, 19, NULL, NULL, 3, 3, 240, '20170401 10:31:45.000', NULL, NULL, '20170328 05:02:25.217', NULL, NULL, NULL, '20170601 00:00:00.000', 0)
SET IDENTITY_INSERT [dbo].[PreventiveMaintenance] OFF
SET IDENTITY_INSERT [dbo].[UserAssignments] ON
INSERT INTO [dbo].[UserAssignments] ([Id], [PreventiveMaintenanceId], [UserId]) VALUES (7331, 1507, 9)
INSERT INTO [dbo].[UserAssignments] ([Id], [PreventiveMaintenanceId], [UserId]) VALUES (7332, 1508, 9)
INSERT INTO [dbo].[UserAssignments] ([Id], [PreventiveMaintenanceId], [UserId]) VALUES (7333, 1509, 9)
INSERT INTO [dbo].[UserAssignments] ([Id], [PreventiveMaintenanceId], [UserId]) VALUES (7334, 1510, 9)
INSERT INTO [dbo].[UserAssignments] ([Id], [PreventiveMaintenanceId], [UserId]) VALUES (7335, 1511, 9)
INSERT INTO [dbo].[UserAssignments] ([Id], [PreventiveMaintenanceId], [UserId]) VALUES (7336, 1512, 9)
SET IDENTITY_INSERT [dbo].[UserAssignments] OFF
update PreventiveMaintenance set WorkId=20 where Id=35;
ALTER TABLE [dbo].[UserAssignments]
    ADD CONSTRAINT [FK_UserAssignments_PreventiveMaintenance] FOREIGN KEY ([PreventiveMaintenanceId]) REFERENCES [dbo].[PreventiveMaintenance] ([Id])
ALTER TABLE [dbo].[UserAssignments]
    ADD CONSTRAINT [FK_UserAssignments_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
ALTER TABLE [dbo].[PreventiveMaintenance]
    ADD CONSTRAINT [FK_PreventiveMaintenance_Machine] FOREIGN KEY ([MachineId]) REFERENCES [dbo].[Machine] ([Id])
ALTER TABLE [dbo].[PreventiveMaintenance]
    ADD CONSTRAINT [FK_PreventiveMaintenance_Plant] FOREIGN KEY ([PlantId]) REFERENCES [dbo].[Plant] ([Id])
ALTER TABLE [dbo].[PreventiveMaintenance]
    ADD CONSTRAINT [FK_PreventiveMaintenance_PreventiveScheduleType] FOREIGN KEY ([ScheduleType]) REFERENCES [dbo].[PreventiveScheduleType] ([Id])
ALTER TABLE [dbo].[PreventiveMaintenance]
    ADD CONSTRAINT [FK_PreventiveMaintenance_PreventiveWorkDescription] FOREIGN KEY ([WorkId]) REFERENCES [dbo].[PreventiveWorkDescription] ([Id])
ALTER TABLE [dbo].[PreventiveMaintenance]
    ADD CONSTRAINT [FK_PreventiveMaintenance_Line] FOREIGN KEY ([LineId]) REFERENCES [dbo].[Line] ([Id])
COMMIT TRANSACTION
