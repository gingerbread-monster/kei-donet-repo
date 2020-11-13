using System;

namespace Ex02.Services
{
    /// <summary>
    /// Класс который представляет сервис с жизненным циклом
    /// <b>Transient</b> (при каждом обращении к сервису создается
    /// новый объект сервиса).
    /// </summary>
    public class TransientService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    /// <summary>
    /// Класс который представляет сервис с жизненным циклом
    /// <b>Scoped</b> (В пределах одного запроса создается и используется
    /// один экземпляр сервиса. Для другого запроса создаётся другой экземпляр
    /// сервиса).
    /// </summary>
    public class ScopedService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }

    /// <summary>
    /// Класс который представляет сервис с жизненным циклом
    /// <b>Singleton</b> (экземпляр сервиса создаётся при первом обращении
    /// к нему. Все последующие запросы используют уже существующий
    /// экземпляр сервиса).
    /// </summary>
    public class SingletonService
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
