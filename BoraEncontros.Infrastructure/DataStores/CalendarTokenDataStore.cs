using BoraEncontros.Accounts;
using BoraEncontros.Accounts.Repositories;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace BoraEncontros.Infrastructure.DataStores;

internal class CalendarTokenDataStore(ICalendarTokenRepository calendarTokenRepository) : IDataStore
{
    public async Task StoreAsync<TResponse>(string email, TResponse response)
    {
        if (response is not TokenResponse tokenResponse)
            throw new ArgumentException("Invalid token response type.", nameof(response));

        var calendarToken = await calendarTokenRepository.GetAsync(email);
        if (calendarToken?.UpdatedAt.Date == DateTime.Today)
        {
            return;
        }
        var expiration = DateTime.Now.AddSeconds((double)tokenResponse.ExpiresInSeconds);
        calendarToken ??= CalendarToken.Create(email, "Google", expiration);
        calendarToken.AccessToken = tokenResponse.AccessToken;
        calendarToken.RefreshToken = tokenResponse.RefreshToken ?? calendarToken.RefreshToken;
        await calendarTokenRepository.CommitScope.CommitAsync();
    }

    public async Task<TResponse> GetAsync<TResponse>(string email)
    {
        var calendarToken = await calendarTokenRepository.GetAsync(email);
        if (calendarToken == null)
            throw new ValidationException("Calendário não autorizado.");

        var tokenResponse = new TokenResponse
        {
            AccessToken = calendarToken.AccessToken,
            RefreshToken = calendarToken.RefreshToken,
            ExpiresInSeconds = (int)(calendarToken.Expiration - DateTime.UtcNow).TotalSeconds
        };

        return (TResponse)(object)tokenResponse;
    }


    public async Task DeleteAsync<T>(string email)
    {
        throw new NotImplementedException();
    }

    public async Task ClearAsync()
    {
        throw new NotImplementedException();
    }
}