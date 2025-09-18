using Data.Entity;
using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contract
{
    public interface ILocationService
    {
        Task<IEnumerable<CountryDto>> GetCountries();

        Task<IEnumerable<StateDto>> GetStates(int CountryId);
        Task<IEnumerable<CityDto>> GetCities(int StateId);

        Task<List<Address>> AddAddress(List<AddAddressDto> address);

        Task<List<AllAddresses>> GetAllAddresses(string userId);

        Task<bool> DeleteAddress(int addressId);

        Task<Address> GetAddresssById(int addressId);
        Task<bool> EditAddress(AddAddressDto address);
    }
}
