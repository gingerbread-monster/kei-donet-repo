using System.Collections.Generic;
using Ex02.Services.Interfaces;

namespace Ex02.Services
{
    /// <summary>
    /// Сервис предосталяющий методы для работы с пользователями.
    /// </summary>
    public class UserService : IUserService
    {
        public IEnumerable<string> GetAll()
        {
            return new List<string>
            {
                "Вася, vasya@mail.com",
                "Петя, petya@outlook.com",
                "Коля, kolya@gmail.com",
                "Саша, sasha@yahoo.com"
            };
        }
    }
}
