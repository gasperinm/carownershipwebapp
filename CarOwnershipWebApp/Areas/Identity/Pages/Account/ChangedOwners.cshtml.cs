using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [BindProperty]
        public InputModel Input { get; set; }

        public string Registration { get; set; }
        public string LicensePlate { get; set; }
        public string Date { get; set; }
        public string Owners { get; set; }
        public string Vin { get; set; }
        public string VehicleName { get; set; }

        public class InputModel
        {
            public string Registration { get; set; }
            public string LicensePlate { get; set; }
            public string Date { get; set; }
            public string Owners { get; set; }
            public string Vin { get; set; }
            public string VehicleName { get; set; }
        }

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

        public async Task<IActionResult> OnPostAsync()
        {
            var resp = await _blockchainService.AddCar(new CarData
            {
                Registration = Input.Registration,
                License = Input.LicensePlate,
                Vin = Input.Vin,
                Date = Input.Date,
                Owners = (int.Parse(Input.Owners) + 1).ToString(),
                VehicleName = Input.VehicleName
            });

            if (!resp)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong. Try again.");
            }

            return Page();
        }
    }
}