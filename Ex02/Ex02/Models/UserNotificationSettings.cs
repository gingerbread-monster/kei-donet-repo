namespace Ex02.Models
{
    /// <summary>
    /// Модель которая предсталяет соответствующие настройки
    /// из <i>appsettings.json</i> для оповещения пользователя.
    /// </summary>
    public class UserNotificationSettings
    {
        /// <summary>
        /// Название текущего сервиса для оповещений.
        /// </summary>
        public string CurrentUserNotificationService { get; set; }
    }
}
