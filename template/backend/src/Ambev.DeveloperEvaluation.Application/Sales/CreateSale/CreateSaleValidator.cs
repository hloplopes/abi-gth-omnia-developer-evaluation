using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
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
            .SetValidator(new CreateSaleItemCommandValidator());
    }
}

/// <summary>
/// Validator for CreateSaleItemCommand with quantity-based business rules.
/// </summary>
public class CreateSaleItemCommandValidator : AbstractValidator<CreateSaleItemCommand>
{
    public CreateSaleItemCommandValidator()
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
