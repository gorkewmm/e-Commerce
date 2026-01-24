using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MultiShop.Cargo.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.Cargo.DataAccessLayer.Concrete
{
    public class CargoContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
                =>options.UseSqlServer("Server=localhost,1441;Initial Catalog=MultiShopCargoDb;User=sa;Password=123456aA*;TrustServerCertificate=True;");

        DbSet<CargoCompany> CargoCompanies { get; set; }
        DbSet<CargoDetail> CargoDetails { get; set; }
        DbSet<CargoCustomer> CargoCustomers{ get; set; }
        DbSet<CargoOperation> CargoOperations { get; set; }

    }
}
