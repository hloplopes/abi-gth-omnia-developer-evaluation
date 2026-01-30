using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData;

/// <summary>
/// Provides methods for generating test data for CreateSaleHandler tests using the Bogus library.
/// </summary>
public static class CreateSaleHandlerTestData
{
    private static readonly Faker<CreateSaleItemCommand> createSaleItemFaker = new Faker<CreateSaleItemCommand>()
        .RuleFor(i => i.ProductId, f => f.Random.Guid())
        .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.UnitPrice, f => decimal.Parse(f.Commerce.Price(10, 500)));

    private static readonly Faker<CreateSaleCommand> createSaleHandlerFaker = new Faker<CreateSaleCommand>()
        .RuleFor(s => s.SaleNumber, f => $"SALE-{f.Random.Number(10000, 99999)}")
        .RuleFor(s => s.SaleDate, f => f.Date.Recent())
        .RuleFor(s => s.CustomerId, f => f.Random.Guid())
        .RuleFor(s => s.CustomerName, f => f.Person.FullName)
        .RuleFor(s => s.BranchId, f => f.Random.Guid())
        .RuleFor(s => s.BranchName, f => f.Company.CompanyName())
        .RuleFor(s => s.Items, f => createSaleItemFaker.Generate(f.Random.Int(1, 5)));

    /// <summary>
    /// Generates a valid CreateSaleCommand with randomized data.
    /// </summary>
    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleHandlerFaker.Generate();
    }

    /// <summary>
    /// Generates a valid CreateSaleItemCommand with a specific quantity.
    /// </summary>
    public static CreateSaleItemCommand GenerateItemWithQuantity(int quantity)
    {
        return createSaleItemFaker.Clone()
            .RuleFor(i => i.Quantity, _ => quantity)
            .Generate();
    }
}
