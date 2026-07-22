using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace HomeFlow.Web.Controllers.Api;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected static Dictionary<string, string> ToErrors(ValidationResult result) =>
        result.Errors.GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => string.Join(" ", g.Select(e => e.ErrorMessage)));
}