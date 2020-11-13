using System;
using Ex02.Services.Interfaces;

namespace Ex02.Services
{
    /// <summary>
    /// Сервис для оповещения пользователя с помощью email.
    /// </summary>
    public class EmailNotifierService : IUserNotifierService
    {
        public string ServiceName { get { return nameof(EmailNotifierService); } }

        public string NotifyUser(string user)
        {
            string emailMessage = BuildEmailMessage(user);

            SendEmail(user, "An example", emailMessage);

            return emailMessage;
        }

        string BuildEmailMessage(string user) 
            => $"Hello, {user}. In case you were wondering, today is {DateTime.Now}";

        void SendEmail(string userEmail, string subject, string messageText) { }
    }
}
