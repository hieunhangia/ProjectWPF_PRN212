using Repository;
using Repository.Repository.product;

namespace Service.product
{
    public class ProductService(ProductRepository productRepository)
    {
        private readonly ProductRepository _productRepository = productRepository;

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }


        public Product? GetProductById(long id)
        {
            return _productRepository.GetProductById(id);
        }

        public void AddProduct(Product product)
        {
            _productRepository.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            _productRepository.Update(product);
        }
    }
}
