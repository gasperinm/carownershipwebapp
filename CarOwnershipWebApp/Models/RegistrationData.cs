using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Models
{
    public class RegistrationData
    {
        public string FirstRegistrationDate { get; set; }
        public string FirstRegistrationCountry { get; set; }
        public string FirstRegistrationDateInSlovenia { get; set; }
        public string FirstRegistrationNumberInSlovenia { get; set; }
        public string AdministrativeUnitOfFirstRegistration { get; set; }
        public string NumberOfDifferentOwners { get; set; }
        public string NumberOfDifferentUsers { get; set; }
    }
}
