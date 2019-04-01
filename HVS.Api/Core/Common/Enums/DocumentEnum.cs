using System.ComponentModel;

namespace HVS.Api.Core.Common.Enums
{
    public class DocumentEnum
    {
        public enum DocumentStatus
        {
            [Description("Draft")]
            Draft = 1,
            [Description("Pending Review")]
            PendingReview = 2,
            [Description("Published")]
            Published = 3
        }

        public enum DocumentFileType
        {
            [Description("PDF")]
            PDF = 1,
            [Description("EPUB")]
            EPUB
        }

        public enum DocumentClassification
        {
            [Description("Public")]
            Public = 1,
            [Description("Private")]
            Private,
            [Description("Confidental")]
            Confidental
        }
    }
}
