using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;

namespace LudusGestao.Shared.Domain.QueryParams
{
    /// <summary>
    /// Model binder para deserializar o parâmetro filter da query string como JSON
    /// </summary>
    public class QueryFilterModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
            var value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
                return Task.CompletedTask;

            try
            {
                // Primeiro, tenta deserializar como um objeto único
                if (value.TrimStart().StartsWith("{"))
                {
                    var singleFilter = JsonSerializer.Deserialize<QueryFilter>(value, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = false
                    });
                    
                    var filters = new List<QueryFilter> { singleFilter };
                    bindingContext.Result = ModelBindingResult.Success(filters);
                }
                else
                {
                    // Se não for um objeto único, tenta como lista
                    var filters = JsonSerializer.Deserialize<List<QueryFilter>>(value, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    bindingContext.Result = ModelBindingResult.Success(filters);
                }
            }
            catch (JsonException ex)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, 
                    $"Erro ao deserializar filtro: {ex.Message}");
            }

            return Task.CompletedTask;
        }
    }
} 