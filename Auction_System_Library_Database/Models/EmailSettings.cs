using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction_System_Library_Database.Models
{
    public class EmailSettings
    {
        public string FromEmail { get; set; }   
        public string AppPassword { get; set; } 
        public string SmtpHost {  get; set; }
        public int SmtpPort { get; set; }
    }
}
