namespace Ex02.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса который предоставляет 
    /// методы для оповещения пользователя.
    /// </summary>
    public interface IUserNotifierService
    {
        string ServiceName { get; }

        string NotifyUser(string user);
    }
}
