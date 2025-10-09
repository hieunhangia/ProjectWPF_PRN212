using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repository.Models.user;

namespace Repository
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        // User entities
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Seller> Sellers { get; set; }

        // Product entities
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductBatch> ProductBatches { get; set; }
        public DbSet<ProductUnit> ProductUnits { get; set; }

        // Address entities
        public DbSet<ProvinceCity> ProvinceCities { get; set; }
        public DbSet<CommuneWard> CommuneWards { get; set; }

        public DbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User inheritance (Table-per-Type)
            modelBuilder.Entity<User>().ToTable("user");
            modelBuilder.Entity<Admin>().ToTable("admin");
            modelBuilder.Entity<Seller>().ToTable("seller");

            // Configure Product-ProductUnit relationship (many-to-one)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductUnit)
                .WithMany(pu => pu.Products)
                .HasForeignKey(p => p.ProductUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Product-ProductBatch relationship (one-to-many)
            modelBuilder.Entity<ProductBatch>()
                .HasOne(pb => pb.Product)
                .WithMany(p => p.ProductBatches)
                .HasForeignKey(pb => pb.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure ProvinceCity-CommuneWard relationship (one-to-many)
            modelBuilder.Entity<CommuneWard>()
                .HasOne(cw => cw.ProvinceCity)
                .WithMany(pc => pc.CommuneWards)
                .HasForeignKey(cw => cw.ProvinceCityCode)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Seller-CommuneWard relationship (many-to-one)
            modelBuilder.Entity<Seller>()
                .HasOne(s => s.CommuneWard)
                .WithMany()
                .HasForeignKey(s => s.CommuneWardCode)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure indexes for performance
            ConfigureIndexes(modelBuilder);


        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            // User email should be unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Product name index for searching
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);

            // ProvinceCity and CommuneWard name indexes for searching
            modelBuilder.Entity<ProvinceCity>()
                .HasIndex(pc => pc.Name);

            modelBuilder.Entity<CommuneWard>()
                .HasIndex(cw => cw.Name);
        }

    }
}