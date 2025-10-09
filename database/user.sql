insert into [user] (email, password, enabled) values ('admin@shop.com', '@0Admin', 1)
insert into admin values ((select id from [user] where email = 'admin@shop.com'))

INSERT INTO [dbo].[user] ([email], [password], [enabled]) VALUES ('s1@shop.com', '@0Seller', 1);
INSERT INTO [dbo].[seller] VALUES ((SELECT id FROM [dbo].[user] WHERE email = 's1@shop.com'), N'Hoá Thanh Sư', '2004-05-15', '363636363636', N'Thủ Đô Linh Thiêng', '16279');

INSERT INTO [dbo].[user] ([email], [password], [enabled]) VALUES ('s2@shop.com', '@0Seller', 1);
INSERT INTO [dbo].[seller] VALUES ((SELECT id FROM [dbo].[user] WHERE email = 's2@shop.com'), N'Trà Từ Tay', '2003-11-20', '120120120', N'Ngõ 120', '08980');

INSERT INTO [dbo].[user] ([email], [password], [enabled]) VALUES ('s3@shop.com', '@0Seller', 1);
INSERT INTO [dbo].[seller] VALUES ((SELECT id FROM [dbo].[user] WHERE email = 's3@shop.com'), N'Khả Se Ni', '2006-01-01', '888888888', N'Bệnh Viện', '11443');