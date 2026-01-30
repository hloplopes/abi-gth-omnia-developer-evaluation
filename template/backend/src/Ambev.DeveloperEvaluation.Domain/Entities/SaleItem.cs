using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item within a sale, containing product details, quantity, pricing and discount information.
/// Uses the External Identities pattern for product references.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale identifier this item belongs to.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the external product identifier (External Identity).
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the denormalized product name for display purposes.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the product in this sale item.
    /// Must be between 1 and 20 as per business rules.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product at the time of sale.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount amount applied to this item.
    /// Calculated based on quantity-based discount tiers.
    /// </summary>
    public decimal Discount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item after discount.
    /// Calculated as (UnitPrice * Quantity) - Discount.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets whether this item has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets the navigation property to the parent Sale.
    /// </summary>
    public Sale Sale { get; set; } = null!;

    /// <summary>
    /// Calculates the discount percentage based on quantity business rules.
    /// </summary>
    /// <returns>The discount percentage (0, 10, or 20).</returns>
    public static decimal GetDiscountPercentage(int quantity)
    {
        if (quantity >= 10 && quantity <= 20)
            return 20m;
        if (quantity >= 4)
            return 10m;
        return 0m;
    }

    /// <summary>
    /// Calculates and sets the discount and total amount based on quantity and unit price.
    /// Applies business rules for quantity-based discounting.
    /// </summary>
    public void CalculateAmounts()
    {
        var discountPercentage = GetDiscountPercentage(Quantity);
        var grossAmount = UnitPrice * Quantity;
        Discount = grossAmount * (discountPercentage / 100m);
        TotalAmount = grossAmount - Discount;
    }

    /// <summary>
    /// Cancels this sale item.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
    }
}
