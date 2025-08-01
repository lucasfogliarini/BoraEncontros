using BoraEncontros.Events;

namespace BoraEncontros.Accounts;

public class CalendarToken : Entity
{
    public int AccountId { get; set; }
    public required Account Account { get; set; }
    public required string Provider { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}
