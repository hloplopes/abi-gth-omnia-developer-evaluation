using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the Sale entity class.
/// Tests cover business rules, cancellation, and total calculation.
/// </summary>
public class SaleTests
{
    /// <summary>
    /// Tests that a sale can be cancelled and all items are also cancelled.
    /// </summary>
    [Fact(DisplayName = "Sale cancellation should cancel all items")]
    public void Given_ActiveSale_When_Cancelled_Then_AllItemsShouldBeCancelled()
    {
        // Arrange
        var sale = CreateValidSale();

        // Act
        sale.Cancel();

        // Assert
        sale.IsCancelled.Should().BeTrue();
        sale.Items.Should().AllSatisfy(item => item.IsCancelled.Should().BeTrue());
        sale.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that total amount is calculated correctly from non-cancelled items.
    /// </summary>
    [Fact(DisplayName = "Total amount should only include non-cancelled items")]
    public void Given_SaleWithCancelledItems_When_CalculatingTotal_Then_ExcludesCancelledItems()
    {
        // Arrange
        var sale = CreateValidSale();
        sale.Items[0].TotalAmount = 100m;
        sale.Items[1].TotalAmount = 200m;
        sale.Items[1].IsCancelled = true;

        // Act
        sale.CalculateTotalAmount();

        // Assert
        sale.TotalAmount.Should().Be(100m);
    }

    /// <summary>
    /// Tests that validation passes for valid sale data.
    /// </summary>
    [Fact(DisplayName = "Validation should pass for valid sale data")]
    public void Given_ValidSaleData_When_Validated_Then_ShouldReturnValid()
    {
        // Arrange
        var sale = CreateValidSale();

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    /// <summary>
    /// Tests that validation fails for sale without items.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for sale without items")]
    public void Given_SaleWithoutItems_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = CreateValidSale();
        sale.Items.Clear();

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that validation fails for empty sale number.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for empty sale number")]
    public void Given_EmptySaleNumber_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var sale = CreateValidSale();
        sale.SaleNumber = string.Empty;

        // Act
        var result = sale.Validate();

        // Assert
        result.IsValid.Should().BeFalse();
    }

    private static Sale CreateValidSale()
    {
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = "SALE-001",
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            CustomerName = "John Doe",
            BranchId = Guid.NewGuid(),
            BranchName = "Main Branch",
            Items = new List<SaleItem>
            {
                new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product A",
                    Quantity = 5,
                    UnitPrice = 10m,
                    Discount = 5m,
                    TotalAmount = 45m
                },
                new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product B",
                    Quantity = 2,
                    UnitPrice = 20m,
                    Discount = 0m,
                    TotalAmount = 40m
                }
            }
        };
        return sale;
    }
}
