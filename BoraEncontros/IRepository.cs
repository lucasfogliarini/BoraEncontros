namespace BoraEncontros
{
    public interface IRepository
    {
        ICommitScope CommitScope { get; }
    }
    public interface IAddRepository<TEntity> : IRepository
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
    }

    public interface ICommitScope
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        int Commit(CancellationToken cancellationToken = default);
    }
}
