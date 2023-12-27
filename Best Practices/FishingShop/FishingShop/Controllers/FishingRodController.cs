using FishingShop.Core.Contracts;
using FishingShop.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FishingShop.Controllers
{
    public class FishingRodController : Controller
    {
        private IFishingRodService fishingRodService;
        public FishingRodController(IFishingRodService fishingRodService)
        {
            this.fishingRodService = fishingRodService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task< IActionResult> Create()
        {
            var model =new FishingRodModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Core.Models.FishingRodModel model)
        {
            if (ModelState.IsValid)
            {
                await fishingRodService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
