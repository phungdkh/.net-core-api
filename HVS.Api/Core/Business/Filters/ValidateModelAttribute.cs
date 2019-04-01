using Microsoft.AspNetCore.Mvc.Filters;
using HVS.Api.Core.Business.Filters.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HVS.Api.Core.Business.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var invalidModels = new List<InvalidModel>();
                var states = context.ModelState;

                foreach (var state in states)
                {
                    string[] models = state.Key.Split('.');
                    var invalidModel = new InvalidModel();
                    var errorMessage = string.Empty;
                    if (models.Length == 2)
                    {
                        invalidModel.PropertyName = models[1];
                    }
                    else if (models.Length == 1)
                    {
                        invalidModel.PropertyName = models[0];
                    }
                    foreach (var error in state.Value.Errors)
                    {
                        if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
                        {
                            errorMessage += error.ErrorMessage + ";";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(errorMessage))
                    {
                        errorMessage = errorMessage.TrimEnd(';');
                    }

                    if (string.IsNullOrWhiteSpace(errorMessage))
                    {
                        errorMessage = "Please check format of input data!";
                    }

                    invalidModel.ErrorMessage = errorMessage;

                    invalidModels.Add(invalidModel);
                }
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(invalidModels);
            }
        }
        private class InvalidModel
        {
            public string PropertyName { get; set; }
            public string ErrorMessage { get; set; }
        }
    }
}
