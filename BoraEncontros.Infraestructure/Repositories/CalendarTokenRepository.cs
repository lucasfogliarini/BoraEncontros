using BoraEncontros.Accounts;
using BoraEncontros.Accounts.Repositories;
using BoraEncontros.Infraestructure.Repositories;
using BoraEncontros.Infrastructure;
using Google.Apis.Auth.OAuth2.Responses;

internal class CalendarTokenRepository(BoraEncontrosDbContext dbContext) : Repository(dbContext), ICalendarTokenRepository
{
    public async Task AuthorizeCalendarAsync(string email, TokenResponse tokenResponse)
    {
        //var account = _accountService.GetAccount(email);
        //account.CalendarAuthorized = true;
        //account.CalendarAccessToken = tokenResponse.AccessToken;
        //account.CalendarRefreshAccessToken = tokenResponse.RefreshToken;

        //_boraRepository.Update(account);
        //await _boraRepository.CommitAsync();
    }

    public async Task<CalendarToken?> GetAsync(string email)
    {
        var calendarToken = await FirstOrDefaultAsync<CalendarToken>(x => x.Account.Email == email);
        return calendarToken;
    }
}
