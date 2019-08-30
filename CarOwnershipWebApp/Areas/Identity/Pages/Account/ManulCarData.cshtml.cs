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
    public class ManulCarDataModel : PageModel
    {
        private readonly IBlockchainService _blockchainService;

        public ManulCarDataModel(IBlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string Registration { get; set; }
        public string LicensePlate { get; set; }

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
            Registration = carData.Registration;
            LicensePlate = carData.License;
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("./SaveCarData", new CarData
            {
                Registration = Input.Registration,
                License = Input.LicensePlate,
                Vin = Input.Vin,
                Date = Input.Date,
                Owners = Input.Owners
            });
        }
    }
}