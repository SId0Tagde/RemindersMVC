using Reminder.Models;
namespace Reminder.Services
{
    public interface IReminderRepository
    {
        Task<List<ModelReminder>> GetAllRemindersAsync();
        Task<ModelReminder> GetReminderByIdAsync(int id);
        Task<List<ModelReminder>> GetRemindersDueAsync(DateTime dueDate);
        Task AddReminderAsync(ModelReminder reminder);
        Task UpdateReminderAsync(ModelReminder reminder);
        Task DeleteReminderAsync(int id);
    }
}
