USE [master]
GO
/****** Object:  Database [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF]    Script Date: 5/21/2021 2:11:07 AM ******/
CREATE DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PCP4', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\PCP4.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PCP4_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\PCP4_log.ldf' , SIZE = 1600KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET ARITHABORT OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET AUTO_SHRINK ON 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET  DISABLE_BROKER 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET  MULTI_USER 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET DB_CHAINING OFF 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET QUERY_STORE = OFF
GO
USE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 5/21/2021 2:11:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Account_id] [int] IDENTITY(1,1) NOT NULL,
	[Full_name] [nvarchar](max) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[password] [nvarchar](50) NOT NULL,
	[a_type] [nvarchar](10) NOT NULL,
	[isEmailVerified] [bit] NULL,
	[ActivationCode] [uniqueidentifier] NULL,
	[phone_number] [bigint] NOT NULL,
	[address] [nvarchar](max) NOT NULL,
	[map_long] [float] NULL,
	[map_lat] [float] NULL,
	[email_address] [nvarchar](max) NOT NULL,
	[UC_Area_id] [int] NULL,
	[Town_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Account_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Complaints]    Script Date: 5/21/2021 2:11:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Complaints](
	[complaint_Id] [int] IDENTITY(1,1) NOT NULL,
	[c_title] [nvarchar](50) NOT NULL,
	[c_category] [nvarchar](50) NULL,
	[account_id] [int] NOT NULL,
	[description] [nvarchar](max) NULL,
	[UC_area_id] [int] NULL,
	[map_long] [float] NULL,
	[map_lat] [float] NULL,
	[Severity_lvl] [int] NULL,
	[date_creation] [date] NOT NULL,
	[date_approval] [date] NULL,
	[date_approval_for_budget] [date] NULL,
	[date_progress_1] [date] NULL,
	[date_finished] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[complaint_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Town]    Script Date: 5/21/2021 2:11:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Town](
	[town_id] [int] NOT NULL,
	[town_name] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[town_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UCAreas]    Script Date: 5/21/2021 2:11:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UCAreas](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UCName] [nvarchar](max) NULL,
	[TownID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Feedback]    Script Date: 5/21/2021 2:11:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedback](
	[Feedback_id] [int] IDENTITY(1,1) NOT NULL,
	[complaint_id] [int] NULL,
	[FeedbackMessage] [nvarchar](max) NULL,
	[Rating] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Feedback_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SupportingComplaints]    Script Date: 5/21/2021 2:11:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupportingComplaints](
	[S_id] [int] IDENTITY(1,1) NOT NULL,
	[Complaint_id] [int] NOT NULL,
	[account_id] [int] NOT NULL,
	[Title] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[S_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([Account_id], [Full_name], [username], [password], [a_type], [isEmailVerified], [ActivationCode], [phone_number], [address], [map_long], [map_lat], [email_address], [UC_Area_id], [Town_id]) VALUES (1, N'hammad ahmed', N'hammad', N'abc', N'town', NULL, NULL, 333222233, N'zamzama', NULL, NULL, N'hammad@gmail.com', NULL, 16)
INSERT [dbo].[Account] ([Account_id], [Full_name], [username], [password], [a_type], [isEmailVerified], [ActivationCode], [phone_number], [address], [map_long], [map_lat], [email_address], [UC_Area_id], [Town_id]) VALUES (2, N'abeer khatri', N'abeer', N'abc', N'uc', NULL, NULL, 33223322, N'4 st zamzam', NULL, NULL, N'abeer@gmail.com', 88, NULL)
INSERT [dbo].[Account] ([Account_id], [Full_name], [username], [password], [a_type], [isEmailVerified], [ActivationCode], [phone_number], [address], [map_long], [map_lat], [email_address], [UC_Area_id], [Town_id]) VALUES (3, N'safeer sajjad', N'safeer', N'abc', N'public', NULL, NULL, 9233222332, N'46 st bathisland', NULL, NULL, N'safeer@gmail.com', NULL, NULL)
INSERT [dbo].[Account] ([Account_id], [Full_name], [username], [password], [a_type], [isEmailVerified], [ActivationCode], [phone_number], [address], [map_long], [map_lat], [email_address], [UC_Area_id], [Town_id]) VALUES (4, N'hammad khatri', N'hammad123', N'fRpUEnsiJQL1t5tfsIAwYRUqRPkrN+I8ZSe69mXU2po=', N'public', 1, N'46257a8f-0ca3-460d-ad8e-746ae3cee7ee', 323322332, N'st 7th zamzama dha phase 5', NULL, NULL, N'hammadkhatri2011@hotmail.com', NULL, NULL)
INSERT [dbo].[Account] ([Account_id], [Full_name], [username], [password], [a_type], [isEmailVerified], [ActivationCode], [phone_number], [address], [map_long], [map_lat], [email_address], [UC_Area_id], [Town_id]) VALUES (5, N'hammad ahmed', N'hammad12313', N'lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4=', N'public', 0, N'fd62c6b5-c13e-4295-a082-44ab1b4feeb8', 123123123, N'something something', NULL, NULL, N'hammad123@gmail.com', NULL, NULL)
INSERT [dbo].[Account] ([Account_id], [Full_name], [username], [password], [a_type], [isEmailVerified], [ActivationCode], [phone_number], [address], [map_long], [map_lat], [email_address], [UC_Area_id], [Town_id]) VALUES (6, N'haseeb ahmed', N'haseeb123', N'lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4=', N'public', 0, N'897f8f4e-fc0f-4b24-823e-760659864738', 123123123, N'something something', NULL, NULL, N'haseeb@gmail.com', NULL, NULL)
INSERT [dbo].[Account] ([Account_id], [Full_name], [username], [password], [a_type], [isEmailVerified], [ActivationCode], [phone_number], [address], [map_long], [map_lat], [email_address], [UC_Area_id], [Town_id]) VALUES (7, N'haseeb ahmed', N'haseeb12345', N'lsrjXOipsCRBeL8o5JZsLOG4OFcjqWprg4hYzdbKCh4=', N'public', 0, N'd5bad855-915a-4d3d-ad86-0c3c3d473305', 123123123, N'something something', NULL, NULL, N'haseeb123@gmail.com', NULL, NULL)
INSERT [dbo].[Account] ([Account_id], [Full_name], [username], [password], [a_type], [isEmailVerified], [ActivationCode], [phone_number], [address], [map_long], [map_lat], [email_address], [UC_Area_id], [Town_id]) VALUES (8, N'safeer', N'safeer123', N'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'public', 0, N'defb49f6-162c-4e55-afd5-c12e2795e44b', 123123123, N'st 5 something something', NULL, NULL, N'safeer@safeer.com', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Complaints] ON 

INSERT [dbo].[Complaints] ([complaint_Id], [c_title], [c_category], [account_id], [description], [UC_area_id], [map_long], [map_lat], [Severity_lvl], [date_creation], [date_approval], [date_approval_for_budget], [date_progress_1], [date_finished]) VALUES (1, N'road destroyed', N'road', 1, N'road is damaged and has been 6 months nobody is tacking any action ', 111, NULL, NULL, 3, CAST(N'2018-10-21' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Complaints] ([complaint_Id], [c_title], [c_category], [account_id], [description], [UC_area_id], [map_long], [map_lat], [Severity_lvl], [date_creation], [date_approval], [date_approval_for_budget], [date_progress_1], [date_finished]) VALUES (2, N'trash chaos', N'sanitary', 2, N'garbage is everywhere and authorities are not taking any action the garbage is piling up like a mountain', 56, NULL, NULL, 4, CAST(N'2018-10-25' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Complaints] ([complaint_Id], [c_title], [c_category], [account_id], [description], [UC_area_id], [map_long], [map_lat], [Severity_lvl], [date_creation], [date_approval], [date_approval_for_budget], [date_progress_1], [date_finished]) VALUES (3, N'sewage water everywhere', N'sewage', 2, N'gutters are block and the problem keeps on rising', 20, NULL, NULL, 4, CAST(N'2018-10-26' AS Date), NULL, NULL, NULL, NULL)
INSERT [dbo].[Complaints] ([complaint_Id], [c_title], [c_category], [account_id], [description], [UC_area_id], [map_long], [map_lat], [Severity_lvl], [date_creation], [date_approval], [date_approval_for_budget], [date_progress_1], [date_finished]) VALUES (4, N'road damaged', N'road', 2, N'road is damaged in most parts of aur neighbourhood, this problem is causing sever disturbance', 162, NULL, NULL, 3, CAST(N'2018-10-27' AS Date), NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Complaints] OFF
GO
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (1, N'Baldia')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (2, N'Bin Qasim')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (3, N'Gadap')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (4, N'Gulberg')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (5, N'Gulshan')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (6, N'Jamshed')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (7, N'Kemari')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (8, N'Korangi')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (9, N'Landhi')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (10, N'Liaquatabad')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (11, N'Lyari')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (12, N'Malir')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (13, N'New Karachi')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (14, N'North Nazimabad')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (15, N'Orangi')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (16, N'Saddar')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (17, N'Shah Faisal')
INSERT [dbo].[Town] ([town_id], [town_name]) VALUES (18, N'S.I.T.E')
GO
SET IDENTITY_INSERT [dbo].[UCAreas] ON 

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
GO
INSERT [dbo].[UCAreas] ([ID], [UCName], [TownID]) VALUES (100, N' Nawabad', 11)
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
SET IDENTITY_INSERT [dbo].[UCAreas] OFF
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Account_Town] FOREIGN KEY([Town_id])
REFERENCES [dbo].[Town] ([town_id])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Account_Town]
GO
ALTER TABLE [dbo].[Account]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_ToUC] FOREIGN KEY([UC_Area_id])
REFERENCES [dbo].[UCAreas] ([ID])
GO
ALTER TABLE [dbo].[Account] CHECK CONSTRAINT [FK_Accounts_ToUC]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_ToAccount] FOREIGN KEY([account_id])
REFERENCES [dbo].[Account] ([Account_id])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_ToAccount]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_ToUC] FOREIGN KEY([UC_area_id])
REFERENCES [dbo].[UCAreas] ([ID])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_ToUC]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_ToComplaint] FOREIGN KEY([complaint_id])
REFERENCES [dbo].[Complaints] ([complaint_Id])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [FK_Feedback_ToComplaint]
GO
ALTER TABLE [dbo].[SupportingComplaints]  WITH CHECK ADD  CONSTRAINT [FK_SupportingComplaints_ToComplaints] FOREIGN KEY([Complaint_id])
REFERENCES [dbo].[Complaints] ([complaint_Id])
GO
ALTER TABLE [dbo].[SupportingComplaints] CHECK CONSTRAINT [FK_SupportingComplaints_ToComplaints]
GO
ALTER TABLE [dbo].[SupportingComplaints]  WITH CHECK ADD  CONSTRAINT [FK_SupportingComplaints_ToTable] FOREIGN KEY([account_id])
REFERENCES [dbo].[Account] ([Account_id])
GO
ALTER TABLE [dbo].[SupportingComplaints] CHECK CONSTRAINT [FK_SupportingComplaints_ToTable]
GO
USE [master]
GO
ALTER DATABASE [E:\FYP-I\FYP-PCP\FYP-PCP\APP_DATA\PCP4.MDF] SET  READ_WRITE 
GO
