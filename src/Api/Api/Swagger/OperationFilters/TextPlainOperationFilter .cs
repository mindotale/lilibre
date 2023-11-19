using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace Lilibre.Api.Swagger.OperationFilters;

public class TextPlainOperationFilter : IOperationFilter
{
    private const string TextPlainMediaType = "text/plain";
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.RequestBody is not null && operation.RequestBody.Content.TryGetValue(TextPlainMediaType, out var mediaType))
        {
            mediaType.Schema = new OpenApiSchema { Type = "string", Format = "text" };
        }
    }
}