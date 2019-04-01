using Microsoft.Extensions.Options;
using HVS.Api.Core.Business.IoC;
using HVS.Api.Core.Business.Models;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace HVS.Api.Core.Common.Helpers
{
    public static class SMSHelper
    {
        public static bool SendMessage(string text,string to)
        {
            var appSetting = IoCHelper.GetInstance<IOptions<AppSettings>>();

            TwilioClient.Init(appSetting.Value.AccountSid, appSetting.Value.AuthToken);

            var message = MessageResource.Create(
                body: text,
                from: new Twilio.Types.PhoneNumber("+12015716421"),
                to: new Twilio.Types.PhoneNumber(to)
            );

            return true;
        }

        public static string GenerateVerifyNumber()
        {
            Random generator = new Random();
            return generator.Next(0, 999999).ToString("D6");
        }
    }
}
