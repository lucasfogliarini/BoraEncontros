namespace BoraEncontros.Events;

public class Event
{
    public required int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string? Name { get; set; }
    public string? Photo { get; set; }
}
