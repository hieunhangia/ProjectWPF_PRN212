using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Repository.product;

namespace Service.product
{
    public class ProductService(ProductRepository productRepository, ProductUnitService productUnitService)
    {

        public List<Product> GetAllProducts()
        {
            return productRepository.GetAll();
        }


        public Product? GetProductById(long id)
        {
            return productRepository.GetProductById(id);
        }

        public void AddProduct(Product product)
        {
            product.ProductUnitId = product.ProductUnit!.Id;
            product.ProductUnit = null;
            productRepository.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            product.ProductUnitId = product.ProductUnit!.Id;
            product.ProductUnit = null;
            productRepository.UpdateWithBatches(product);
        }
    }
}
