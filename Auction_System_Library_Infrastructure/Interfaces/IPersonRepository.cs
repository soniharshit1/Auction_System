using Auction_System_Library_Database.Models;
using Auction_System_Library_Infrastructure.DTOs;

namespace Auction_System_Library.Interfaces
{
    public interface IPersonRepository
    {
        Task<Person> RegisterPersonAsync(PersonDTO person); //ADD USERS , only admin

        Task<IEnumerable<Person>> GetAllPersonsAsync(); //only admin
        Task<Person?> UpdatePersonDetailsAsync(int id, updatedPersonDTO person); //for everyone 
        Task<string?> DeletePersonAsync(int userOrAgentId); //only admin

        Task<Person?> FindPersonbyIdAsync(int userOrAgentId); //only admin
    }
}
