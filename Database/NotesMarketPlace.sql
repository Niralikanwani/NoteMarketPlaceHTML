USE [master]
GO
/****** Object:  Database [NotesMarketPlace]    Script Date: 11-02-2021 18:29:53 ******/
CREATE DATABASE [NotesMarketPlace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NotesMarketPlace', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\NotesMarketPlace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NotesMarketPlace_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\NotesMarketPlace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [NotesMarketPlace] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NotesMarketPlace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ARITHABORT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NotesMarketPlace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NotesMarketPlace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NotesMarketPlace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NotesMarketPlace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET RECOVERY FULL 
GO
ALTER DATABASE [NotesMarketPlace] SET  MULTI_USER 
GO
ALTER DATABASE [NotesMarketPlace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NotesMarketPlace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NotesMarketPlace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NotesMarketPlace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NotesMarketPlace] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [NotesMarketPlace] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'NotesMarketPlace', N'ON'
GO
ALTER DATABASE [NotesMarketPlace] SET QUERY_STORE = OFF
GO
USE [NotesMarketPlace]
GO
/****** Object:  Table [dbo].[tbl_ContactUs]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ContactUs](
	[Id] [int] NOT NULL,
	[FullName] [nvarchar](50) NOT NULL,
	[Email_Id] [nvarchar](50) NOT NULL,
	[Subject] [nvarchar](100) NOT NULL,
	[Question] [nvarchar](max) NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_ContactUs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Country]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Country](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CountryCode] [nvarchar](100) NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_Country] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Download]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Download](
	[Id] [int] NOT NULL,
	[Note_Id] [int] NOT NULL,
	[Seller_Id] [int] NOT NULL,
	[Downloader_Id] [int] NOT NULL,
	[IsSellerHasAllowedDownload] [bit] NOT NULL,
	[AttachmentPath] [nvarchar](max) NULL,
	[IsAttachmentDownloaded] [bit] NOT NULL,
	[AttachmentDownloadedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [nvarchar](100) NOT NULL,
	[NoteCategory] [nvarchar](100) NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
 CONSTRAINT [PK_tbl_Download] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_NoteAttachments]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_NoteAttachments](
	[Id] [int] NOT NULL,
	[Note_Id] [int] NOT NULL,
	[FileName] [nvarchar](100) NOT NULL,
	[FilePath] [nvarchar](max) NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_NoteAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_NoteCategory]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_NoteCategory](
	[Id] [int] NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_NoteCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_NoteReviews]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_NoteReviews](
	[Id] [int] NOT NULL,
	[Note_Id] [int] NOT NULL,
	[ReviewedBy_Id] [int] NOT NULL,
	[IsDownload_Id] [int] NOT NULL,
	[Ratings] [decimal](18, 0) NOT NULL,
	[Reviews] [nvarchar](max) NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_NoteReviews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Notes]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Notes](
	[Id] [int] NOT NULL,
	[Seller_Id] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[StatusActionedBy] [int] NULL,
	[AdminRemarks] [nvarchar](max) NULL,
	[PublishedDate] [datetime] NULL,
	[NoteTitle] [nvarchar](100) NOT NULL,
	[NoteCategory_Id] [int] NOT NULL,
	[DisplayPicture] [nvarchar](500) NULL,
	[NoteType_Id] [int] NULL,
	[NumberOfPages] [int] NULL,
	[Description] [nvarchar](max) NOT NULL,
	[University] [nvarchar](200) NULL,
	[Country_Id] [int] NULL,
	[Course] [nvarchar](100) NULL,
	[CourseCode] [nvarchar](100) NULL,
	[Professor] [nvarchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice] [decimal](18, 0) NULL,
	[NotesPreview] [nvarchar](max) NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_Notes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_NoteType]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_NoteType](
	[Id] [int] NOT NULL,
	[TypeName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](100) NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_NoteType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ReferenceData]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ReferenceData](
	[Id] [int] NOT NULL,
	[Value] [nvarchar](100) NOT NULL,
	[DataValue] [nvarchar](100) NOT NULL,
	[RefCategory] [nvarchar](100) NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_ReferenceData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_ReportedNotes]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_ReportedNotes](
	[Id] [int] NOT NULL,
	[Note_Id] [int] NOT NULL,
	[ReportedBy_Id] [int] NOT NULL,
	[IsDownload_Id] [int] NOT NULL,
	[Remarks] [nvarchar](max) NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
 CONSTRAINT [PK_tbl_ReportedNotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SystemConfiguration]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SystemConfiguration](
	[Id] [int] NOT NULL,
	[Key] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_SystemConfiguration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_UserProfile]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_UserProfile](
	[Id] [int] NOT NULL,
	[User_Id] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NULL,
	[SecondaryEmailAddress] [nvarchar](100) NULL,
	[PhoneCountryCode] [nvarchar](5) NOT NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL,
	[ProfilePicture] [nvarchar](500) NULL,
	[AddressLine1] [nvarchar](100) NOT NULL,
	[AddressLine2] [nvarchar](100) NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
	[ZipCode] [nvarchar](50) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[University] [nvarchar](100) NULL,
	[College] [nvarchar](100) NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
 CONSTRAINT [PK_tbl_UserProfile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_UserRoles]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_UserRoles](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_UserRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Users]    Script Date: 11-02-2021 18:29:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Users](
	[Id] [int] NOT NULL,
	[Role_Id] [int] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email_Id] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](24) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[DateAdded] [datetime] NULL,
	[AddedBy] [int] NULL,
	[DateEdited] [datetime] NULL,
	[EditedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_tbl_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (1, N'Male', N'M', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (2, N'Female', N'Fe', N'Gender', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (3, N'Unknown', N'U', N'Gender', NULL, NULL, NULL, NULL, 0)
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (4, N'Paid', N'P', N'Selling Mode', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (5, N'Free', N'F', N'Selling Mode', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (6, N'Draft', N'Draft', N'Note Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (7, N'Submitted For Review', N'Submitted For Review', N'Note Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (8, N'In Review', N'In Review', N'Note Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (9, N'Published', N'Published', N'Note Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (10, N'Rejected', N'Rejected', N'Note Status', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_ReferenceData] ([Id], [Value], [DataValue], [RefCategory], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (11, N'Removed', N'Removed', N'Note Status', NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tbl_UserRoles] ([Id], [Name], [Description], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (1, N'Super Admin', NULL, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_UserRoles] ([Id], [Name], [Description], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (2, N'Admin', NULL, NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[tbl_UserRoles] ([Id], [Name], [Description], [DateAdded], [AddedBy], [DateEdited], [EditedBy], [IsActive]) VALUES (3, N'Member', NULL, NULL, NULL, NULL, NULL, 1)
GO
ALTER TABLE [dbo].[tbl_UserRoles] ADD  CONSTRAINT [DF_tbl_UserRoles_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbl_Download]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Download_tbl_Notes] FOREIGN KEY([Note_Id])
REFERENCES [dbo].[tbl_Notes] ([Id])
GO
ALTER TABLE [dbo].[tbl_Download] CHECK CONSTRAINT [FK_tbl_Download_tbl_Notes]
GO
ALTER TABLE [dbo].[tbl_Download]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Download_tbl_Users] FOREIGN KEY([Seller_Id])
REFERENCES [dbo].[tbl_Users] ([Id])
GO
ALTER TABLE [dbo].[tbl_Download] CHECK CONSTRAINT [FK_tbl_Download_tbl_Users]
GO
ALTER TABLE [dbo].[tbl_Download]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Download_tbl_Users1] FOREIGN KEY([Downloader_Id])
REFERENCES [dbo].[tbl_Users] ([Id])
GO
ALTER TABLE [dbo].[tbl_Download] CHECK CONSTRAINT [FK_tbl_Download_tbl_Users1]
GO
ALTER TABLE [dbo].[tbl_NoteAttachments]  WITH CHECK ADD  CONSTRAINT [FK_tbl_NoteAttachments_tbl_Notes] FOREIGN KEY([Note_Id])
REFERENCES [dbo].[tbl_Notes] ([Id])
GO
ALTER TABLE [dbo].[tbl_NoteAttachments] CHECK CONSTRAINT [FK_tbl_NoteAttachments_tbl_Notes]
GO
ALTER TABLE [dbo].[tbl_NoteReviews]  WITH CHECK ADD  CONSTRAINT [FK_tbl_NoteReviews_tbl_Download] FOREIGN KEY([IsDownload_Id])
REFERENCES [dbo].[tbl_Download] ([Id])
GO
ALTER TABLE [dbo].[tbl_NoteReviews] CHECK CONSTRAINT [FK_tbl_NoteReviews_tbl_Download]
GO
ALTER TABLE [dbo].[tbl_NoteReviews]  WITH CHECK ADD  CONSTRAINT [FK_tbl_NoteReviews_tbl_Notes] FOREIGN KEY([Note_Id])
REFERENCES [dbo].[tbl_Notes] ([Id])
GO
ALTER TABLE [dbo].[tbl_NoteReviews] CHECK CONSTRAINT [FK_tbl_NoteReviews_tbl_Notes]
GO
ALTER TABLE [dbo].[tbl_NoteReviews]  WITH CHECK ADD  CONSTRAINT [FK_tbl_NoteReviews_tbl_Users] FOREIGN KEY([ReviewedBy_Id])
REFERENCES [dbo].[tbl_Users] ([Id])
GO
ALTER TABLE [dbo].[tbl_NoteReviews] CHECK CONSTRAINT [FK_tbl_NoteReviews_tbl_Users]
GO
ALTER TABLE [dbo].[tbl_Notes]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Notes_tbl_Country] FOREIGN KEY([Country_Id])
REFERENCES [dbo].[tbl_Country] ([Id])
GO
ALTER TABLE [dbo].[tbl_Notes] CHECK CONSTRAINT [FK_tbl_Notes_tbl_Country]
GO
ALTER TABLE [dbo].[tbl_Notes]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Notes_tbl_NoteCategory] FOREIGN KEY([NoteCategory_Id])
REFERENCES [dbo].[tbl_NoteCategory] ([Id])
GO
ALTER TABLE [dbo].[tbl_Notes] CHECK CONSTRAINT [FK_tbl_Notes_tbl_NoteCategory]
GO
ALTER TABLE [dbo].[tbl_Notes]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Notes_tbl_NoteType] FOREIGN KEY([NoteType_Id])
REFERENCES [dbo].[tbl_NoteType] ([Id])
GO
ALTER TABLE [dbo].[tbl_Notes] CHECK CONSTRAINT [FK_tbl_Notes_tbl_NoteType]
GO
ALTER TABLE [dbo].[tbl_Notes]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Notes_tbl_ReferenceData] FOREIGN KEY([Status])
REFERENCES [dbo].[tbl_ReferenceData] ([Id])
GO
ALTER TABLE [dbo].[tbl_Notes] CHECK CONSTRAINT [FK_tbl_Notes_tbl_ReferenceData]
GO
ALTER TABLE [dbo].[tbl_Notes]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Notes_tbl_Users] FOREIGN KEY([Seller_Id])
REFERENCES [dbo].[tbl_Users] ([Id])
GO
ALTER TABLE [dbo].[tbl_Notes] CHECK CONSTRAINT [FK_tbl_Notes_tbl_Users]
GO
ALTER TABLE [dbo].[tbl_Notes]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Notes_tbl_Users1] FOREIGN KEY([StatusActionedBy])
REFERENCES [dbo].[tbl_Users] ([Id])
GO
ALTER TABLE [dbo].[tbl_Notes] CHECK CONSTRAINT [FK_tbl_Notes_tbl_Users1]
GO
ALTER TABLE [dbo].[tbl_ReportedNotes]  WITH CHECK ADD  CONSTRAINT [FK_tbl_ReportedNotes_tbl_Download] FOREIGN KEY([IsDownload_Id])
REFERENCES [dbo].[tbl_Download] ([Id])
GO
ALTER TABLE [dbo].[tbl_ReportedNotes] CHECK CONSTRAINT [FK_tbl_ReportedNotes_tbl_Download]
GO
ALTER TABLE [dbo].[tbl_ReportedNotes]  WITH CHECK ADD  CONSTRAINT [FK_tbl_ReportedNotes_tbl_Notes] FOREIGN KEY([Note_Id])
REFERENCES [dbo].[tbl_Notes] ([Id])
GO
ALTER TABLE [dbo].[tbl_ReportedNotes] CHECK CONSTRAINT [FK_tbl_ReportedNotes_tbl_Notes]
GO
ALTER TABLE [dbo].[tbl_ReportedNotes]  WITH CHECK ADD  CONSTRAINT [FK_tbl_ReportedNotes_tbl_Users] FOREIGN KEY([ReportedBy_Id])
REFERENCES [dbo].[tbl_Users] ([Id])
GO
ALTER TABLE [dbo].[tbl_ReportedNotes] CHECK CONSTRAINT [FK_tbl_ReportedNotes_tbl_Users]
GO
ALTER TABLE [dbo].[tbl_UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_tbl_UserProfile_tbl_ReferenceData] FOREIGN KEY([Gender])
REFERENCES [dbo].[tbl_ReferenceData] ([Id])
GO
ALTER TABLE [dbo].[tbl_UserProfile] CHECK CONSTRAINT [FK_tbl_UserProfile_tbl_ReferenceData]
GO
ALTER TABLE [dbo].[tbl_UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_tbl_UserProfile_tbl_Users] FOREIGN KEY([User_Id])
REFERENCES [dbo].[tbl_Users] ([Id])
GO
ALTER TABLE [dbo].[tbl_UserProfile] CHECK CONSTRAINT [FK_tbl_UserProfile_tbl_Users]
GO
ALTER TABLE [dbo].[tbl_Users]  WITH CHECK ADD  CONSTRAINT [FK_tbl_Users_tbl_UserRoles] FOREIGN KEY([Role_Id])
REFERENCES [dbo].[tbl_UserRoles] ([Id])
GO
ALTER TABLE [dbo].[tbl_Users] CHECK CONSTRAINT [FK_tbl_Users_tbl_UserRoles]
GO
USE [master]
GO
ALTER DATABASE [NotesMarketPlace] SET  READ_WRITE 
GO
