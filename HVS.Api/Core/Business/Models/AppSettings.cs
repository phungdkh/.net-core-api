using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HVS.Api.Core.Business.Models
{
    public class AppSettings
    {
        public string ProfileImageFolder { get; set; }

        public string SendGridKey { get; set; }

        public string EmailFrom { get; set; }

        public string FromName { get; set; }

        public string Secret { get; set; }

        public string AccountSid { get; set; }

        public string AuthToken { get; set; }
    }
}
