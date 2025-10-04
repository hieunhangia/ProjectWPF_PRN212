using Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

class Program
{
    static void Main()
    {
        //Tao file appsetting.json de chua connection string
        //Vao appsetting.json -> properties -> Copy to output directory -> Copy if newer
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Build DbContext options
        var options = new DbContextOptionsBuilder<FruitShopContext>()
            .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            .Options;

        // Apply migrations (creates database if not exists)
        using var context = new FruitShopContext(options);
        //Tao DB khi chua co DB hoac chua co table
        context.Database.EnsureCreated();
        //Xoa DB ngay lap tuc
        //context.Database.EnsureDeleted();
        Console.WriteLine("Database created successfully!");
    }
}
