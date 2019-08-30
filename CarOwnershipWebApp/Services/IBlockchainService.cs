using CarOwnershipWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Services
{
    public interface IBlockchainService
    {
        Task<bool> AddCar(CarData carData);
        Task<CarData> GetListOfRecordsForVin(string vin);
        Task<List<CarData>> GetListOfCars();
    }
}
