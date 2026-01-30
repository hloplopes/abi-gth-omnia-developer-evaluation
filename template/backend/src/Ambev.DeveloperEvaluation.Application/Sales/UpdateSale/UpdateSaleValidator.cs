using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Validator for UpdateSaleCommand.
/// </summary>
public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidator()
    {
        RuleFor(sale => sale.Id)
            .NotEmpty();

        RuleFor(sale => sale.SaleNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(sale => sale.SaleDate)
            .NotEmpty();

        RuleFor(sale => sale.CustomerId)
            .NotEmpty();

        RuleFor(sale => sale.CustomerName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(sale => sale.BranchId)
            .NotEmpty();

        RuleFor(sale => sale.BranchName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(sale => sale.Items)
            .NotEmpty()
            .WithMessage("A sale must have at least one item.");

        RuleForEach(sale => sale.Items)
            .SetValidator(new UpdateSaleItemCommandValidator());
    }
}

public class UpdateSaleItemCommandValidator : AbstractValidator<UpdateSaleItemCommand>
{
    public UpdateSaleItemCommandValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty();

        RuleFor(item => item.ProductName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.")
            .LessThanOrEqualTo(20)
            .WithMessage("It's not possible to sell above 20 identical items.");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero.");
    }
}
