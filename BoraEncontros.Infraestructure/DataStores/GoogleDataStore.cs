using BoraEncontros.Accounts.Repositories;
using Google.Apis.Util.Store;

namespace BoraEncontros.Infraestructure.DataStores;

public class GoogleDataStore(ICalendarTokenRepository calendarTokenRepository) : IDataStore
{
    public async Task StoreAsync<TResponse>(string email, TResponse tokenResponse)
    {
        throw new NotImplementedException();
    }
    public async Task<TResponse> GetAsync<TResponse>(string email)
    {
        throw new NotImplementedException();

        //var calendarToken = await calendarTokenRepository.GetAsync(email);

        //var tokenResponse = new TokenResponse
        //{
        //    AccessToken = calendarToken?.AccessToken,
        //    RefreshToken = calendarToken?.RefreshToken,
        //    ExpiresInSeconds = (int)(calendarToken?.Expiration - DateTime.UtcNow).TotalSeconds
        //};
        //return tokenResponse;
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