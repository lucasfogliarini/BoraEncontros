namespace BoraEncontros.Accounts;

public class Account : Entity
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string? Name { get; set; }
    public string? Photo { get; set; }
}
