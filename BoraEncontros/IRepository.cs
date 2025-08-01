namespace BoraEncontros
{
    public interface IRepository
    {
        ICommitScope CommitScope { get; }
    }

    public interface ICommitScope
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default);
        int Commit(CancellationToken cancellationToken = default);
    }
}
