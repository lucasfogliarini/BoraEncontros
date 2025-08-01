using BoraEncontros;
using BoraEncontros.Accounts;
using BoraEncontros.Accounts.Repositories;
using BoraEncontros.Infrastructure;
using Google.Apis.Util.Store;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

public class CalendarTokenRepository(BoraEncontrosDbContext dbContext) : ICalendarTokenRepository, IDataStore
{
    public ICommitScope CommitScope => dbContext;

    public async Task ClearAsync()
    {
        dbContext.Set<CalendarToken>().RemoveRange(dbContext.Set<CalendarToken>());
        await CommitScope.CommitAsync();
    }

    public async Task StoreAsync<T>(string email, T tokenResponse)
    {
        var json = JsonSerializer.Serialize(tokenResponse);

        var existing = await dbContext.Set<CalendarToken>()
            .FirstOrDefaultAsync(ct => ct.Key == email);

        if (existing is null)
        {
            dbContext.Add(new CalendarToken
            {
                Key = email,
                JsonData = json,
                LastUpdated = DateTime.UtcNow
            });
        }
        else
        {
            existing.JsonData = json;
            existing.LastUpdated = DateTime.UtcNow;
            dbContext.Update(existing);
        }

        await CommitScope.CommitAsync();
    }

    public async Task<T> GetAsync<T>(string email)
    {
        var calendarToken = await dbContext.Set<CalendarToken>()
            .AsNoTracking()
            .FirstOrDefaultAsync(ct => ct.Key == email);

        return calendarToken?.JsonData is not null
            ? JsonSerializer.Deserialize<T>(calendarToken.JsonData)!
            : default!;
    }

    public async Task DeleteAsync<T>(string email)
    {
        var token = await dbContext.Set<CalendarToken>()
            .FirstOrDefaultAsync(ct => ct.Key == email);

        if (token != null)
        {
            dbContext.Remove(token);
            await CommitScope.CommitAsync();
        }
    }
}
