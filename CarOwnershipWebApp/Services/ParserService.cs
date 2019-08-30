using CarOwnershipWebApp.Models;
using CarOwnershipWebApp.Models.Parser;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CarOwnershipWebApp.Services
{
    public class ParserService : IParserService
    {
        private readonly EUpravaSettings _eUpravaSettings;

        public ParserService(IOptions<EUpravaSettings> options)
        {
            _eUpravaSettings = options.Value;
        }

        public async Task<GetCarDataResp> GetCarData(string registration, string licensePlate)
        {
            //return new GetCarDataResp
            //{
            //    Success = true,
            //    Data = new CarData
            //    {
            //        Vin = "KNEBA24424T037196",
            //        Owners = "3",
            //        Date = "2004-08-09", //yyyy-MM-dd
            //        Registration = "4580929",
            //        License = "MBDH-592",
            //        VehicleName = "KIA PICANTO"
            //    }
            //};

            string uri = _eUpravaSettings.RequestUri;
            uri = uri.Replace("{mgnlModelExecutionUUID}", _eUpravaSettings.MgnlModelExecutionUUID);
            uri = uri.Replace("{prometna_tip}", _eUpravaSettings.LicenseType);
            uri = uri.Replace("{prometna}", registration);
            uri = uri.Replace("{registrska}", licensePlate);

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(uri, new StringContent(""));

            if (!response.IsSuccessStatusCode)
            {
                var errContent = await response.Content.ReadAsStringAsync();
                //string errMessage = RespMessages.SomethingWrong.Replace("{message}", errContent);
                string errMessage = $"Something went wrong. {errContent}";

                return new GetCarDataResp
                {
                    Success = false,
                    Message = errMessage
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            HtmlDocument eUprava = new HtmlDocument();
            eUprava.LoadHtml(content);

            string dataNotFound = string.Empty;
            try
            {
                dataNotFound = eUprava.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/h2").InnerText.Trim();
            }
            catch
            {

            }

            if (!string.IsNullOrEmpty(dataNotFound))
            {
                return new GetCarDataResp
                {
                    Success = false,
                    Message = "Car data was not found"
                };
            }

            VehicleData vehicleData = GetVehicleData(eUprava);
            RegistrationData registrationData = GetRegistrationData(eUprava);

            string[] date = registrationData.FirstRegistrationDate.Split(".");
            int year = int.Parse(date[2].Trim());
            int month = int.Parse(date[1].Trim());
            int day = int.Parse(date[0].Trim());

            DateTime dt = new DateTime(year, month, day);

            return new GetCarDataResp
            {
                Success = true,
                Data = new CarData
                {
                    Vin = vehicleData.Vin,
                    Owners = registrationData.NumberOfDifferentOwners,
                    Date = dt.ToString("yyyy-MM-dd"),
                    Registration = registration,
                    License = licensePlate,
                    VehicleName = vehicleData.Make + " " + vehicleData.CommercialSign
                }
            };
        }

        private VehicleData GetVehicleData(HtmlDocument htmlDocument)
        {
            VehicleData vehicleData = new VehicleData();

            string status = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[1]/div[2]").InnerText.Trim();
            vehicleData.Status = status;

            string make = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[2]/div[2]").InnerText.Trim();
            vehicleData.Make = make;

            string factorySign = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[3]/div[2]").InnerText.Trim();
            vehicleData.FactorySign = factorySign;

            string commercialSign = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[4]/div[2]").InnerText.Trim();
            vehicleData.CommercialSign = commercialSign;

            string manufacturer = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[5]/div[2]").InnerText.Trim();
            vehicleData.Manufacturer = manufacturer;

            string dateOfCocDocument = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[6]/div[2]").InnerText.Trim();
            vehicleData.DateOfCocDocument = dateOfCocDocument;

            string vin = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[7]/div[2]").InnerText.Trim();
            vehicleData.Vin = vin;

            string homologationSign = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[8]/div[2]").InnerText.Trim();
            vehicleData.HomologationSign = homologationSign;

            string category = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[9]/div[2]").InnerText.Trim();
            vehicleData.Category = category;

            string upgrade = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[10]/div[2]").InnerText.Trim();
            vehicleData.Upgrade = upgrade;

            string length = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[11]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.Length = double.Parse(length);
            }
            catch
            {
                vehicleData.Length = 0;
            }

            string width = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[12]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.Width = double.Parse(width);
            }
            catch
            {
                vehicleData.Width = 0;
            }

            string height = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[13]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.Height = double.Parse(height);
            }
            catch
            {
                vehicleData.Height = 0;
            }

            string numberOfAxles = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[14]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.NumberOfAxles = int.Parse(numberOfAxles);
            }
            catch
            {
                vehicleData.NumberOfAxles = 0;
            }

            string betweenAxles = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[15]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.BetweenAxles = double.Parse(betweenAxles);
            }
            catch
            {
                vehicleData.BetweenAxles = 0;
            }

            string rearOverhang = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[16]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.RearOverhang = double.Parse(rearOverhang);
            }
            catch
            {
                vehicleData.RearOverhang = 0;
            }

            string maximumTechnicallAllowedMass = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[17]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.MaximumTechnicallAllowedMass = double.Parse(maximumTechnicallAllowedMass);
            }
            catch
            {
                vehicleData.MaximumTechnicallAllowedMass = 0;
            }

            string mass = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[18]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.Mass = double.Parse(mass);
            }
            catch
            {
                vehicleData.Mass = 0;
            }

            string allowedAxleTensions = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[19]/div[2]").InnerText.Trim();
            vehicleData.AllowedAxleTensions = allowedAxleTensions;

            string maximumAllowedTrailerWithSingleAxisMass = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[20]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.MaximumAllowedTrailerWithSingleAxisMass = double.Parse(maximumAllowedTrailerWithSingleAxisMass);
            }
            catch
            {
                vehicleData.MaximumAllowedTrailerWithSingleAxisMass = 0;
            }

            string maximumAllowedNonBrakingTrailerMass = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[21]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.MaximumAllowedNonBrakingTrailerMass = double.Parse(maximumAllowedNonBrakingTrailerMass);
            }
            catch
            {
                vehicleData.MaximumAllowedNonBrakingTrailerMass = 0;
            }

            string verticalTensionOfTrailer = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[22]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.VerticalTensionOfTrailer = double.Parse(verticalTensionOfTrailer);
            }
            catch
            {
                vehicleData.VerticalTensionOfTrailer = 0;
            }

            string workingVolume = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[23]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.WorkingVolume = double.Parse(workingVolume);
            }
            catch
            {
                vehicleData.WorkingVolume = 0;
            }

            string maximumNetPower = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[24]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.MaximumNetPower = double.Parse(maximumNetPower);
            }
            catch
            {
                vehicleData.MaximumNetPower = 0;
            }

            string fuelType = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[25]/div[2]").InnerText.Trim();
            vehicleData.FuelType = fuelType;

            string specifiedSpinningFrequency = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[26]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.SpecifiedSpinningFrequency = int.Parse(specifiedSpinningFrequency);
            }
            catch
            {
                vehicleData.SpecifiedSpinningFrequency = 0;
            }

            string engineSign = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[27]/div[2]").InnerText.Trim();
            vehicleData.EngineSign = engineSign;

            string color = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[28]/div[2]").InnerText.Trim();
            vehicleData.Color = color;

            string numberOfSeats = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[29]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.NumberOfSeats = int.Parse(numberOfSeats);
            }
            catch
            {
                vehicleData.NumberOfSeats = 0;
            }

            string maximumSpeed = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[30]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.MaximumSpeed = double.Parse(maximumSpeed);
            }
            catch
            {
                vehicleData.MaximumSpeed = 0;
            }

            string noiseWhileStill = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[31]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.NoiseWhileStill = double.Parse(noiseWhileStill);
            }
            catch
            {
                vehicleData.NoiseWhileStill = 0;
            }

            string spinningFrequencyOfEngineInNeutral = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[32]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.SpinningFrequencyOfEngineInNeutral = int.Parse(spinningFrequencyOfEngineInNeutral);
            }
            catch
            {
                vehicleData.SpinningFrequencyOfEngineInNeutral = 0;
            }

            string noiseWhileDriving = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[33]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.NoiseWhileDriving = double.Parse(noiseWhileDriving);
            }
            catch
            {
                vehicleData.NoiseWhileDriving = 0;
            }

            string cO = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[34]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.CO = double.Parse(cO);
            }
            catch
            {
                vehicleData.CO = 0;
            }

            string hC = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[35]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.HC = double.Parse(hC);
            }
            catch
            {
                vehicleData.HC = 0;
            }

            string nAX = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[36]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.NAX = double.Parse(nAX);
            }
            catch
            {
                vehicleData.NAX = 0;
            }

            string cO2Emissions = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[37]/div[2]").InnerText.Trim();
            try
            {
                vehicleData.CO2Emissions = double.Parse(cO2Emissions);
            }
            catch
            {
                vehicleData.CO2Emissions = 0;
            }

            string ecologySign = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[38]/div[2]").InnerText.Trim();
            vehicleData.EcologySign = ecologySign;

            string allowedTiresAndRims = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[39]/div[2]").InnerText.Trim();
            vehicleData.AllowedTiresAndRims = allowedTiresAndRims;

            return vehicleData;
        }

        private RegistrationData GetRegistrationData(HtmlDocument htmlDocument)
        {
            RegistrationData registrationData = new RegistrationData();

            string firstRegistrationDate = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[46]/div[2]").InnerText.Trim();
            registrationData.FirstRegistrationDate = firstRegistrationDate;

            string firstRegistrationCountry = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[47]/div[2]").InnerText.Trim();
            registrationData.FirstRegistrationCountry = firstRegistrationCountry;

            string firstRegistrationDateInSlovenia = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[48]/div[2]").InnerText.Trim();
            registrationData.FirstRegistrationDateInSlovenia = firstRegistrationDateInSlovenia;

            string firstRegistrationNumberInSlovenia = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[49]/div[2]").InnerText.Trim();
            registrationData.FirstRegistrationNumberInSlovenia = firstRegistrationNumberInSlovenia;

            string administrativeUnitOfFirstRegistration = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[50]/div[2]").InnerText.Trim();
            registrationData.AdministrativeUnitOfFirstRegistration = administrativeUnitOfFirstRegistration;

            string numberOfDifferentOwners = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[51]/div[2]").InnerText.Trim();
            registrationData.NumberOfDifferentOwners = numberOfDifferentOwners;

            string numberOfDifferentUsers = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='main']/div[1]/div[3]/div[3]/div[52]/div[2]").InnerText.Trim();
            registrationData.NumberOfDifferentUsers = numberOfDifferentUsers;

            return registrationData;
        }
    }
}
