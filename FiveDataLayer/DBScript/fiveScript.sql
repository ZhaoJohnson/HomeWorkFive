USE [master]
GO
/****** Object:  Database [StockMarket]    Script Date: 2016/11/7 17:23:57 ******/
CREATE DATABASE [StockMarket]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'StockMarket', FILENAME = N'D:\DBhome\StockMarket.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'StockMarket_log', FILENAME = N'D:\DBhome\StockMarket_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [StockMarket] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [StockMarket].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [StockMarket] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [StockMarket] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [StockMarket] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [StockMarket] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [StockMarket] SET ARITHABORT OFF 
GO
ALTER DATABASE [StockMarket] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [StockMarket] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [StockMarket] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [StockMarket] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [StockMarket] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [StockMarket] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [StockMarket] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [StockMarket] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [StockMarket] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [StockMarket] SET  DISABLE_BROKER 
GO
ALTER DATABASE [StockMarket] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [StockMarket] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [StockMarket] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [StockMarket] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [StockMarket] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [StockMarket] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [StockMarket] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [StockMarket] SET RECOVERY FULL 
GO
ALTER DATABASE [StockMarket] SET  MULTI_USER 
GO
ALTER DATABASE [StockMarket] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [StockMarket] SET DB_CHAINING OFF 
GO
ALTER DATABASE [StockMarket] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [StockMarket] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [StockMarket] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'StockMarket', N'ON'
GO
USE [StockMarket]
GO
/****** Object:  Table [dbo].[Stock]    Script Date: 2016/11/7 17:23:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Stock](
	[StockCodeId] [varchar](20) NOT NULL,
	[StockName] [nvarchar](20) NOT NULL,
	[StockTypeId] [int] NOT NULL,
	[IsActivity] [bit] NOT NULL,
	[CreatedAt] [datetimeoffset](0) NULL,
	[LastModifiedAt] [datetimeoffset](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[StockCodeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StockOperationTracking]    Script Date: 2016/11/7 17:23:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockOperationTracking](
	[StockOperationTrackingId] [int] IDENTITY(1,1) NOT NULL,
	[OperationDate] [datetime] NOT NULL,
	[StockCount] [int] NULL,
	[CreatedAt] [datetimeoffset](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[StockOperationTracking] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StockReport]    Script Date: 2016/11/7 17:23:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StockReport](
	[StockReportId] [int] IDENTITY(1,1) NOT NULL,
	[StockCodeId] [varchar](20) NULL,
	[Author] [nvarchar](100) NULL,
	[Change] [nvarchar](100) NULL,
	[Companycode] [int] NULL,
	[ReportTime] [datetime] NULL,
	[InstitutionId] [int] NULL,
	[Infocode] [nvarchar](100) NULL,
	[InsCode] [int] NULL,
	[InsName] [nvarchar](100) NULL,
	[InsStar] [int] NULL,
	[Rate] [nvarchar](100) NULL,
	[SratingName] [nvarchar](100) NULL,
	[CurrentProfit] [decimal](10, 2) NULL,
	[FutureProfit] [decimal](10, 2) NULL,
	[CurrentIncomeRate] [decimal](10, 4) NULL,
	[FutureIncomeRate] [decimal](10, 4) NULL,
	[Title] [nvarchar](500) NULL,
	[ProfitYear] [int] NULL,
	[NewPrice] [decimal](10, 2) NULL,
	[DataReportUrl] [nvarchar](200) NULL,
 CONSTRAINT [PK__StockReport] PRIMARY KEY CLUSTERED 
(
	[StockReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StockType]    Script Date: 2016/11/7 17:23:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockType](
	[StockTypeId] [int] NOT NULL,
	[StockTypeName] [nvarchar](100) NOT NULL,
	[description] [nvarchar](500) NULL,
	[CreatedAt] [datetimeoffset](0) NULL,
PRIMARY KEY CLUSTERED 
(
	[StockTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT [dbo].[StockType] ([StockTypeId], [StockTypeName], [description], [CreatedAt]) VALUES (1, N'SH', N'上海交易市场', GETDATE())
GO
INSERT [dbo].[StockType] ([StockTypeId], [StockTypeName], [description], [CreatedAt]) VALUES (2, N'SZ', N'深圳交易市场',  GETDATE())
GO

CREATE TABLE [dbo].[StockRepordData](
	[StockReportId] [int] NOT NULL,
	[DataReport] [text] NULL,
	[CreatedAt] [datetimeoffset](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StockReportId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


USE [master]
GO
ALTER DATABASE [StockMarket] SET  READ_WRITE 
GO
