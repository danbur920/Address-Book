using Address_Book.Models;
using Address_Book.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Address_Book.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressBookController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly ILogger<AddressBookController> _logger;

        public AddressBookController(IAddressService addressService, ILogger<AddressBookController> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        // Dodawanie nowego adresu:

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] Address address)
        {
            try
            {
                var lengthBeforeAdd = (await _addressService.GetAllAddresses()).Count();
                await _addressService.AddAddress(address);
                var lengthAfterAdd = (await _addressService.GetAllAddresses()).Count();

                if (lengthAfterAdd > lengthBeforeAdd)
                {
                    _logger.LogInformation($"Successful POST request (AddAddress): Street: {address.Street}, HouseNumber: {address.HouseNumber}," +
                        $" City: {address.City}, Postal code: {address.PostalCode}, Country: {address.Country}, Voivodeship: {address.Voivodeship}, State: {address.State}");
                    return Ok();
                }
                else
                {
                    _logger.LogWarning("Adding a new address failed.");
                    return BadRequest("Adding a new address failed.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a new address.");
                return StatusCode(500, new { Message = "Error while adding a new address." });

            }
        }

        // Usuwanie adresu o wskazanym ID:

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                var addresses = await _addressService.GetAllAddresses();
                var addressToDelete = addresses.Find(x => x.Id == id);

                if (addressToDelete != null)
                {
                    await _addressService.DeleteAddress(id);
                    _logger.LogInformation($"Successful DELETE request (DeleteAddress): ID: {id}");
                    return Ok(new { Message = "Address deleted successfully." });
                }
                else
                {
                    _logger.LogWarning($"Address not found for DELETE request (DeleteAddress): ID: {id}");
                    return NotFound(new { Message = "Address not found." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting address with ID: {id}");
                return StatusCode(500, new { Message = "Error while deleting address." });
            }
        }

        // Aktualizacja adresu o wskazanym ID:

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] Address address)
        {
            try
            {
                await _addressService.UpdateAddress(id, address);
                _logger.LogInformation($"Address updated successfully: ID: {id}");
                return Ok(new { Message = "Address updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while updating address: ID: {id}");
                return StatusCode(500, new { Message = "Error while updating address." })
;            }
        }

        // Zwracanie ostatnio dodanego adresu:

        [HttpGet]
        public async Task<IActionResult> GetLastAddress()
        {
            try
            {
                var lastAddress = await _addressService.GetLastAddress();

                if (lastAddress != null)
                {
                    _logger.LogInformation($"Successful GET request (GetLastAddress): Street: {lastAddress.Street}, HouseNumber: {lastAddress.HouseNumber}, " +
                        $"City: {lastAddress.City}, Postal code: {lastAddress.PostalCode}, Country: {lastAddress.Country}, Voivodeship: {lastAddress.Voivodeship}, State: {lastAddress.State}");
                    return Ok(lastAddress);
                }
                else
                {
                    _logger.LogWarning("The recently added address was not found.");
                    return BadRequest("The recently added address was not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching last address.");
                return BadRequest("Error while fetching last address.");
            }
        }

        // Zwracanie wszystkich adresów z podanym miastem:

        [HttpGet("{city}")]
        public async Task<IActionResult> GetAddressesByCity(string city)
        {
            try
            {
                var cities = await _addressService.GetAddressesByCity(city);
                if (cities != null)
                {
                    _logger.LogInformation($"Successful GET request (GetAddressesByCity): Number of addresses from {city}: {cities.Count}");
                    return Ok(cities);
                }
                else
                {
                    _logger.LogWarning($"There are no addresses with a given city: {city}");
                    return BadRequest("There are no addresses with a given city.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while fetching addresses for city: {city}");
                return StatusCode(500, new { Message = "Error while fetching addresses for city." });
            }

        }
    }
}
