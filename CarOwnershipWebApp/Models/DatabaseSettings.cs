using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Models
{
    public class DatabaseSettings
    {
        public string Host { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Port { get; set; }
        public string Password { get; set; }
    }
}
