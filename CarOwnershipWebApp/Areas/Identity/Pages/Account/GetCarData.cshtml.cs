using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CarOwnershipWebApp.Models;
using CarOwnershipWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace CarOwnershipWebApp.Areas.Identity.Pages.Account
{
    public class GetCarDataModel : PageModel
    {
        private string carDataCacheKey = "carDataCacheKey_{registration}_{license}";

        private readonly IMemoryCache _memoryCache;
        private readonly IParserService _parserService;

        public GetCarDataModel(IMemoryCache memoryCache,
            IParserService parserService)
        {
            _memoryCache = memoryCache;
            _parserService = parserService;
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

            registration = "4580929";
            licensePlate = "MBDH-592";

            if (string.IsNullOrEmpty(registration) || string.IsNullOrEmpty(licensePlate))
            {
                return null;
                //return BadRequest(new ErrorResp
                //{
                //    Message = errMessage
                //});
            }

            carDataCacheKey = carDataCacheKey.Replace("{registration}", registration);
            carDataCacheKey = carDataCacheKey.Replace("{license}", licensePlate);

            if (_memoryCache.TryGetValue(carDataCacheKey, out carData))
            {
                if (carData == null)
                {
                    //string errMessage = RespMessages.SomethingWrong.Replace("{message}", "Try again");

                    ModelState.AddModelError(string.Empty, "Something went wrong. Try again.");

                    return Page();

                    //return BadRequest(new ErrorResp
                    //{
                    //    Message = errMessage
                    //});
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

                    //return new JsonResult(carData);
                    return RedirectToPage("./SaveCarData", carData);
                }

                ModelState.AddModelError(string.Empty, "Error while retrieving car data.");

                return Page();
            }
        }
    }
}