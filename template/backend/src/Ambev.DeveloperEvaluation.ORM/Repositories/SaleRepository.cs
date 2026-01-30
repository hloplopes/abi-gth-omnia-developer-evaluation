using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core.
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        var existingItems = await _context.SaleItems
            .Where(i => i.SaleId == sale.Id)
            .ToListAsync(cancellationToken);

        _context.SaleItems.RemoveRange(existingItems);

        _context.Sales.Update(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);
        if (sale == null)
            return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<(List<Sale> Sales, int TotalCount)> GetAllAsync(
        int page = 1,
        int size = 10,
        string? order = null,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Sales
            .Include(s => s.Items)
            .AsQueryable();

        query = ApplyOrdering(query, order);

        var totalCount = await query.CountAsync(cancellationToken);

        var sales = await query
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(cancellationToken);

        return (sales, totalCount);
    }

    private static IQueryable<Sale> ApplyOrdering(IQueryable<Sale> query, string? order)
    {
        if (string.IsNullOrWhiteSpace(order))
            return query.OrderByDescending(s => s.SaleDate);

        var orderParts = order.Split(',', StringSplitOptions.TrimEntries);
        IOrderedQueryable<Sale>? orderedQuery = null;

        foreach (var part in orderParts)
        {
            var tokens = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var field = tokens[0].ToLowerInvariant();
            var descending = tokens.Length > 1 && tokens[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            orderedQuery = (field, descending, orderedQuery == null) switch
            {
                ("salenumber", false, true) => query.OrderBy(s => s.SaleNumber),
                ("salenumber", true, true) => query.OrderByDescending(s => s.SaleNumber),
                ("salenumber", false, false) => orderedQuery!.ThenBy(s => s.SaleNumber),
                ("salenumber", true, false) => orderedQuery!.ThenByDescending(s => s.SaleNumber),

                ("saledate", false, true) => query.OrderBy(s => s.SaleDate),
                ("saledate", true, true) => query.OrderByDescending(s => s.SaleDate),
                ("saledate", false, false) => orderedQuery!.ThenBy(s => s.SaleDate),
                ("saledate", true, false) => orderedQuery!.ThenByDescending(s => s.SaleDate),

                ("customername", false, true) => query.OrderBy(s => s.CustomerName),
                ("customername", true, true) => query.OrderByDescending(s => s.CustomerName),
                ("customername", false, false) => orderedQuery!.ThenBy(s => s.CustomerName),
                ("customername", true, false) => orderedQuery!.ThenByDescending(s => s.CustomerName),

                ("totalamount", false, true) => query.OrderBy(s => s.TotalAmount),
                ("totalamount", true, true) => query.OrderByDescending(s => s.TotalAmount),
                ("totalamount", false, false) => orderedQuery!.ThenBy(s => s.TotalAmount),
                ("totalamount", true, false) => orderedQuery!.ThenByDescending(s => s.TotalAmount),

                ("branchname", false, true) => query.OrderBy(s => s.BranchName),
                ("branchname", true, true) => query.OrderByDescending(s => s.BranchName),
                ("branchname", false, false) => orderedQuery!.ThenBy(s => s.BranchName),
                ("branchname", true, false) => orderedQuery!.ThenByDescending(s => s.BranchName),

                _ => orderedQuery ?? query.OrderByDescending(s => s.SaleDate)
            };
        }

        return orderedQuery ?? query.OrderByDescending(s => s.SaleDate);
    }
}
