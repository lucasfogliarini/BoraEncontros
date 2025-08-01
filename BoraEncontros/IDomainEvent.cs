namespace BoraEncontros;

/// <summary>
/// Representa um evento de domínio que indica algo relevante que aconteceu no passado no domínio.
/// Pode ser utilizado para comunicação entre contextos limitados (bounded contexts) ou para processamento assíncrono.
/// Cor no EventStorming: <b>Laranja</b>.
/// </summary>
public interface IDomainEvent;

public abstract class DomainEvent : IDomainEvent
{
    public DateTime OccurredOn { get; protected set; } = DateTime.Now;
}

public interface IDomainEventHandler<in TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken);
}
