using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Infrastructure.Interfaces
{
    public interface IEmailService
    {
        Task SendSimpleEmailAsync(string toEmail, string subject, string body);
    }
}
