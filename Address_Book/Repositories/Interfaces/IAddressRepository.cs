using Address_Book.Models;

namespace Address_Book.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAllAddresses();
        Task AddAddress(Address address);
        Task DeleteAddress(Address address);
        Task UpdateAddress(int id, Address address);
        Task SaveAddress();
    }
}
