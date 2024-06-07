using Address_Book.Models;

namespace Address_Book.Services.Interfaces
{
    public interface IAddressService
    {
        Task<Address> GetLastAddress();
        Task<List<Address>> GetAddressesByCity(string city);
        Task AddAddress(Address address);
        Task DeleteAddress(int id);
        Task UpdateAddress(int id, Address address);
        Task<List<Address>> GetAllAddresses();
    }
}