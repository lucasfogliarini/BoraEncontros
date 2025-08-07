using Google.Apis.Auth.OAuth2;
using System.ComponentModel.DataAnnotations;

namespace BoraEncontros.GoogleCalendar;

public class GoogleCalendarSettings
{
    [Required]
    public required string ClientId { get; set; }
    [Required]
    public required string ClientSecret { get; set; }
    [Required]
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
