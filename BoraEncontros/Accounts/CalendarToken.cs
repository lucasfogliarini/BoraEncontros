using System.Net.Mail;

namespace BoraEncontros.Accounts;

public class CalendarToken : AggregateRoot
{
    public int AccountId { get; set; }
    public required Account Account { get; set; }
    public string? Provider { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime Expiration { get; set; }

    public static CalendarToken Create(string email, string provider, DateTime expiration)
    {
        var account = new Account
        {
            Username = new MailAddress(email).User,
            Email = email
        };
        account.CreatedNow();
        var calendarToken = new CalendarToken
        {
            Account = account,
            Provider = provider,
            Expiration = expiration
        };
        calendarToken.CreatedNow();
        return calendarToken;
    }
}
