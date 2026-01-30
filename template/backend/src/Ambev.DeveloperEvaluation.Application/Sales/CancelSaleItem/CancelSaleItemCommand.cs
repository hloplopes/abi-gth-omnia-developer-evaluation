using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Command for cancelling a specific item in a sale.
/// </summary>
public record CancelSaleItemCommand(Guid SaleId, Guid ItemId) : IRequest<CancelSaleItemResponse>;
