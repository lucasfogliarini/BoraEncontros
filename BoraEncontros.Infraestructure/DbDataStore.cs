using Google.Apis.Util.Store;
using System.Text.Json;

public class GoogleDataStore(ICalendarTokenRepository repository) : IDataStore
{
    public async Task StoreAsync<T>(string key, T value)
    {
        var json = JsonSerializer.Serialize(value);
        await repository.SaveAsync(key, json);
    }

    public async Task DeleteAsync<T>(string key)
    {
        await repository.DeleteAsync(key);
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var json = await repository.GetAsync(key);
        return json is not null
            ? JsonSerializer.Deserialize<T>(json)!
            : default!;
    }

    public async Task ClearAsync()
    {
        await repository.ClearAsync();
    }
}
