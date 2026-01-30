using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale record in the system.
/// Uses External Identities pattern for Customer and Branch references.
/// Contains business rules for quantity-based discounting.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets or sets the unique business sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the sale was made.
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// Gets or sets the external customer identifier (External Identity).
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the denormalized customer name for display purposes.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the total sale amount, calculated from all non-cancelled items.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets the branch identifier where the sale was made (External Identity).
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the denormalized branch name for display purposes.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the sale has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the sale was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the last update to the sale.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the collection of items in this sale.
    /// </summary>
    public List<SaleItem> Items { get; set; } = new();

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// </summary>
    public Sale()
    {
        CreatedAt = DateTime.UtcNow;
        SaleDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Recalculates the total sale amount from all non-cancelled items.
    /// </summary>
    public void CalculateTotalAmount()
    {
        TotalAmount = Items
            .Where(i => !i.IsCancelled)
            .Sum(i => i.TotalAmount);
    }

    /// <summary>
    /// Cancels the entire sale and all its items.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
        UpdatedAt = DateTime.UtcNow;
        foreach (var item in Items)
            item.Cancel();
    }

    /// <summary>
    /// Performs validation of the sale entity using the SaleValidator rules.
    /// </summary>
    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}
