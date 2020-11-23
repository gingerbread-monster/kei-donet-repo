# 02: Внедрение зависимостей и сервисы
23/11/2020 | C# 9 | <span>ASP.NET</span> Core 5
___
### Содержание
* [Введение](#Введение)
    * [Внедрение зависимостей (DI)](#Внедрение-зависимостей-(DI))
    * [Инверсия управления (IoC)](#Инверсия-управления-(IoC))
* [Встроенные сервисы](#Встроенные-сервисы)
* [Самый простой созданный сервис](#Самый-простой-созданный-сервис)
* [Зависимость от интерфейса сервиса](#Зависимость-от-интерфейса-сервиса)
* [Жизненный цикл сервисов](#Жизненный-цикл-сервисов)
* [Несколько реализаций интерфейса сервиса](#Несколько-реализаций-интерфейса-сервиса)
    * [Пример с динамическим выбором реализации на основании настроек веб-приложения](#Пример-с-динамическим-выбором-реализации-на-основании-настроек-веб-приложения)
    * [Пример с выбором реализации с помощью "фабрики реализаций"](#Пример-с-выбором-реализации-с-помощью-"фабрики-реализаций")
* [Полезные ссылки](#Полезные-ссылки)
___
## Введение
<span>ASP.NET</span> Core имеет встроенную поддержку [внедрения зависимостей](#Внедрение-зависимостей) (Dependency Injection). При внедрении зависимостей достигается [инверсия управления](#Инверсия-управления-(IoC)) (Inversion of Control) между классами и их зависимостями.

### Внедрение зависимостей (DI)
*Зависмостью* называется любой объект от которого зависит некоторый другой объект. Например:
```cs
class A 
{
    public void SomeAction(){}
}

class B
{
    A obj = new A();
    
    void UseDependency()
    {
        obj.SomeAction();
    }
}
```
В данном случае класс `B` зависит от `A` , и использует его публичный метод. А класс `A` в данном случае называется *зависимостью* класса `B`.

Однако прямая зависимость объектов, как в примере выше, имеет ряд недостатков:
* Если понадобится заменить `A` на другую реализацию, то класс `B` тоже необходимо будет изменить.
* Если у `А` также будут зависимости, то `B` обязан будет конфигурировать и их тоже. И в большом приложении, когда несколько объектов будут зависеть от `A`, код конфигурирования зависимостей для `A` будет разбросан и повторяться по всему приложению.
* При прямой зависимости от `A` нет возможности провести юнит-тесты для `B`. В данном случае нет возможности изолированно протестировать объект `B`, т.к. мы не можем сделать mock или заглушку объекта `A`.

Внедрение зависимостей решает эти недостатки следующим образом:
* Использование зависимости от интерфейса или базового класса для абстрагирования от конкретной реализации.
* Зависимость добавляется в контейнер (коллекцию) сервисов. ASP<span>.</span>NET Core предоставляет встроенный контейнер для сервисов - [`IServiceProvider`](https://docs.microsoft.com/en-us/dotnet/api/system.iserviceprovider?view=net-5.0).
* Передача (внедрение) зависимостей через конструктор. Платформа сама отвечает за создание и последующее удаление нового экземпляра зависимости.

### Инверсия управления (IoC)
Зависимость в приложении должна быть направлена в сторону абстракции, а не на конкретные реализации.

Большинство приложений написано таким образом что зависимость "во время выполнения" совпадает с зависимостью "во время компиляции", что создаёт граф с прямой зависимостью:

![граф с прямой зависимостью](../images/direct-dependency-graph.svg)

В данном случае класс `A` вызывает метод класса `B`, который вызывает метод класса `C`. Поэтому "во время выполнения" `A` будет зависеть от `B`, который зависит от `C`. Кодом это можно представить так:
```cs
class A
{
    B obj = new B();

    void SomeWorkA() => obj.SomeWorkB();
}

class B 
{
    C obj = new C();

    public void SomeWorkB() => obj.SomeWorkC();
}

class C 
{
    public void SomeWorkC() {}
}
```

Применение принципа инверсии зависимостей позволяет классу `A` вызывать методы абстракции, которую реализует класс `B`. Это значит что класс `A` может вызывать класс `B` во время выполнения, однако `B` будет зависеть от интерфейса управляемого классом `A` во время компиляции (таким образом зависимость "во время компиляции" *инвертируется*).

![граф с инвертированной зависимостью](../images/inverted-dependency-graph.svg)

Во время выполнения поток выполнения программы остается неизменным, однако при этом легко могут быть подключены новые реализации интерфейсов.

Изменим пример приведенный выше с использованием внедрения зависимостей через конструктор, и принципа инверсии управления:
```cs
class A
{
    InterfaceB _interfaceB;

    public A(InterfaceB interfaceB)
    {
        _interfaceB = interfaceB;
    }

    void SomeWorkA() => _interfaceB.SomeWorkB();
}

interface InterfaceB
{
    void SomeWorkB();
}

class B : InterfaceB
{
    InterfaceC _interfaceC;

    public B(InterfaceC interfaceC)
    {
        _interfaceC = interfaceC;
    }

    public void SomeWorkB() => _interfaceC.SomeWorkC();
}

interface InterfaceC
{
    void SomeWorkC();
}

class C : InterfaceC
{
    public void SomeWorkC() {}
}
```
Инверсия зависимостей является ключевой частью для создания слабо связанных приложений, т.к. конкретные реализации зависят от более высокоуровневых абстракций, а не наоборот. В результате приложения легче и удобнее тестировать, легче поддерживать, и компоненты приложения становятся слабосвязанными легкозаменяемыми модулями. Практика *внедрения зависимостей* основывается на соблюдении принципа инверсии зависимостей.
___

*Сервисами* называются классы которые используются веб-приложением, и которые отвечают за решение определённых задач. Например встроенный сервис [`ILogger`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.ilogger?view=dotnet-plat-ext-5.0) это тип который отвечает за логгирование каких-либо данных.

## Встроенные сервисы
Платформа ASP<span>.</span>NET Core предоставляет ряд готовых [встроенных сервисов](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0#framework-provided-services) которые можно использовать в вашем веб-приложении.

Для того чтобы в веб-приложении использовать сервисы, их следует добавить в коллекцию сервисов. Это делается в методе `ConfigureServices()` класса `Startup`. Ниже приведён фрагмент кода с добавлением встроенных сервисов для данного примера веб-приложения:
```cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
    }
}
```
Вызов метода расширения `AddRazorPages()` добавляет в коллекцию сервисов встроенные сервисы для поддержки страниц Razor, которые будут использоваться для наглядной демонстрации остальных примеров данного.

## Самый простой созданный сервис
Приведём пример собственного простого пользовательского сервиса. Создадим новый класс `MyAppEnvironmentService` который и будет представлять наш сервис:
```cs
public class MyAppEnvironmentService
{
    readonly IWebHostEnvironment _environment;

    public MyAppEnvironmentService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public string GetAppEnvironmentDescription()
    {
        string toNewLine = Environment.NewLine;

        return $"Имя приложения : {_environment.ApplicationName}{toNewLine}" +
            $"Имя окружения : {_environment.EnvironmentName}{toNewLine}" +
            $"Путь к корню приложения : {_environment.ContentRootPath}{toNewLine}" +
            $"Путь к корню веб-содержимого приложения : {_environment.WebRootPath ?? "Каталог отсутствует"}";
    }
}
```
Вся суть данного сервиса в том чтобы возвращать некоторую строку с информацией об окружении данного веб-приложения. Данный класс имеет зависимость от `IWebHostEnvironment`, встроенного сервиса который и содержит необходимую информацию об окружении. Зависимость внедряется через конструктор. А метод `GetAppEnvironmentDescription()` использует полученную зависимость и возвращает строку с информацией об окружении веб-приложения.

Теперь, для того чтобы можно было получать и использовать наш сервис в любом месте веб-приложения, необходимо добавить наш сервис в коллекцию сервисов:
```cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 01: Самый простой созданный сервис
        services.AddTransient<MyAppEnvironmentService>();
    }
    // ...
}
```
Регистрируем наш сервис в коллекцию с помощью метода `AddTransient()`.

Далее для демонстрации использования сервиса создадим Razor страницу в которой получим наш сервис:
```cs
public class Example1Model : PageModel
{
    MyAppEnvironmentService _myAppEnvironmentService;

    public Example1Model(MyAppEnvironmentService myAppEnvironmentService)
    {
        _myAppEnvironmentService = myAppEnvironmentService;
    }

    public void OnGet()
    {
        ViewData["appEnvironmentDescription"] = 
            _myAppEnvironmentService.GetAppEnvironmentDescription();
    }
}
```
Получаем наш сервис через конструктор. При GET запросе к данной странице (метод `OnGet()`) будет использоватся наш сервис и возвращать строку, которая записывается в словарь `ViewData` и передается в представление. Содержимое *.cshtml* файла приведено ниже:
```html+razor
@page
@model Ex02.Pages.Example1Model

<div>
    @{ 
        string envDescription = Model.ViewData["appEnvironmentDescription"] as string;
        string[] outputLines = envDescription.Split(Environment.NewLine);

        <b>Окружение веб-приложения:</b>
        foreach (string line in outputLines)
        {
            <p>@line</p>
        }
    }
</div>
```
В данной Razor странице мы получаем строку с информацией об окружении веб-приложения из `ViewData`, и выводим её построчно.

В результате по запросу к данной странице имеем следующий результат:

![Самый простой созданный сервис](../screenshots/ex02/ex02-01.png)

## Зависимость от интерфейса сервиса
Однако сервисы принято добавлять согласно принципу [инверсии управления](#Инверсия-управления-(IoC)) (зависимость в приложении должна быть направлена в сторону абстракции, а не на конкретные реализации).

Приведём пример добавления сервиса и использования зависимости от абстракции, а не от конкретной реализации, как в предыдущем примере. Для примера создадим интерфейс `IUserService`, который будет представлять некий сервис для работы с пользователями:
```cs
public interface IUserService
{
    IEnumerable<string> GetAll();
}
```
Положим что реализация метода `GetAll()` должна будет возвращать перечисление из всех пользователей. 

Теперь создадим конкретную реализацию данного сервиса - класс `UserService`:
```cs
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
```
Теперь следует зарегистрировать наши сервисы в коллекции сервисов:
```cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 02: Зависимость от интерфейса сервиса
        services.AddTransient<IUserService, UserService>();
    }
    // ...
}
```
Данная конфигурация означает что при каждом запросе сервиса `IUserService` система будет создавать и передавать новый экземпляр класса `UserService`.

Далее для демонстрации использования сервиса создадим Razor страницу в которой получим наш сервис:
```cs
public class Example2Model : PageModel
{
    readonly IUserService _userService;

    public Example2Model(IUserService userService)
    {
        _userService = userService;
    }

    public void OnGet()
    {
        ViewData["allUsers"] = _userService.GetAll();
    }
}
```
Данный класс Razor страницы через конструктор получает `IUserService`, и в методе `OnGet()` использует метод данного сервиса.

Данный использующий класс вообще ничего не знает о конкретной реализации данного сервиса. Для данного класса важно лишь то что метод `GetAll()` вернёт набор всех пользователей. 

Это пример когда использующий класс не зависит от конкретной реализации. Соответственно какие-либо изменения или полная замена конкретной реализации никак не повлияют на логику данного класса и использование интерфейса.

Ниже приведён код Razor страницы для вывода списка всех пользователей, ранее полученный из сервиса:
```html+razor
@page
@model Ex02.Pages.Example2Model

@{ 
    var allUsers = ViewData["allUsers"] as IEnumerable<string>;

    <b>Список пользователей:</b>
    foreach (var user in allUsers)
    {
        <p>@user</p>
    }
}
```
По запросу к данной странице получим следующий результат:

![Зависимость от интерфейса сервиса](../screenshots/ex02/ex02-02.png)

## Жизненный цикл сервисов
При помощи разных методов добавления сервисов в коллекцию, мы можем задавать различные жизненные циклы для регистрируемых сервисов. Встроенный механизм внедрения зависимостей поддерживает 3 варианта жизненных циклов для регистрируемых сервисов:
1. **Transient**. При каждом обращении к сервису создается новый экземпляр сервиса.
1. **Scoped**. В пределах одного запроса создается и используется один экземпляр сервиса. Для другого запроса создаётся другой экземпляр cервиса.
1. **Singleton**. Экземпляр сервиса создаётся при первом обращении к нему. Все последующие запросы используют уже существующий экземпляр сервиса.

Для регистрации сервиса с каждым типом жизненного цикла существуют соответствующие обобщённые методы `AddTransient()`, `AddScoped()` и `AddSingleton()`.

Рассмотрим пример с использованием каждого типа жизненного цикла. Создадим три новых класса сервисов для каждого из типов:
```CS
public class TransientService
{
    public Guid Id { get; } = Guid.NewGuid();
}

public class ScopedService
{
    public Guid Id { get; } = Guid.NewGuid();
}

public class SingletonService
{
    public Guid Id { get; } = Guid.NewGuid();
}
```
Функционально каждый из этих сервисов одинаков. Каждый имеет get-свойство `Id`, которое при создании экземпляра класса инициализируется новым уникальным идентификатором ([`Guid`](https://docs.microsoft.com/en-us/dotnet/api/system.guid?view=net-5.0)).

Далее, для удобства, создадим метод расширения который зарегистрирует сразу три этих сервиса в коллекции сервисов:
```cs
public static class Extensions
{
    public static void AddDifferentLifetimeDemoServices(
        this IServiceCollection services)
    {
        services.AddTransient<TransientService>();
        services.AddScoped<ScopedService>();
        services.AddSingleton<SingletonService>();
    }
}
```
В данном методе расширения используем соответствующие методы `AddTransient()`, `AddScoped()` и `AddSingleton()` для регистрации сервисов с разным жизненным циклом.

Теперь следует добавить вызов данного метода расширения в метод `Startup.ConfigureServices()`:
```cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 03: Жизненный цикл сервисов
        services.AddDifferentLifetimeDemoServices();
    }
    // ...
}
```

Далее, для демонстрации работы этих сервисов с разными жизненными циклами, создадим Razor страницу. На ней с помощью цикла выводятся значения массива `Result`:
```html+razor
@page
@model Ex02.Pages.Example3Model

@{
    if (!(Model.Result is null))
    {
        <b>Результат:</b>
        for (int i = 0; i < Model.IterationsNumber; i++)
        {
            <p>@Model.Result[i]</p>
        }
    }
    else
    {
        <b>Некорректное значение query параметра.</b>
    }
}
```

Ниже приведён код класса страницы в которой мы получаем и используем ранее определённые сервисы:
```cs
    public class Example3Model : PageModel
    {
        readonly SingletonService _singletonService;

        public int IterationsNumber;
        public string[] Result;

        public Example3Model(SingletonService singletonService)
        {
            _singletonService = singletonService;
        }

        public void OnGet(string lifetime)
        {
            IterationsNumber = 5;

            if (lifetime == "transient")
            {
                Result = new string[IterationsNumber];

                for (int i = 0; i < IterationsNumber; i++)
                {
                    var requestedService =
                    (TransientService) HttpContext.RequestServices.GetService(typeof(TransientService));

                    Result[i] = $"сервис: {nameof(TransientService)}, " +
                        $"обращение к сервису №: {i}, " +
                        $"значение: {requestedService.Id}";
                }
            }
            else if (lifetime == "scoped")
            {
                Result = new string[IterationsNumber];

                for (int i = 0; i < IterationsNumber; i++)
                {
                    var requestedService =
                    (ScopedService) HttpContext.RequestServices.GetService(typeof(ScopedService));

                    Result[i] = $"сервис: {nameof(ScopedService)}, " +
                        $"обращение к сервису №: {i}, " +
                        $"значение: {requestedService.Id}";
                }
            }
            else if (lifetime == "singleton")
            {
                Result = new string[IterationsNumber];

                for (int i = 0; i < IterationsNumber; i++)
                {
                    Result[i] = $"сервис: {nameof(SingletonService)}, " +
                        $"обращение к сервису №: {i}, " +
                        $"значение: {_singletonService.Id}";
                }
            }
        }
    }
```
При GET запросе (метод `OnGet()`) приложение также должно получать query-параметр `lifetime`, по значению которого определяется какой из сервисов следует использовать. Блоки `if` внутри `OnGet()` определяют логику вызова соответствующего сервиса из определённых ранее.

Обратите внимание что альтернативным вариантом для получения экземпляра сервиса, кроме внедрения через конструктор, является использование метода [`GetService()`](https://docs.microsoft.com/en-us/dotnet/api/system.iserviceprovider.getservice?view=net-5.0).

Блок с *transient* сервисом внутри цикла каждый раз запрашивает экземпляр сервиса и добавляет значение свойства `Id` в массив `Result` (который затем выводится в Razor странице). Суть данного блока продемонстрировать тип жизненного цикла *transient*, при котором при каждом запросе этого сервиса создаётся новый экземпляр класса сервиса. Соответственно при каждой итерации цикла в массив `Result` будет записываться новое уникальное значение. Ниже приведён результат запроса к странице с выведенным массивом `Result`:

![результат transient сервиса](../screenshots/ex02/ex02-03.png)

Блок со *scoped* сервисом также внутри цикла каждый раз запрашивает сервис и добавляет значение свойства `Id` в массив `Result`. Но в отличие от *transient* у *scoped* в пределах одного запроса `GetService()` будет возвращать один и тот же экземпляр сервиса. То есть при первой итерации цикла будет создан экземпляр сервиса, и при последующих итерациях будет возвращатся всё тот же экземпляр. Ниже приведён результат запроса к странице с выведенным массивом `Result`:

![результат scoped сервиса](../screenshots/ex02/ex02-04.png)

Блок с *singleton* сервисом внутри также внутри цикла каждый добавляет значение свойства `Id` в массив `Result`. Однако он не запрашивает *singleton* сервис в цикле т.к. в этом нет никакого смысла, ведь один раз созданный экземпляр будет использоватся для всех последующих запросов на протяжении всего жизненного цикла приложения. Ниже приведён результат запроса к странице с выведенным массивом `Result`:

![результат singleton сервиса](../screenshots/ex02/ex02-05.png)

## Несколько реализаций интерфейса сервиса
В коллекцию сервисов можно добавить несколько конкретных реализаций для одного и того же интерфейса сервиса. Рассмотрим несколько примеров с динамическим использованием той или иной реализации сервиса по ходу работы веб-приложения.

### Пример с динамическим выбором реализации на основании настроек веб-приложения

Для примера создадим интерфейс `IUserNotifierService`, который будет представлять некий сервис для оповещения пользователей:
```cs
public interface IUserNotifierService
{
    string ServiceName { get; }

    string NotifyUser(string user);
}
```
Данный интерфейс имеет get-свойтсво которое должно будет возвращать имя текущего сервиса и собственно сам метод для оповещения пользователя - `NotifyUser()`.

Теперь создадим несколько реализаций данного интерфейса. Первым сервисом который будет реализовывать данный интерфейс будет класс `EmailNotifierService`:
```cs
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
```
Данный класс представляет сервис который, условно, используется для отправки оповещения пользователю с помощью email. Данный класс имеет 2 приватных вспомогательных метода. `BuildEmailMessage()` "подготавливает" и возвращает строку с содержимым email сообщения. А `SendEmail()`, условно, отправляет email нужному пользователю. Реализованный метод интерфейса `NotifyUser()` вызывает оба вспомогательных метода и возвращает строку с заготовленным сообщением.

Теперь создадим вторую реализацию интерфейса - класс `MailNotifierService`:
```cs
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
```
Этот сервис также отвечает за оповещение пользователя. Однако, в отличие от `EmailNotifierService`, представим что он для доставки письма будет использовать голубиную почту (приватный метод `SendCarrierPigeon()`).

Но как именно приложение будет решать какую из реализаций использовать? Для первого примера сделаем так, чтобы приложение использовало реализацию название которой вынесено в файл конфигурации приложения - файл *appsettings.json*. Для этого добавим следующее значение в JSON файл:
```json
{
    // ...
  "UserNotificationSettings": {
    "CurrentUserNotificationService": "EmailNotifierService"
  }
}
```
И создадим класс-модель к которой будет привязыватся текущая настройка оповещения пользователя из JSON файла:
```cs
public class UserNotificationSettings
{
    public string CurrentUserNotificationService { get; set; }
}
```

Далее, для удобства, создадим метод расширения который зарегистрирует сразу обе реализации сервисов для данного интерфейса, и привяжет соответствующую настройку к классу `UserNotificationSettings`:
```cs
public static class Extensions
{
    public static void AddUserNotificationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IUserNotifierService, EmailNotifierService>();
        services.AddTransient<IUserNotifierService, MailNotifierService>();

        services.Configure<UserNotificationSettings>(
            configuration.GetSection("UserNotificationSettings"));
    }
}
```
Отметьте что благодаря методу [`Configure()`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.optionsconfigurationservicecollectionextensions.configure?view=dotnet-plat-ext-5.0), `UserNotificationSettings` будет также доступен в коллекции сервисов (в специальном типе-обёртке [`IOptions`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.options.ioptions-1?view=dotnet-plat-ext-5.0)`<UserNotificationSettings>` предназначенном для настроек).

Теперь следует добавить вызов данного метода расширения в метод `Startup.ConfigureServices()`:
```cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // 04: Несколько реализаций интерфейса сервиса
        services.AddUserNotificationServices(_configuration);
    }
    // ...
}
```

Далее, для демонстрации работы этих сервисов создадим Razor страницу:
```cs
public class Example4Model : PageModel
{
    readonly IEnumerable<IUserNotifierService> _notifiers;
    readonly IOptions<UserNotificationSettings> _notificationSettings;

    public Example4Model(
        IEnumerable<IUserNotifierService> notifiers,
        IOptions<UserNotificationSettings> notificationSettings)
    {
        _notifiers = notifiers;
        _notificationSettings = notificationSettings;
    }

    public void OnGet()
    {
        var notificationService = _notifiers
            .FirstOrDefault(service =>
                service.ServiceName == _notificationSettings.Value.CurrentUserNotificationService);

        string result = notificationService?.NotifyUser("Петя");

        ViewData["notificationMessage"] = result;
    }
}
```
В данном примере через конструктор мы получаем коллекцию реализаций нашего сервиса для оповещения пользователя (тип `IEnumerable<IUserNotifierService>`). Однако через конструктор мы можем и просто запросить тип `IUserNotifierService`. В данном случае мы получим экземпляр сериса который был зарегистрирован последним для данного интерфейса.

По GET запросу (в методе `OnGet()`), с помощью LINQ метода к пришедшей коллекции реализаций, приложение ищет сервис с названием которое соответствует тому что получено из файла настроек. Затем у найденной реализации используется метод `NotifyUser()`, возвращаемое значение которого передаётся и выводится в представлении. Однако обратите внимание что, ради соблюдения хорошей практики программирования, вызов `notificationService?.NotifyUser("Петя");` содержит проверку на `null` в случае если необходимый сервис не был найден.

Ниже приведено содержание самой Razor страницы в которой выводится значение которое вернул метод `NotifyUser()`:
```html+razor
@page
@model Ex02.Pages.Example4Model

@{
    var notificationMessage = ViewData["notificationMessage"] as string;

    if (notificationMessage is not null)
    {
        <b>Оповещение для пользователя:</b>
        <p>@notificationMessage</p>
    }
}
```
Результат обращения к данной странице приведён ниже:

![email сервис](../screenshots/ex02/ex02-06.png)

Теперь в файле *appsettings.json* изменим значение названия сервиса который следует использовать на `MailNotifierService`. Сделаем ещё один запрос к данной странице:

![mail сервис](../screenshots/ex02/ex02-07.png)

### Пример с выбором реализации с помощью "фабрики реализаций"
Рассмотрим ещё один вариант как мы можем динамически выбирать конкретную реализацию интерфейса сервиса. Для данного примера мы будем использовать интерфейс и те же две реализации определённые выше. 

Изменим метод расширения который регистрирует сервисы в коллекции следующим образом:
```cs
public static class Extensions
{
    public static void AddUserNotificationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IUserNotifierService>(provider =>
        {
            int currentCentury = DateTime.Now.Year / 100 + 1;
            if (currentCentury < 21)
            {
                return new MailNotifierService();
            }
            else
            {
                return new EmailNotifierService();
            }
        });
    }
}
```
Используем следующую перегрузку метода `AddTransient()`, которая в качестве аргумента принимает делегат `Func<IServiceProvider, TService>`, который отвечает за создание нужного экземпляра конкретной реализации для указанного типа интерфейса сервиса.\

Внутри мы определим следующую логику: если числовое представление текущего столетия меньше 21 - возвращаем новый экземпляр `MailNotifierService`, иначе возвращаем новый экземпляр `EmailNotifierService`.

Также изменим логику класса Razor страницы:
```cs
public class Example4Model : PageModel
{
    readonly IUserNotifierService _userNotifierService;

    public Example4Model(IUserNotifierService userNotifierService)
    {
        _userNotifierService = userNotifierService;
    }

    public void OnGet()
    {
        string result = _userNotifierService.NotifyUser("Петя");

        ViewData["notificationMessage"] = result;
    }
}
```
В данном случае нам не нужно больше добавлять никакой логики для выбора конкретной реализации, т.к. она уже определена выше (в методе расширения). Здесь мы просто получаем нужный экземпляр сервиса и используем его.

Т.к. на момент написания данного материала текущим столетием является 21-е, то для обработки запроса к данной Razor странице будет использоваться `EmailNotifierService`:

![email сервис](../screenshots/ex02/ex02-08.png)

## Полезные ссылки
* [Fundamentals of dependency injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0#overview-of-dependency-injection)