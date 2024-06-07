using Address_Book.Models;
using Address_Book.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO;
using System.Text.Json;

namespace Address_Book.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private static readonly string FilePath = Path.Combine("Data", "addresses.json");
        private List<Address> _addresses;

        public AddressRepository()
        {
            _addresses = LoadAddresses();
        }

        public async Task AddAddress(Address address)
        {
            _addresses.Add(address);
            await SaveAddress();
        }

        public async Task DeleteAddress(Address address)
        {
            if (address != null)
            {
                var addresses = await GetAllAddresses();
                addresses.Remove(address);
                await SaveAddress();
            }
        }

        public async Task UpdateAddress(int id, Address address)
        {
            if (address != null)
            {
                var existingAddress = _addresses.Find(x => x.Id == id);
                if (existingAddress != null)
                {
                    existingAddress.Street = address.Street;
                    existingAddress.City = address.City;
                    existingAddress.HouseNumber = address.HouseNumber;
                    existingAddress.PostalCode = address.PostalCode;
                    existingAddress.Country = address.Country;
                    existingAddress.Voivodeship = address.Voivodeship;
                    existingAddress.State = address.State;

                    await SaveAddress();
                }
            }
        }

        public async Task<List<Address>> GetAllAddresses()
        {
            return _addresses;
        }

        public async Task SaveAddress()
        {
            var json = JsonSerializer.Serialize(_addresses);
            await File.WriteAllTextAsync(FilePath, json);
        }

        private List<Address> LoadAddresses()
        {
            if (!File.Exists(FilePath))
                return new List<Address>();

            var json = File.ReadAllText(FilePath);
            var result = JsonSerializer.Deserialize<List<Address>>(json);

            return result;
        }
    }
}
