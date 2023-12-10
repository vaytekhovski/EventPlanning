using System;
using EventPlanning.Interfaces;

namespace EventPlanning.Services.Messages
{
    public class SmsMessageService : IMessageService
    {
        public async Task SendMessageAsync(string recipientPhoneNumber, string message)
        {
            Console.WriteLine("send sms");
        }
    }


}

