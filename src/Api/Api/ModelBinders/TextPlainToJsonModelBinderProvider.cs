using Lilibre.Api.V1.Genres;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Lilibre.Api.ModelBinders;

public class TextPlainToJsonModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.Metadata is { ModelType: not null, BinderType: null })
        {
            return new TextPlainToJsonModelBinder();
        }

        return null;
    }
}