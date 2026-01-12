USE [master]
GO
/****** Object:  Database [Gen2]    Script Date: 9/01/2026 5:46:14 pm ******/
CREATE DATABASE [Gen2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Gen2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Gen2.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Gen2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Gen2_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Gen2] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Gen2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Gen2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Gen2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Gen2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Gen2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Gen2] SET ARITHABORT OFF 
GO
ALTER DATABASE [Gen2] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Gen2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Gen2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Gen2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Gen2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Gen2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Gen2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Gen2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Gen2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Gen2] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Gen2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Gen2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Gen2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Gen2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Gen2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Gen2] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Gen2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Gen2] SET RECOVERY FULL 
GO
ALTER DATABASE [Gen2] SET  MULTI_USER 
GO
ALTER DATABASE [Gen2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Gen2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Gen2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Gen2] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Gen2] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Gen2] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Gen2', N'ON'
GO
ALTER DATABASE [Gen2] SET QUERY_STORE = OFF
GO
USE [Gen2]
GO
/****** Object:  User [GAL-011\x_visalk]    Script Date: 9/01/2026 5:46:15 pm ******/
CREATE USER [GAL-011\x_visalk] FOR LOGIN [GAL-011\x_visalk] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [GAL-011\Visal Kathriarachchi]    Script Date: 9/01/2026 5:46:15 pm ******/
CREATE USER [GAL-011\Visal Kathriarachchi] FOR LOGIN [GAL-011\Visal Kathriarachchi] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [GAL-011\x_visalk]
GO
ALTER ROLE [db_owner] ADD MEMBER [GAL-011\Visal Kathriarachchi]
GO
/****** Object:  Table [dbo].[AuthLoginAudit]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthLoginAudit](
	[AuthLoginAuditId] [uniqueidentifier] NOT NULL,
	[OccurredAtUtc] [datetime2](0) NOT NULL,
	[AuthUserId] [uniqueidentifier] NULL,
	[Email] [nvarchar](320) NULL,
	[Success] [bit] NOT NULL,
	[EventType] [nvarchar](50) NOT NULL,
	[FailureReason] [nvarchar](200) NULL,
	[IpAddress] [nvarchar](45) NULL,
	[UserAgent] [nvarchar](512) NULL,
 CONSTRAINT [PK_AuthLoginAudit] PRIMARY KEY CLUSTERED 
(
	[AuthLoginAuditId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuthRefreshTokens]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthRefreshTokens](
	[RefreshTokenId] [uniqueidentifier] NOT NULL,
	[AuthUserId] [uniqueidentifier] NOT NULL,
	[TokenHash] [varbinary](32) NOT NULL,
	[CreatedAtUtc] [datetime2](0) NOT NULL,
	[ExpiresAtUtc] [datetime2](0) NOT NULL,
	[RevokedAtUtc] [datetime2](0) NULL,
	[ReplacedByRefreshTokenId] [uniqueidentifier] NULL,
	[CreatedByIp] [nvarchar](45) NULL,
	[UserAgent] [nvarchar](512) NULL,
 CONSTRAINT [PK_AuthRefreshTokens] PRIMARY KEY CLUSTERED 
(
	[RefreshTokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuthRoles]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthRoles](
	[AuthRoleId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[NormalizedName] [nvarchar](128) NOT NULL,
	[CreatedAtUtc] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_AuthRoles] PRIMARY KEY CLUSTERED 
(
	[AuthRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuthUsers]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthUsers](
	[AuthUserId] [uniqueidentifier] NOT NULL,
	[Email] [nvarchar](320) NOT NULL,
	[NormalizedEmail] [nvarchar](320) NOT NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[FailedAccessCount] [int] NOT NULL,
	[LockoutUntilUtc] [datetime2](0) NULL,
	[LastLoginAtUtc] [datetime2](0) NULL,
	[PasswordChangedAtUtc] [datetime2](0) NULL,
	[CreatedAtUtc] [datetime2](0) NOT NULL,
	[UpdatedAtUtc] [datetime2](0) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_AuthUsers] PRIMARY KEY CLUSTERED 
(
	[AuthUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankAccount]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankAccount](
	[ID] [uniqueidentifier] NOT NULL,
	[AccountNumber] [smallint] NULL,
	[Reference] [nvarchar](50) NULL,
	[EntityID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_BankAccount] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BankAccountDocument]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BankAccountDocument](
	[ID] [uniqueidentifier] NOT NULL,
	[BankAccountID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BankAccountDocument] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Business]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Business](
	[ID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cause]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cause](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Title] [varchar](255) NULL,
	[Description] [varchar](max) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
 CONSTRAINT [PK_Cause] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Charity]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Charity](
	[ID] [uniqueidentifier] NOT NULL,
	[CCNumber] [nchar](10) NOT NULL,
 CONSTRAINT [PK_Charity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailTemplates]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailTemplates](
	[TemplateId] [uniqueidentifier] NOT NULL,
	[TemplateKey] [nvarchar](100) NOT NULL,
	[TemplateType] [nvarchar](50) NOT NULL,
	[SubjectTemplate] [nvarchar](300) NOT NULL,
	[BodyTemplate] [nvarchar](max) NOT NULL,
	[IsHtml] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[Description] [nvarchar](400) NULL,
	[CreatedAtUtc] [datetime2](0) NOT NULL,
	[UpdatedAtUtc] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED 
(
	[TemplateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_EmailTemplates_TemplateKey] UNIQUE NONCLUSTERED 
(
	[TemplateKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Entity]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity](
	[ID] [uniqueidentifier] NOT NULL,
	[IsIndividual] [bit] NOT NULL,
	[IRDNumber] [varchar](12) NULL,
 CONSTRAINT [PK_Entity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityAddress]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityAddress](
	[ID] [uniqueidentifier] NOT NULL,
	[IsDefault] [bit] NULL,
	[EntityID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ContactAddress] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityDocument]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityDocument](
	[ID] [uniqueidentifier] NOT NULL,
	[EntityID] [uniqueidentifier] NOT NULL,
	[EntityDocumentTypeID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EntityDocuments] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityDocumentType]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityDocumentType](
	[ID] [uniqueidentifier] NOT NULL,
	[DocumentType] [varchar](50) NOT NULL,
	[Description] [varbinary](50) NULL,
 CONSTRAINT [PK_EntityDocumentTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityEmail]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityEmail](
	[ID] [uniqueidentifier] NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[EntityID] [uniqueidentifier] NOT NULL,
	[IsVerified] [bit] NOT NULL,
 CONSTRAINT [PK_ContactEmail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityIdentification]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityIdentification](
	[ID] [uniqueidentifier] NOT NULL,
	[DrivingLicenseNumer] [varbinary](4096) NULL,
	[DrivingLicenseVersion] [varbinary](4096) NULL,
	[PassportNumber] [varbinary](4096) NULL,
	[PassportExpiryDate] [varbinary](4096) NULL,
	[DocumentID] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityPhone]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityPhone](
	[ID] [uniqueidentifier] NOT NULL,
	[Number] [nvarchar](50) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[EntityID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ContactPhone] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityRelationship]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityRelationship](
	[ID] [uniqueidentifier] NOT NULL,
	[EntityID] [uniqueidentifier] NOT NULL,
	[RelatedEntityID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EntityRelationship] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityRelationshipRole]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityRelationshipRole](
	[ID] [uniqueidentifier] NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[Description] [varchar](50) NULL,
	[EntityRelationshipID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EntityRelationshipRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityRole]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityRole](
	[ID] [uniqueidentifier] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[EntityID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EntityRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GlobalConfig]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GlobalConfig](
	[Id] [uniqueidentifier] NOT NULL,
	[Key] [nvarchar](150) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[KeyGroup] [nvarchar](100) NOT NULL,
	[CreatedAtUtc] [datetime2](7) NOT NULL,
	[UpdatedAtUtc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_GlobalConfig] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_GlobalConfig_Key] UNIQUE NONCLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GmailOAuthConfigs]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GmailOAuthConfigs](
	[GmailConfigId] [uniqueidentifier] NOT NULL,
	[ClientId] [nvarchar](300) NOT NULL,
	[ClientSecret] [nvarchar](300) NOT NULL,
	[RefreshToken] [nvarchar](1000) NOT NULL,
	[AccessToken] [nvarchar](2000) NULL,
	[AccessTokenExpiresAtUtc] [datetime2](0) NULL,
	[CreatedAtUtc] [datetime2](0) NOT NULL,
	[UpdatedAtUtc] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_GmailOAuthConfigs] PRIMARY KEY CLUSTERED 
(
	[GmailConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Group]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[ID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Individual]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Individual](
	[ID] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[MiddleName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[IsMinor] [bit] NULL,
 CONSTRAINT [PK_Individual] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MsGraphOAuthConfigs]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MsGraphOAuthConfigs](
	[MsGraphConfigId] [uniqueidentifier] NOT NULL,
	[TenantId] [nvarchar](100) NOT NULL,
	[ClientId] [nvarchar](300) NOT NULL,
	[ClientSecret] [nvarchar](300) NOT NULL,
	[RefreshToken] [nvarchar](1000) NULL,
	[AccessToken] [nvarchar](2000) NULL,
	[AccessTokenExpiresAtUtc] [datetime2](0) NULL,
	[CreatedAtUtc] [datetime2](0) NOT NULL,
	[UpdatedAtUtc] [datetime2](0) NOT NULL,
 CONSTRAINT [PK_MsGraphOAuthConfigs] PRIMARY KEY CLUSTERED 
(
	[MsGraphConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OtpRecords]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OtpRecords](
	[Id] [uniqueidentifier] NOT NULL,
	[UserEmail] [nvarchar](256) NOT NULL,
	[Purpose] [int] NOT NULL,
	[OtpHash] [varbinary](64) NOT NULL,
	[Salt] [varbinary](32) NOT NULL,
	[CreatedAtUtc] [datetime2](3) NOT NULL,
	[ExpiresAtUtc] [datetime2](3) NOT NULL,
	[FailedAttempts] [int] NOT NULL,
	[MaxAttempts] [int] NOT NULL,
	[LockedUntilUtc] [datetime2](3) NULL,
	[UsedAtUtc] [datetime2](3) NULL,
	[NextResendAllowedAtUtc] [datetime2](3) NULL,
 CONSTRAINT [PK_OtpRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [uniqueidentifier] NOT NULL,
	[Name] [varbinary](50) NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[School]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[School](
	[ID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_School] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchoolClass]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchoolClass](
	[ID] [uniqueidentifier] NOT NULL,
	[ClassYear] [int] NOT NULL,
	[ClassRoomNumber] [int] NOT NULL,
 CONSTRAINT [PK_SchoolClass] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchoolStudent]    Script Date: 9/01/2026 5:46:15 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchoolStudent](
	[IndividualID] [uniqueidentifier] NOT NULL,
	[SchoolID] [uniqueidentifier] NOT NULL,
	[ClassID] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Index [IX_AuthRefreshTokens_AuthUserId_ExpiresAtUtc]    Script Date: 9/01/2026 5:46:15 pm ******/
CREATE NONCLUSTERED INDEX [IX_AuthRefreshTokens_AuthUserId_ExpiresAtUtc] ON [dbo].[AuthRefreshTokens]
(
	[AuthUserId] ASC,
	[ExpiresAtUtc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_AuthRefreshTokens_AuthUserId_RevokedAtUtc]    Script Date: 9/01/2026 5:46:15 pm ******/
CREATE NONCLUSTERED INDEX [IX_AuthRefreshTokens_AuthUserId_RevokedAtUtc] ON [dbo].[AuthRefreshTokens]
(
	[AuthUserId] ASC,
	[RevokedAtUtc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_AuthRefreshTokens_TokenHash]    Script Date: 9/01/2026 5:46:15 pm ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_AuthRefreshTokens_TokenHash] ON [dbo].[AuthRefreshTokens]
(
	[TokenHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_AuthRoles_NormalizedName]    Script Date: 9/01/2026 5:46:15 pm ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_AuthRoles_NormalizedName] ON [dbo].[AuthRoles]
(
	[NormalizedName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_AuthUsers_NormalizedEmail]    Script Date: 9/01/2026 5:46:15 pm ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_AuthUsers_NormalizedEmail] ON [dbo].[AuthUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AuthLoginAudit] ADD  CONSTRAINT [DF_AuthLoginAudit_Id]  DEFAULT (newsequentialid()) FOR [AuthLoginAuditId]
GO
ALTER TABLE [dbo].[AuthLoginAudit] ADD  CONSTRAINT [DF_AuthLoginAudit_OccurredAtUtc]  DEFAULT (sysutcdatetime()) FOR [OccurredAtUtc]
GO
ALTER TABLE [dbo].[AuthRefreshTokens] ADD  CONSTRAINT [DF_AuthRefreshTokens_CreatedAtUtc]  DEFAULT (sysutcdatetime()) FOR [CreatedAtUtc]
GO
ALTER TABLE [dbo].[AuthRoles] ADD  CONSTRAINT [DF_AuthRoles_CreatedAtUtc]  DEFAULT (sysutcdatetime()) FOR [CreatedAtUtc]
GO
ALTER TABLE [dbo].[AuthUsers] ADD  CONSTRAINT [DF_AuthUsers_EmailConfirmed]  DEFAULT ((0)) FOR [EmailConfirmed]
GO
ALTER TABLE [dbo].[AuthUsers] ADD  CONSTRAINT [DF_AuthUsers_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AuthUsers] ADD  CONSTRAINT [DF_AuthUsers_FailedAccessCount]  DEFAULT ((0)) FOR [FailedAccessCount]
GO
ALTER TABLE [dbo].[AuthUsers] ADD  CONSTRAINT [DF_AuthUsers_CreatedAtUtc]  DEFAULT (sysutcdatetime()) FOR [CreatedAtUtc]
GO
ALTER TABLE [dbo].[AuthUsers] ADD  CONSTRAINT [DF_AuthUsers_UpdatedAtUtc]  DEFAULT (sysutcdatetime()) FOR [UpdatedAtUtc]
GO
ALTER TABLE [dbo].[EmailTemplates] ADD  CONSTRAINT [DF_EmailTemplates_Id]  DEFAULT (newsequentialid()) FOR [TemplateId]
GO
ALTER TABLE [dbo].[EmailTemplates] ADD  CONSTRAINT [DF_EmailTemplates_IsHtml]  DEFAULT ((1)) FOR [IsHtml]
GO
ALTER TABLE [dbo].[EmailTemplates] ADD  CONSTRAINT [DF_EmailTemplates_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[EmailTemplates] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAtUtc]
GO
ALTER TABLE [dbo].[EmailTemplates] ADD  DEFAULT (sysutcdatetime()) FOR [UpdatedAtUtc]
GO
ALTER TABLE [dbo].[Entity] ADD  CONSTRAINT [DF_Entity_Id]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [dbo].[EntityEmail] ADD  CONSTRAINT [DF_EntityEmail_Id]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [dbo].[EntityEmail] ADD  CONSTRAINT [DF_EntityEmail_IsVerified]  DEFAULT ((0)) FOR [IsVerified]
GO
ALTER TABLE [dbo].[GlobalConfig] ADD  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[GlobalConfig] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAtUtc]
GO
ALTER TABLE [dbo].[GlobalConfig] ADD  DEFAULT (sysutcdatetime()) FOR [UpdatedAtUtc]
GO
ALTER TABLE [dbo].[GmailOAuthConfigs] ADD  CONSTRAINT [DF_GmailOAuthConfigs_Id]  DEFAULT (newsequentialid()) FOR [GmailConfigId]
GO
ALTER TABLE [dbo].[GmailOAuthConfigs] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAtUtc]
GO
ALTER TABLE [dbo].[GmailOAuthConfigs] ADD  DEFAULT (sysutcdatetime()) FOR [UpdatedAtUtc]
GO
ALTER TABLE [dbo].[Individual] ADD  CONSTRAINT [DF_Individual_Id]  DEFAULT (newsequentialid()) FOR [ID]
GO
ALTER TABLE [dbo].[MsGraphOAuthConfigs] ADD  CONSTRAINT [DF_MsGraphOAuthConfigs_Id]  DEFAULT (newsequentialid()) FOR [MsGraphConfigId]
GO
ALTER TABLE [dbo].[MsGraphOAuthConfigs] ADD  DEFAULT (sysutcdatetime()) FOR [CreatedAtUtc]
GO
ALTER TABLE [dbo].[MsGraphOAuthConfigs] ADD  DEFAULT (sysutcdatetime()) FOR [UpdatedAtUtc]
GO
ALTER TABLE [dbo].[OtpRecords] ADD  CONSTRAINT [DF_OtpRecords_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO
ALTER TABLE [dbo].[OtpRecords] ADD  CONSTRAINT [DF_OtpRecords_FailedAttempts]  DEFAULT ((0)) FOR [FailedAttempts]
GO
ALTER TABLE [dbo].[OtpRecords] ADD  CONSTRAINT [DF_OtpRecords_MaxAttempts]  DEFAULT ((5)) FOR [MaxAttempts]
GO
ALTER TABLE [dbo].[AuthRefreshTokens]  WITH CHECK ADD  CONSTRAINT [FK_AuthRefreshTokens_EntityEmail] FOREIGN KEY([AuthUserId])
REFERENCES [dbo].[EntityEmail] ([ID])
GO
ALTER TABLE [dbo].[AuthRefreshTokens] CHECK CONSTRAINT [FK_AuthRefreshTokens_EntityEmail]
GO
ALTER TABLE [dbo].[BankAccount]  WITH CHECK ADD  CONSTRAINT [FK_BankAccount_Entity] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[BankAccount] CHECK CONSTRAINT [FK_BankAccount_Entity]
GO
ALTER TABLE [dbo].[BankAccountDocument]  WITH CHECK ADD  CONSTRAINT [FK_BankAccountDocument_BankAccount] FOREIGN KEY([BankAccountID])
REFERENCES [dbo].[BankAccount] ([ID])
GO
ALTER TABLE [dbo].[BankAccountDocument] CHECK CONSTRAINT [FK_BankAccountDocument_BankAccount]
GO
ALTER TABLE [dbo].[Business]  WITH CHECK ADD  CONSTRAINT [FK_Business_NonIndividual1] FOREIGN KEY([ID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[Business] CHECK CONSTRAINT [FK_Business_NonIndividual1]
GO
ALTER TABLE [dbo].[Charity]  WITH CHECK ADD  CONSTRAINT [FK_Charity_Entity] FOREIGN KEY([ID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[Charity] CHECK CONSTRAINT [FK_Charity_Entity]
GO
ALTER TABLE [dbo].[EntityAddress]  WITH CHECK ADD  CONSTRAINT [FK_EntityAddress_Entity] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[EntityAddress] CHECK CONSTRAINT [FK_EntityAddress_Entity]
GO
ALTER TABLE [dbo].[EntityDocument]  WITH CHECK ADD  CONSTRAINT [FK_EntityDocuments_Entity] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[EntityDocument] CHECK CONSTRAINT [FK_EntityDocuments_Entity]
GO
ALTER TABLE [dbo].[EntityDocument]  WITH CHECK ADD  CONSTRAINT [FK_EntityDocuments_EntityDocumentTypes] FOREIGN KEY([EntityDocumentTypeID])
REFERENCES [dbo].[EntityDocumentType] ([ID])
GO
ALTER TABLE [dbo].[EntityDocument] CHECK CONSTRAINT [FK_EntityDocuments_EntityDocumentTypes]
GO
ALTER TABLE [dbo].[EntityEmail]  WITH CHECK ADD  CONSTRAINT [FK_EntityEmail_Entity] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[EntityEmail] CHECK CONSTRAINT [FK_EntityEmail_Entity]
GO
ALTER TABLE [dbo].[EntityIdentification]  WITH CHECK ADD  CONSTRAINT [FK_EntityIdentification_Entity] FOREIGN KEY([ID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[EntityIdentification] CHECK CONSTRAINT [FK_EntityIdentification_Entity]
GO
ALTER TABLE [dbo].[EntityIdentification]  WITH CHECK ADD  CONSTRAINT [FK_EntityIdentification_EntityDocuments] FOREIGN KEY([DocumentID])
REFERENCES [dbo].[EntityDocument] ([ID])
GO
ALTER TABLE [dbo].[EntityIdentification] CHECK CONSTRAINT [FK_EntityIdentification_EntityDocuments]
GO
ALTER TABLE [dbo].[EntityPhone]  WITH CHECK ADD  CONSTRAINT [FK_EntityPhone_Entity] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[EntityPhone] CHECK CONSTRAINT [FK_EntityPhone_Entity]
GO
ALTER TABLE [dbo].[EntityRelationship]  WITH CHECK ADD  CONSTRAINT [FK_EntityRelationship_Individual] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[EntityRelationship] CHECK CONSTRAINT [FK_EntityRelationship_Individual]
GO
ALTER TABLE [dbo].[EntityRelationship]  WITH CHECK ADD  CONSTRAINT [FK_EntityRelationship_NonIndividual] FOREIGN KEY([RelatedEntityID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[EntityRelationship] CHECK CONSTRAINT [FK_EntityRelationship_NonIndividual]
GO
ALTER TABLE [dbo].[EntityRelationshipRole]  WITH CHECK ADD  CONSTRAINT [FK_EntityRelationshipRole_EntityRelationship] FOREIGN KEY([EntityRelationshipID])
REFERENCES [dbo].[EntityRelationship] ([ID])
GO
ALTER TABLE [dbo].[EntityRelationshipRole] CHECK CONSTRAINT [FK_EntityRelationshipRole_EntityRelationship]
GO
ALTER TABLE [dbo].[EntityRole]  WITH CHECK ADD  CONSTRAINT [FK_EntityRole_Entity] FOREIGN KEY([EntityID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[EntityRole] CHECK CONSTRAINT [FK_EntityRole_Entity]
GO
ALTER TABLE [dbo].[EntityRole]  WITH CHECK ADD  CONSTRAINT [FK_EntityRole_Role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
GO
ALTER TABLE [dbo].[EntityRole] CHECK CONSTRAINT [FK_EntityRole_Role]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Group_Entity] FOREIGN KEY([ID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_Entity]
GO
ALTER TABLE [dbo].[Individual]  WITH CHECK ADD  CONSTRAINT [FK_Individual_Entity] FOREIGN KEY([ID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[Individual] CHECK CONSTRAINT [FK_Individual_Entity]
GO
ALTER TABLE [dbo].[School]  WITH CHECK ADD  CONSTRAINT [FK_School_Entity] FOREIGN KEY([ID])
REFERENCES [dbo].[Entity] ([ID])
GO
ALTER TABLE [dbo].[School] CHECK CONSTRAINT [FK_School_Entity]
GO
ALTER TABLE [dbo].[SchoolStudent]  WITH CHECK ADD  CONSTRAINT [FK_SchoolStudent_Individual] FOREIGN KEY([IndividualID])
REFERENCES [dbo].[Individual] ([ID])
GO
ALTER TABLE [dbo].[SchoolStudent] CHECK CONSTRAINT [FK_SchoolStudent_Individual]
GO
ALTER TABLE [dbo].[SchoolStudent]  WITH CHECK ADD  CONSTRAINT [FK_SchoolStudent_School] FOREIGN KEY([SchoolID])
REFERENCES [dbo].[School] ([ID])
GO
ALTER TABLE [dbo].[SchoolStudent] CHECK CONSTRAINT [FK_SchoolStudent_School]
GO
ALTER TABLE [dbo].[SchoolStudent]  WITH CHECK ADD  CONSTRAINT [FK_SchoolStudent_SchoolClass] FOREIGN KEY([ClassID])
REFERENCES [dbo].[SchoolClass] ([ID])
GO
ALTER TABLE [dbo].[SchoolStudent] CHECK CONSTRAINT [FK_SchoolStudent_SchoolClass]
GO
USE [master]
GO
ALTER DATABASE [Gen2] SET  READ_WRITE 
GO
