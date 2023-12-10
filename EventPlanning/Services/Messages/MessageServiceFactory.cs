using System;
using EventPlanning.Interfaces;
using EventPlanning.Models;

namespace EventPlanning.Services.Messages
{
    public class MessageServiceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public MessageServiceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IMessageService GetService(MessageType messageType)
        {
            return messageType switch
            {
                MessageType.Email => _serviceProvider.GetRequiredService<EmailMessageService>(),
                MessageType.SMS => _serviceProvider.GetRequiredService<SmsMessageService>(),
                _ => throw new ArgumentException("Invalid message type")
            };
        }
    }

}

