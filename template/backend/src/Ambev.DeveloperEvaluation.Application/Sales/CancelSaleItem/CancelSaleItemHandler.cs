using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;

/// <summary>
/// Handler for processing CancelSaleItemCommand requests.
/// </summary>
public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, CancelSaleItemResponse>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleItemHandler> _logger;

    public CancelSaleItemHandler(ISaleRepository saleRepository, ILogger<CancelSaleItemHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    public async Task<CancelSaleItemResponse> Handle(CancelSaleItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new CancelSaleItemCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

        if (sale.IsCancelled)
            throw new InvalidOperationException("Cannot cancel an item from a cancelled sale");

        var item = sale.Items.FirstOrDefault(i => i.Id == command.ItemId);
        if (item == null)
            throw new KeyNotFoundException($"Item with ID {command.ItemId} not found in sale {command.SaleId}");

        if (item.IsCancelled)
            throw new InvalidOperationException("Item is already cancelled");

        item.Cancel();
        sale.CalculateTotalAmount();
        sale.UpdatedAt = DateTime.UtcNow;

        await _saleRepository.UpdateAsync(sale, cancellationToken);

        // Publish ItemCancelled event (logged)
        var itemCancelledEvent = new ItemCancelledEvent(item, sale.Id);
        _logger.LogInformation(
            "Event: ItemCancelled - Item {ProductName} (ID: {ItemId}) cancelled from Sale {SaleNumber}. New sale total: {TotalAmount}",
            item.ProductName, item.Id, sale.SaleNumber, sale.TotalAmount);

        return new CancelSaleItemResponse { Success = true };
    }
}
