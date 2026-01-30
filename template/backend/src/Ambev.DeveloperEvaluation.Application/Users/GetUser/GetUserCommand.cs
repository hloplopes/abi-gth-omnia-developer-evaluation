using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUser;

/// <summary>
/// Command for retrieving a user by their ID
/// </summary>
public record GetUserCommand : IRequest<GetUserResult>
{
    public Guid Id { get; }

    public GetUserCommand(Guid id)
    {
        Id = id;
    }
}
