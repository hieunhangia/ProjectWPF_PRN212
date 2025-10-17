using Repository;
using Repository.Repository.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.product
{
    public class ProductService(ProductRepository productRepository)
    {
        private readonly ProductRepository _productRepository = productRepository;

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public List<ProductUnit> GetProductUnits()
        {
            return _productRepository.GetProductUnits();
        }
    }
}
