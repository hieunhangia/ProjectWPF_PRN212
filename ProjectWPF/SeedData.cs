using Microsoft.EntityFrameworkCore;
using ProjectWPF.Models;
using Repository;
using Repository.Models.user;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectWPF
{
    public class SeedData
    {

        public static void CreatedDatabase(Repository.DbContext context)
        {
            if (context.Database.EnsureCreated())
            {
                String sqlFilePath = "C:\\Users\\binh\\Source\\Repos\\ProjectWPF_PRN212\\ProjectWPF\\all-prn.sql";
                String sqlScript = System.IO.File.ReadAllText(sqlFilePath);
                context.Database.ExecuteSqlRaw(sqlScript);
                var firstCommuneWardCode = context.CommuneWards.First().Code;
                var firstProductUnit = context.ProductUnits.First();
                var addRequestTypeId = context.SellerRequestTypes.First(rt => rt.Name == "Thêm mới").Id;
                var pendingStatusId = context.SellerRequestStatuses.First(rs => rs.Name == "Đang Chờ Duyệt").Id;



                // 3. Seed Users (Seller and Admin)
                Seller s = new Seller
                {
                    FullName = "Sample Seller",
                    Cid = "123456789",
                    SpecificAddress = "123 Main St",
                    CommuneWardCode = firstCommuneWardCode, // Use cached value
                    Password = "seller",
                    Email = "seller1@shop.com",
                    IsActive = true
                };
                context.Set<Seller>().Add(s); // Add to context, but DON'T save yet.

                Admin a = new Admin
                {
                    Password = "admin",
                    Email = "admin",
                    IsActive = true
                };
                context.Set<Admin>().Add(a); // Add to context, but DON'T save yet.

                // 4. Seed Product
                Product p = new Product
                {
                    Name = "Táo tàu",
                    Description = "Quả táo tàu",
                    IsActive = true,
                    Price = 100,
                    ProductUnit = firstProductUnit
                };
                context.Set<Product>().Add(p);
                context.SaveChanges();
                Product newProduct = new Product
                {
                    Name = "Táo tàu 2",
                    Description = "Quả táo tàu 2",
                    IsActive = true,
                    Price = 150,
                    ProductUnitId = p.ProductUnitId
                };
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };

                SellerRequest sellerRequest = new SellerRequest();
                sellerRequest.CreatedAt = DateTime.Now;
                sellerRequest.EntityName = "Product";
                sellerRequest.Content = JsonSerializer.Serialize(newProduct, options);
                sellerRequest.RequestTypeId = addRequestTypeId;
                sellerRequest.StatusId = pendingStatusId;
                sellerRequest.SellerId = context.Sellers.First().Id;
                sellerRequest.Seller = context.Sellers.First();

                context.Set<SellerRequest>().Add(sellerRequest); // Add to context.

                context.SaveChanges();
            }
        }

    }
}
