namespace ProductApp.Asp.NetWebApi.Migrations
{
    using ProductApp.Asp.NetWebApi.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProductApp.Asp.NetWebApi.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProductApp.Asp.NetWebApi.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            context.Products.AddOrUpdate(
              new Product() { Name = "Product1", Price = 219.5m, ProductionYear = 2018 },
              new Product() { Name = "Product2", Price = 55.8m, ProductionYear = 2019 },
              new Product() { Name = "Product3", Price = 120, ProductionYear = 2020 });
            context.SaveChanges();
        }
    }
}
