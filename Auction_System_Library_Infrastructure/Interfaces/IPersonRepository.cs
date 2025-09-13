using Auction_System_Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
