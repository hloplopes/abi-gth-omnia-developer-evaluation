namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;

/// <summary>
/// Request model for deleting (cancelling) a sale.
/// </summary>
public class DeleteSaleRequest
{
    public Guid Id { get; set; }
}
