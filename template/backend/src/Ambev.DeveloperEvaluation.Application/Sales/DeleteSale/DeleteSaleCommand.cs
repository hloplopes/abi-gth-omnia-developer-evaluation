using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;

/// <summary>
/// Command for cancelling (deleting) a sale.
/// </summary>
public record DeleteSaleCommand(Guid Id) : IRequest<DeleteSaleResponse>;
