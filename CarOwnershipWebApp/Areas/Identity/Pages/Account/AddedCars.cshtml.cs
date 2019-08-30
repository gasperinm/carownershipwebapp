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
    public class AddedCarsModel : PageModel
    {
        private readonly IBlockchainService _blockchainService;

        public AddedCarsModel(IBlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Search by VIN number")]
        public string Vin { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetCarDetailsAsync(string vin)
        {
            var resp = await _blockchainService.GetListOfRecordsForVin(vin);

            if (resp == null)
            {
                ModelState.AddModelError(string.Empty, "Car not found.");

                return Page();
            }

            return RedirectToPage("./CarDetails", resp);
        }
    }
}