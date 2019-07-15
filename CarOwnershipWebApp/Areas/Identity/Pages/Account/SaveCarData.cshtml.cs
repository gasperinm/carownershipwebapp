using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CarOwnershipWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarOwnershipWebApp.Areas.Identity.Pages.Account
{
    public class SaveCarDataModel : PageModel
    {
        public InputModel Input { get; set; }

        public string Registration { get; set; }
        public string LicensePlate { get; set; }
        public string Vin { get; set; }
        public string Date { get; set; }
        public string Owners { get; set; }

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
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.AddModelError(string.Empty, "Not implemented yet.");

            return Page();
        }
    }
}