namespace HVS.Api.Core.Business.Models.Base
{
    public class BaseRequestViewModel
    {
        public BaseRequestViewModel()
        {
        }

        public int? Offset { get; set; }
        public int? Limit { get; set; }
    }
}
