using CSharpFunctionalExtensions;

namespace BoraEncontros.Application;

/// <summary>
/// Representa um comando que expressa uma intenção de executar uma ação no domínio, geralmente iniciada por um ator (usuário ou sistema).
/// Este comando não espera um retorno
/// Cor no EventStorming: <b>Azul</b>.
/// </summary>
public interface ICommand;

/// <summary>
/// Representa um comando que expressa uma intenção de executar uma ação no domínio, geralmente iniciada por um ator (usuário ou sistema).
/// Este comando espera uma resposta tipada após sua execução.
/// Cor no EventStorming: <b>Azul</b>.
/// </summary>
public interface ICommand<TResponse>;

public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    Task<Result> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    Task<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
