namespace BoraEncontros.Accounts.Repositories;
public interface ICalendarTokenRepository : IRepository
{
    Task<CalendarToken?> GetAsync(string email);
}
