using Ex02.Services.Interfaces;

namespace Ex02.Services
{
    /// <summary>
    /// Сервис для оповещения пользователя 
    /// с помощью альтернативного вида почты.
    /// </summary>
    public class MailNotifierService : IUserNotifierService
    {
        public string ServiceName { get { return nameof(MailNotifierService); } }

        public string NotifyUser(string user)
        {
            return SendCarrierPigeon(user);
        }

        string SendCarrierPigeon(string user)
            => $"🐦 Отправляем почтового голубя для {user}";
    }
}
