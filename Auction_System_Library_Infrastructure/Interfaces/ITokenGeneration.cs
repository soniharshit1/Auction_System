using Auction_System_Library_Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface ITokenGeneration
    {
        public string GenerateJWT(Person user);
    }
}
