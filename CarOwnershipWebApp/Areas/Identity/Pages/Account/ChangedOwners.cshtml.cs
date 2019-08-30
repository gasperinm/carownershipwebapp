using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarOwnershipWebApp.Models;
using CarOwnershipWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarOwnershipWebApp.Areas.Identity.Pages.Account
{
    public class ChangedOwnersModel : PageModel
    {
        private readonly IBlockchainService _blockchainService;

        public ChangedOwnersModel(IBlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
        }

        public string Registration { get; set; }
        public string LicensePlate { get; set; }
        public string Date { get; set; }
        public string Owners { get; set; }
        public string Vin { get; set; }
        public string VehicleName { get; set; }

        public void OnGet(CarData carData)
        {
            ViewData["Title"] = "Vehicle " + carData.VehicleName + " is changing owners.";

            Registration = carData.Registration;
            LicensePlate = carData.License;
            Date = carData.Date;
            Owners = carData.Owners;
            Vin = carData.Vin;
            VehicleName = carData.VehicleName;
        }

        public async Task<IActionResult> OnConfirmChangePostAsync()
        {
            var resp = await _blockchainService.AddCar(new CarData
            {
                Registration = Registration,
                License = LicensePlate,
                Vin = Vin,
                Date = Date,
                Owners = Owners,
                VehicleName = VehicleName
            });

            if (!resp)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong. Try again.");
            }

            return Page();
        }
    }
}