using System.Collections.Generic;

namespace AdoDemo;

public class Category
{
    public int IdCategory;
    public string Name;
}

public class Product
{
    public int IdProduct;
    public decimal Price;
    public string Name;
    public int Quantity;
    public Category CategoryId;
}

public class Database
{
    public List<Product> Products;
    public List<Category> CategoryList;

    public Database()
    {
        CategoryList = new List<Category>
        {
            new Category { IdCategory = 1, Name = "Автотовары"},
            new Category { IdCategory = 2, Name = "Товары для дома"},
            new Category { IdCategory = 3, Name = "Одежда"},
        };

        Products = new List<Product>
        {
            new Product { IdProduct = 1, Price = 5000, Name = "Моторное масло Shell Helix", Quantity = 5, CategoryId = CategoryList[0]},
            new Product { IdProduct = 2, Price = 6000, Name = "Набор инструментов в чемодане", Quantity = 10, CategoryId = CategoryList[0]},
            new Product { IdProduct = 3, Price = 900, Name = "Удалитель ржавчины КППС", Quantity = 20, CategoryId = CategoryList[0]},
            new Product { IdProduct = 4, Price = 1700, Name = "Синие джинсы", Quantity = 5, CategoryId = CategoryList[2]},
            new Product { IdProduct = 5, Price = 500, Name = "Бордовая футболка", Quantity = 15, CategoryId = CategoryList[2]},
            new Product { IdProduct = 6, Price = 1500, Name = "Чёрная кофта", Quantity = 12, CategoryId = CategoryList[2]},
            new Product { IdProduct = 7, Price = 1400, Name = "Постельное белье", Quantity = 7, CategoryId = CategoryList[1]},
            new Product { IdProduct = 8, Price = 700, Name = "Сушилка для посуды", Quantity = 4, CategoryId = CategoryList[1]},
            new Product { IdProduct = 9, Price = 400, Name = "Швабра", Quantity = 20, CategoryId = CategoryList[1]},
        };
    }
}