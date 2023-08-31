using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;
using SharedLibrary.Extensions;
using System.Net;

namespace AuthServer.API.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilder<TValidate, TProperty> Required<TValidate, TProperty>(this IRuleBuilder<TValidate, TProperty> source)
            => source.NotNull().NotEmpty();

        public static void UseCustomValidationError(this IServiceCollection source)
        {
            source.Configure<ApiBehaviorOptions>(configuration =>
            {
                configuration.InvalidModelStateResponseFactory = response =>
                {
                    var errors = response.ModelState
                    .Values
                    .Where(x => !x.Errors.IsNullOrNotAny())
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToList();

                    ErrorDto error = new(errors, true);
                    var errorResponse = Response<NoContentResult>.Error(error, (int)HttpStatusCode.BadRequest);

                    return new BadRequestObjectResult(errorResponse);
                };
            });
        }
    }
}
