using Google.Apis.Auth.OAuth2;

namespace BoraEncontros.GoogleCalendar;

public class GoogleCalendarSettings
{
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string[] Scopes { get; set; }
    public string? TokenFolder { get; set; }
    public string? ApplicationName { get; set; }
    public ClientSecrets GetClientSecrets()
    {
        return new ClientSecrets
        {
            ClientId = ClientId,
            ClientSecret = ClientSecret
        };
    }
}
