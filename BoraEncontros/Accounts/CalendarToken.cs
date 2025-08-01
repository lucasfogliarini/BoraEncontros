namespace BoraEncontros.Accounts;

public class CalendarToken
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public required string Provider { get; set; }
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime Expiration { get; set; }
}
