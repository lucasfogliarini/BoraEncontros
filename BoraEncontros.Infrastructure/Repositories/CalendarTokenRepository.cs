using BoraEncontros.Accounts;
using BoraEncontros.Accounts.Repositories;
using BoraEncontros.Infrastructure.Repositories;
using BoraEncontros.Infrastructure;
using Microsoft.EntityFrameworkCore;

internal class CalendarTokenRepository(BoraEncontrosDbContext dbContext) : Repository(dbContext), ICalendarTokenRepository
{
    public async Task<CalendarToken?> GetAsync(string email)
    {
        var calendarToken = await dbContext.Set<CalendarToken>().Include(e=>e.Account).FirstOrDefaultAsync(x => x.Account.Email == email);
        return calendarToken;
    }
    public async Task<CalendarToken?> GetByUsernameAsync(string username)
    {
        var calendarToken = await dbContext.Set<CalendarToken>().Include(e => e.Account).FirstOrDefaultAsync(x => x.Account.Username == username);
        return calendarToken;
    }
}
