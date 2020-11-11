namespace Ex01
{
    /// <summary>
    /// Класс с опциями который представляет одноимённые
    /// свойства приложения из <i>appsettings.json</i>.
    /// </summary>
    public class AppJwtData
    {
        /// <summary>
        /// Секретный ключ которым подписывается JWT.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Издатель JWT.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Тот для кого предназначается изданный JWT.
        /// </summary>
        public string Audience { get; set; }
    }
}
