using RocketseatAuction.API.Contracts;
using RocketseatAuction.API.Entities;

namespace RocketseatAuction.API.Services;

public class LoggedUser : ILoggedUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _repository;

    public LoggedUser(IHttpContextAccessor httpContext, IUserRepository repository)
    {
        _httpContextAccessor = httpContext;
        _repository = repository;
    }

    public User User()
    {
        var userEmail = _httpContextAccessor.HttpContext?.Items["email"]?.ToString();

        if (string.IsNullOrEmpty(userEmail))
            throw new Exception("User email not found");

        return _repository.GetUserByEmail(userEmail);
    }
}