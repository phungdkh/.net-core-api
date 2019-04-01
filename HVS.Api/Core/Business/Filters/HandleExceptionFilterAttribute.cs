using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HVS.Api.Core.Business.Exceptions;
using HVS.Api.Core.Common.Constants;
using HVS.Api.Core.Common.Enums;

namespace HVS.Api.Core.Business.Filters
{
    /// <summary>
    /// Catch exceptions and return appropriate error messages from the Api
    /// </summary>
    public class HandleExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        /// <summary>
        ///
        /// </summary>
        /// <param name="logger"></param>
        public HandleExceptionFilterAttribute(ILogger<HandleExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handle different types of exceptions
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is DbUpdateException)
            {
                HandleDbUpdateException(context);
                return;
            }
            if (context.Exception is DatabaseException)
            {
                HandleDataBaseException(context);
                return;
            }
            if (context.Exception is BusinessRuleException)
            {
                HandleBusinessException(context);
                return;
            }
            //Any other unhandled exceptions are logged and returned as bad request
            context.HttpContext.Response.StatusCode = 500;
            //Detail of error
            //context.Result = new JsonResult(context.Exception);
            context.Result = new JsonResult(ErrorConstants.Common.InternalServerError);

            _logger.LogError(context.Exception, "General Error");
            base.OnException(context);
        }

        private void HandleDbUpdateException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "DbUpdate Exception");
            context.HttpContext.Response.StatusCode = 500;
            // TODO: It's better to have errorCode in the response
            // For automation test, for front-end mapping
            context.Result = new JsonResult(context.Exception.Message);
        }

        private void HandleBusinessException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Business Exception");
            context.HttpContext.Response.StatusCode = 400;
            // TODO: It's better to have errorCode in the response
            // For automation test, for front-end mapping
            context.Result = new JsonResult(context.Exception.Message);
        }

        private void HandleDataBaseException(ExceptionContext context)
        {
            var databaseException = context.Exception as DatabaseException;
            if (databaseException != null)
            {
                _logger.LogError(context.Exception, "DataBase Exception: " + databaseException.ExceptionType.ToString());
                if (databaseException != null && databaseException.ExceptionType == DatabaseExceptionType.EntityNotFound)
                {
                    context.HttpContext.Response.StatusCode = 404;
                    // TODO: It's better to have errorCode in the response
                    // For automation test, for front-end mapping
                    context.Result = new JsonResult(context.Exception.Message);
                }
                else
                {
                    context.HttpContext.Response.StatusCode = 500;
                    // TODO: It's better to have errorCode in the response
                    // For automation test, for front-end mapping
                    context.Result = new JsonResult(context.Exception.Message);
                }
            }
            else
            {
                _logger.LogError(context.Exception, "Error Exception");
                context.HttpContext.Response.StatusCode = 500;
                // TODO: It's better to have errorCode in the response
                // For automation test, for front-end mapping
                context.Result = new JsonResult(context.Exception.Message);
            }
        }
    }
}
