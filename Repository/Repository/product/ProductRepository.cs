using Microsoft.EntityFrameworkCore;
using Repository.Models.user;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Repository.product
{
    public class ProductRepository(IDbContextFactory<DbContext> contextFactory) : BasicRepository<Product, long>(contextFactory)
    {
        public new List<Product> GetAll()
        {
            using var context = _contextFactory.CreateDbContext();
            return [.. context.Set<Product>().Include(p => p.ProductBatches).Include(p => p.ProductUnit)];
        }

        public Product? GetProductById(long id)
        {
            using var context = _contextFactory.CreateDbContext();
            return context.Set<Product>()
                .Include(p => p.ProductBatches)
                .Include(p => p.ProductUnit)
                .FirstOrDefault(p => p.Id == id);
        }

        public void UpdateWithBatches(Product product)
        {
            using var context = _contextFactory.CreateDbContext();
            
            // Get the existing product with its batches from database
            var existingProduct = context.Set<Product>()
                .Include(p => p.ProductBatches)
                .FirstOrDefault(p => p.Id == product.Id);

            if (existingProduct == null)
            {
                throw new InvalidOperationException($"Product with ID {product.Id} not found");
            }

            // Update product properties
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.IsActive = product.IsActive;
            existingProduct.ProductUnitId = product.ProductUnitId;

            if (product.ProductBatches != null)
            {
                var incomingBatchIds = product.ProductBatches
                    .Where(b => b.Id > 0)
                    .Select(b => b.Id)
                    .ToList();

                var batchesToRemove = existingProduct.ProductBatches
                    .Where(b => !incomingBatchIds.Contains(b.Id))
                    .ToList();

                foreach (var batch in batchesToRemove)
                {
                    context.Set<ProductBatch>().Remove(batch);
                }

                foreach (var incomingBatch in product.ProductBatches)
                {
                    if (incomingBatch.Id > 0)
                    {
                        var existingBatch = existingProduct.ProductBatches
                            .FirstOrDefault(b => b.Id == incomingBatch.Id);

                        if (existingBatch != null)
                        {
                            existingBatch.ExpiryDate = incomingBatch.ExpiryDate;
                            existingBatch.Quantity = incomingBatch.Quantity;
                        }
                    }
                    else
                    {
                        // Add new batch
                        var newBatch = new ProductBatch
                        {
                            ExpiryDate = incomingBatch.ExpiryDate,
                            Quantity = incomingBatch.Quantity,
                            ProductId = product.Id
                        };
                        existingProduct.ProductBatches.Add(newBatch);
                    }
                }
            }
            else
            {
                var allBatches = existingProduct.ProductBatches.ToList();
                foreach (var batch in allBatches)
                {
                    context.Set<ProductBatch>().Remove(batch);
                }
            }

            context.SaveChanges();
        }
    }
}
