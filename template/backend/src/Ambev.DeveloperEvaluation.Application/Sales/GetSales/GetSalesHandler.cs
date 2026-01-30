using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales;

/// <summary>
/// Handler for processing GetSalesCommand requests (paginated list).
/// </summary>
public class GetSalesHandler : IRequestHandler<GetSalesCommand, GetSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;

    public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    public async Task<GetSalesResult> Handle(GetSalesCommand command, CancellationToken cancellationToken)
    {
        var (sales, totalCount) = await _saleRepository.GetAllAsync(
            command.Page, command.Size, command.Order, cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)command.Size);

        return new GetSalesResult
        {
            Data = _mapper.Map<List<GetSaleResult>>(sales),
            TotalItems = totalCount,
            CurrentPage = command.Page,
            TotalPages = totalPages
        };
    }
}
