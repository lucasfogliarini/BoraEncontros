using BoraEncontros.Accounts;
using BoraEncontros.Accounts.Repositories;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Util.Store;
using System.ComponentModel.DataAnnotations;

namespace BoraEncontros.Infrastructure.DataStores;

internal class CalendarTokenDataStore(ICalendarTokenRepository calendarTokenRepository) : IDataStore
{
    public async Task StoreAsync<TResponse>(string email, TResponse response)
    {
        if (response is not TokenResponse tokenResponse)
            throw new ArgumentException("Invalid token response type.", nameof(response));

        var calendarToken = await calendarTokenRepository.GetAsync(email);        
        if (calendarToken == null)
        {
            calendarToken ??= CalendarToken.Create(email, "Google");
            calendarTokenRepository.Add(calendarToken);
        }
        calendarToken.UpdatedNow();
        calendarToken.AccessToken = tokenResponse.AccessToken;
        calendarToken.RefreshToken = tokenResponse.RefreshToken ?? calendarToken.RefreshToken;
        calendarToken.Expiration = DateTime.Now.AddDays(1);
        await calendarTokenRepository.CommitScope.CommitAsync();
    }

    public async Task<TResponse> GetAsync<TResponse>(string email)
    {
        var calendarToken = await calendarTokenRepository.GetAsync(email);
        if (calendarToken == null)
            throw new ValidationException("Calendário não autorizado.");

        var issuedUtc = calendarToken.UpdatedAt;
        long expiresIn = (long)(calendarToken.Expiration - issuedUtc).TotalSeconds;

        var tokenResponse = new TokenResponse
        {
            IssuedUtc = issuedUtc,
            AccessToken = calendarToken.AccessToken,
            RefreshToken = calendarToken.RefreshToken,
            ExpiresInSeconds = expiresIn
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