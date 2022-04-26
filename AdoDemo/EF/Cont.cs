using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoDemo.EF
{
    internal class Cont : DbContext
    {
        public DbSet<ContCategory> ContCategorys { get; set; }
        public DbSet<ContProduct> ContProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server = DESKTOP-HRANBB9; Database = OnlineStoreEF; Trusted_Connection = True;");
            base.OnConfiguring(optionsBuilder);
        }

        public void CreateDbIfNotExist()
        {
            this.Database.EnsureCreated();
        }

        public void DropDB()
        {
            this.Database.EnsureDeleted();
        }

        public class ContCategory
        {
            public int Id { get; set; }
            public string? Name { get; set; }
        }

        public class ContProduct
        {
            public int Id { get; set; }
            public Decimal Price { get; set; }
            public string? Name { get; set; }
            public int Quantity { get; set; }
            public ContCategory CategoryId { get; set; }
        }

        public void AddCategory()
        {

            ContCategorys.AddRange(entities: new ContCategory[]
            {
                new ContCategory { Name = "Автотовары" },
                new ContCategory { Name = "Товары для дома" },
                new ContCategory { Name = "Одежда" }
            });
        }


        public void AddProduct()
        {

            ContProducts.AddRange(entities: new ContProduct[]
            {
                new ContProduct { Price = 5000, Name = "Моторное масло Shell Helix", Quantity = 5, CategoryId = ContCategorys.First(x=> x.Id==1)},
                new ContProduct { Price = 6000, Name = "Набор инструментов в чемодане", Quantity = 10, CategoryId = ContCategorys.First(x=> x.Id==1)},
                new ContProduct { Price = 900, Name = "Удалитель ржавчины КППС", Quantity = 20, CategoryId = ContCategorys.First(x=> x.Id==1)},
                new ContProduct { Price = 1700, Name = "Синие джинсы", Quantity = 5, CategoryId = ContCategorys.First(x=> x.Id==3)},
                new ContProduct { Price = 500, Name = "Бордовая футболка", Quantity = 15, CategoryId = ContCategorys.First(x=> x.Id==3)},
                new ContProduct { Price = 1500, Name = "Чёрная кофта", Quantity = 12, CategoryId = ContCategorys.First(x=> x.Id==3)},
                new ContProduct { Price = 1400, Name = "Постельное белье", Quantity = 7, CategoryId = ContCategorys.First(x=> x.Id==2)},
                new ContProduct { Price = 700, Name = "Сушилка для посуды", Quantity = 4, CategoryId = ContCategorys.First(x=> x.Id==2)},
                new ContProduct { Price = 400, Name = "Швабра", Quantity = 20, CategoryId = ContCategorys.First(x=> x.Id==2)}
            });
        }

    }
}
