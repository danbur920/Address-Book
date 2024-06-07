using Address_Book.Models;
using Address_Book.Repositories;
using System.Text.Json;

namespace Tests
{
    public class UnitTest1
    {
        // Test sprawdza czy adres zostal usuniêty oraz czy poprawnie pobiera siê lista adresów
        [Fact]
        public async Task Test1()
        {
            var filePath = Path.Combine("Data", "addresses.json");
            var repository = new AddressRepository();

            var addressesList = await repository.GetAllAddresses();
            var addressToDelete = addressesList.Find(x => x.Id == 2); // wybraæ istniej¹ce ID

            await repository.DeleteAddress(addressToDelete);

            Assert.True(File.Exists(filePath));

            var json = File.ReadAllText(filePath);
            var addresses = JsonSerializer.Deserialize<List<Address>>(json);

            Assert.NotNull(addressToDelete);
            Assert.NotNull(addresses);
            Assert.DoesNotContain(addressToDelete, addresses);
        }
    }
}