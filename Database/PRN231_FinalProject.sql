USE [master]
GO
/****** Object:  Database [PRN231_FinalProject]    Script Date: 7/2/2024 8:39:18 PM ******/
CREATE DATABASE [PRN231_FinalProject]
GO
USE [PRN231_FinalProject]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 7/2/2024 8:39:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[ContactId] [int] IDENTITY(1,1) NOT NULL,
	[Firstname] [nvarchar](50) NULL,
	[Lastname] [nvarchar](50) NULL,
	[DateOfBirth] [date] NULL,
	[Notes] [nvarchar](500) NULL,
	[History] [date] NULL,
	[Favorite] [bit] NULL,
	[ProfileImage] [nvarchar](1000) NULL,
	[UserId] [int] NOT NULL,
	[DeleteDate] [date] NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[ContactId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactEmail]    Script Date: 7/2/2024 8:39:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactEmail](
	[ContactEmailId] [int] IDENTITY(1,1) NOT NULL,
	[ContactId] [int] NOT NULL,
	[Email] [nchar](100) NOT NULL,
	[Label] [nvarchar](50) NULL,
 CONSTRAINT [PK_ContactEmail] PRIMARY KEY CLUSTERED 
(
	[ContactEmailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactPhone]    Script Date: 7/2/2024 8:39:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactPhone](
	[ContactPhoneId] [int] IDENTITY(1,1) NOT NULL,
	[Phone] [nchar](10) NOT NULL,
	[ContactId] [int] NOT NULL,
	[Code] [nchar](10) NOT NULL,
	[Label] [nvarchar](50) NULL,
 CONSTRAINT [PK_ContactPhone] PRIMARY KEY CLUSTERED 
(
	[ContactPhoneId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContactsLabel]    Script Date: 7/2/2024 8:39:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactsLabel](
	[ContactLabelId] [int] IDENTITY(1,1) NOT NULL,
	[LabelId] [int] NOT NULL,
	[ContactId] [int] NOT NULL,
 CONSTRAINT [PK_ContactsLabel] PRIMARY KEY CLUSTERED 
(
	[ContactLabelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_ContactsLabel] UNIQUE NONCLUSTERED 
(
	[ContactId] ASC,
	[LabelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Label]    Script Date: 7/2/2024 8:39:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Label](
	[LabelId] [int] IDENTITY(1,1) NOT NULL,
	[LabelName] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Label] PRIMARY KEY CLUSTERED 
(
	[LabelId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 7/2/2024 8:39:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nchar](100) NOT NULL,
	[PhoneNumber] [nchar](20) NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Password] [nchar](50) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Contact] CHECK CONSTRAINT [FK_Contact_User]
GO
ALTER TABLE [dbo].[ContactEmail]  WITH CHECK ADD  CONSTRAINT [FK_ContactEmail_Contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([ContactId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContactEmail] CHECK CONSTRAINT [FK_ContactEmail_Contact]
GO
ALTER TABLE [dbo].[ContactPhone]  WITH CHECK ADD  CONSTRAINT [FK_ContactPhone_Contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([ContactId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContactPhone] CHECK CONSTRAINT [FK_ContactPhone_Contact]
GO
ALTER TABLE [dbo].[ContactsLabel]  WITH CHECK ADD  CONSTRAINT [FK_ContactsLabel_Contact] FOREIGN KEY([ContactId])
REFERENCES [dbo].[Contact] ([ContactId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContactsLabel] CHECK CONSTRAINT [FK_ContactsLabel_Contact]
GO
ALTER TABLE [dbo].[ContactsLabel]  WITH CHECK ADD  CONSTRAINT [FK_ContactsLabel_Label] FOREIGN KEY([LabelId])
REFERENCES [dbo].[Label] ([LabelId])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ContactsLabel] CHECK CONSTRAINT [FK_ContactsLabel_Label]
GO
ALTER TABLE [dbo].[Label]  WITH CHECK ADD  CONSTRAINT [FK_Label_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Label] CHECK CONSTRAINT [FK_Label_User]
GO
USE [master]
GO
ALTER DATABASE [PRN231_FinalProject] SET  READ_WRITE 
GO
