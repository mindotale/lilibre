using Microsoft.AspNetCore.Mvc.ModelBinding;

using Newtonsoft.Json;

namespace Lilibre.Api.ModelBinders;

public class TextPlainToJsonModelBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var request = bindingContext.HttpContext.Request;

        try
        {
            using var reader = new StreamReader(request.Body);
            var content = await reader.ReadToEndAsync();
            var jsonObject = JsonConvert.DeserializeObject(content, bindingContext.ModelType);
            bindingContext.Result = ModelBindingResult.Success(jsonObject);
        }
        catch (Exception)
        {
            bindingContext.Result = ModelBindingResult.Failed();
        }
    }
}