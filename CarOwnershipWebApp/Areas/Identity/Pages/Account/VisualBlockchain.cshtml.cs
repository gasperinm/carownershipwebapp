using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarOwnershipWebApp.Models.Block;
using CarOwnershipWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarOwnershipWebApp.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Admin")]
    public class VisualBlockchainModel : PageModel
    {
        private readonly IBlockchainService _blockchainService;

        public VisualBlockchainModel(IBlockchainService blockchainService)
        {
            _blockchainService = blockchainService;
        }

        public List<Block> blockchain = new List<Block>();

        public async Task<IActionResult> OnGet()
        {
            blockchain = await _blockchainService.GetBlockchain();

            return Page();
        }

        public async Task<IActionResult> OnGetChangeData(int index, string newData)
        {
            var resp = await _blockchainService.TestChangeData(index, newData);

            return new JsonResult(resp);
        }

        public async Task<IActionResult> OnGetIsBlockchainValid()
        {
            var resp = await _blockchainService.TestBlockchainValid();

            return new JsonResult(resp);
        }

        public async Task<IActionResult> OnGetAddNewBlock(string newData)
        {
            var resp = await _blockchainService.TestAddBlock(newData);

            return new JsonResult(resp);
        }

        public async Task<IActionResult> OnGetMineBlock(int index)
        {
            var resp = await _blockchainService.TestMineBlock(index);

            return new JsonResult(resp);
        }
    }
}