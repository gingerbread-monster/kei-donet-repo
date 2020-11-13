using System.Collections.Generic;

namespace Ex02.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса который предоставляет методы
    /// для работы с пользователями.
    /// </summary>
    public interface IUserService
    {
        IEnumerable<string> GetAll();
    }
}
