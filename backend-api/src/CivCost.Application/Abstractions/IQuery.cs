using CivCost.Domain.Abstractions;
using MediatR;

namespace CivCost.Application.Abstractions;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
