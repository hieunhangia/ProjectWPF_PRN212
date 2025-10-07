
using Repository;
using Repository.Repository.address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AddressService
    {
        private readonly ProvinceCityRepository _provinceCityRepository = new();
        private readonly CommuneWardRepository _communeWardRepository = new();

        public List<ProvinceCity> GetAllProvinceCities()
        {
            return _provinceCityRepository.GetAll();
        }

        public List<CommuneWard> GetCommuneWardsByProvinceCityCode(string provinceCityCode)
        {
            return _communeWardRepository.GetByCondition(cw => cw.ProvinceCityCode == provinceCityCode);
        }
    }
}
