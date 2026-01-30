using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation.TestHelper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Validation;

/// <summary>
/// Contains unit tests for the SaleItemValidator class.
/// Tests cover quantity limits and business rule validations.
/// </summary>
public class SaleItemValidatorTests
{
    private readonly SaleItemValidator _validator;

    public SaleItemValidatorTests()
    {
        _validator = new SaleItemValidator();
    }

    /// <summary>
    /// Tests that validation passes for a valid sale item.
    /// </summary>
    [Fact(DisplayName = "Valid sale item should pass validation")]
    public void Given_ValidSaleItem_When_Validated_Then_ShouldNotHaveErrors()
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
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    /// <summary>
    /// Tests that quantity above 20 fails validation.
    /// </summary>
    [Fact(DisplayName = "Quantity above 20 should fail validation")]
    public void Given_QuantityAbove20_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Test Product",
            Quantity = 21,
            UnitPrice = 100m
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(i => i.Quantity);
    }

    /// <summary>
    /// Tests that quantity of zero fails validation.
    /// </summary>
    [Fact(DisplayName = "Quantity of zero should fail validation")]
    public void Given_ZeroQuantity_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Test Product",
            Quantity = 0,
            UnitPrice = 100m
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(i => i.Quantity);
    }

    /// <summary>
    /// Tests that zero unit price fails validation.
    /// </summary>
    [Fact(DisplayName = "Zero unit price should fail validation")]
    public void Given_ZeroUnitPrice_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = "Test Product",
            Quantity = 5,
            UnitPrice = 0m
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(i => i.UnitPrice);
    }

    /// <summary>
    /// Tests that empty product ID fails validation.
    /// </summary>
    [Fact(DisplayName = "Empty product ID should fail validation")]
    public void Given_EmptyProductId_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.Empty,
            ProductName = "Test Product",
            Quantity = 5,
            UnitPrice = 100m
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(i => i.ProductId);
    }

    /// <summary>
    /// Tests that empty product name fails validation.
    /// </summary>
    [Fact(DisplayName = "Empty product name should fail validation")]
    public void Given_EmptyProductName_When_Validated_Then_ShouldHaveError()
    {
        // Arrange
        var item = new SaleItem
        {
            ProductId = Guid.NewGuid(),
            ProductName = string.Empty,
            Quantity = 5,
            UnitPrice = 100m
        };

        // Act
        var result = _validator.TestValidate(item);

        // Assert
        result.ShouldHaveValidationErrorFor(i => i.ProductName);
    }
}
