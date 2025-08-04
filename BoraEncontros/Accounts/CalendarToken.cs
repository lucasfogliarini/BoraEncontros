namespace BoraEncontros.Accounts;

public class CalendarToken : Entity
{
    public int AccountId { get; set; }
    public required Account Account { get; set; }
    public string? Provider { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}
