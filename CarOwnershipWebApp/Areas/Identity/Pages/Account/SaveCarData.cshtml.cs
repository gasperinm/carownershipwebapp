using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CarOwnershipWebApp.Models;
using CarOwnershipWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarOwnershipWebApp.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class SaveCarDataModel : PageModel
    {
        private readonly IBlockchainService _blockchainService;

        public SaveCarDataModel(IBlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Registration { get; set; }
        public string LicensePlate { get; set; }
        public string Vin { get; set; }
        public string Date { get; set; }
        public string Owners { get; set; }
        public string VehicleName { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Registration")]
            public string Registration { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "License plate")]
            public string LicensePlate { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [Display(Name = "Date")]
            public string Date { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Owners")]
            public string Owners { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Vin")]
            public string Vin { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Vehicle")]
            public string VehicleName { get; set; }
        }

        public void OnGet(CarData carData)
        {
            if (carData != null)
            {
                Registration = carData.Registration;
                LicensePlate = carData.License;
                Vin = carData.Vin;
                Date = carData.Date;
                Owners = carData.Owners;
                VehicleName = carData.VehicleName;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var resp = await _blockchainService.AddCar(new CarData
            {
                Registration = Input.Registration,
                License = Input.LicensePlate,
                Vin = Input.Vin,
                Date = Input.Date,
                Owners = Input.Owners,
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