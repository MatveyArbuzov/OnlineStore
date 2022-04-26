using System.Data.SqlClient;
using AdoDemo;
using AdoDemo.EF;
using static AdoDemo.EF.Cont;

static void AdoNetDemo()
{
    var connection = new SqlConnection(@"Server = DESKTOP-HRANBB9; Database = OnlineStore; Trusted_Connection = True;");
    connection.Open();

    //1----------------------------------------------
    var command = connection.CreateCommand();
    command.CommandText = "SELECT Product.Name, Category.Name AS Category, Product.Price FROM Product, Category WHERE Product.CategoryId = Category.IdCategory ORDER BY Category, Price";
    var reader = command.ExecuteReader();

    Console.WriteLine("1. Cписок товаров, отсортированных по названию своей группы, далее по цене");
    while (reader.Read())
    {
        Console.WriteLine($"Имя = {reader.GetString(reader.GetOrdinal("Name"))}, Категория = {reader.GetString(reader.GetOrdinal("Category"))}, Цена = {reader.GetDecimal(reader.GetOrdinal("Price"))} ");
    }
    reader.Close();
    Console.WriteLine(" ");

    //2------------------------------------------
    var command1 = connection.CreateCommand();
    command1.CommandText = "SELECT Name, Quantity, Price, Quantity * Price AS Amount FROM Product ORDER BY Amount";
    var reader1 = command1.ExecuteReader();

    Console.WriteLine("2. Cписок товаров, отсортированных по своей суммарной стоимости на складе (единиц товара * цена единицы)");
    while (reader1.Read())
    {
        Console.WriteLine($"Имя = {reader1.GetString(reader1.GetOrdinal("Name"))}, Количество = {reader1.GetInt32(reader1.GetOrdinal("Quantity"))}, Цена = {reader1.GetDecimal(reader1.GetOrdinal("Price"))}, Сумма = {reader1.GetDecimal(reader1.GetOrdinal("Amount"))} ");
    }
    reader1.Close();
    Console.WriteLine(" ");

    //3------------------------------------------
    var command3 = connection.CreateCommand();
    command3.CommandText = "SELECT AVG(Price) AS AVG FROM Product";
    var reader3 = command3.ExecuteReader();
    reader3.Read();
    Console.WriteLine($"3.\nСредняя стоимость товаров = {reader3.GetDecimal(reader3.GetOrdinal("AVG"))}");
    reader3.Close();

    var command2 = connection.CreateCommand();
    command2.CommandText = "DECLARE @sr MONEY SELECT @sr = AVG(Price) FROM Product SELECT Name, Price FROM Product WHERE Price < @sr";
    var reader2 = command2.ExecuteReader();

    Console.WriteLine("Список товаров, стоимостью “ниже среднего по базе”");
    while (reader2.Read())
    {
        Console.WriteLine($"Имя = {reader2.GetString(reader2.GetOrdinal("Name"))}, Цена = {reader2.GetDecimal(reader2.GetOrdinal("Price"))}");
    }
    reader2.Close();
    Console.WriteLine(" ");

    //4------------------------------
    var command4 = connection.CreateCommand();
    command4.CommandText = "SELECT Category.Name, Sum (Quantity) AS Sum FROM Product, Category WHERE Product.CategoryId = Category.IdCategory GROUP BY CategoryId, Category.Name ORDER BY Sum";
    var reader4 = command4.ExecuteReader();

    Console.WriteLine("4. Пары: {группа товара, число единиц товара на складе в группе}");
    while (reader4.Read())
    {
        Console.WriteLine($"Категория = {reader4.GetString(reader4.GetOrdinal("Name"))}, Количество = {reader4.GetInt32(reader4.GetOrdinal("Sum"))}");
    }
    reader4.Close();
    Console.WriteLine(" ");

    //5-----------------------------------
    var command5 = connection.CreateCommand();
    command5.CommandText = "DECLARE @mn MONEY, @idd int SELECT @mn = MIN(Price) FROM Product SELECT @idd = IdProduct FROM Product WHERE Price = @mn SELECT Name, Quantity, Price FROM Product WHERE IdProduct = @idd";
    var reader5 = command5.ExecuteReader();
    reader5.Read();
    Console.WriteLine($"5.Извлечь товар с минимальной ценой, увеличить её на 20%. Результат сохранить\nТовар с минимальной ценой:\nИмя = {reader5.GetString(reader5.GetOrdinal("Name"))}, Количество = {reader5.GetInt32(reader5.GetOrdinal("Quantity"))}, Цена = {reader5.GetDecimal(reader5.GetOrdinal("Price"))}");
    reader5.Close();
    //Id
    var command6 = connection.CreateCommand();
    command6.CommandText = "DECLARE @mni MONEY SELECT @mni = MIN(Price) FROM Product SELECT IdProduct FROM Product WHERE Price = @mni";
    var reader6 = command6.ExecuteReader();
    reader6.Read();
    int iddddd = reader6.GetInt32(reader6.GetOrdinal("IdProduct"));
    reader6.Close();
    //Цена
    var command7 = connection.CreateCommand();
    command7.CommandText = "DECLARE @mnp MONEY SELECT @mnp = MIN(Price) FROM Product SELECT Price FROM Product WHERE Price = @mnp";
    var reader7 = command7.ExecuteReader();
    reader7.Read();

    double pr = Convert.ToDouble(reader7.GetDecimal(reader7.GetOrdinal("Price"))) * 1.2;
    reader7.Close();
    var command8 = connection.CreateCommand();
    command8.CommandText = $"UPDATE Product SET Price = {pr} WHERE IdProduct = {iddddd}";
    var reader8 = command8.ExecuteReader();
    reader8.Read();

    Console.WriteLine($"Была установлена следующая цена: {pr}");
    Console.WriteLine("\n");
    reader8.Close();

    //6-------------------------------------------
    var command9 = connection.CreateCommand();
    command9.CommandText = "DECLARE @ma MONEY, @idd1 int SELECT @ma = MAX(Price) FROM Product SELECT @idd1 = IdProduct FROM Product WHERE Price = @ma SELECT Name, Quantity, Price FROM Product WHERE IdProduct = @idd1";
    var reader9 = command9.ExecuteReader();
    reader9.Read();
    Console.WriteLine($"6. Найти товар с максимальной ценой и удалить его из базы.\nТовар с максимальной ценой:\nИмя = {reader9.GetString(reader9.GetOrdinal("Name"))}, Количество = {reader9.GetInt32(reader9.GetOrdinal("Quantity"))}, Цена = {reader9.GetDecimal(reader9.GetOrdinal("Price"))}");
    reader9.Close();
    //Id
    var command10 = connection.CreateCommand();
    command10.CommandText = "DECLARE @mai MONEY SELECT @mai = MAX(Price) FROM Product SELECT IdProduct FROM Product WHERE Price = @mai";
    var reader10 = command10.ExecuteReader();
    reader10.Read();
    int idl = reader10.GetInt32(reader10.GetOrdinal("IdProduct"));
    reader10.Close();

    var command11 = connection.CreateCommand();
    command11.CommandText = $"DELETE Product WHERE IdProduct = {idl}";
    var reader11 = command11.ExecuteReader();
    reader11.Read();

    Console.WriteLine($"Продукт успешно удалён");
    Console.WriteLine("\n");
    reader11.Close();

    //7---------------------------
    var command12 = connection.CreateCommand();
    command12.CommandText = "SELECT TOP(1) CategoryId FROM Product GROUP BY CategoryId ORDER BY COUNT(*)";
    var reader12 = command12.ExecuteReader();
    reader12.Read();
    int cater = reader12.GetInt32(reader12.GetOrdinal("CategoryId"));
    reader12.Close();

    var command13 = connection.CreateCommand();
    command13.CommandText = $"INSERT INTO Product (Price, Name, Quantity, CategoryId) VALUES(1000, 'Добавленный товар', 100, {cater})";
    var reader13 = command13.ExecuteReader();
    reader13.Read();
    reader13.Close();

    var command14 = connection.CreateCommand();
    command14.CommandText = $"SELECT Name FROM Category WHERE IdCategory = {cater}";
    var reader14 = command14.ExecuteReader();
    reader14.Read();
    Console.WriteLine($"7. Найти категорию с наименьшим числом товарных позиций. Добавить в неё новую товарную позицию\nНовый товар добавлен в категорию {reader14.GetString(reader14.GetOrdinal("Name"))}");
    reader14.Close();

    connection.Close();
}


static void LinqDemo()
{
    Database db = new Database();
    Console.WriteLine("1. Cписок товаров, отсортированных по названию своей группы, далее по цене.\n");

    foreach (Product e in db.Products.OrderBy(e => e.CategoryId.Name).ThenBy(e => e.Price))
    {
        Console.WriteLine($"{e.Name}, {e.CategoryId.Name}, {e.Price} ");
    }

    Console.WriteLine("\n");

    Console.WriteLine("2. Cписок товаров, отсортированных по своей суммарной стоимости на складе (единиц товара * цена единицы)\n");
    foreach (Product e in db.Products.OrderBy(e => e.Quantity * e.Price))
    {
        Console.WriteLine($"{e.Name}, {e.Quantity * e.Price}");
    }
    Console.WriteLine("\n");


    Console.WriteLine("3. Cписок товаров, стоимостью “ниже среднего по базе”\n");
    double sr = 0;
    int chisl = 0;
    foreach (Product e in db.Products)
    {
        sr = sr + Convert.ToDouble(e.Price);
        chisl++;
    }
    sr = sr / chisl;
    Console.WriteLine($"Средняя стоимость = {sr}\n");
    Console.WriteLine($"Товары:");
    foreach (Product e in db.Products)
    {
        if (Convert.ToDouble(e.Price) < sr)
        {
            Console.WriteLine($"{e.Name}, {e.Price}");
        }
    }
    Console.WriteLine("\n");

    Console.WriteLine("4. Пары: {группа товара, число единиц товара на складе в группе}");

    foreach (Category e in db.CategoryList)
    {
        Console.WriteLine($"{e.Name}: " + db.Products.Where(g => g.CategoryId.IdCategory == e.IdCategory).Sum(g => g.Quantity));
    }
    Console.WriteLine("\n");

    Console.WriteLine("5. Извлечь товар с минимальной ценой, увеличить её на 20%. Результат сохранить\n");
    decimal minimum = db.Products.Min(e => Convert.ToDecimal(e.Price));
    Console.WriteLine("До изменений:");
    foreach (Product e in db.Products.Where(e => e.Price == minimum))
    {
        Console.WriteLine($"{e.Name} {e.Price}");
    }

    foreach (Product e in db.Products.Where(e => e.Price == minimum))
    {
        e.Price = Convert.ToDecimal(Convert.ToDouble(minimum) * 1.2);
    }
    Console.WriteLine("\n");
    Console.WriteLine("После изменений:");
    foreach (Product e in db.Products.Where(e => e.Price == Convert.ToDecimal(Convert.ToDouble(minimum) * 1.2)))
    {
        Console.WriteLine($"{e.Name} {e.Price}");
    }
    Console.WriteLine("\n");

    Console.WriteLine("6. Найти товар с максимальной ценой и удалить его из базы.\n");

    Console.WriteLine("До изменений:");
    foreach (Product e in db.Products)
    {
        Console.WriteLine($"{e.Name} {e.Price}");
    }

    decimal maxs = db.Products.Max(e => Convert.ToDecimal(e.Price));
    foreach (Product e in db.Products.Where(e => e.Price == maxs).Take(1))
    {
        db.Products.Remove(e);
    }

    Console.WriteLine("\n");
    Console.WriteLine("После изменений:");
    foreach (Product e in db.Products)
    {
        Console.WriteLine($"{e.Name} {e.Price}");
    }
    Console.WriteLine("\n");

    Console.WriteLine("7. Найти категорию с наименьшим числом товарных позиций. Добавить в неё новую товарную позицию.\n");
    int ab = 0;
    int mini = 1000000000;

    foreach (Category e in db.CategoryList)
    {
        if (db.Products.Where(g => g.CategoryId.IdCategory == e.IdCategory).Count() < mini)
        {
            mini = db.Products.Where(g => g.CategoryId.IdCategory == e.IdCategory).Count();
            ab = e.IdCategory;
        }
    }
    int maxid = 0;
    foreach (Product e in db.Products)
    {
        if (e.IdProduct > maxid)
        {
            maxid = e.IdProduct;
        }
    }
    db.Products.Add(new Product { IdProduct = maxid + 1, Price = 1000, Name = "Добавленный товар", Quantity = 100, CategoryId = db.CategoryList[ab - 1] });

    foreach (Product e in db.Products)
    {
        Console.WriteLine($"{e.IdProduct} {e.Name} {e.Price} {e.Quantity} {e.CategoryId.Name}");
    }
}

static void EFDemo()
{
    Cont db = new Cont();
    db.DropDB();
    db.CreateDbIfNotExist();
    db.AddCategory();
    db.SaveChanges();
    db.AddProduct();
    db.SaveChanges();

    Console.WriteLine("1. Cписок товаров, отсортированных по названию своей группы, далее по цене.\n");

    foreach (ContProduct e in db.ContProducts.OrderBy(e => e.CategoryId.Name).ThenBy(e => e.Price))
    {
        Console.WriteLine($"{e.Name}, {e.CategoryId.Name}, {e.Price} ");
    }

    Console.WriteLine("\n");

    Console.WriteLine("2. Cписок товаров, отсортированных по своей суммарной стоимости на складе (единиц товара * цена единицы)\n");
    foreach (ContProduct e in db.ContProducts.OrderBy(e => e.Quantity * e.Price))
    {
        Console.WriteLine($"{e.Name}, {e.Quantity * e.Price}");
    }
    Console.WriteLine("\n");


    Console.WriteLine("3. Cписок товаров, стоимостью “ниже среднего по базе”\n");
    double sr = 0;
    int chisl = 0;
    foreach (ContProduct e in db.ContProducts)
    {
        sr = sr + Convert.ToDouble(e.Price);
        chisl++;
    }
    sr = sr / chisl;
    Console.WriteLine($"Средняя стоимость = {sr}\n");
    Console.WriteLine($"Товары:");
    foreach (ContProduct e in db.ContProducts)
    {
        if (Convert.ToDouble(e.Price) < sr)
        {
            Console.WriteLine($"{e.Name}, {e.Price}");
        }
    }
    Console.WriteLine("\n");

    Console.WriteLine("4. Пары: {группа товара, число единиц товара на складе в группе}");

    foreach (ContCategory e in db.ContCategorys)
    {
        Cont db1 = new Cont();
        Console.WriteLine($"{e.Name}: " + db1.ContProducts.Where(g => g.CategoryId.Id == e.Id).Sum(g => g.Quantity));
        db1.Dispose();
    }
    Console.WriteLine("\n");

    Console.WriteLine("5. Извлечь товар с минимальной ценой, увеличить её на 20%. Результат сохранить\n");
    decimal minimum = db.ContProducts.Min(e => Convert.ToDecimal(e.Price));
    Console.WriteLine("До изменений:");
    foreach (ContProduct e in db.ContProducts)
    {
        Console.WriteLine($"{e.Name} {e.Price}");
    }

    foreach (var g in db.ContProducts)
    {
        Cont db2 = new Cont();
        if (g.Price == db2.ContProducts.Min(g => g.Price))
        {
            g.Price = g.Price * Convert.ToDecimal(1.2);
            break;
        }
        db2.SaveChanges();
        db2.Dispose();
    }
    db.SaveChanges();

    Console.WriteLine("\n");
    Console.WriteLine("После изменений:");
    foreach (ContProduct e in db.ContProducts)
    {
        Console.WriteLine($"{e.Name} {e.Price}");
    }
    Console.WriteLine("\n");

    Console.WriteLine("6. Найти товар с максимальной ценой и удалить его из базы.\n");

    Console.WriteLine("До изменений:");
    foreach (ContProduct e in db.ContProducts)
    {
        Console.WriteLine($"{e.Name} {e.Price}");
    }

    decimal maxs = db.ContProducts.Max(e => Convert.ToDecimal(e.Price));
    Cont db3 = new Cont();
    foreach (ContProduct e in db3.ContProducts.Where(e => e.Price == maxs))
    {
        db3.ContProducts.Remove(e);
    }
    db3.SaveChanges();
    db3.Dispose();
    db.SaveChanges();
    
    Console.WriteLine("\n");
    Console.WriteLine("После изменений:");
    foreach (ContProduct e in db.ContProducts)
    {
        Console.WriteLine($"{e.Name} {e.Price}");
    }
    Console.WriteLine("\n");
    //db.Dispose();

    Console.WriteLine("7. Найти категорию с наименьшим числом товарных позиций. Добавить в неё новую товарную позицию.\n");
    
    int ab = 0;
    int mini = 1000000000;
    
    foreach (ContCategory e in db.ContCategorys)
    {
        Cont db4 = new Cont();
        if (db4.ContProducts.Where(g => g.CategoryId.Id == e.Id).Count() < mini)
        {
            mini = db4.ContProducts.Where(g => g.CategoryId.Id == e.Id).Count();
            ab = e.Id;
        }
        db4.SaveChanges();
        db4.Dispose();
    }
    ContCategory ab1 = db.ContCategorys.First(x => x.Id == ab);
    db.ContProducts.Add(new ContProduct {Price = 1000, Name = "Добавленный товар", Quantity = 100, CategoryId = ab1});
    db.SaveChanges();
    //db.Dispose();
    foreach (ContProduct e in db.ContProducts)
    {
        Console.WriteLine($"{e.Id} {e.Name} {e.Price} {e.Quantity} {e.CategoryId.Name}");
    }
    db.SaveChanges();
}

//AdoNetDemo();
//LinqDemo();
EFDemo();



