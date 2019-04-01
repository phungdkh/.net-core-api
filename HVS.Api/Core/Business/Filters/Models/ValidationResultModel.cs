using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace HVS.Api.Core.Business.Filters.Models
{
    public class ValidationResultModel
    {
        public string Message { get; }

        public List<ValidationError> Errors { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Message = "Validation Failed";
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors
                        .Select(x => new ValidationError(key,
                            string.IsNullOrWhiteSpace(x.ErrorMessage) ? string.Format("The values of '{0}' is invalid.", key) : x.ErrorMessage)))
                    .ToList();
        }
    }
}
