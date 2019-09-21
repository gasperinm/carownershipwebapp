using CarOwnershipWebApp.Models;
using CarOwnershipWebApp.Models.Block;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Services
{
    public class BlockchainService : IBlockchainService
    {
        private readonly IOutcallsService _outcallsService;
        private readonly EndpointSettings _endpointSettings;

        public BlockchainService(IOutcallsService outcallsService, IOptions<EndpointSettings> options)
        {
            _outcallsService = outcallsService;
            _endpointSettings = options.Value;
        }

        public async Task<bool> AddCar(CarData carData)
        {
            string data = JsonConvert.SerializeObject(carData);

            var resp = await _outcallsService.Post<EmptyResp>(_endpointSettings.BlockchainApiUri + "addblock", data);

            if (resp == null)
            {
                return false;
            }

            return true;
        }

        public async Task<List<CarData>> GetListOfCars()
        {
            var blockchain = await _outcallsService.Get<List<Block>>(_endpointSettings.BlockchainApiUri + "getblockchain");

            List<CarData> list = new List<CarData>();

            foreach (var block in blockchain)
            {
                if (block.Index != 0)
                {
                    CarData carData = JsonConvert.DeserializeObject<CarData>(block.Data);
                    list.Add(carData);
                }
            }

            return list;
        }

        public async Task<List<Block>> GetBlockchain()
        {
            var blockchain = await _outcallsService.Get<List<Block>>(_endpointSettings.TestBlockchainApiUri + "testgetblockchain");

            return blockchain;
        }

        public async Task<List<Block>> TestBlockchainValid()
        {
            var resp = await _outcallsService.Get<List<Block>>(_endpointSettings.TestBlockchainApiUri + "testblockchainvalid");

            return resp;
        }

        public async Task<EmptyResp> TestChangeData(int index, string newData)
        {
            try
            {
                var resp = await _outcallsService.Get<EmptyResp>(_endpointSettings.TestBlockchainApiUri + "testchangedata?index=" + index + "&newData=" + newData);

                return resp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<EmptyResp> TestAddBlock(string newData)
        {
            var resp = await _outcallsService.Get<EmptyResp>(_endpointSettings.TestBlockchainApiUri + "testaddblock?data=" + newData);

            return resp;
        }

        public async Task<EmptyResp> TestMineBlock(int index)
        {
            var resp = await _outcallsService.Get<EmptyResp>(_endpointSettings.TestBlockchainApiUri + "testmineblock2?index=" + index);

            return resp;
        }

        public async Task<List<CarData>> GetListOfRecordsForVin(string vin)
        {
            //return new CarData
            //{
            //    Vin = "KNEBA24424T037196",
            //    Owners = "3",
            //    Date = "2004-08-09", //yyyy-MM-dd
            //    Registration = "4580929",
            //    License = "MBDH-592",
            //    VehicleName = "KIA PICANTO"
            //};

            var blockchain = await _outcallsService.Get<List<Block>>(_endpointSettings.BlockchainApiUri + "getblockchain");

            CarData carData = null;
            List<CarData> listOfCarData = new List<CarData>();

            foreach (var block in blockchain)
            {
                if (block.Index != 0)
                {
                    carData = JsonConvert.DeserializeObject<CarData>(block.Data);
                    if (carData.Vin == vin)
                    {
                        listOfCarData.Add(carData);
                    }
                    carData = null;
                }
            }

            return listOfCarData;
        }
    }
}
