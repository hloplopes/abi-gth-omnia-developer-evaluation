using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

/// <summary>
/// Contains unit tests for the SaleItem entity class.
/// Tests cover business rules for quantity-based discounting.
/// </summary>
public class SaleItemTests
{
    /// <summary>
    /// Tests that items with less than 4 units receive no discount.
    /// </summary>
    [Theory(DisplayName = "Items with less than 4 units should have no discount")]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Given_QuantityBelow4_When_CalculatingAmounts_Then_NoDiscount(int quantity)
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Test Product",
            Quantity = quantity,
            UnitPrice = 100m
        };

        // Act
        item.CalculateAmounts();

        // Assert
        item.Discount.Should().Be(0m);
        item.TotalAmount.Should().Be(100m * quantity);
    }

    /// <summary>
    /// Tests that items with 4-9 units receive 10% discount.
    /// </summary>
    [Theory(DisplayName = "Items with 4-9 units should have 10% discount")]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(9)]
    public void Given_QuantityBetween4And9_When_CalculatingAmounts_Then_10PercentDiscount(int quantity)
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Test Product",
            Quantity = quantity,
            UnitPrice = 100m
        };

        // Act
        item.CalculateAmounts();

        // Assert
        var grossAmount = 100m * quantity;
        var expectedDiscount = grossAmount * 0.10m;
        item.Discount.Should().Be(expectedDiscount);
        item.TotalAmount.Should().Be(grossAmount - expectedDiscount);
    }

    /// <summary>
    /// Tests that items with 10-20 units receive 20% discount.
    /// </summary>
    [Theory(DisplayName = "Items with 10-20 units should have 20% discount")]
    [InlineData(10)]
    [InlineData(15)]
    [InlineData(20)]
    public void Given_QuantityBetween10And20_When_CalculatingAmounts_Then_20PercentDiscount(int quantity)
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Test Product",
            Quantity = quantity,
            UnitPrice = 100m
        };

        // Act
        item.CalculateAmounts();

        // Assert
        var grossAmount = 100m * quantity;
        var expectedDiscount = grossAmount * 0.20m;
        item.Discount.Should().Be(expectedDiscount);
        item.TotalAmount.Should().Be(grossAmount - expectedDiscount);
    }

    /// <summary>
    /// Tests that the static GetDiscountPercentage returns correct values.
    /// </summary>
    [Theory(DisplayName = "GetDiscountPercentage returns correct tier percentage")]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 10)]
    [InlineData(9, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 20)]
    public void Given_Quantity_When_GettingDiscountPercentage_Then_ReturnsCorrectTier(int quantity, decimal expectedPercentage)
    {
        // Act
        var result = SaleItem.GetDiscountPercentage(quantity);

        // Assert
        result.Should().Be(expectedPercentage);
    }

    /// <summary>
    /// Tests that cancelling an item sets IsCancelled to true.
    /// </summary>
    [Fact(DisplayName = "Cancelling an item should set IsCancelled to true")]
    public void Given_ActiveItem_When_Cancelled_Then_IsCancelledShouldBeTrue()
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Test Product",
            Quantity = 5,
            UnitPrice = 100m
        };

        // Act
        item.Cancel();

        // Assert
        item.IsCancelled.Should().BeTrue();
    }
}
