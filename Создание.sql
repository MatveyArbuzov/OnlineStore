Use OnlineStore

CREATE TABLE [Product] (
[IdProduct] int IDENTITY(1,1) PRIMARY KEY NOT NULL,
[Price] money NOT NULL,
[Name] nvarchar(30) NOT NULL,
[Quantity] int NOT NULL,
[CategoryId] int NOT NULL
)
GO

CREATE TABLE [Category] (
[IdCategory] int IDENTITY(1,1) PRIMARY KEY NOT NULL,
[Name] nvarchar(30) NOT NULL
)
GO

ALTER TABLE [Product] ADD FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([IdCategory])
GO

INSERT Category
(Name)
VALUES
('����������'),
('������ ��� ����'),
('������');

INSERT Product
(Price, Name, Quantity, CategoryId)
VALUES
(5000, '�������� ����� Shell Helix', 5, 1),
(6000, '����� ������������ � ��������', 10, 1),
(900, '��������� �������� ����', 20, 1),
(1700, '����� ������', 5, 3),
(500, '�������� ��������', 15, 3),
(1500, '׸���� �����', 12, 3),
(1400, '���������� �����', 7, 2),
(700, '������� ��� ������', 4, 2),
(400, '������', 20, 2);




--�������--
--1-- ������ �������, ��������������� �� �������� ����� ������, ����� �� ����
SELECT Product.Name, Category.Name AS Category, Product.Price
FROM Product, Category
WHERE Product.CategoryId = Category.IdCategory
ORDER BY Category, Price

--2-- ������� ������ �������, ��������������� �� ����� ��������� ��������� �� ������ (������ ������ * ���� �������)
SELECT Name, Quantity, Price, Quantity * Price AS Amount
FROM Product
ORDER BY Amount

--3-- ������� ������ �������, ���������� ����� �������� �� ����
SELECT AVG(Price) AS AVG FROM Product 

DECLARE
@sr MONEY
SELECT @sr = AVG(Price) FROM Product
SELECT Name, Price
FROM Product
WHERE Price < @sr

--4-- ������� ����: {������ ������, ����� ������ ������ �� ������ � ������}, ������������ ����������� (GroupBy, Sum)
SELECT Category.Name, Sum (Quantity) AS Sum
FROM Product, Category
WHERE Product.CategoryId = Category.IdCategory
GROUP BY CategoryId, Category.Name
ORDER BY Sum

--5-- ������� ����� � ����������� �����, ��������� � �� 20%. ��������� ���������
DECLARE
@mn MONEY,
@idd int
SELECT @mn = MIN(Price) FROM Product
SELECT @idd = IdProduct FROM Product
WHERE Price = @mn
SELECT Name, Quantity, Price FROM Product WHERE IdProduct = @idd

DECLARE @mni MONEY SELECT @mni = MIN(Price) FROM Product SELECT IdProduct FROM Product WHERE Price = @mni

DECLARE @mnp MONEY SELECT @mnp = MIN(Price) FROM Product SELECT Price FROM Product WHERE Price = @mnp


--6-- ����� ����� � ������������ ����� � ������� ��� �� ����.
DECLARE
@ma MONEY,
@idd1 int
SELECT @ma = MAX(Price) FROM Product
SELECT @idd1 = IdProduct FROM Product
WHERE Price = @ma
SELECT Name, Quantity, Price FROM Product WHERE IdProduct = @idd1

DECLARE
@mai MONEY
SELECT @mai = MAX(Price) FROM Product
SELECT IdProduct FROM Product
WHERE Price = @mai

--7-- ����� ��������� � ���������� ������ �������� �������. �������� � �� ����� �������� ������� (����� ��������)
SELECT TOP(1) CategoryId
FROM Product
GROUP BY CategoryId
ORDER BY COUNT(*)
