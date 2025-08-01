namespace BoraEncontros;

/// <summary>
/// Representa a raiz de um agregado, sendo a única entrada para modificar o estado interno do agregado.
/// É responsável por garantir as invariantes do domínio.
/// Cor no EventStorming: <b>Roxo</b>.
/// </summary>
public abstract class AggregateRoot : Entity, IAuditable
{
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    private List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents ??= [];
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    public void CreatedNow()
    {
        CreatedAt = DateTime.Now;
    }
    public void UpdatedNow()
    {
        UpdatedAt = DateTime.Now;
    }
}

public interface IAuditable
{
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; }
    public void CreatedNow();
    public void UpdatedNow();
}   
