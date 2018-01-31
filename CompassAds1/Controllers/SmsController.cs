using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Mvc;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace CompassAds1.Controllers
{
    public class SmsController : TwilioController
    {
        // GET: Sms
        public String SendSms(String PhoneNumber)
        {

            //1lt7+/+E1fY-x8lYT6S31NymidWVbZnxmxaaQHpOA3

           const string accountSid = "ACbd6b81e1601a859a7f2418f3c47b05cf";
             const string authToken = "058f03f20e884a0234dd6d8820f951f7";
             TwilioClient.Init(accountSid, authToken);

             var to = new PhoneNumber("+972 059-785-7508");
             var message = MessageResource.Create(
                 to,
                 from: new PhoneNumber("+1 256-414-4899 "),
                 body: "This is the ship that made the Kessel Run in fourteen parsecs?");

            return message.Sid;



        }
    }
}     