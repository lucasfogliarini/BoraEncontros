namespace BoraEncontros.Accounts.Repositories;
public interface ICalendarTokenRepository : IRepository
{
    void Add<TEntity>(TEntity entity) where TEntity : Entity;
    Task<CalendarToken?> GetAsync(string email);
}
