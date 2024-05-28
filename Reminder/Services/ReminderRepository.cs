using Microsoft.EntityFrameworkCore;
using Reminder.Data;
using Reminder.Models;
using Reminder.Services;

namespace ReminderModal.Services
{
    public class ReminderRepository : IReminderRepository
    {
        #region Provate Variables.
        #region Dependency
        private readonly ReminderContext _dbContext;
        #endregion
        #endregion

        #region Constructor.
        public ReminderRepository(ReminderContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Public Methods.
        public async Task<List<ModelReminder>> GetAllRemindersAsync()
        {
            return await _dbContext.Reminders.ToListAsync();
        }

        public async Task<ModelReminder> GetReminderByIdAsync(int id)
        {
            return await _dbContext.Reminders.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<ModelReminder>> GetRemindersDueAsync(DateTime dueDate)
        {
            return await _dbContext.Reminders.Where(r => r.DateTime <= dueDate && !r.IsSent).ToListAsync();
        }

        public async Task AddReminderAsync(ModelReminder ReminderModal)
        {
            _dbContext.Reminders.Add(ReminderModal);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateReminderAsync(ModelReminder ReminderModal)
        {
            _dbContext.Reminders.Update(ReminderModal);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteReminderAsync(int id)
        {
            var ReminderModalToDelete = await _dbContext.Reminders.FindAsync(id);
            if (ReminderModalToDelete != null)
            {
                _dbContext.Reminders.Remove(ReminderModalToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
    #endregion
}