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
using Microsoft.Extensions.Caching.Memory;

namespace CarOwnershipWebApp.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class GetCarDataModel : PageModel
    {
        private string carDataCacheKey = "carDataCacheKey_{registration}_{license}";

        private readonly IMemoryCache _memoryCache;
        private readonly IParserService _parserService;
        private readonly IOutcallsService _outcallsService;
        private readonly IBlockchainService _blockchainService;

        public GetCarDataModel(IMemoryCache memoryCache,
            IParserService parserService,
            IOutcallsService outcallsService,
            IBlockchainService blockchainService)
        {
            _memoryCache = memoryCache;
            _parserService = parserService;
            _outcallsService = outcallsService;
            _blockchainService = blockchainService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Registration")]
        public string Registration { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "License plate")]
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
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetCarDataAsync(string registration, string licensePlate)
        {
            CarData carData = new CarData();

            if (string.IsNullOrEmpty(registration) || string.IsNullOrEmpty(licensePlate))
            {
                ModelState.AddModelError(string.Empty, "Empty fields.");

                return Page();
            }

            carDataCacheKey = carDataCacheKey.Replace("{registration}", registration);
            carDataCacheKey = carDataCacheKey.Replace("{license}", licensePlate);

            if (_memoryCache.TryGetValue(carDataCacheKey, out carData))
            {
                if (carData == null)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Try again.");

                    return Page();
                }

                var alreadyAdded = await _blockchainService.GetListOfRecordsForVin(carData.Vin);

                if (alreadyAdded != null && alreadyAdded.Count >= 1)
                {
                    return RedirectToPage("./ChangedOwners", carData);
                }

                return RedirectToPage("./SaveCarData", carData);
            }

            else
            {
                _memoryCache.Remove(carDataCacheKey);

                var resp = await _parserService.GetCarData(registration, licensePlate);
                if (resp.Success)
                {
                    carData = resp.Data;

                    _memoryCache.Set(carDataCacheKey, carData, TimeSpan.FromMinutes(60));

                    var alreadyAdded = await _blockchainService.GetListOfRecordsForVin(carData.Vin);

                    if (alreadyAdded != null && alreadyAdded.Count >= 1)
                    {
                        return RedirectToPage("./ChangedOwners", carData);
                    }

                    return RedirectToPage("./SaveCarData", carData);
                }

                if (!resp.Success && resp.Message == "Car data was not found")
                {
                    carData = new CarData();
                    carData.Registration = registration;
                    carData.License = licensePlate;
                    return RedirectToPage("./ManulCarData", carData);
                }

                ModelState.AddModelError(string.Empty, "Error while retrieving car data.");

                return Page();
            }
        }
    }
}