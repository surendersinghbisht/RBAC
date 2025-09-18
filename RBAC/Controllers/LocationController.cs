using Azure;
using Data.Entity;
using Data.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Model.Dto;
using Service.Contract;

namespace RBAC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationController(
            ILocationService locationService
            ) {
            _locationService = locationService;
        }

        [HttpGet("countries")]
        public async Task<ActionResult<CountryDto>> GetCountries()
        {
            var res = await _locationService.GetCountries();
            return Ok(res);
        }

        [HttpGet("states/{countryId}")]
        public async Task<ActionResult<StateDto>> GetStates(int countryId)
        {
            var res = await _locationService.GetStates(countryId);
            return Ok(res);
        }

        [HttpGet("cities/{stateId}")]
        public async Task<ActionResult<CityDto>> GetCities(int stateId)
        {
            var res = await _locationService.GetCities(stateId);

            return Ok(res);
        }

        [HttpPost("add-address")]
        public async Task<ActionResult<List<Address>>> AddAddress(List<AddAddressDto> model)
        {
            var address = await _locationService.AddAddress(model);
            return Ok(address);
        }


        [HttpGet("get-addresses/{userId}")]
        public async Task<ActionResult<List<AllAddresses>>> GetAllAddresses(string userId)
        {
            var addresses = await _locationService.GetAllAddresses(userId);
            return Ok(addresses);
        }

        [HttpDelete("delete-address/{addressId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var res = await _locationService.DeleteAddress(addressId);


            if (!res)
                return NotFound(new { message = "Address not found" });

            return Ok(new { message = "Address deleted successfully" });
        }

        [HttpPut("edit-address")]
        public async Task<IActionResult> EditAddress(AddAddressDto address)
        {
            if (address == null)
                return BadRequest("Address cannot be null");

            var res = await _locationService.EditAddress(address);

            if (!res)
                return NotFound("Address not found");

            return Ok(new { message = "Edited successfully" });
        }

    }
}
