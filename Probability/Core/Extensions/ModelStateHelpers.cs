using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Probability.Core.Extensions
{
    public static class ModelStateHelpers
    {
        public static string GetModelStateErrors(this ModelStateDictionary modelState)
        {
            var errors = modelState.IsValid
                ? null
                : modelState
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray())
                    .Where(m => m.Value.Any());

            var output = new StringBuilder("{");

            if (errors != null)
            {
                foreach (KeyValuePair<string, string[]> kvp in errors)
                {
                    output.Append($"\"{kvp.Key.Replace(".","")}\":\"{string.Join(", ", kvp.Value)}\",");
                }
            }

            var jsonModelState = output.ToString().TrimEnd(',');
            jsonModelState += "}";
            return jsonModelState;
        }
    }
}
