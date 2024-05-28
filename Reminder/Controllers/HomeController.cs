using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reminder.Models;
using Reminder.Services;
using System.Diagnostics;

namespace Reminder.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Private Variables.
        #region Dependency.
        private readonly ILogger<HomeController> _logger;
        private readonly IReminderRepository _reminderRepository;
        private readonly IEmailService _emailService;
        #endregion
        #endregion

        #region Constructor.
        public HomeController(ILogger<HomeController> logger, IReminderRepository reminderRepository, IEmailService emailService)
        {
            _logger = logger;
            _reminderRepository = reminderRepository;
            _emailService = emailService;
        }
        #endregion

        #region Public Methods.
        public async Task<IActionResult> Index()
        {
            try
            {
                var reminders = await _reminderRepository.GetAllRemindersAsync();
                return View(reminders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return RedirectToAction("Error", "Home", null);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModelReminder reminder)
        {
            if(ModelState.IsValid)
            {
                await _reminderRepository.AddReminderAsync(reminder);
                return RedirectToAction(nameof(Index));
            }
            return View(reminder);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var reminder = await _reminderRepository.GetReminderByIdAsync(id);
            if(reminder == null)
            {
                return NotFound();
            }
            return View(reminder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ModelReminder reminder)
        {
            if(id != reminder.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await _reminderRepository.UpdateReminderAsync(reminder);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "");
                    return View(reminder);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(reminder);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reminder = await _reminderRepository.GetReminderByIdAsync(id);
            if (reminder == null)
            {
                return NotFound();
            }

            return View(reminder);
        }

        // Action to handle POST request for deleting reminder
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _reminderRepository.DeleteReminderAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                return RedirectToAction("Error","Home",null);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
