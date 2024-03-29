USE [mojeDatabase]
GO
SET IDENTITY_INSERT [dbo].[AppUsers] ON 
GO
INSERT [dbo].[AppUsers] ([Id], [Firstname], [Lastname], [Username], [PasswordHash], [PasswordSalt], [Email], [Created], [AppRole]) VALUES (1, N'adminF', N'adminL', N'admin', 0x88D84F549BAC68409A31876C3AAD3C316FED0FA66B1AD21CF1E233406C38A785, 0x507D7F1425FE44B7EAEB49314C632C460E0B6FD0C8570764CC935DEF8CF9DA0C, N'admin@admin.com', CAST(N'2023-04-21T12:59:43.3733092' AS DateTime2), 0)
GO
INSERT [dbo].[AppUsers] ([Id], [Firstname], [Lastname], [Username], [PasswordHash], [PasswordSalt], [Email], [Created], [AppRole]) VALUES (2, N'uzivatelF', N'uzivatelL', N'uzivatel', 0xB38F2006AB24CA874C84296786552C96DB649E9E04FA00D9636916674DCB631F, 0x4F0D22A01D682DF42465CB317C4D12D794660D66C9624775F3134A2E4B31CD32, N'uzivatel@email.com', CAST(N'2023-04-22T13:09:20.8462982' AS DateTime2), 1)
GO
SET IDENTITY_INSERT [dbo].[AppUsers] OFF
GO
SET IDENTITY_INSERT [dbo].[Projects] ON 
GO
INSERT [dbo].[Projects] ([Id], [Name]) VALUES (1, N'Projekt1')
GO
SET IDENTITY_INSERT [dbo].[Projects] OFF
GO
INSERT [dbo].[AppUserProjects] ([AppUserId], [ProjectId], [Role]) VALUES (1, 1, 0)
GO
INSERT [dbo].[AppUserProjects] ([AppUserId], [ProjectId], [Role]) VALUES (2, 1, 2)
GO
