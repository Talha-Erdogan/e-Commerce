USE [e-Commerce]
GO
/****** Object:  Table [dbo].[Auth]    Script Date: 22.03.2021 11:53:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Auth](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[NameTR] [nvarchar](150) NULL,
	[NameEN] [nvarchar](150) NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDateTime] [datetime] NULL,
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK_Auth] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 22.03.2021 11:53:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameTR] [nvarchar](100) NOT NULL,
	[NameEN] [nvarchar](100) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedBy] [int] NULL,
	[DeletedDateTime] [datetime] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 22.03.2021 11:53:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TRNationalId] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[MobilePhone] [nvarchar](50) NULL,
	[Email] [nvarchar](250) NULL,
	[SexId] [int] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 22.03.2021 11:53:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[NameTR] [nvarchar](100) NOT NULL,
	[NameEN] [nvarchar](100) NOT NULL,
	[DescriptionTR] [nvarchar](500) NULL,
	[DescriptionEN] [nvarchar](500) NULL,
	[ImageFilePath] [nvarchar](500) NULL,
	[CategoryId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedBy] [int] NULL,
	[DeletedDateTime] [datetime] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 22.03.2021 11:53:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](150) NOT NULL,
	[NameTR] [nvarchar](150) NULL,
	[NameEN] [nvarchar](150) NULL,
	[IsDeleted] [bit] NOT NULL,
	[DeletedDateTime] [datetime] NULL,
	[DeletedBy] [int] NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfileDetails]    Script Date: 22.03.2021 11:53:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProfileId] [int] NOT NULL,
	[AuthId] [int] NOT NULL,
 CONSTRAINT [PK_ProfileDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfileEmployees]    Script Date: 22.03.2021 11:53:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileEmployees](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ProfileId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
 CONSTRAINT [PK_ProfileEmployees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sex]    Script Date: 22.03.2021 11:53:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sex](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NameTR] [nvarchar](50) NOT NULL,
	[NameEN] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Sex] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Auth] ON 

INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (1, N'PAGE_AUTH_LIST', N'Yetki Listeleme', N'Auth List', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (2, N'PAGE_AUTH_ADD', N'Yetki Ekleme', N'Auth Add', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (3, N'PAGE_AUTH_EDIT', N'Yetki Güncelleme', N'Auth Edit', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (4, N'PAGE_AUTH_DISPLAY', N'Yetki Görüntüleme', N'Auth Display', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (5, N'PAGE_AUTH_DELETE', N'Yetki Silme', N'Auth Delete', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (6, N'PAGE_CATEGORY_LIST', N'Kategori Listeleme', N'Category List', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (7, N'PAGE_CATEGORY_ADD', N'Kategori Ekleme', N'Category Add', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (8, N'PAGE_CATEGORY_EDIT', N'Kategori Güncelleme', N'Category Edit', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (9, N'PAGE_CATEGORY_DISPLAY', N'Kategori Görüntüleme', N'Category Display', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (10, N'PAGE_CATEGORY_DELETE', N'Kategori Silme', N'Category Delete', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (11, N'PAGE_EMPLOYEE_LIST', N'Kullanıcı Listesi', N'Employee List', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (12, N'PAGE_EMPLOYEE_ADD', N'Kullanıcı Ekleme', N'Employee Add', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (13, N'PAGE_EMPLOYEE_EDIT', N'Kullanıcı Güncelleme', N'Employee Edit', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (14, N'PAGE_EMPLOYEE_DISPLAY', N'Kullanıcı Görüntüleme', N'Employee Display', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (15, N'PAGE_PROFILE_LIST', N'Profil Listesi', N'Profile List', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (16, N'PAGE_PROFILE_ADD', N'Profil Ekleme', N'Profile Add', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (17, N'PAGE_PROFILE_EDIT', N'Profil Güncelleme', N'Profile Edit', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (18, N'PAGE_PROFILE_DISPLAY', N'Profil Görüntüleme', N'Profile Display', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (19, N'PAGE_PROFILE_DELETE', N'Profil Silme', N'Profile Delete', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (20, N'PAGE_PROFILEDETAIL_BATCHEDIT', N'Profil Detay Toplu Güncelleme', N'Profile Detail Batch Edit', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (21, N'PAGE_PROFILEEMPLOYEE_BATCHEDIT', N'Profil Kullanıcıları Toplu Güncelleme Menüde Göster', N'Profile Employee Batch Edit', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (22, N'PAGE_PRODUCT_LIST', N'Ürün Listeleme', N'Product List', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (23, N'PAGE_PRODUCT_ADD', N'Ürün Ekleme', N'Product Add', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (24, N'PAGE_PRODUCT_EDIT', N'Ürün Güncelleme', N'Product Edit', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (25, N'PAGE_PRODUCT_DISPLAY', N'Ürün Görüntüleme', N'Product Display', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (26, N'PAGE_PRODUCT_DELETE', N'Ürün Silme', N'Product Delete', 0, NULL, NULL)
INSERT [dbo].[Auth] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (27, N'PRODUCT_LISTFORCUSTOMER', N'Ürün Listeleme - Müşteri İçin', N'Product List For Customer', 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Auth] OFF
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (2, N'Elektronik', N'Electronic', 1, 1, CAST(N'2021-03-21T20:55:35.057' AS DateTime))
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (3, N'Moda', N'Fashion ', 1, 1, CAST(N'2021-03-21T20:56:20.813' AS DateTime))
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (4, N'Telefon', N'Telephone', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (5, N'Bilgisayar', N'Computer', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (6, N'Televizyon', N'Television', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (7, N'Kamera', N'Camera', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (8, N'Müzik', N'Music', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (9, N'Klima', N'Air Conditioning', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (10, N'Ev Aletleri', N'Home Appliances', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (11, N'Aksesuar', N'Accessory', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (12, N'Sağlık', N'Health', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (13, N'Spor', N'Sport', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (14, N'Hafıza Kartı', N'Memory Card', 0, NULL, NULL)
INSERT [dbo].[Category] ([Id], [NameTR], [NameEN], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (15, N'DENEME', N'DENEME EN', 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Employees] ON 

INSERT [dbo].[Employees] ([Id], [TRNationalId], [Name], [LastName], [MobilePhone], [Email], [SexId], [UserName], [Password]) VALUES (1, N'12345678912', N'Admin', N'Profile', N'5556669988', N'admin@gmail.com', 1, N'admin', N'1')
INSERT [dbo].[Employees] ([Id], [TRNationalId], [Name], [LastName], [MobilePhone], [Email], [SexId], [UserName], [Password]) VALUES (2, N'99988844456', N'User', N'Profile', N'5551112233', N'ttt@gmail.com', 1, N'user', N'1')
INSERT [dbo].[Employees] ([Id], [TRNationalId], [Name], [LastName], [MobilePhone], [Email], [SexId], [UserName], [Password]) VALUES (3, N'99988844452', N'Customer', N'Profile', N'5551112233', N'ttt@gmail.com', 2, N'customer', N'1')
SET IDENTITY_INSERT [dbo].[Employees] OFF
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([Id], [NameTR], [NameEN], [DescriptionTR], [DescriptionEN], [ImageFilePath], [CategoryId], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (1, N'Askı Aparatı TR', N'Askı Aparatı EN', N'Askı Aparatı TR desc', N'Askı Aparatı EN desc', N'ProductFiles/be836575-64d4-4e02-8093-cd92dd84b2d1Askı-Aparatı.jpg', 11, 0, NULL, NULL)
INSERT [dbo].[Product] ([Id], [NameTR], [NameEN], [DescriptionTR], [DescriptionEN], [ImageFilePath], [CategoryId], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (2, N'Leptop TR', N'Leptop  EN', N'Leptop  TR DESC', N'Leptop  EN DESC', N'ProductFiles/4ee079c6-8827-4cbc-9776-3974df6209a3bilgisayar.jpg', 5, 0, NULL, NULL)
INSERT [dbo].[Product] ([Id], [NameTR], [NameEN], [DescriptionTR], [DescriptionEN], [ImageFilePath], [CategoryId], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (3, N'Tv TR', N'Tv EN', N'Tv TR DESC', N'Tv EN DESC', N'ProductFiles/6714bb54-298b-453f-a76c-0f150c7b2d0aTV.jpg', 6, 0, NULL, NULL)
INSERT [dbo].[Product] ([Id], [NameTR], [NameEN], [DescriptionTR], [DescriptionEN], [ImageFilePath], [CategoryId], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (4, N'Klima TR', N'Klima EN', N'Klima TR DESC', N'Klima EN DESC', N'ProductFiles/f1aa847d-4771-4b2b-a475-61866036088bklima.jpg', 9, 0, NULL, NULL)
INSERT [dbo].[Product] ([Id], [NameTR], [NameEN], [DescriptionTR], [DescriptionEN], [ImageFilePath], [CategoryId], [IsDeleted], [DeletedBy], [DeletedDateTime]) VALUES (5, N'Telefon TR', N'TelefonDeneme EN', N'Telefon TR DESC', N'TelefonDeneme DESC - ', N'ProductFiles/968b2552-0f37-4771-bb2a-263137de3402TELEFON.jpg', 4, 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[Profile] ON 

INSERT [dbo].[Profile] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (1, N'SYSTEMADMIN', N'Sistem Admin Profili', N'System Admin Profile', 0, NULL, NULL)
INSERT [dbo].[Profile] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (2, N'USER', N'Kullanıcı Profili', N'User Profile', 0, NULL, NULL)
INSERT [dbo].[Profile] ([Id], [Code], [NameTR], [NameEN], [IsDeleted], [DeletedDateTime], [DeletedBy]) VALUES (3, N'CUSTOMER', N'Müşteri Profili', N'Customer Profile', 0, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Profile] OFF
GO
SET IDENTITY_INSERT [dbo].[ProfileDetails] ON 

INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (1, 1, 1)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (2, 1, 2)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (3, 1, 3)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (4, 1, 4)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (5, 1, 5)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (6, 2, 23)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (7, 2, 25)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (8, 2, 24)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (9, 2, 22)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (10, 2, 26)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (12, 1, 7)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (13, 1, 9)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (14, 1, 8)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (15, 1, 6)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (16, 1, 10)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (17, 1, 12)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (18, 1, 14)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (19, 1, 13)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (20, 1, 11)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (21, 1, 20)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (22, 1, 16)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (23, 1, 18)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (24, 1, 17)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (25, 1, 21)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (26, 1, 15)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (27, 1, 19)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (28, 1, 23)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (29, 1, 25)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (30, 1, 24)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (31, 1, 22)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (32, 1, 26)
INSERT [dbo].[ProfileDetails] ([Id], [ProfileId], [AuthId]) VALUES (33, 3, 27)
SET IDENTITY_INSERT [dbo].[ProfileDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[ProfileEmployees] ON 

INSERT [dbo].[ProfileEmployees] ([Id], [ProfileId], [EmployeeId]) VALUES (1, 1, 1)
INSERT [dbo].[ProfileEmployees] ([Id], [ProfileId], [EmployeeId]) VALUES (3, 3, 3)
INSERT [dbo].[ProfileEmployees] ([Id], [ProfileId], [EmployeeId]) VALUES (4, 2, 2)
SET IDENTITY_INSERT [dbo].[ProfileEmployees] OFF
GO
SET IDENTITY_INSERT [dbo].[Sex] ON 

INSERT [dbo].[Sex] ([Id], [NameTR], [NameEN]) VALUES (1, N'Erkek', N'Man')
INSERT [dbo].[Sex] ([Id], [NameTR], [NameEN]) VALUES (2, N'Kadın', N'Woman')
SET IDENTITY_INSERT [dbo].[Sex] OFF
GO
