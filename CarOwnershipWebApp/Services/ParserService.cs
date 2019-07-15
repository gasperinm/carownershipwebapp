using CarOwnershipWebApp.Models;
using CarOwnershipWebApp.Models.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Services
{
    public class ParserService : IParserService
    {
        public ParserService()
        {
            
        }

        public async Task<GetCarDataResp> GetCarData(string registration, string licensePlate)
        {
            return new GetCarDataResp
            {
                Success = true,
                Data = new CarData
                {
                    Vin = "KNEBA24424T037196",
                    Owners = "3",
                    Date = "2004-08-09", //yyyy-MM-dd
                    Registration = "4580929",
                    License = "MBDH-592"
                }
            };

            //string uri = eUpravaSettings.RequestUri;
            //uri = uri.Replace("{mgnlModelExecutionUUID}", eUpravaSettings.MgnlModelExecutionUUID);
            //uri = uri.Replace("{prometna_tip}", eUpravaSettings.LicenseType);
            //uri = uri.Replace("{prometna}", getCarDataReq.Registration);
            //uri = uri.Replace("{registrska}", getCarDataReq.License);

            //var httpClient = new HttpClient();
            //var response = await httpClient.PostAsync(uri, new StringContent(""));

            //if (!response.IsSuccessStatusCode)
            //{
            //    var errContent = await response.Content.ReadAsStringAsync();
            //    string errMessage = RespMessages.SomethingWrong.Replace("{message}", errContent);

            //    return new GetCarDataResp
            //    {
            //        Success = false,
            //        Message = errMessage
            //    };
            //}

            //var content = await response.Content.ReadAsStringAsync();

            //HtmlDocument eUprava = new HtmlDocument();
            //eUprava.LoadHtml(content);

            //VehicleData vehicleData = GetVehicleData(eUprava);
            //RegistrationData registrationData = GetRegistrationData(eUprava);

            //return new GetCarDataResp
            //{
            //    Success = true,
            //    Data = new CarData
            //    {
            //        Vin = vehicleData.Vin,
            //        Owners = registrationData.NumberOfDifferentOwners,
            //        Date = registrationData.FirstRegistrationDate,
            //        Registration = getCarDataReq.Registration,
            //        License = getCarDataReq.License
            //    }
            //};
        }
    }
}
