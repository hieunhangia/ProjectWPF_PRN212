using Data.Models.address;
using Data.Models.product;
using Data.Models.user;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class FruitShopContext : DbContext
    {
        // User entities
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<Role> Roles { get; set; }

        // Product entities
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductBatch> ProductBatches { get; set; }
        public DbSet<ProductUnit> ProductUnits { get; set; }

        // Address entities
        public DbSet<ProvinceCity> ProvinceCities { get; set; }
        public DbSet<CommuneWard> CommuneWards { get; set; }

        public FruitShopContext(DbContextOptions<FruitShopContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=FruitShopProject;User Id=sa;Password=12345678;TrustServerCertificate=True;");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User inheritance (Table Per Hierarchy)
            modelBuilder.Entity<User>()
                .HasDiscriminator<string>("user_type")
                .HasValue<User>("user")
                .HasValue<Admin>("admin")
                .HasValue<Seller>("seller");

            // Configure User-Role relationship (one-to-one)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithOne(r => r.User)
                .HasForeignKey<User>(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

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
                .HasForeignKey(cw => cw.ProvinceCode)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Seller-CommuneWard relationship (many-to-one)
            modelBuilder.Entity<Seller>()
                .HasOne(s => s.CommuneWard)
                .WithMany()
                .HasForeignKey(s => s.CommuneWardCode)
                .OnDelete(DeleteBehavior.SetNull);


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

            // Role name should be unique
            modelBuilder.Entity<Role>()
                .HasIndex(r => r.Name)
                .IsUnique();

            // ProvinceCity and CommuneWard name indexes for searching
            modelBuilder.Entity<ProvinceCity>()
                .HasIndex(pc => pc.Name);

            modelBuilder.Entity<CommuneWard>()
                .HasIndex(cw => cw.Name);
        }
        public FruitShopContext()
        {
        }

    }
}