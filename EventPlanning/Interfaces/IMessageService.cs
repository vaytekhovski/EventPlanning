using System;
namespace EventPlanning.Interfaces
{
    public interface IMessageService
    {
        Task SendMessageAsync(string recipient, string message);
    }

}

