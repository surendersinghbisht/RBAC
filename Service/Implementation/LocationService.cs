using Data.DbContext;
using Data.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Dto;
using Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class LocationService: ILocationService
    {
        private readonly IdentityDbContext _context;
        private readonly UserManager<IdentityUser> _userManager; 
        public LocationService(
            IdentityDbContext context,
            UserManager<IdentityUser> userManager
            ) {
        _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<CountryDto>> GetCountries()
        {
            var countries = await _context.Country.Select(c =>
            
                new CountryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                }
            ).ToListAsync();

            return countries;
        }

        public async Task<IEnumerable<StateDto>> GetStates(int countryId)
        {
            var cities = await _context.State.Where(s => s.CountryId == countryId).Select(s =>
            new StateDto
            {
                Id = s.Id,
                CountryId = countryId,
                Name = s.Name
            }).ToListAsync();

            return cities;
        }


        public async Task<IEnumerable<CityDto>> GetCities(int stateId)
        {
            var cities = await _context.City.Where(c => c.StateId == stateId).Select(c =>
            new CityDto
            {
                Id = c.Id,
                StateId = c.StateId,
                Name = c.Name
            }).ToListAsync();

            return cities;
        }

        public async Task<List<Address>> AddAddress(List<AddAddressDto> addressDto)
        {
            var addedAddress = new List<Address>();
            foreach (var addressD in addressDto)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == addressD.UserId);
                var country = await _context.Country.FirstOrDefaultAsync(c => c.Id == addressD.CountryId);
                var state = await _context.State.FirstOrDefaultAsync(s => s.Id == addressD.StateId);
                var city = await _context.City.FirstOrDefaultAsync(c => c.Id == addressD.CityId);
                var addressStr = $"State: {state.Name}, City: {city.Name}, Country: {country.Name}";
                var address = new Address
                {
                    UserId = user.Id,
                    CountryId = addressD.CountryId,
                    StateId = addressD.StateId,
                    CityId = addressD.CityId,
                    FullAddress = addressStr
                };

                addedAddress.Add(address);
            }
          
           await _context.Addresses.AddRangeAsync(addedAddress);

           _context.SaveChanges();

            return addedAddress;
        }

        public async Task<List<AllAddresses>> GetAllAddresses(string userId)
        {
            //var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);

            var addresses = (from a in _context.Addresses
                            join c in _context.Country on a.CountryId equals c.Id
                            join s in _context.State on a.StateId equals s.Id
                            join ci in _context.City on a.CityId equals ci.Id
                            where a.UserId == userId
                            select new AllAddresses
                            {
                                AddressId = a.Id,
                                CountryId = a.CountryId,
                                Country = c.Name,
                                StateId = a.StateId,
                                State = s.Name,
                                CityId = a.CityId,
                                City = ci.Name,
                            }).ToList();
            return addresses;
        }

        public async Task<bool> DeleteAddress(int addressId)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
             _context.Addresses.Remove(address);

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<Address> GetAddresssById(int addressId)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
            if (address == null) return null;
            return address;
        }

        public async Task<bool> EditAddress(AddAddressDto editedAddress)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == editedAddress.AddressId);
            if (address == null) return false;
            address.StateId = editedAddress.StateId;
            address.UserId = editedAddress.UserId;
            address.CityId = editedAddress.CityId;
            address.CountryId = editedAddress.CountryId;

           await _context.SaveChangesAsync();

            return true;

        }
    }
}
