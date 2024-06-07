using Address_Book.Models;
using Address_Book.Repositories.Interfaces;
using Address_Book.Services.Interfaces;

namespace Address_Book.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task AddAddress(Address address)
        {
            if(address != null)
            {
                if (GetLastAddress() == null)
                    address.Id = 1;

                var lastAddress = await GetLastAddress(); 
                var lastId = lastAddress.Id;
                address.Id = lastId + 1;

                await _addressRepository.AddAddress(address);
            }
        }

        public async Task DeleteAddress(int id)
        {
            var addresses = await GetAllAddresses();
            var addressToDelete = addresses.Find(x => x.Id == id);

            if (addressToDelete != null)
                await _addressRepository.DeleteAddress(addressToDelete);
        }

        public async Task UpdateAddress(int id, Address newAddress)
        {
            if (newAddress != null)
            {
                await _addressRepository.UpdateAddress(id, newAddress);
            }
        }

        public async Task<List<Address>> GetAddressesByCity(string city)
        {
            var addresses = await _addressRepository.GetAllAddresses();
            var result = addresses.
                Where(x => x.City.ToLower() == city.ToLower()).
                ToList();

            return result;
        }

        public async Task<Address> GetLastAddress()
        {
            var addresses = await _addressRepository.GetAllAddresses();
            return addresses.LastOrDefault();
        }

        public async Task<List<Address>> GetAllAddresses() 
        {
            var addresses = await _addressRepository.GetAllAddresses();
            return addresses;
        }
    }
}
