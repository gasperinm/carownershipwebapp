using CarOwnershipWebApp.Models;
using CarOwnershipWebApp.Models.Block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Services
{
    public interface IBlockchainService
    {
        Task<bool> AddCar(CarData carData);
        Task<List<CarData>> GetListOfRecordsForVin(string vin);
        Task<List<CarData>> GetListOfCars();

        Task<List<Block>> GetBlockchain();
        Task<List<Block>> TestBlockchainValid();
        Task<EmptyResp> TestChangeData(int index, string newData);
        Task<EmptyResp> TestAddBlock(string newData);
        Task<EmptyResp> TestMineBlock(int index);
    }
}
