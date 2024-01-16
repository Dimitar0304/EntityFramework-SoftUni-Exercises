using EventMe.Core.Constants;
using EventMe.Core.Contracts;
using EventMe.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EventMe.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService service;
        private readonly ILogger logger;
        public EventController(IEventService _service, ILogger<EventController> _logger)
        {
            service = _service;
            logger = _logger;
        }
        public async Task<IActionResult> Index()
        {
            var model = await service.GetAllAsync();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new EventModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(EventModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await service.Create(model);
                    return RedirectToAction(nameof(Index));
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);

                }
                catch (Exception ec)
                {
                    logger.LogError(ec, "Error creating event");
                    ModelState.AddModelError(string.Empty, UserMessageConstants.UnknownCommand);
                }

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = service.GetByIdAsync(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EventModel model)
        {
            try
            {
                await service.Edit(model);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ec)
            {
                ModelState.AddModelError(string.Empty, ec.Message);
                
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error editing event");
                ModelState.AddModelError(string.Empty, UserMessageConstants.UnknownCommand);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await service.Delete(id);
            }
            catch (Exception e)
            {
                logger.LogError(string.Empty, e.Message);
                
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
          var model = await service.GetByIdAsync(id);
            return View(model);
        }


    }
}
