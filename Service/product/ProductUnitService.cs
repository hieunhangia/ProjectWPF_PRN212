using Repository;
using Repository.Repository.product;

namespace Service.product
{
    public class ProductUnitService(ProductUnitRepository productUnitRepository)
    {
        private readonly ProductUnitRepository _productUnitRepository = productUnitRepository;

        public List<ProductUnit> GetAll()
        {
            return _productUnitRepository.GetAll();
        }

        public ProductUnit? GetById(long id)
        {
            return _productUnitRepository.GetById(id);
        }
    }
}
