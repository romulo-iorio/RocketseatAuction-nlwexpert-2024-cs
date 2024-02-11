using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RocketseatAuction.API.Contracts;

namespace RocketseatAuction.API.Filters;

public class AuthenticationUserAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public IUserRepository _repository;

    public AuthenticationUserAttribute(IUserRepository repository)
    {
        _repository = repository;
    }

    private string TokenOnRequest(HttpContext context)
    {
        var authentication = context.Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(authentication))
            throw new Exception("Token is missing");

        var positionAfterBearer = "Bearer ".Length;

        return authentication[positionAfterBearer..];
    }

    private string FromBase64String(string base64)
    {
        var decoded = Convert.FromBase64String(base64);
        return System.Text.Encoding.UTF8.GetString(decoded);
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context.HttpContext);
            var email = FromBase64String(token);

            var userExists = _repository.ExistsUserWithEmail(email);

            if (!userExists)
                throw new Exception("User with this email not found");

            context.HttpContext.Items.Add("email", email);
        }
        catch (Exception ex)
        {
            context.Result = new UnauthorizedObjectResult(ex.Message);
        }
    }
}