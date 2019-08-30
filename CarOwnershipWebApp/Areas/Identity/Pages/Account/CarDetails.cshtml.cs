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
    public class CarDetailsModel : PageModel
    {
        public void OnGet(CarData carData)
        {
            ViewData["Title"] = "Details for " + carData.VehicleName;

            ViewData["Vin"] = carData.Vin;
            ViewData["Registration"] = carData.Registration;
            ViewData["License"] = carData.License;
            ViewData["Owners"] = carData.Owners;
            ViewData["Date"] = carData.Date;
        }
    }
}