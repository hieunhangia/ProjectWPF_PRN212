
using Repository;
using Repository.Repository.address;

namespace Service
{
    public class AddressService(ProvinceCityRepository provinceCityRepository,
        CommuneWardRepository communeWardRepository)
    {
        private readonly ProvinceCityRepository _provinceCityRepository = provinceCityRepository;
        private readonly CommuneWardRepository _communeWardRepository = communeWardRepository;

        public List<ProvinceCity> GetAllProvinceCities()
        {
            return _provinceCityRepository.GetAll();
        }

        public List<CommuneWard> GetCommuneWardsByProvinceCityCode(string provinceCityCode)
        {
            return _communeWardRepository.GetByCondition(cw => cw.ProvinceCityCode == provinceCityCode);
        }

        public CommuneWard? GetCommuneWardByCode(string code)
        {
            return _communeWardRepository.GetById(code);
        }
    }
}
