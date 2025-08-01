namespace BoraEncontros.Accounts.Repositories;
public interface ICalendarTokenRepository : IRepository
{
    Task SaveAsync(string key, string json);
    Task<string?> GetAsync(string key);
    Task DeleteAsync(string key);
    Task ClearAsync();
}
