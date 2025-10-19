using Repository;
using Repository.Repository.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.product
{
    public class ProductUnitService(ProductUnitRepository productUnitRepository)
    {
        private readonly ProductUnitRepository _productUnitRepository = productUnitRepository;

        public List<ProductUnit> GetAll()
        {
            return _productUnitRepository.GetAll();
        }

        public ProductUnit? GetById(long id) {
            return _productUnitRepository.GetById(id);
        }
    }
}
