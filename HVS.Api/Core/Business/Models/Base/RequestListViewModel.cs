namespace HVS.Api.Core.Business.Models.Base
{
    public class RequestListViewModel : BaseRequestViewModel
    {
        public RequestListViewModel() : base()
        {
            IsDesc = false;
        }
        public string SortName { get; set; }
        public bool IsDesc { get; set; }
    }
}
