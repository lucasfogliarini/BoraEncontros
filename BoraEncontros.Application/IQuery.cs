using CSharpFunctionalExtensions;

namespace BoraEncontros.Application;

/// <summary>
/// Representa uma consulta (query) que solicita dados do domínio sem causar efeitos colaterais.
/// Utilizada para leitura, projeção ou apresentação de informações.
/// Pode ser mapeada a um <b>Read Model</b> no EventStorming, o qual representa uma projeção otimizada para leitura.
/// Cor no EventStorming: <b>Verde</b>.
/// </summary>
public interface IQuery<TResponse>;

public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken = default);
}
