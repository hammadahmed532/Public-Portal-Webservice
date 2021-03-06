USE [master]
GO
/****** Object:  Database [PCP5]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE DATABASE [PCP5]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PCP5', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\PCP5.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PCP5_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\PCP5_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [PCP5] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PCP5].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PCP5] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PCP5] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PCP5] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PCP5] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PCP5] SET ARITHABORT OFF 
GO
ALTER DATABASE [PCP5] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PCP5] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PCP5] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PCP5] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PCP5] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PCP5] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PCP5] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PCP5] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PCP5] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PCP5] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PCP5] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PCP5] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PCP5] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PCP5] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PCP5] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PCP5] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PCP5] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PCP5] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PCP5] SET  MULTI_USER 
GO
ALTER DATABASE [PCP5] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PCP5] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PCP5] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PCP5] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PCP5] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PCP5] SET QUERY_STORE = OFF
GO
USE [PCP5]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Account_id] [int] IDENTITY(1,1) NOT NULL,
	[Full_name] [nvarchar](max) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[isEmailVerified] [bit] NULL,
	[ActivationCode] [uniqueidentifier] NULL,
	[phone_number] [bigint] NOT NULL,
	[address] [nvarchar](max) NOT NULL,
	[map_long] [float] NULL,
	[map_lat] [float] NULL,
	[email_address] [nvarchar](max) NOT NULL,
	[UC_Area_id] [int] NULL,
	[Town_id] [int] NULL,
	[date_created] [datetime] NULL,
	[profession] [nvarchar](max) NULL,
	[gender] [nvarchar](max) NULL,
	[department_id] [int] NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[category_id] [int] NOT NULL,
	[Category1] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Complaint_Det_Remarks]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Complaint_Det_Remarks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Complaint_id] [int] NOT NULL,
	[Remarks_Field_Off] [nvarchar](max) NULL,
	[Remarks_Member] [nvarchar](max) NULL,
	[Remarks_Coordinator] [nvarchar](max) NULL,
	[Remarks_Nazim] [nvarchar](max) NULL,
	[updated_Image1] [nvarchar](max) NULL,
	[updated_Image2] [nvarchar](max) NULL,
	[updated_Image3] [nvarchar](max) NULL,
 CONSTRAINT [PK_Complaint_Det_Remarks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Complaints]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Complaints](
	[complaint_Id] [int] IDENTITY(1,1) NOT NULL,
	[c_title] [nvarchar](50) NULL,
	[account_id] [int] NULL,
	[description] [nvarchar](max) NULL,
	[UC_area_id] [int] NULL,
	[map_long] [float] NULL,
	[map_lat] [float] NULL,
	[date_creation] [datetime] NULL,
	[date_stage_2] [datetime] NULL,
	[date_stage_3] [datetime] NULL,
	[date_stage_4] [datetime] NULL,
	[date_stage_6] [datetime] NULL,
	[image1] [nvarchar](max) NULL,
	[image2] [nvarchar](max) NULL,
	[image3] [nvarchar](max) NULL,
	[stage_id] [int] NOT NULL,
	[date_stage_5] [datetime] NULL,
	[category_id] [int] NULL,
	[view_public] [nvarchar](10) NULL,
	[admin_confirm] [bit] NULL,
	[date_last_modified] [datetime] NULL,
	[Forwarded_By_Account_id] [int] NULL,
	[complaint_Type] [nvarchar](max) NULL,
	[is_Notified] [bit] NULL,
	[Expected_amount] [float] NULL,
 CONSTRAINT [PK_Complaints] PRIMARY KEY CLUSTERED 
(
	[complaint_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedbacks]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedbacks](
	[Feedback_id] [int] IDENTITY(1,1) NOT NULL,
	[complaint_id] [int] NULL,
	[FeedbackMessage] [nvarchar](max) NULL,
	[Rating] [int] NULL,
	[time_created] [datetime] NULL,
 CONSTRAINT [PK_Feedbacks] PRIMARY KEY CLUSTERED 
(
	[Feedback_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PendingComplaints]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PendingComplaints](
	[p_id] [int] IDENTITY(1,1) NOT NULL,
	[complaint_id] [int] NOT NULL,
	[severity_Lvl] [int] NULL,
	[Year] [int] NULL,
 CONSTRAINT [PK_PendingComplaints] PRIMARY KEY CLUSTERED 
(
	[p_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Role_Id] [int] NOT NULL,
	[Role_Name] [nvarchar](50) NULL,
	[Role_Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Role_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[stage_id] [int] NOT NULL,
	[stage_description] [nvarchar](max) NULL,
	[Status_Percent] [int] NULL,
	[Status_first] [nvarchar](max) NULL,
	[Status_Budget] [nvarchar](max) NULL,
	[Status_Bogus] [nvarchar](max) NULL,
	[Status_Already_Done] [nvarchar](max) NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[stage_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupportingComplaints]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupportingComplaints](
	[S_id] [int] IDENTITY(1,1) NOT NULL,
	[Complaint_id] [int] NOT NULL,
	[account_id] [int] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[time_created] [datetime] NULL,
 CONSTRAINT [PK_SupportingComplaints] PRIMARY KEY CLUSTERED 
(
	[S_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Towns]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Towns](
	[town_id] [int] NOT NULL,
	[town_name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Towns] PRIMARY KEY CLUSTERED 
(
	[town_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UCAreas]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UCAreas](
	[ID] [int] NOT NULL,
	[UCName] [nvarchar](max) NULL,
	[TownID] [int] NULL,
 CONSTRAINT [PK_UCAreas] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Votings]    Script Date: 5/21/2021 2:19:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Votings](
	[vote_id] [int] IDENTITY(1,1) NOT NULL,
	[complaint_id] [int] NOT NULL,
	[account_id] [int] NOT NULL,
	[time_created] [datetime] NULL,
 CONSTRAINT [PK_Votings] PRIMARY KEY CLUSTERED 
(
	[vote_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (1, N'Baldia')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (2, N'Bin Qasim')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (3, N'Gadap')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (4, N'Gulberg')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (5, N'Gulshan')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (6, N'Jamshed')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (7, N'Kemari')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (8, N'Korangi')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (9, N'Landhi')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (10, N'Liaquatabad')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (11, N'Lyari')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (12, N'Malir')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (13, N'New Karachi')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (14, N'North Nazimabad')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (15, N'Orangi')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (16, N'Saddar')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (17, N'Shah Faisal')
INSERT [dbo].[Towns] ([town_id], [town_name]) VALUES (18, N'S.I.T.E')
GO
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (1, N' Gulshan-e-Ghazi', 1)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (2, N' Ittehad Town', 1)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (3, N' Islam Nagar', 1)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (4, N' Nai Abadi', 1)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (5, N' Saeedabad', 1)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (6, N' Muslim Mujahid Colony', 1)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (7, N' Muhajir Camp', 1)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (8, N' Rasheedabad', 1)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (9, N' Ibrahim Hyderi', 2)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (10, N' Rehri', 2)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (11, N' Cattle Colony', 2)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (12, N' Qaidabad', 2)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (13, N' Landhi Colony', 2)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (14, N' Gulshan-e-Hadeed', 2)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (15, N' Gaghar', 2)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (16, N'  Murad Memon Goth', 3)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (17, N' Darsano Chana', 3)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (18, N' Gadap', 3)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (19, N' Gujro', 3)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (20, N' Songal', 3)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (21, N' Maymarabad', 3)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (22, N' Yousuf Goth', 3)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (23, N' Manghopir', 3)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (24, N'  Azizabad', 4)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (25, N' Karimabad', 4)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (26, N' Aisha Manzil', 4)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (27, N' Ancholi', 4)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (28, N' Naseerabad', 4)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (29, N' Yaseenabad', 4)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (30, N' Water Pump', 4)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (31, N' Shafiq Mill Colony', 4)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (32, N' Delhi Mercantile Society', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (33, N' Civic Centre', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (34, N' Pir Ilahi Buksh Colony', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (35, N' Essa Nagri', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (36, N' Gulshan-e-Iqbal', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (37, N' Gillani Railway Station', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (38, N' Shanti Nagar', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (39, N' Jamali Colony', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (40, N' Gulshan-e-Iqbal II', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (41, N' Pehlwan Goth', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (42, N' Matrovil Colony', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (43, N' Gulzar-e-Hijri', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (44, N' Safooran Goth', 5)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (45, N' Akhtar Colony', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (46, N' Manzoor Colony', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (47, N' Azam Basti', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (48, N' Chanesar Goth', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (49, N' Mahmudabad', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (50, N' P.E.C.H.S I', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (51, N' P.E.C.H.S. II', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (52, N' Jut Line', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (53, N' Central Jacob Lines', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (54, N' Jamshed Quarters', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (55, N' Garden East', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (56, N' Soldier Bazar', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (57, N' Pakistan Quarters', 6)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (58, N' Sher Shah Village', 7)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (59, N' laibour square', 7)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (60, N' Kiamari', 7)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (61, N' Baba Bhit', 7)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (62, N' Machar Colony', 7)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (63, N' Maripur', 7)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (64, N' SherShah', 7)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (65, N' Gabo Pat', 7)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (66, N' Bilal Colony', 8)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (67, N' Nasir Colony', 8)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (68, N' Chakra Goth', 8)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (69, N' Mustafa Taj Colony', 8)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (70, N' Hundred Quarters', 8)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (71, N' Gulzar Colony', 8)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (72, N' Korangi Sector', 8)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (73, N' Zaman Town', 8)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (74, N' Hasrat Mohani Colony', 8)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (75, N' Muzafarabad', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (76, N' Muslimabad', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (77, N' Dawood Chowrangi', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (78, N' Moinabad', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (79, N' Sharafi Goth', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (80, N' Bhutto Nagar', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (81, N' Khawaja Ajmeer Colony', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (82, N' Landhi', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (83, N' Awami Colony', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (84, N' Burmee Colony', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (85, N' Korangi', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (86, N' Sherabad', 9)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (87, N' Rizvia Society (R.C.H.S.)', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (88, N' Firdous Colony', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (89, N' Super Market', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (90, N' Dak Khana', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (91, N' Qasimabad', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (92, N' Bandhani Colony', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (93, N' Sharifabad', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (94, N' Commercial Area', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (95, N' Mujahid Colony', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (96, N' Nazimabad', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (97, N' Abbasi Shaheed', 10)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (98, N' Agra Taj Colony', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (99, N' Daryaabad', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (100, N' Nawabad', 11)
GO
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (101, N' Khada Memon Society', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (102, N' Baghdadi', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (103, N' Baghdadi', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (104, N' Shah Baig Line', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (105, N' Bihar Colony', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (106, N' Ragiwara', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (107, N' Singo Line', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (108, N' Chakiwara', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (109, N' Allama Iqbal Colony', 11)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (110, N'  Model Colony', 12)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (111, N' Kala Board', 12)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (112, N' Saudabad', 12)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (113, N' Khokhra Par', 12)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (114, N' Jafar-e-Tayyar', 12)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (115, N' Gharibabad', 12)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (116, N' Ghazi Brohi Goth', 12)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (117, N'  North Karachi', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (118, N' Sir Syed Colony', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (119, N' Fatima Jinnah Colony', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (120, N' Godhra', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (121, N' Abu Zar Ghaffari', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (122, N' Hakim Ahsan', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (123, N' Madina Colony', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (124, N' Faisal Colony', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (125, N' Khamiso Goth', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (126, N' Mustufa Colony', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (127, N' Khawaja Ajmeer Nagri', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (128, N' Gulshan-e-Saeed', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (129, N' Shah Nawaz Bhutto Colony', 13)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (130, N' Paposh Nagar', 14)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (131, N' Pahar Ganj', 14)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (132, N' Khandu Goth', 14)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (133, N' Hyderi', 14)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (134, N' Sakhi Hassan', 14)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (135, N' Farooq-e-Azam', 14)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (136, N' Nusrat Bhutto Colony', 14)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (137, N' Shadman Town', 14)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (138, N' Buffer Zone', 14)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (139, N' Buffer Zone II', 14)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (140, N' Mominabad', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (141, N' Haryana Colony', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (142, N' Hanifabad', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (143, N' Mohammad Nagar', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (144, N' Madina Colony', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (145, N' Ghaziabad', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (146, N' Chisti Nagar', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (147, N' Bilal Colony/sector  &', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (148, N' Iqbal Baloch Colony', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (149, N' Gabol Colony', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (150, N' Data Nagar', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (151, N' Mujahidabad', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (152, N' Baloch Goth', 15)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (153, N' Old Haji Camp', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (154, N' Garden', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (155, N' Kharadar', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (156, N' City Railway Colony', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (157, N' Nanak Wara', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (158, N' Gazdarabad', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (159, N' Millat Nagar/Islam Pura', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (160, N' Saddar', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (161, N' Civil Line', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (162, N' Clifton', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (163, N' Kehkashan', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (164, N' Dehli Colony', 16)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (165, N' Natha Khan Goth', 17)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (166, N' Pak Sadat Colony', 17)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (167, N' Shah Faisal Colony', 17)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (168, N' Raita Plot', 17)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (169, N' Moria Khan Goth', 17)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (170, N' Rafa-e-Aam Society', 17)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (171, N' Al-Falah Society', 17)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (172, N' Pak Colony', 18)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (173, N' Old Golimar', 18)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (174, N' Jahanabad', 18)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (175, N' Metrovil', 18)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (176, N' Bhawani Chali', 18)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (177, N' Frontier Colony', 18)
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (178, N' Banaras Colony', 18)
GO
/****** Object:  Index [IX_FK_Account_ToCategory]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Account_ToCategory] ON [dbo].[Accounts]
(
	[department_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Account_ToRoles]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Account_ToRoles] ON [dbo].[Accounts]
(
	[Role] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Account_Town]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Account_Town] ON [dbo].[Accounts]
(
	[Town_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Accounts_ToUC]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Accounts_ToUC] ON [dbo].[Accounts]
(
	[UC_Area_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Complaint_Det_Remarks_ToTable]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Complaint_Det_Remarks_ToTable] ON [dbo].[Complaint_Det_Remarks]
(
	[Complaint_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Complaints_Category]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Complaints_Category] ON [dbo].[Complaints]
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Complaints_ToAccount]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Complaints_ToAccount] ON [dbo].[Complaints]
(
	[account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Complaints_ToForward_Admin_id]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Complaints_ToForward_Admin_id] ON [dbo].[Complaints]
(
	[Forwarded_By_Account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Complaints_ToStatus]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Complaints_ToStatus] ON [dbo].[Complaints]
(
	[stage_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Complaints_ToUC]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Complaints_ToUC] ON [dbo].[Complaints]
(
	[UC_area_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Feedback_ToComplaint]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Feedback_ToComplaint] ON [dbo].[Feedbacks]
(
	[complaint_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_PendingComplaints_ToTable]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_PendingComplaints_ToTable] ON [dbo].[PendingComplaints]
(
	[complaint_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_SupportingComplaints_ToComplaints]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_SupportingComplaints_ToComplaints] ON [dbo].[SupportingComplaints]
(
	[Complaint_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_SupportingComplaints_ToTable]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_SupportingComplaints_ToTable] ON [dbo].[SupportingComplaints]
(
	[account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Voting_ToAccount]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Voting_ToAccount] ON [dbo].[Votings]
(
	[account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Voting_ToComplaints]    Script Date: 5/21/2021 2:19:57 AM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Voting_ToComplaints] ON [dbo].[Votings]
(
	[complaint_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Account_ToCategory] FOREIGN KEY([department_id])
REFERENCES [dbo].[Categories] ([category_id])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Account_ToCategory]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Account_ToRoles] FOREIGN KEY([Role])
REFERENCES [dbo].[Roles] ([Role_Id])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Account_ToRoles]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Account_Town] FOREIGN KEY([Town_id])
REFERENCES [dbo].[Towns] ([town_id])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Account_Town]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_ToUC] FOREIGN KEY([UC_Area_id])
REFERENCES [dbo].[UCAreas] ([ID])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_ToUC]
GO
ALTER TABLE [dbo].[Complaint_Det_Remarks]  WITH CHECK ADD  CONSTRAINT [FK_Complaint_Det_Remarks_ToTable] FOREIGN KEY([Complaint_id])
REFERENCES [dbo].[Complaints] ([complaint_Id])
GO
ALTER TABLE [dbo].[Complaint_Det_Remarks] CHECK CONSTRAINT [FK_Complaint_Det_Remarks_ToTable]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_Category] FOREIGN KEY([category_id])
REFERENCES [dbo].[Categories] ([category_id])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_Category]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_ToAccount] FOREIGN KEY([account_id])
REFERENCES [dbo].[Accounts] ([Account_id])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_ToAccount]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_ToForward_Admin_id] FOREIGN KEY([Forwarded_By_Account_id])
REFERENCES [dbo].[Accounts] ([Account_id])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_ToForward_Admin_id]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_ToStatus] FOREIGN KEY([stage_id])
REFERENCES [dbo].[Status] ([stage_id])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_ToStatus]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_ToUC] FOREIGN KEY([UC_area_id])
REFERENCES [dbo].[UCAreas] ([ID])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_ToUC]
GO
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_ToComplaint] FOREIGN KEY([complaint_id])
REFERENCES [dbo].[Complaints] ([complaint_Id])
GO
ALTER TABLE [dbo].[Feedbacks] CHECK CONSTRAINT [FK_Feedback_ToComplaint]
GO
ALTER TABLE [dbo].[PendingComplaints]  WITH CHECK ADD  CONSTRAINT [FK_PendingComplaints_ToTable] FOREIGN KEY([complaint_id])
REFERENCES [dbo].[Complaints] ([complaint_Id])
GO
ALTER TABLE [dbo].[PendingComplaints] CHECK CONSTRAINT [FK_PendingComplaints_ToTable]
GO
ALTER TABLE [dbo].[SupportingComplaints]  WITH CHECK ADD  CONSTRAINT [FK_SupportingComplaints_ToComplaints] FOREIGN KEY([Complaint_id])
REFERENCES [dbo].[Complaints] ([complaint_Id])
GO
ALTER TABLE [dbo].[SupportingComplaints] CHECK CONSTRAINT [FK_SupportingComplaints_ToComplaints]
GO
ALTER TABLE [dbo].[SupportingComplaints]  WITH CHECK ADD  CONSTRAINT [FK_SupportingComplaints_ToTable] FOREIGN KEY([account_id])
REFERENCES [dbo].[Accounts] ([Account_id])
GO
ALTER TABLE [dbo].[SupportingComplaints] CHECK CONSTRAINT [FK_SupportingComplaints_ToTable]
GO
ALTER TABLE [dbo].[Votings]  WITH CHECK ADD  CONSTRAINT [FK_Voting_ToAccount] FOREIGN KEY([account_id])
REFERENCES [dbo].[Accounts] ([Account_id])
GO
ALTER TABLE [dbo].[Votings] CHECK CONSTRAINT [FK_Voting_ToAccount]
GO
ALTER TABLE [dbo].[Votings]  WITH CHECK ADD  CONSTRAINT [FK_Voting_ToComplaints] FOREIGN KEY([complaint_id])
REFERENCES [dbo].[Complaints] ([complaint_Id])
GO
ALTER TABLE [dbo].[Votings] CHECK CONSTRAINT [FK_Voting_ToComplaints]
GO
USE [master]
GO
ALTER DATABASE [PCP5] SET  READ_WRITE 
GO
