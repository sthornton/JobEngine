Create Database [JobEngine]
GO

USE [JobEngine]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AssemblyJobParameters]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssemblyJobParameters](
	[AssemblyJobParameterId] [int] IDENTITY(1,1) NOT NULL,
	[AssemblyJobId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[DataType] [int] NOT NULL,
	[IsRequired] [bit] NOT NULL,
	[IsEncrypted] [bit] NOT NULL,
	[InputValidationRegExPattern] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AssemblyJobParameters] PRIMARY KEY CLUSTERED 
(
	[AssemblyJobParameterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AssemblyJobs]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AssemblyJobs](
	[AssemblyJobId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[PluginName] [nvarchar](max) NULL,
	[PluginFileName] [nvarchar](max) NULL,
	[PluginFile] [varbinary](max) NULL,
	[DateModified] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.AssemblyJobs] PRIMARY KEY CLUSTERED 
(
	[AssemblyJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerId] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[Name] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DataType]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ELMAH_Error]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ELMAH_Error](
	[ErrorId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ELMAH_Error_ErrorId]  DEFAULT (newid()),
	[Application] [nvarchar](60) NOT NULL,
	[Host] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](100) NOT NULL,
	[Source] [nvarchar](60) NOT NULL,
	[Message] [nvarchar](500) NOT NULL,
	[User] [nvarchar](50) NOT NULL,
	[StatusCode] [int] NOT NULL,
	[TimeUtc] [datetime] NOT NULL,
	[Sequence] [int] IDENTITY(1,1) NOT NULL,
	[AllXml] [ntext] NOT NULL,
 CONSTRAINT [PK_ELMAH_Error] PRIMARY KEY NONCLUSTERED 
(
	[ErrorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobEngineClients]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobEngineClients](
	[JobEngineClientId] [uniqueidentifier] NOT NULL DEFAULT (newsequentialid()),
	[CustomerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[HostName] [nvarchar](max) NULL,
	[IpAddress] [nvarchar](max) NULL,
	[Username] [nvarchar](max) NULL,
	[Password] [nvarchar](max) NULL,
	[IsEnabled] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[LastConnected] [datetime] NULL,
	[DateModified] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.JobEngineClients] PRIMARY KEY CLUSTERED 
(
	[JobEngineClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobExecutionLogs]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobExecutionLogs](
	[JobExecutionLogId] [bigint] IDENTITY(1,1) NOT NULL,
	[JobExecutionQueueId] [bigint] NOT NULL,
	[Date] [datetime] NOT NULL,
	[LogLevel] [int] NOT NULL,
	[Logger] [nvarchar](max) NULL,
	[Message] [nvarchar](max) NULL,
	[Exception] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.JobExecutionLogs] PRIMARY KEY CLUSTERED 
(
	[JobExecutionLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobExecutionQueue]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobExecutionQueue](
	[JobExecutionQueueId] [bigint] IDENTITY(1,1) NOT NULL,
	[JobEngineClientId] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[JobType] [int] NOT NULL,
	[JobSettings] [nvarchar](max) NULL,
	[JobExecutionStatus] [int] NOT NULL,
	[ResultMessage] [nvarchar](max) NULL,
	[TotalExecutionTimeInMs] [bigint] NULL,
	[ScheduledJobId] [int] NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[DateReceivedByClient] [datetime] NULL,
	[DateExecutionCompleted] [datetime] NULL,
	[DateEnteredQueue] [datetime] NULL,
 CONSTRAINT [PK_dbo.JobExecutionQueue] PRIMARY KEY CLUSTERED 
(
	[JobExecutionQueueId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobExecutionStatus]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobExecutionStatus](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[JobType]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobType](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[LogLevel]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogLevel](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PowerShellJobParameters]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PowerShellJobParameters](
	[PowerShellJobParameterId] [int] IDENTITY(1,1) NOT NULL,
	[PowerShellJobId] [int] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[DataType] [int] NOT NULL,
	[IsRequired] [bit] NOT NULL,
	[IsEncrypted] [bit] NOT NULL,
	[InputValidationRegExPattern] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PowerShellJobResults]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PowerShellJobResults](
	[PowerShellJobResultsId] [int] IDENTITY(1,1) NOT NULL,
	[JobExecutionQueueId] [bigint] NOT NULL,
	[ScheduledJobId] [int] NULL,
	[Results] [varchar](max) NULL,
	[Errors] [varchar](max) NULL,
	[DateCompleted] [datetime] NOT NULL,
 CONSTRAINT [PK_PowerShellJobResults] PRIMARY KEY CLUSTERED 
(
	[PowerShellJobResultsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PowerShellJobs]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PowerShellJobs](
	[PowerShellJobId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[Script] [varchar](max) NOT NULL,
	[PSResultType] [int] NOT NULL,
	[OverwriteExistingData] [bit] NULL,
	[DateModified] [datetime] NOT NULL,
	[ModifiedBy] [varchar](100) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_PowerShellJobs] PRIMARY KEY CLUSTERED 
(
	[PowerShellJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PSResultType]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PSResultType](
	[PSResultTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
 CONSTRAINT [PK_PSResultType] PRIMARY KEY CLUSTERED 
(
	[PSResultTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ScheduledJobs]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ScheduledJobs](
	[ScheduledJobId] [int] IDENTITY(1,1) NOT NULL,
	[JobEngineClientId] [uniqueidentifier] NOT NULL,
	[CustomerId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[JobType] [int] NOT NULL,
	[JobSettings] [nvarchar](max) NULL,
	[CronSchedule] [nvarchar](max) NULL,
	[LastExecutionResult] [nvarchar](max) NULL,
	[LastExecutionTime] [datetime] NULL,
	[TotalExecutionTimeInMs] [bigint] NULL,
	[IsActive] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](max) NULL,
	[DateModified] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.ScheduledJobs] PRIMARY KEY CLUSTERED 
(
	[ScheduledJobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AssemblyJobParameters]  WITH CHECK ADD  CONSTRAINT [FK_AssemblyJobParameters_DataType] FOREIGN KEY([DataType])
REFERENCES [dbo].[DataType] ([Id])
GO
ALTER TABLE [dbo].[AssemblyJobParameters] CHECK CONSTRAINT [FK_AssemblyJobParameters_DataType]
GO
ALTER TABLE [dbo].[AssemblyJobParameters]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AssemblyJobParameters_dbo.AssemblyJobs_AssemblyJobId] FOREIGN KEY([AssemblyJobId])
REFERENCES [dbo].[AssemblyJobs] ([AssemblyJobId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AssemblyJobParameters] CHECK CONSTRAINT [FK_dbo.AssemblyJobParameters_dbo.AssemblyJobs_AssemblyJobId]
GO
ALTER TABLE [dbo].[JobEngineClients]  WITH CHECK ADD  CONSTRAINT [FK_dbo.JobEngineClients_dbo.Customers_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobEngineClients] CHECK CONSTRAINT [FK_dbo.JobEngineClients_dbo.Customers_CustomerId]
GO
ALTER TABLE [dbo].[JobExecutionLogs]  WITH CHECK ADD  CONSTRAINT [FK_JobExecutionLogs_LogLevel] FOREIGN KEY([LogLevel])
REFERENCES [dbo].[LogLevel] ([Id])
GO
ALTER TABLE [dbo].[JobExecutionLogs] CHECK CONSTRAINT [FK_JobExecutionLogs_LogLevel]
GO
ALTER TABLE [dbo].[JobExecutionQueue]  WITH CHECK ADD  CONSTRAINT [FK_JobExecutionQueue_JobExecutionStatus] FOREIGN KEY([JobExecutionStatus])
REFERENCES [dbo].[JobExecutionStatus] ([Id])
GO
ALTER TABLE [dbo].[JobExecutionQueue] CHECK CONSTRAINT [FK_JobExecutionQueue_JobExecutionStatus]
GO
ALTER TABLE [dbo].[JobExecutionQueue]  WITH CHECK ADD  CONSTRAINT [FK_JobExecutionQueue_JobType] FOREIGN KEY([JobType])
REFERENCES [dbo].[JobType] ([Id])
GO
ALTER TABLE [dbo].[JobExecutionQueue] CHECK CONSTRAINT [FK_JobExecutionQueue_JobType]
GO
ALTER TABLE [dbo].[ScheduledJobs]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ScheduledJobs_dbo.Customers_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ScheduledJobs] CHECK CONSTRAINT [FK_dbo.ScheduledJobs_dbo.Customers_CustomerId]
GO
ALTER TABLE [dbo].[ScheduledJobs]  WITH CHECK ADD  CONSTRAINT [FK_ScheduledJobs_JobType] FOREIGN KEY([JobType])
REFERENCES [dbo].[JobType] ([Id])
GO
ALTER TABLE [dbo].[ScheduledJobs] CHECK CONSTRAINT [FK_ScheduledJobs_JobType]
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorsXml]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml]
(
    @Application NVARCHAR(60),
    @PageIndex INT = 0,
    @PageSize INT = 15,
    @TotalCount INT OUTPUT
)
AS 

    SET NOCOUNT ON

    DECLARE @FirstTimeUTC DATETIME
    DECLARE @FirstSequence INT
    DECLARE @StartRow INT
    DECLARE @StartRowIndex INT

    SELECT 
        @TotalCount = COUNT(1) 
    FROM 
        [ELMAH_Error]
    WHERE 
        [Application] = @Application

    -- Get the ID of the first error for the requested page

    SET @StartRowIndex = @PageIndex * @PageSize + 1

    IF @StartRowIndex <= @TotalCount
    BEGIN

        SET ROWCOUNT @StartRowIndex

        SELECT  
            @FirstTimeUTC = [TimeUtc],
            @FirstSequence = [Sequence]
        FROM 
            [ELMAH_Error]
        WHERE   
            [Application] = @Application
        ORDER BY 
            [TimeUtc] DESC, 
            [Sequence] DESC

    END
    ELSE
    BEGIN

        SET @PageSize = 0

    END

    -- Now set the row count to the requested page size and get
    -- all records below it for the pertaining application.

    SET ROWCOUNT @PageSize

    SELECT 
        errorId     = [ErrorId], 
        application = [Application],
        host        = [Host], 
        type        = [Type],
        source      = [Source],
        message     = [Message],
        [user]      = [User],
        statusCode  = [StatusCode], 
        time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + 'Z'
    FROM 
        [ELMAH_Error] error
    WHERE
        [Application] = @Application
    AND
        [TimeUtc] <= @FirstTimeUTC
    AND 
        [Sequence] <= @FirstSequence
    ORDER BY
        [TimeUtc] DESC, 
        [Sequence] DESC
    FOR
        XML AUTO


GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorXml]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]
(
    @Application NVARCHAR(60),
    @ErrorId UNIQUEIDENTIFIER
)
AS

    SET NOCOUNT ON

    SELECT 
        [AllXml]
    FROM 
        [ELMAH_Error]
    WHERE
        [ErrorId] = @ErrorId
    AND
        [Application] = @Application


GO
/****** Object:  StoredProcedure [dbo].[ELMAH_LogError]    Script Date: 7/17/2015 10:39:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ELMAH_LogError]
(
    @ErrorId UNIQUEIDENTIFIER,
    @Application NVARCHAR(60),
    @Host NVARCHAR(30),
    @Type NVARCHAR(100),
    @Source NVARCHAR(60),
    @Message NVARCHAR(500),
    @User NVARCHAR(50),
    @AllXml NTEXT,
    @StatusCode INT,
    @TimeUtc DATETIME
)
AS

    SET NOCOUNT ON

    INSERT
    INTO
        [ELMAH_Error]
        (
            [ErrorId],
            [Application],
            [Host],
            [Type],
            [Source],
            [Message],
            [User],
            [AllXml],
            [StatusCode],
            [TimeUtc]
        )
    VALUES
        (
            @ErrorId,
            @Application,
            @Host,
            @Type,
            @Source,
            @Message,
            @User,
            @AllXml,
            @StatusCode,
            @TimeUtc
        )


GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DataType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JobExecutionStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'JobType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Automatically generated. Contents will be overwritten on app startup. Table & contents generated by https://github.com/timabell/ef-enum-to-lookup' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LogLevel'
GO


USE [JobEngine]
GO

/****** Object:  Table [dbo].[ClientInstallFiles]    Script Date: 3/7/2016 1:08:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ClientInstallFiles](
	[ClientInstallFileId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[File] [varbinary](max) NOT NULL,
	[Version] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](100) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_ClientInstallFiles] PRIMARY KEY CLUSTERED 
(
	[ClientInstallFileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO




/*

  ____                _         ____        _         ____  _                          _               _____     _     _       
 / ___|_ __ ___  __ _| |_ ___  |  _ \  __ _| |_ ___  |  _ \(_)_ __ ___   ___ _ __  ___(_) ___  _ __   |_   _|_ _| |__ | | ___  
| |   | '__/ _ \/ _` | __/ _ \ | | | |/ _` | __/ _ \ | | | | | '_ ` _ \ / _ \ '_ \/ __| |/ _ \| '_ \    | |/ _` | '_ \| |/ _ \ 
| |___| | |  __/ (_| | ||  __/ | |_| | (_| | ||  __/ | |_| | | | | | | |  __/ | | \__ \ | (_) | | | |   | | (_| | |_) | |  __/ 
 \____|_|  \___|\__,_|\__\___| |____/ \__,_|\__\___| |____/|_|_| |_| |_|\___|_| |_|___/_|\___/|_| |_|   |_|\__,_|_.__/|_|\___| 

 */

 --Make sure you set the Start and End Date below on row 58 and 59 
--Create the tables 
BEGIN TRY 
DROP TABLE [DateDimension] 
END TRY 
BEGIN CATCH 
--DO NOTHING 
END CATCH 
CREATE TABLE [dbo].[DateDimension]( 
--[DateSK] [int] IDENTITY(1,1) NOT NULL--Use this line if you just want an autoincrementing counter AND COMMENT BELOW LINE 
[DateSK] [int] NOT NULL--TO MAKE THE DateSK THE YYYYMMDD FORMAT USE THIS LINE AND COMMENT ABOVE LINE. 
, [FullDate] [datetime] NOT NULL 
, [Day] [tinyint] NOT NULL 
, [DaySuffix] [varchar](4) NOT NULL 
, [DayOfWeek] [varchar](9) NOT NULL 
, [DayOfWeekNumber] [int] NOT NULL 
, [DayOfWeekInMonth] [tinyint] NOT NULL 
, [DayOfYearNumber] [int] NOT NULL 
, [RelativeDays] int NOT NULL 
, [WeekOfYearNumber] [tinyint] NOT NULL 
, [WeekOfMonthNumber] [tinyint] NOT NULL 
, [RelativeWeeks] int NOT NULL 
, [CalendarMonthNumber] [tinyint] NOT NULL 
, [CalendarMonthName] [varchar](9) NOT NULL 
, [RelativeMonths] int NOT NULL 
, [CalendarQuarterNumber] [tinyint] NOT NULL 
, [CalendarQuarterName] [varchar](6) NOT NULL 
, [RelativeQuarters] int NOT NULL 
, [CalendarYearNumber] int NOT NULL 
, [RelativeYears] int NOT NULL 
, [StandardDate] [varchar](10) NULL 
, [WeekDayFlag] bit NOT NULL 
, [HolidayFlag] bit NOT NULL 
, [OpenFlag] bit NOT NULL 
, [FirstDayOfCalendarMonthFlag] bit NOT NULL 
, [LastDayOfCalendarMonthFlag] bit NOT NULL 
, [HolidayText] [varchar](50) NULL 
CONSTRAINT [PK_DateDimension] PRIMARY KEY CLUSTERED 
( 
[DateSK] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY] 
) ON [PRIMARY]

GO


--Populate Date dimension

TRUNCATE TABLE DateDimension

--IF YOU ARE USING THE YYYYMMDD format for the primary key then you need to comment out this line. 
--DBCC CHECKIDENT (DateDimension, RESEED, 60000) --In case you need to add earlier dates later.

DECLARE @tmpDOW TABLE (DOW INT, Cntr INT)--Table for counting DOW occurance in a month 
INSERT INTO @tmpDOW(DOW, Cntr) VALUES(1,0)--Used in the loop below 
INSERT INTO @tmpDOW(DOW, Cntr) VALUES(2,0) 
INSERT INTO @tmpDOW(DOW, Cntr) VALUES(3,0) 
INSERT INTO @tmpDOW(DOW, Cntr) VALUES(4,0) 
INSERT INTO @tmpDOW(DOW, Cntr) VALUES(5,0) 
INSERT INTO @tmpDOW(DOW, Cntr) VALUES(6,0) 
INSERT INTO @tmpDOW(DOW, Cntr) VALUES(7,0)

DECLARE @StartDate datetime 
, @EndDate datetime 
, @Date datetime 
, @WDofMonth INT 
, @CurrentMonth INT 
, @CurrentDate date = getdate() 
  
SELECT @StartDate = '1/1/2016'  -- Set The start and end date 
, @EndDate = '1/01/2030'--Non inclusive. Stops on the day before this. 
, @CurrentMonth = 1 --Counter used in loop below.

SELECT @Date = @StartDate

WHILE @Date < @EndDate 
BEGIN 
  
IF DATEPART(MONTH,@Date) <> @CurrentMonth 
BEGIN 
SELECT @CurrentMonth = DATEPART(MONTH,@Date) 
UPDATE @tmpDOW SET Cntr = 0 
END

UPDATE @tmpDOW 
SET Cntr = Cntr + 1 
WHERE DOW = DATEPART(DW,@DATE)

SELECT @WDofMonth = Cntr 
FROM @tmpDOW 
WHERE DOW = DATEPART(DW,@DATE) 

INSERT INTO DateDimension 
( 
[DateSK],--TO MAKE THE DateSK THE YYYYMMDD FORMAT UNCOMMENT THIS LINE... Comment for autoincrementing. 
[FullDate] 
, [Day] 
, [DaySuffix] 
, [DayOfWeek] 
, [DayOfWeekNumber] 
, [DayOfWeekInMonth] 
, [DayOfYearNumber] 
, [RelativeDays] 
  
, [WeekOfYearNumber] 
, [WeekOfMonthNumber] 
, [RelativeWeeks] 
  
, [CalendarMonthNumber] 
, [CalendarMonthName] 
, [RelativeMonths] 
  
, [CalendarQuarterNumber] 
, [CalendarQuarterName] 
, [RelativeQuarters] 
  
, [CalendarYearNumber] 
, [RelativeYears] 
  
, [StandardDate] 
, [WeekDayFlag] 
, [HolidayFlag] 
, [OpenFlag] 
, [FirstDayOfCalendarMonthFlag] 
, [LastDayOfCalendarMonthFlag] 
  
) 
  
SELECT 
  
     CONVERT(VARCHAR,@Date,112), --TO MAKE THE DateSK THE YYYYMMDD FORMAT UNCOMMENT THIS LINE COMMENT FOR AUTOINCREMENT 
     @Date [FullDate] 
     , DATEPART(DAY,@DATE) [Day] 
     , CASE 
     WHEN DATEPART(DAY,@DATE) IN (11,12,13) THEN CAST(DATEPART(DAY,@DATE) AS VARCHAR) + 'th' 
     WHEN RIGHT(DATEPART(DAY,@DATE),1) = 1 THEN CAST(DATEPART(DAY,@DATE) AS VARCHAR) + 'st' 
     WHEN RIGHT(DATEPART(DAY,@DATE),1) = 2 THEN CAST(DATEPART(DAY,@DATE) AS VARCHAR) + 'nd' 
     WHEN RIGHT(DATEPART(DAY,@DATE),1) = 3 THEN CAST(DATEPART(DAY,@DATE) AS VARCHAR) + 'rd' 
     ELSE CAST(DATEPART(DAY,@DATE) AS VARCHAR) + 'th' 
     END AS [DaySuffix] 
     , CASE DATEPART(DW, @DATE) 
     WHEN 1 THEN 'Sunday' 
     WHEN 2 THEN 'Monday' 
     WHEN 3 THEN 'Tuesday' 
     WHEN 4 THEN 'Wednesday' 
     WHEN 5 THEN 'Thursday' 
     WHEN 6 THEN 'Friday' 
     WHEN 7 THEN 'Saturday' 
     END AS [DayOfWeek] 
     ,DATEPART(DW, @DATE) AS [DayOfWeekNumber] 
     , @WDofMonth [DOWInMonth]--Occurance of this day in this month. If Third Monday then 3 and DOW would be Monday. 
     , DATEPART(dy,@Date) [DayOfYearNumber]--Day of the year. 0 - 365/366 
     , DATEDIFF(dd,@CurrentDate,@Date) as [RelativeDays] 
     
     , DATEPART(ww,@Date) [WeekOfYearNumber]--0-52/53 
     , DATEPART(ww,@Date) + 1 - 
            DATEPART(ww,CAST(DATEPART(mm,@Date) AS VARCHAR) + '/1/' + CAST(DATEPART(yy,@Date) AS VARCHAR)) [WeekOfMonthNumber] 
     , DATEDIFF(ww,@CurrentDate,@Date) as [RelativeWeeks] 
     
     , DATEPART(MONTH,@DATE) as [CalendarMonthNumber] --To be converted with leading zero later. 
     , DATENAME(MONTH,@DATE) as [CalendarMonthName] 
     , DATEDIFF(MONTH,@CurrentDate,@Date) as [RelativeMonths] 
     
     , DATEPART(qq,@DATE) as [CalendarQuarterNumber] --Calendar quarter 
     , CASE DATEPART(qq,@DATE) 
             WHEN 1 THEN 'First' 
             WHEN 2 THEN 'Second' 
             WHEN 3 THEN 'Third' 
             WHEN 4 THEN 'Fourth' 
        END AS [CalendarQuarterName] 
     , DATEDIFF(qq,@CurrentDate,@Date) as [RelativeQuarters] 
        
        
     , DATEPART(YEAR,@Date) as [CalendarYearNumber] 
     , DATEDIFF(YEAR,@CurrentDate,@Date) as [RelativeYears] 
     
     , RIGHT('0' + convert(varchar(2),MONTH(@Date)),2) + '/' + Right('0' + convert(varchar(2),DAY(@Date)),2) + '/' + convert(varchar(4),YEAR(@Date)) 
     , CASE DATEPART(DW, @DATE) 
             WHEN 1 THEN 0 
             WHEN 2 THEN 1 
             WHEN 3 THEN 1 
             WHEN 4 THEN 1 
             WHEN 5 THEN 1 
             WHEN 6 THEN 1 
             WHEN 7 THEN 0 
         END AS [WeekDayFlag] 
         
     , 0 as HolidayFlag 
     
     , CASE DATEPART(DW, @DATE) 
             WHEN 1 THEN 0 
             WHEN 2 THEN 1 
             WHEN 3 THEN 1 
             WHEN 4 THEN 1 
             WHEN 5 THEN 1 
             WHEN 6 THEN 1 
             WHEN 7 THEN 1 
         END AS OpenFlag 
         
     , CASE DATEPART(dd,@Date) 
        WHEN 1 
            THEN 1 
        ELSE 0 
        END as [FirstDayOfCalendarMonthFlag] 
        
     , CASE 
            WHEN DateAdd(day, -1, DateAdd( month, DateDiff(month , 0,@Date)+1 , 0)) = @Date 
                THEN 1 
            ELSE 0 
        END as [LastDayOfCalendarMonthFlag]

SELECT @Date = DATEADD(dd,1,@Date) 
END

 

--Add HOLIDAYS --------------------------------------------------------------------------------------------------------------

-- New Years Day --------------------------------------------------------------------------------------------- 
UPDATE dbo.DateDimension 
SET HolidayText = 'New Year''s Day', 
    HolidayFlag = 1, 
    OpenFlag = 0 
WHERE [CalendarMonthNumber] = 1 AND [DAY] = 1 
--Set OpenFlag = 0 if New Year's Day is on weekend 
UPDATE dbo.DateDimension 
SET OpenFlag = 0 
WHERE DateSK in 
(Select CASE 
    WHEN DayOfWeek = 'Sunday' 
    THEN DATESK + 1 
    END 
FRom DateDimension 
where CalendarMonthNumber = 1 
and [DAY] = 1)

--Martin Luther King Day --------------------------------------------------------------------------------------- 
--Third Monday in January starting in 1983 
UPDATE DateDimension 
SET HolidayText = 'Martin Luther King Jr. Day', 
    HolidayFlag = 1, 
    OpenFlag = 0 
WHERE [CalendarMonthNumber] = 1--January 
AND [Dayofweek] = 'Monday' 
AND CalendarYearNumber >= 1983--When holiday was official 
AND [DayOfWeekInMonth] = 3--Third X day of current month. 
GO

--President's Day --------------------------------------------------------------------------------------- 
--Third Monday in February. 
UPDATE DateDimension 
SET HolidayText = 'President''s Day', 
    HolidayFlag = 1, 
    OpenFlag = 0 
WHERE [CalendarMonthNumber] = 2--February 
AND [Dayofweek] = 'Monday' 
AND [DayOfWeekInMonth] = 3--Third occurance of a monday in this month. 
GO

--Memorial Day ---------------------------------------------------------------------------------------- 
--Last Monday in May 
UPDATE dbo.DateDimension 
SET HolidayText = 'Memorial Day', 
    HolidayFlag = 1, 
    OpenFlag = 0 
FROM DateDimension 
WHERE DateSK IN 
( 
SELECT MAX([DateSK]) 
FROM dbo.DateDimension 
WHERE [CalendarMonthName] = 'May' 
AND [DayOfWeek] = 'Monday' 
GROUP BY CalendarYearNumber, [CalendarMonthNumber] 
)

--4th of July --------------------------------------------------------------------------------------------- 
UPDATE dbo.DateDimension 
SET HolidayText = 'Independance Day', 
    HolidayFlag = 1, 
    OpenFlag = 0 
WHERE [CalendarMonthNumber] = 7 AND [DAY] = 4 
--Set OpenFlag = 0 if July 4th is on weekend 
UPDATE dbo.DateDimension 
SET OpenFlag = 0 
WHERE DateSK in 
(Select CASE 
    WHEN DayOfWeek = 'Sunday' 
    THEN DATESK + 1 
    END 
FRom DateDimension 
where CalendarMonthNumber = 7 
and [DAY] = 4)

--Labor Day ------------------------------------------------------------------------------------------- 
--First Monday in September 
UPDATE dbo.DateDimension 
SET HolidayText = 'Labor Day', 
    HolidayFlag = 1, 
    OpenFlag = 0 
FROM DateDimension 
WHERE DateSK IN 
( 
SELECT MIN([DateSK]) 
FROM dbo.DateDimension 
WHERE [CalendarMonthName] = 'September' 
AND [DayOfWeek] = 'Monday' 
GROUP BY CalendarYearNumber, [CalendarMonthNumber] 
)

--Columbus Day------------------------------------------------------------------------------------------ 
--2nd Monday in October 
UPDATE dbo.DateDimension 
SET HolidayText = 'Columbus Day', 
    HolidayFlag = 1, 
    OpenFlag = 0 
FROM DateDimension 
WHERE DateSK IN 
( 
SELECT MIN(DateSK) 
FROM dbo.DateDimension 
WHERE [CalendarMonthName] = 'October' 
AND [DayOfWeek] = 'Monday' 
AND [DayOfWeekInMonth] = 2 
GROUP BY CalendarYearNumber, 
    [CalendarMonthNumber] 
)

--Veteran's Day -------------------------------------------------------------------------------------------------------------- 
UPDATE DateDimension 
SET HolidayText = 'Veteran''s Day', 
    HolidayFlag = 1, 
    OpenFlag = 0 
WHERE DateSK in ( 
    Select CASE 
        WHEN DayOfWeek = 'Saturday' 
            THEN DateSK - 1 
        WHEN DayOfWeek = 'Sunday' 
            THEN DateSK + 1 
        ELSE DateSK 
        END as VeteransDateSK 
    FROM DateDimension 
    WHERE [CalendarMonthNumber]  = 11 
     AND [DAY] = 11) 
GO

--THANKSGIVING -------------------------------------------------------------------------------------------------------------- 
--Fourth THURSDAY in November. 
UPDATE DateDimension 
SET HolidayText = 'Thanksgiving Day', 
    HolidayFlag = 1, 
    OpenFlag = 0 
WHERE [CalendarMonthNumber] = 11 
AND [DAYOFWEEK] = 'Thursday' 
AND [DayOfWeekInMonth] = 4 
GO

--CHRISTMAS ------------------------------------------------------------------------------------------- 
UPDATE dbo.DateDimension 
SET HolidayText = 'Christmas Day', 
    HolidayFlag = 1, 
    OpenFlag = 0 
WHERE [CalendarMonthNumber] = 12 AND [DAY] = 25 
--Set OpenFlag = 0 if Christmas on weekend 
UPDATE dbo.DateDimension 
SET OpenFlag = 0 
WHERE DateSK in 
(Select CASE 
    WHEN DayOfWeek = 'Sunday' 
    THEN DATESK + 1 
    WHEN Dayofweek = 'Saturday' 
    THEN DateSK - 1 
    END 
FRom DateDimension 
where CalendarMonthNumber = 12 
and DAY = 25) 

-- Valentine's Day --------------------------------------------------------------------------------------------- 
UPDATE dbo.DateDimension 
SET HolidayText = 'Valentine''s Day' 
WHERE CalendarMonthNumber = 2 AND [DAY] = 14

-- Saint Patrick's Day ----------------------------------------------------------------------------------------- 
UPDATE dbo.DateDimension 
SET HolidayText = 'Saint Patrick''s Day' 
WHERE [CalendarMonthNumber] = 3 AND [DAY] = 17 
GO

--Mother's Day --------------------------------------------------------------------------------------- 
--Second Sunday of May 
UPDATE DateDimension 
SET HolidayText = 'Mother''s Day'--select * from DateDimension 
WHERE [CalendarMonthNumber] = 5--May 
AND [Dayofweek] = 'Sunday' 
AND [DayOfWeekInMonth] = 2--Second occurance of a monday in this month. 
GO 
--Father's Day --------------------------------------------------------------------------------------- 
--Third Sunday of June 
UPDATE DateDimension 
SET HolidayText = 'Father''s Day'--select * from DateDimension 
WHERE [CalendarMonthNumber] = 6--June 
AND [Dayofweek] = 'Sunday' 
AND [DayOfWeekInMonth] = 3--Third occurance of a monday in this month. 
GO 
--Halloween 10/31 ---------------------------------------------------------------------------------- 
UPDATE dbo.DateDimension 
SET HolidayText = 'Halloween' 
WHERE [CalendarMonthNumber] = 10 AND [DAY] = 31 
--Election Day-------------------------------------------------------------------------------------- 
--The first Tuesday after the first Monday in November. 
BEGIN TRY 
drop table #tmpHoliday 
END TRY 
BEGIN CATCH 
--do nothing 
END CATCH

CREATE TABLE #tmpHoliday(ID INT IDENTITY(1,1), DateID int, Week TINYINT, YEAR CHAR(4), DAY CHAR(2))

INSERT INTO #tmpHoliday(DateID, [YEAR],[DAY]) 
SELECT [DateSK], CalendarYearNumber, [DAY] 
FROM dbo.DateDimension 
WHERE [CalendarMonthNumber] = 11 
AND [Dayofweek] = 'Monday' 
ORDER BY CalendarYearNumber, [DAY]

DECLARE @CNTR INT, @POS INT, @STARTYEAR INT, @ENDYEAR INT, @CURRENTYEAR INT, @MINDAY INT

SELECT @CURRENTYEAR = MIN([YEAR]) 
, @STARTYEAR = MIN([YEAR]) 
, @ENDYEAR = MAX([YEAR]) 
FROM #tmpHoliday

WHILE @CURRENTYEAR <= @ENDYEAR 
BEGIN 
SELECT @CNTR = COUNT([YEAR]) 
FROM #tmpHoliday 
WHERE [YEAR] = @CURRENTYEAR

SET @POS = 1

WHILE @POS <= @CNTR 
BEGIN 
SELECT @MINDAY = MIN(DAY) 
FROM #tmpHoliday 
WHERE [YEAR] = @CURRENTYEAR 
AND [WEEK] IS NULL

UPDATE #tmpHoliday 
SET [WEEK] = @POS 
WHERE [YEAR] = @CURRENTYEAR 
AND [DAY] = @MINDAY

SELECT @POS = @POS + 1 
END

SELECT @CURRENTYEAR = @CURRENTYEAR + 1 
END

UPDATE DT 
SET HolidayText = 'Election Day' 
FROM dbo.DateDimension DT 
JOIN #tmpHoliday HL 
ON (HL.DateID + 1) = DT.DateSK 
WHERE [WEEK] = 1

DROP TABLE #tmpHoliday 
GO 
-------------------------------------------------------------------------------------------------------- 
PRINT CONVERT(VARCHAR,GETDATE(),113)--USED FOR CHECKING RUN TIME.

--DateDimension indexes--------------------------------------------------------------------------------------------- 
CREATE UNIQUE NONCLUSTERED INDEX [IDX_DateDimension_Date] ON [dbo].[DateDimension] 
( 
[FullDate] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_Day] ON [dbo].[DateDimension] 
( 
[Day] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_DayOfWeek] ON [dbo].[DateDimension] 
( 
[DayOfWeek] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_DOWInMonth] ON [dbo].[DateDimension] 
( 
[DayOfWeekInMonth] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_DayOfYear] ON [dbo].[DateDimension] 
( 
[DayOfYearNumber] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_WeekOfYear] ON [dbo].[DateDimension] 
( 
[WeekOfYearNumber] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_WeekOfMonth] ON [dbo].[DateDimension] 
( 
[WeekOfMonthNumber] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_Month] ON [dbo].[DateDimension] 
( 
[CalendarMonthNumber] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_MonthName] ON [dbo].[DateDimension] 
( 
[CalendarMonthName] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_Quarter] ON [dbo].[DateDimension] 
( 
[CalendarQuarterNumber] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_QuarterName] ON [dbo].[DateDimension] 
( 
[CalendarQuarterName] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_DateDimension_Year] ON [dbo].[DateDimension] 
( 
[CalendarYearNumber] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IDX_dim_Time_HolidayText] ON [dbo].[DateDimension] 
( 
[HolidayText] ASC 
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]

PRINT convert(varchar,getdate(),113)--USED FOR CHECKING RUN TIME.


