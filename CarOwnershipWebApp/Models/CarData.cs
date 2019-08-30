using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Models
{
    public class CarData
    {
        public string Vin { get; set; }
        public string Owners { get; set; } //number of owners
        public string Date { get; set; }  //date of first registration
        public string Registration { get; set; }
        public string License { get; set; }
        public string VehicleName { get; set; }
    }
}
