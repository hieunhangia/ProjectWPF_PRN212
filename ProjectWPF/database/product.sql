SET IDENTITY_INSERT product_unit ON;
INSERT INTO product_unit (id, name) VALUES
(1, N'Kg'),
(2, N'Quả'),
(3, N'Hộp'),
(4, N'Giỏ');

SET IDENTITY_INSERT product_unit OFF;
SET IDENTITY_INSERT product ON;
-- Product table
INSERT INTO product (id, name, description, price, enabled, product_unit_id) VALUES
(1, N'Táo đỏ', N'Táo đỏ nhập khẩu, quả to đều, mọng nước, vị ngọt thanh, giàu vitamin C và chất xơ. Thích hợp ăn trực tiếp, làm salad hoặc nước ép.', 50000, 1, 1),
(2, N'Táo vàng', N'Táo vàng Mỹ, giòn, ngọt, mọng nước, giàu chất chống oxy hóa và vitamin. Phù hợp ăn trực tiếp, làm bánh hoặc salad.', 60000, 1, 1),
(3, N'Cam sành', N'Cam sành Việt Nam, ngọt đậm đà, ít chua, giàu vitamin C và folate. Phù hợp vắt nước hoặc ăn trực tiếp.', 35000, 1, 1),
(4, N'Cam vàng', N'Cam vàng tươi ngon, mọng nước, ngọt tự nhiên và giàu vitamin C giúp tăng cường sức đề kháng. Phù hợp ăn trực tiếp hoặc làm nước ép.', 40000, 1, 1),
(5, N'Quýt', N'Quýt ngọt lịm, mọng nước, giàu vitamin C và chất xơ, giúp tăng cường hệ miễn dịch và tiêu hóa. Thích hợp ăn trực tiếp hoặc làm nước ép.', 45000, 1, 1),
(6, N'Chuối tiêu', N'Chuối tiêu chín vàng, mềm, ngọt tự nhiên, giàu kali và năng lượng. Thích hợp ăn trực tiếp, làm sinh tố hoặc bánh trái cây.', 25000, 1, 1),
(7, N'Chuối sứ', N'Chuối sứ chín tự nhiên, mềm, ngọt dịu, giàu kali và năng lượng. Thích hợp ăn trực tiếp, làm sinh tố hoặc bánh trái cây.', 30000, 1, 1),
(8, N'Dâu tây', N'Dâu tây Đà Lạt siêu ngọt, tươi ngon, mọng nước, giàu vitamin C và chất chống oxy hóa. Lý tưởng cho ăn trực tiếp, salad hoặc làm mứt.', 70000, 1, 3),
(9, N'Dưa hấu', N'Dưa hấu mọng nước, ngọt thanh, giàu lycopene và vitamin C. Phù hợp ăn trực tiếp, làm nước ép hoặc tráng miệng giải khát.', 25000, 1, 1),
(10, N'Dưa lưới', N'Dưa lưới vàng ngọt, thơm đặc trưng, mọng nước, giàu beta-carotene. Ăn trực tiếp làm tráng miệng hoặc sinh tố.', 45000, 1, 1),
(11, N'Nho xanh', N'Nho xanh không hạt, giòn ngọt, mọng nước, giàu resveratrol và vitamin K. Thích hợp ăn trực tiếp, làm rượu vang hoặc salad.', 85000, 1, 1),
(12, N'Nho đỏ', N'Nho đỏ không hạt, mọng nước, ngọt thanh, giàu chất chống oxy hóa và vitamin. Thích hợp ăn trực tiếp, làm salad hoặc rượu vang tại nhà.', 90000, 1, 3),
(13, N'Nho tím', N'Nho tím không hạt, mọng, ngọt, giàu chất chống oxy hóa và vitamin. Thích hợp ăn trực tiếp, làm salad hoặc rượu vang tại nhà.', 80000, 1, 3),
(14, N'Xoài cát', N'Xoài cát Hòa Lộc, vàng ươm, thơm ngọt, thịt mềm, giàu vitamin A và C. Phù hợp ăn trực tiếp hoặc làm sinh tố, kem trái cây.', 60000, 1, 1),
(15, N'Xoài tượng', N'Xoài tượng da xanh, quả to, thịt dày, ngọt thanh, giàu vitamin A và C. Thích hợp ăn trực tiếp hoặc làm sinh tố, kem trái cây.', 55000, 1, 1),
(16, N'Xoài keo', N'Xoài keo chín vàng, ngọt mát, thịt mềm, giàu vitamin A và C. Phù hợp ăn trực tiếp hoặc làm sinh tố, kem trái cây.', 50000, 1, 1),
(17, N'Dừa xiêm', N'Dừa xiêm xanh, nước ngọt mát, cùi dừa non béo ngậy, giàu chất điện giải và vitamin. Thích hợp uống trực tiếp hoặc làm sinh tố, chè.', 40000, 1, 2),
(18, N'Dưa chuột', N'Dưa chuột tươi ngon, giòn mát, mọng nước, giàu vitamin K và chất xơ. Thích hợp ăn sống, làm salad hoặc nước detox thanh mát.', 12000, 1, 1),
(19, N'Quýt hồng', N'Quýt hồng ngọt lịm, mọng nước, giàu vitamin C và chất xơ. Phù hợp ăn trực tiếp hoặc làm nước ép giải khát.', 40000, 1, 1),
(20, N'Bưởi da xanh', N'Bưởi da xanh đặc sản miền Tây, múi mọng, ngọt thanh, giàu vitamin C và chất chống oxy hóa. Ăn trực tiếp hoặc làm nước ép đều thơm ngon.', 60000, 1, 2),
(21, N'Chôm chôm', N'Chôm chôm vỏ mỏng, ngọt nước, mọng, giàu vitamin C và chất chống oxy hóa. Phù hợp ăn trực tiếp hoặc làm món tráng miệng.', 50000, 1, 1),
(22, N'Thanh long đỏ', N'Thanh long đỏ mọng nước, ngọt nhẹ, giàu vitamin C và chất chống oxy hóa. Ăn trực tiếp hoặc làm sinh tố giải khát.', 40000, 1, 1),
(23, N'Mít', N'Mít chín vàng, ngọt thơm, giàu vitamin C, chất xơ và năng lượng. Ăn trực tiếp hoặc làm sinh tố, bánh trái cây.', 50000, 1, 1),
(24, N'Kiwi', N'Kiwi nhập khẩu, chua ngọt vừa phải, giàu vitamin C, K và chất xơ, tốt cho tiêu hóa và miễn dịch. Ăn trực tiếp hoặc làm nước ép.', 90000, 1, 1),
(25, N'Dưa vàng', N'Dưa vàng thơm ngọt, mọng nước, giàu vitamin A và C, tốt cho da và mắt. Ăn trực tiếp hoặc làm sinh tố, salad.', 50000, 1, 1),
(26, N'Mơ', N'Mơ chín mọng, vị ngọt nhẹ và hơi chua, giàu vitamin A, C, chất chống oxy hóa. Ăn trực tiếp hoặc làm mứt, nước ép.', 60000, 1, 1),
(27, N'Ổi', N'Ổi tươi ngon, giòn, mọng nước, giàu vitamin C và chất xơ, hỗ trợ tiêu hóa. Ăn trực tiếp hoặc làm nước ép, salad.', 30000, 1, 1),
(28, N'Măng cụt', N'Măng cụt chín mọng, vị ngọt thanh, giàu chất chống oxy hóa, vitamin và khoáng chất. Ăn trực tiếp hoặc làm món tráng miệng.', 100000, 1, 1),
(29, N'Hồng giòn', N'Hồng giòn chín mọng, vị ngọt dịu, giàu chất xơ và vitamin, tốt cho hệ tiêu hóa. Ăn trực tiếp hoặc làm salad.', 70000, 1, 1),
(30, N'Đu đủ', N'Đu đủ chín vàng, ngọt mát, giàu vitamin A, C và enzyme papain hỗ trợ tiêu hóa. Ăn trực tiếp hoặc làm sinh tố.', 30000, 1, 1),
(31, N'Cherry', N'Cherry đỏ tươi ngon, ngọt thanh, giàu chất chống oxy hóa và melatonin tự nhiên. Tốt cho giấc ngủ và chống viêm. Ăn trực tiếp hoặc làm mứt.', 150000, 1, 3),
(32, N'Táo xanh', N'Táo xanh Granny Smith, vị chua ngọt đặc trưng, giàu pectin và chất xơ. Thích hợp ăn trực tiếp, làm bánh hoặc salad.', 55000, 1, 1),
(33, N'Bưởi hồng', N'Bưởi hồng ruột đỏ, ngọt thanh, ít đắng, giàu lycopene và vitamin C. Ăn trực tiếp hoặc làm salad trái cây.', 65000, 1, 2),
(34, N'Vải thiều', N'Vải thiều Lục Ngạn, thịt trắng ngọt lịm, thơm đặc trưng, giàu vitamin C. Ăn trực tiếp hoặc làm nước ép, mứt.', 80000, 1, 1),
(35, N'Nhãn', N'Nhãn tươi ngọt thanh, thịt trắng trong, giàu vitamin C và glucose tự nhiên. Ăn trực tiếp hoặc nấu chè, làm mứt.', 60000, 1, 1),
(36, N'Mãng cầu', N'Mãng cầu chín mềm, vị ngọt đặc biệt, giàu vitamin C và chất xơ. Ăn trực tiếp hoặc làm kem, sinh tố.', 70000, 1, 1);
SET IDENTITY_INSERT product OFF;
SET IDENTITY_INSERT product_batch ON;
INSERT INTO product_batch (id, expiry_date, quantity, product_id) VALUES
-- Táo đỏ
(1, '2025-11-29', 52, 1),
(2, '2025-11-07', 41, 1),
(3, '2025-12-05', 68, 1),
-- Táo vàng
(4, '2025-11-15', 45, 2),
(5, '2025-11-22', 59, 2),
(6, '2025-11-06', 37, 2),
(7, '2025-12-01', 66, 2),
-- Cam sành
(8, '2025-11-10', 48, 3),
(9, '2025-11-25', 55, 3),
(10, '2025-12-03', 61, 3),
-- Cam vàng
(11, '2025-11-08', 39, 4),
(12, '2025-11-19', 50, 4),
(13, '2025-12-08', 64, 4),
(14, '2025-11-11', 42, 4),
-- Quýt
(15, '2025-11-28', 58, 5),
(16, '2025-11-17', 47, 5),
(17, '2025-12-07', 69, 5),
-- Chuối tiêu
(18, '2025-11-20', 53, 6),
(19, '2025-11-13', 44, 6),
(20, '2025-11-30', 60, 6),
-- Chuối sứ
(21, '2025-11-28', 62, 7),
(22, '2025-11-06', 40, 7),
(23, '2025-11-23', 57, 7),
-- Dâu tây
(24, '2025-11-15', 48, 8),
(25, '2025-12-03', 66, 8),
(26, '2025-11-09', 39, 8),
(27, '2025-11-20', 51, 8),
-- Dưa hấu
(28, '2025-12-08', 68, 9),
(29, '2025-11-12', 42, 9),
(30, '2025-11-22', 54, 9),
-- Dưa lưới
(31, '2025-11-13', 45, 10),
(32, '2025-11-27', 60, 10),
(33, '2025-11-05', 37, 10),
(34, '2025-11-17', 50, 10),
-- Nho xanh
(35, '2025-11-26', 59, 11),
(36, '2025-12-04', 67, 11),
(37, '2025-11-11', 41, 11),
-- Nho đỏ
(38, '2025-11-06', 36, 12),
(39, '2025-11-19', 48, 12),
(40, '2025-12-01', 63, 12),
(41, '2025-11-21', 52, 12),
-- Nho tím
(42, '2025-11-14', 44, 13),
(43, '2025-11-24', 55, 13),
(44, '2025-12-07', 69, 13),
-- Xoài cát
(45, '2025-11-08', 40, 14),
(46, '2025-11-25', 58, 14),
(47, '2025-12-05', 65, 14),
(48, '2025-11-18', 47, 14),
-- Xoài tượng
(49, '2025-11-23', 53, 15),
(50, '2025-11-10', 43, 15),
(51, '2025-11-29', 61, 15),
-- Xoài keo
(52, '2025-11-07', 38, 16),
(53, '2025-11-20', 49, 16),
(54, '2025-12-06', 66, 16),
-- Dừa xiêm
(55, '2025-11-27', 56, 17),
(56, '2025-12-02', 62, 17),
(57, '2025-11-16', 46, 17),
-- Dưa chuột
(58, '2025-11-24', 51, 18),
(59, '2025-11-09', 39, 18),
(60, '2025-12-09', 68, 18),
(61, '2025-11-15', 43, 18),
-- Quýt hồng
(62, '2025-11-28', 57, 19),
(63, '2025-11-11', 41, 19),
(64, '2025-12-04', 64, 19),
-- Bưởi da xanh
(65, '2025-11-22', 49, 20),
(66, '2025-11-30', 58, 20),
(67, '2025-11-06', 37, 20),
(68, '2025-12-10', 69, 20),
-- Chôm chôm
(69, '2025-11-25', 50, 21),
(70, '2025-12-06', 65, 21),
(71, '2025-11-13', 42, 21),
-- Thanh long đỏ
(72, '2025-11-27', 54, 22),
(73, '2025-11-08', 38, 22),
(74, '2025-12-01', 61, 22),
(75, '2025-11-20', 47, 22),
-- Mít
(76, '2025-11-26', 52, 23),
(77, '2025-12-08', 67, 23),
(78, '2025-11-10', 40, 23),
-- Kiwi
(79, '2025-11-15', 45, 24),
(80, '2025-12-07', 68, 24),
(81, '2025-11-23', 52, 24),
-- Dưa vàng
(82, '2025-12-01', 61, 25),
(83, '2025-11-09', 39, 25),
(84, '2025-11-26', 55, 25),
(85, '2025-11-14', 43, 25),
-- Mơ
(86, '2025-11-21', 50, 26),
(87, '2025-12-05', 66, 26),
(88, '2025-11-12', 41, 26),
-- Ổi
(89, '2025-11-07', 37, 27),
(90, '2025-11-30', 59, 27),
(91, '2025-11-19', 48, 27),
(92, '2025-12-03', 63, 27),
(93, '2025-11-25', 54, 27),
-- Măng cụt
(94, '2025-12-08', 69, 28),
(95, '2025-11-13', 42, 28),
(96, '2025-11-28', 57, 28),
-- Hồng giòn
(97, '2025-11-17', 46, 29),
(98, '2025-12-02', 60, 29),
(99, '2025-11-22', 51, 29),
-- Đu đủ
(100, '2025-11-08', 38, 30),
(101, '2025-12-04', 65, 30),
(102, '2025-11-16', 44, 30),
(103, '2025-11-29', 58, 30),
-- Cherry
(104, '2025-11-24', 53, 31),
(105, '2025-12-06', 67, 31),
(106, '2025-11-10', 40, 31),
-- Táo xanh
(107, '2025-11-20', 49, 32),
(108, '2025-12-03', 62, 32),
(109, '2025-11-06', 36, 32),
-- Bưởi hồng
(110, '2025-11-27', 56, 33),
(111, '2025-11-18', 47, 33),
(112, '2025-12-05', 64, 33),
(113, '2025-11-11', 39, 33),
-- Vải thiều
(114, '2025-11-30', 58, 34),
(115, '2025-11-12', 41, 34),
(116, '2025-12-09', 69, 34),
(117, '2025-11-24', 52, 34),
-- Nhãn
(118, '2025-11-17', 45, 35),
(119, '2025-12-02', 60, 35),
(120, '2025-11-08', 37, 35),
-- Mãng cầu
(121, '2025-11-18', 49, 36),
(122, '2025-11-26', 56, 36),
(123, '2025-12-02', 63, 36),
(124, '2025-11-09', 38, 36),
(125, '2025-11-21', 51, 36);

SET IDENTITY_INSERT product_batch OFF;