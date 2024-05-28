
using ReminderModal.Services;

namespace Reminder.Services
{
    public class ReminderNotificationService : BackgroundService
    {
        #region Private Variables.
        #region Dependency
        private readonly IServiceProvider _serviceProvider;
        #endregion
        #endregion

        #region Constructors.
        public ReminderNotificationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var reminderRepository = scope.ServiceProvider.GetRequiredService<IReminderRepository>();
                    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                    // Check for reminders due
                    var remindersDue = await reminderRepository.GetRemindersDueAsync(DateTime.Now);

                    foreach (var reminder in remindersDue)
                    {
                        var messageBody = $"<h1>Reminder: {reminder.Title}</h1>" +
                                      $"<p>This is a reminder for the event you scheduled.</p>" +
                                      $"<p><strong>Title:</strong> {reminder.Title}</p>" +
                                      $"<p><strong>Scheduled Time:</strong> {reminder.DateTime}</p>";

                        await emailService.SendEmailAsync(reminder.Email, "Reminder: " + reminder.Title, messageBody);
                        // Optionally, mark the reminder as sent to prevent duplicate notifications
                        reminder.IsSent = true;
                        await reminderRepository.UpdateReminderAsync(reminder);
                    }

                    // Adjust the interval according to your needs
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

                }
            }
        }
    }
}
