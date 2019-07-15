using CarOwnershipWebApp.Models.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Services
{
    public interface IParserService
    {
        Task<GetCarDataResp> GetCarData(string registration, string licensePlate);
    }
}
