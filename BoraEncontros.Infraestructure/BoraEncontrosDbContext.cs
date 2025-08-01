using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BoraEncontros.Infrastructure;

internal class BoraEncontrosDbContext(DbContextOptions options) : DbContext(options), ICommitScope
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var thisAssembly = Assembly.GetExecutingAssembly();
        modelBuilder.ApplyConfigurationsFromAssembly(thisAssembly);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    public int Commit(CancellationToken cancellationToken = default) => CommitAsync(cancellationToken).Result;

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IEnumerable<IDomainEvent> domainEvents = entity.DomainEvents;
                entity.ClearDomainEvents();
                return domainEvents;
            })
            .AsEnumerable();

        //await domainEventsDispatcher.DispatchAsync(domainEvents);
    }
}
