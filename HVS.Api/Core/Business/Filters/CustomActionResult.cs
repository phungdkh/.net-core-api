using System.Threading.Tasks;
using HVS.Api.Core.DataAccess.Repositories.Base;
using Microsoft.AspNetCore.Mvc;

namespace HVS.Api.Core.Business.Filters
{
    public class CustomActionResult : IActionResult
    {
        private readonly ResponseModel _responseModel;

        public CustomActionResult(ResponseModel responseModel)
        {
            _responseModel = responseModel;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            ObjectResult objectResult;
            switch (_responseModel.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    objectResult = new ObjectResult(_responseModel.Data)
                    {
                        StatusCode = (int)_responseModel.StatusCode
                    };
                    break;
                case System.Net.HttpStatusCode.NotFound:
                    objectResult = new ObjectResult(_responseModel.Message)
                    {
                        StatusCode = (int)_responseModel.StatusCode
                    };
                    break;
                default:
                    objectResult = new ObjectResult(_responseModel.Message)
                    {
                        StatusCode = (int)_responseModel.StatusCode
                    };
                    break;
            }
            await objectResult.ExecuteResultAsync(context);
        }
    }
}
