insert into [user] (email, password, enabled) values ('admin@shop.com', '@0Admin', 1)
insert into admin values ((select id from [user] where email = 'admin@shop.com'))

INSERT INTO [dbo].[user] ([email], [password], [enabled]) VALUES ('seller1@shop.com', '@0Seller', 1);
INSERT INTO [dbo].[seller] VALUES ((SELECT id FROM [dbo].[user] WHERE email = 'seller1@shop.com'), N'Trần Thị Thảo', '2004-05-15', '123456789', N'Hoa Thanh Quế 1', '16279');

INSERT INTO [dbo].[user] ([email], [password], [enabled]) VALUES ('seller2@shop.com', '@0Seller', 1);
INSERT INTO [dbo].[seller] VALUES ((SELECT id FROM [dbo].[user] WHERE email = 'seller2@shop.com'), N'Nguyễn Văn An', '2003-11-20', '987654321', N'Hoa Thanh Quế 2', '16279');

INSERT INTO [dbo].[user] ([email], [password], [enabled]) VALUES ('seller3@shop.com', '@0Seller', 1);
INSERT INTO [dbo].[seller] VALUES ((SELECT id FROM [dbo].[user] WHERE email = 'seller3@shop.com'), N'Lê Minh Khang', '2006-01-01', '567891234', N'Hoa Thanh Quế 3', '16279');

INSERT INTO [dbo].[user] ([email], [password], [enabled]) VALUES ('seller4@shop.com', '@0Seller', 1);
INSERT INTO [dbo].[seller] VALUES ((SELECT id FROM [dbo].[user] WHERE email = 'seller4@shop.com'), N'Phạm Thị Yến', '2005-07-23', '345678912', N'Hoa Thanh Quế 4', '16279');

INSERT INTO [dbo].[user] ([email], [password], [enabled]) VALUES ('seller5@shop.com', '@0Seller', 1);
INSERT INTO [dbo].[seller] VALUES ((SELECT id FROM [dbo].[user] WHERE email = 'seller5@shop.com'), N'Đỗ Quốc Bảo', '2003-03-10', '789123456', N'Hoa Thanh Quế 5', '16279');

INSERT INTO [dbo].[user] ([email], [password], [enabled]) VALUES ('seller6@shop.com', '@0Seller', 1);
INSERT INTO [dbo].[seller] VALUES ((SELECT id FROM [dbo].[user] WHERE email = 'seller6@shop.com'), N'Hoàng Lan Hương', '2006-09-05', '219876543', N'Hoa Thanh Quế 6', '16279');

INSERT INTO [dbo].[user] ([email], [password], [enabled]) VALUES ('disabled@shop.com', '@0Seller', 0);
INSERT INTO [dbo].[seller] VALUES ((SELECT id FROM [dbo].[user] WHERE email = 'disabled@shop.com'), N'Đặng Cao Bồ', '2003-06-09', '363636363', N'Hoa Thanh Quế 7', '16279');