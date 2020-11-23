# 01: Конвейер обработки запросов и middleware
06/11/2020 | C# 9 | <span>ASP.NET</span> Core 5
___
### Содержание
* [Введение](#Введение)
* [Самый простой middleware компонент](#Самый-простой-middleware-компонент)
* [Последовательная работа нескольких middleware компонентов](#Последовательная-работа-нескольких-middleware-компонентов)
* [Разветвление middleware маршрутами](#Разветвление-middleware-маршрутами)
* [Жизненный цикл middleware](#Жизненный-цикл-middleware)
* [Использование пользовательского middleware класса](#Использование-пользовательского-middleware-класса)
* [Взаимодействие нескольких пользовательских middleware компонентов (классов)](#Взаимодействие-нескольких-пользовательских-middleware-компонентов-(классов))
* [Порядок добавления middleware компонентов в конвейер](#Порядок-добавления-middleware-компонентов-в-конвейер)
* [Полезные ссылки](#Полезные-ссылки)
___
## Введение
Обработка запросов в <span>ASP.NET</span> Core устроена по принципу конвейера. Конвейер обработки запросов состоит из последовательности *делегатов запроса* которые вызываются поочередно, один за другим. Следующее изображение демонстрирует принцип работы конвейера.

![конвейер обработки запросов](../images/request-pipeline.png)

Эти *делегаты запроса*, или компоненты конвейера, называются **промежуточным ПО (middleware)**. Как видно на иллюстрации выше, каждый из компонентов может обрабатывать запрос как до, так и после следующего за ним компонента в конвейере. Например, компоненты обработки исключений должны "стоять" (вызываться) в начале конвейера, чтобы перехватить и обработать все возникающие ошибки (исключения) на более поздних этапах.

Каждый компонент middleware:
* Выбирает передавать ли обработку запроса следующему компоненту.
* Может выполнять работу как до, так и после компонента стоящего следующим в конвейере.

Из middleware компонентов состоит конвейер обработки запроса. Компоненты middleware обрабатывают каждый запрос.

Для создания компонентов middleware используется делегат [`RequestDelegate`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.requestdelegate?view=aspnetcore-5.0), который выполняет некоторое действие и принимает контекст запроса (тип [`HttpContext`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext?view=aspnetcore-5.0)):
```cs
public delegate Task RequestDelegate(HttpContext context);
```

Добавление элементов в конвейер происходит в методе `Configure()` класса `Startup`. Компоненты middleware добавляются в конвейер с помощью методов расширения  `Use()`, `Run()`, и `Map()` типа [`IApplicationBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder?view=aspnetcore-5.0).

## Самый простой middleware компонент
Приведём пример самого простого <span>ASP.NET</span> Core приложения в котором добавлен единственный middleware компонент:
```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.Run(async context =>
        {
            await context.Response.WriteAsync("Hello, World!");
        });
    }
}
```
Этот middleware компонент обрабатывает каждый запрос и каждый раз записывает в тело ответа строку `Hello, World!`. Однако в случае с примером выше, "конвейер" обработки запросов здесь по факту отсутствует, т.к. определён только 1 компонент который на каждый запрос вызывает лямбда-выражение.

## Последовательная работа нескольких middleware компонентов
Мы можем соеденить несколько компонентов middleware (и создать конвейер) с помощью метода `Use()`. Рассмотрим пример с несколькими компонентами middleware: 
```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            Debug.WriteLine(
                "[USE] doing some work before calling next middleware component");

            await next.Invoke();

            Debug.WriteLine("[USE] working after next component finished it's work");
        });

        app.Run(async context =>
        {
            Debug.WriteLine("terminal middleware is running");

            await context.Response.WriteAsync(
                "Text response from terminal middleware component");
        });
    }
}
```
Метод `Use()`, в отличие от `Run()`, кроме контекста запроса принимает также ссылку на следующее middleware (в примере выше это аргумент `next`). Завершить работу конвейера можно даже если после текущего middleware компонента определено ещё несколько middleware. Для этого нужно просто не выполнять команду `next.Invoke();` (просто не вызывать следующий делегат).

Суть примера выше это продемонстрировать последовательную работу нескольких middleware компонентов. А для имитации некоторой работы, которую компоненты middleware могут выполнять как до, так и после вызова следующего middleware, в примере выше печатаются некоторые строки в консоль отладки. Результат выполнения запроса данным приложением приведён на скриншоте ниже:

![последовательная работа компонентов middleware](../screenshots/ex01/ex01-01.png)

Однако если поменять местами методы `Use()` и `Run()`, то middleware компонент из `Use()` не будет вызываться вовсе, т.к. компоненты определяемые с помощью `Run()` являются терминальными (завершающими) в конвейере.

## Разветвление middleware маршрутами
С помощью метода расширения `Map()` возможно определить несколько "ветвей" middleware компонентов для выполнения запросов. Первым аргументов метод `Map()` принимает строку с маршрутом. Вторым аргументом передается делагат с аргументом типа [`IApplicationBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder?view=aspnetcore-5.0) (для настройки и определения middleware для этого маршрута). Если при сопоставлении запрашиваемого маршрута с переданной в `Map()` строкой совпадут, то будет выполнена часть middleware определенная в `Map()`.

Ниже приведён пример разветвления двух middleware компонентов разными маршрутами:
```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.Map("/map1", appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                await context.Response.WriteAsync("Mapped path 1");
            });
        });

        void SecondMapHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Mapped path 2");
            });
        };
        app.Map("/map2", SecondMapHandler);
    }
}
```
Следующая таблица приводит запросы и ответы к соответствующим маршрутам из примера выше:
Запрос | Ответ
--- | ---
localhost:5000 | Не найдено (cтатус код 404)
localhost:5000/map1 | Mapped path 1
localhost:5000/map2 | Mapped path 2

Также с помощью метода `Map()` можно определить несколько сегментов пути сразу:
```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.Map("/map3/route", appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                await context.Response.WriteAsync("Mapped path 3: multiple segments");
            });
        });
    }
}
```
Следующая таблица приводит запросы и ответы к соответствующим маршрутам из примера выше:
Запрос | Ответ
--- | ---
localhost:5000 | Не найдено (cтатус код 404)
localhost:5000/map3 | Не найдено (cтатус код 404)
localhost:5000/map3/route | Mapped path 3: multiple segments

Также можно определять методы `Map()` внутри друг друга:
```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.Map("/map4", appBuilder =>
        {
            appBuilder.Map("/map5", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    await context.Response.WriteAsync("Mapped path 4 and 5: nested mappings");
                });
            });
        });
    }
}
```
Следующая таблица приводит запросы и ответы к соответствующим маршрутам из примера выше:
Запрос | Ответ
--- | ---
localhost:5000 | Не найдено (cтатус код 404)
localhost:5000/map4 | Не найдено (cтатус код 404)
localhost:5000/map3/map5 | Mapped path 4 and 5: nested mappings

Также существует условный метод `MapWhen()`. В отличие от простого `Map()`, данный метод "ответвляет" конвеер только в том случае если переданный ему предикат возвращает значение `true`. Передаваемый предикат это делегат типа `Func<HttpContext, bool>`. Пример ниже демонстрирует использование метода `MapWhen()`. Переданный предикат проверяет присутствует ли в строке адреса запроса параметр с названием `param`.
```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.MapWhen(
            context => context.Request.Query.ContainsKey("param"),
            appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    string paramValue = context.Request.Query["param"];
                    await context.Response.WriteAsync(
                        $"Path mapped when query key has value.\nParam value: {paramValue}");
                });
            });
    }
}
```
Следующая таблица приводит запросы и ответы к маршрутам из примера выше:
Запрос | Ответ
--- | ---
localhost:5000 | Не найдено (cтатус код 404)
localhost:5000?param=hello | Path mapped when query key has value. Param value: hello

Метод `UseWhen()` также ответвляет конвейер обработки запросов основываясь на результате переданного предиката. Однако, в отличие от `MapWhen()`, последовательный вызов компонентов продолжится сразу после компонента определенного в `UseWhen()` (разумеется, при условии что следующий компонент будет вызван (`await next();`)).

## Жизненный цикл middleware
Метод `Configure()`, в котором добавляются компоненты middleware, выполняется один раз при создании объекта класса `Startup`. Соответственно и компоненты middleware создаются один раз и живут на протяжении всего жизненного цикла приложения. То есть для последующей обработки запросов используются одни и те же компоненты.

Следующий пример наглядно демонстрирует то как при нескольких запросах приложение будет использовать одни и те же, единожды созданные, компоненты middleware:
```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        int x = 1;

        app.Run(async context =>
        {
            x += 1;

            await context.Response.WriteAsync(x.ToString());
        });
    }
}
```
Данный middleware компонент при каждом запросе инкрементирует значение переменной и возвращает её в теле ответа. Ниже приведён скриншот из браузера при первом запросе:

![жизненный цикл middleware 1](../screenshots/ex01/ex01-02.png)

Однако можно заметить что уже при первом запросе значение переменной равно 2. Это происходит из-за того что некоторые браузеры могут посылать отдельный второй запрос к файлу иконки *favicon.ico*.
Ниже приведён скриншот из браузера при втором запросе:

![жизненный цикл middleware 2](../screenshots/ex01/ex01-03.png)

## Использование пользовательского middleware класса
Кроме добавления middleware с помощью методов `Run()`, `Use()` и `Map()` также возможно создавать свои компоненты middleware в виде отдельных классов, которые затем можно добавить в конвейер с помощью обобщённого метода `UseMiddleware()`.

Для примера создадим middleware которое будет проверять некоторый пароль, который пользователь будет передавать как параметр через адресную строку. Для этого создадим в проекте новый класс `PasswordMiddleware`:
```cs
public class PasswordMiddleware
{
    RequestDelegate nextMiddlewareComponent;

    string _validPassword;

    public PasswordMiddleware(
        RequestDelegate next,
        string validPassword)
    {
        nextMiddlewareComponent = next;
        _validPassword = validPassword;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string passwordToCheck = context.Request.Query["password"];

        if (passwordToCheck != _validPassword)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;

            await context.Response.WriteAsync("Wrong password!");
        }
        else
        {
            await nextMiddlewareComponent.Invoke(context);
        }
    }
}
```
Каждый middleware класс должен определять конструктор который принимает аргумент типа `RequestDelegate` (ссылка на следующий middleware в конвеере). Также наш middleware компонент принимает строку которая будет представлять "правильный пароль" (то с чем мы будем сравнивать всё что будет передавать пользователь).

Также в middleware классе обязательно должен быть определён метод `Invoke()` или `InvokeAsync()`. Этот метод обязательно должен возвращать тип `Task` и принимать объект типа `HttpContext` как аргумент. Данный метод вызывается когда очередь в конвеере доход до данного middleware компонента. Данный метод и будет обрабатывать запрос.

Суть данного метода в том чтобы получить параметр с названием `password` из строки запроса, и сравнить его со значением которое считается правильным паролем (переменная инициализированная значением из конструктора). Затем, в случае совпадения значений, вызывается следующий компонент middleware из конвейера. В случае если значение не совпадает, то следующий компонент **не** вызывается, для ответа устанавливается HTTP код 403 (запрещено), и в тело ответа записывается строка с сообщением о неправильном пароле.

Далее добавим данное middleware в конвеер с помощью метода `UseMiddleware()`:
```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        string correctPassword = "1111";

        app.UseMiddleware<PasswordMiddleware>(correctPassword);

        app.Run(async context =>
        {
            await context.Response.WriteAsync("You're authorized!");
        });
    }
}
```
Результат клиентских запросов (с параметром и без) приведен на следующем скриншоте:

![Использование пользовательского middleware класса](../screenshots/ex01/ex01-04.png)

## Взаимодействие нескольких пользовательских middleware компонентов (классов)
Для обработки запросов, как правило, используется не один, а целый ряд компонентов middleware. И в этом случае большую роль может играть их порядок размещения в конвейере обработки запросов, а также то как они взаимодействуют друг с другом.

Кроме того, как уже было описано выше, каждый middleware компонент может обрабатывать запрос до и после последующих в конвейере компонентов. Данное свойство позволяет предыдущим компонентам корректировать результат обработки последующих компонентов.

Рассмотрим следующий пример с определением нескольких middleware компонентов и их взаимодействие. Определим следующий middleware компонент - класс `RoutingMiddleware`:
```cs
public class RoutingMiddleware
{
    public RoutingMiddleware(RequestDelegate next) { }

    public async Task InvokeAsync(HttpContext context)
    {
        string requestedPath = context.Request.Path.Value.ToLower();

        switch (requestedPath)
        {
            case "/index":
                await context.Response.WriteAsync("You've accessed index page!");
                break;
            case "/about":
                await context.Response.WriteAsync("You've accessed the 'about' page!");
                break;
            default:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                break;
        }
    }
}
```
Данное middleware сопоставляет запрашиваемый путь с определёнными маршрутами (строками) и, либо возвращает строку, либо устанавливает код ошибки (404, не найдено) если запрошенный маршрут не совпал ни с одной из строк определённых в `switch` блоке.

Теперь, допустим, нам нужно чтобы пользователь был аутентифицирован перед тем как получит доступ к маршрутам из middleware определённого выше. Для этого добавим новый middleware компонент `JwtAuthenticationMiddleware`:
```cs
public class JwtAuthenticationMiddleware
{
    readonly RequestDelegate _next;

    readonly AppJwtData _appJwtData;

    static readonly string _authorizationHeaderName = "Authorization";

    public JwtAuthenticationMiddleware(
        RequestDelegate next,
        AppJwtData appJwtData)
    {
        _next = next;
        _appJwtData = appJwtData;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        bool containsAuthHeader = context.Request.Headers.ContainsKey(_authorizationHeaderName);

        if (containsAuthHeader)
        {
            string authHeader = context.Request.Headers[_authorizationHeaderName];

            string jwt = GetJwt(authHeader);

            var tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken validatedToken = null;

            try
            {
                tokenHandler.ValidateToken(
                    token: jwt,
                    validationParameters: GetTokenValidationParameters(),
                    validatedToken: out validatedToken);
            }
            catch (Exception)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }

            if (context.Response.StatusCode == StatusCodes.Status200OK)
            {
                await _next.Invoke(context);
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }

    // Выделяет JWT из значения заголовка 'Authorization'.
    string GetJwt(string rawAuthHeaderValue)
    {
        // Валидация размера JWT хедера
        int minRawAuthLength = 10;
        if (string.IsNullOrWhiteSpace(rawAuthHeaderValue) ||
            rawAuthHeaderValue.Length < minRawAuthLength)
            return null;

        // Удаляем приставку 'Bearer' и возвращаем готовый JWT
        return rawAuthHeaderValue.Replace("Bearer ", string.Empty);
    }

    // Возращает объект c установленными параметрами для валидации JWT.
    TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_appJwtData.SecretKey)),

            ValidateIssuer = true,
            ValidIssuer = _appJwtData.Issuer,

            ValidateAudience = true,
            ValidAudience = _appJwtData.Audience,

            ValidateLifetime = false
        };
    }
}
```
Данный middleware компонент представляет аутентификацию с помощью JWT ([JSON Web Token](https://ru.wikipedia.org/wiki/JSON_Web_Token)). JWT не является главным фокусом данного примера. Для понимания данного примера middleware компонентов достаточно абстрактного понимания того что JWT это лишь некоторая строка, которая содержит некоторую закодированную информацию (в т.ч. информацию о клиенте) в формате `xxxxx.yyyyy.zzzzz`, где `x` это заголовок, `y` это полезная нагрузка токена (или просто содержимое) и `z` подпись токена. После login сервер генерирует и возвращает клиенту новый токен. Клиент при запросах передаёт данный токен в заголовке запроса (в заголовке `Authorization`). Сервер проводит валидацию подписи токена для проверки его аутентичности, и предоставляет доступ пользователю к ресурсам требующим авторизации. Подробнее о JWT и аутентификации с помощью JWT можно прочитать в отрытых онлайн источниках. Например [здесь](https://jwt.io/introduction/).

При обработке запроса (в методе `InvokeAsync()`) данный  middleware компонент:
1. Получает значение заголовка `Authorization` из запроса.
1. Извлекает значение JWT из значения заголовка.
1. Проводит валидацию полученного JWT.
1. Вызывает следующий компонент конвейера.

Однако, в случае если нужный заголовок в запросе отсутствует, то следующее middleware НЕ вызывается, и для ответа устанавливается HTTP код 401 (не авторизован). И в случае неудачной попытки валидации токена следующее middleware также НЕ вызывается, и для ответа устанавливается HTTP код 400 (плохой запрос).

Данный middleware компонент в конструкторе принимает объект типа `AppJwtData`, который представляет значения которые используются для валидации JWT получаемого от клиента:
```cs
public class AppJwtData
{
    // Секретный ключ которым подписывается JWT.
    public string SecretKey { get; set; }

    // Издатель JWT.
    public string Issuer { get; set; }

    // Тот для кого предназначается изданный JWT.
    public string Audience { get; set; }
}
```
Класс `AppJwtData` добавляется в коллекцию сервисов приложения, поэтому он доступен для получения через конструктор класса в любом месте приложения. Данный класс получает значения из файла настроек приложения (*appsettings.json*), которые определены там для избежания одной из плохой практик програмирования - хардкода:
```json
{
  "AppJwtData": {
    "SecretKey": "example-01-jwt-secret-key",
    "Issuer": "example-01-asp-net-core-project",
    "Audience": "those-who-study-dotnet"
  }
}
```
Значения из данного JSON файла привязываются к классу `AppJwtData` с помощью следующего метода расширения который вызывается в методе `Startup.ConfigureServices()`:
```cs
public static class Extensions
{
    public static void BindAppJwtData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var appJwtData = new AppJwtData();

        configuration.GetSection("AppJwtData").Bind(appJwtData);

        services.AddSingleton(appJwtData);
    }
}
```

Теперь добавим наши компоненты middleware в конвейер. Поскольку компонент `RoutingMiddleware` не должен обрабатывать запрос если пользователь не аутентифицирован, то компонент `JwtAuthenticationMiddleware` должен стоять в конвейере перед компонентом `RoutingMiddleware`:
```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<JwtAuthenticationMiddleware>();

        app.UseMiddleware<RoutingMiddleware>();
    }
}
```

Сейчас при обращении по адресу `https://localhost:44331` мы увидим следующий результат:

![запрос без JWT](../screenshots/ex01/ex01-05.png)

Мы видим что статус код ответа - 401 (не авторизован), т.к. мы не передали никакого JWT в заголовке запроса. Однако мы хотим видеть ещё хоть какое-то текстовое сообщение об ошибке. Для этого добавим ещё один middleware компонент, который будет отвечать за обработку ошибок:
```cs
public class ErrorHandlingMiddleware
{
    readonly RequestDelegate _next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next.Invoke(context);

        switch (context.Response.StatusCode)
        {
            case StatusCodes.Status400BadRequest:
                await context.Response.WriteAsync("Error: Invalid JWT.");
                break;
            case StatusCodes.Status401Unauthorized:
                await context.Response.WriteAsync("Error: There's no JWT in the request header.");
                break;
            case StatusCodes.Status404NotFound:
                await context.Response.WriteAsync("Error: Page not found.");
                break;
            default:
                break;
        }
    }
}
```
Данное middleware будет стоять в начале конвейера, продолжать работу конвейера, и затем по резульату работы остальных компонентов обрабатывать возможные ошибки. Соответственно здесь, в методе `InvokeAsync()`, сразу вызывается следующий компонент, и уже после него определена логика обработки ошибок. В качестве этого блока в данном примере определён простой блок `switch`, который по установленному в ответе статус коду ошибки добавляет в тело ответа соответствующую строку с описанием ошибки.

Теперь добавим данное middleware в конвейер к остальным компонентам:
```cs
public class Startup
{
    public void Configure(IApplicationBuilder app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();

        app.UseMiddleware<JwtAuthenticationMiddleware>();

        app.UseMiddleware<RoutingMiddleware>();
    }
}
```

Следующая схема демонстрирует получившийся в результате конвейер обработки запросов: 

![общая схема получившегося конвейера](../screenshots/ex01/ex01-06.png)

Однако, в случае если к пользователь сделает запрос без JWT, или в случае если JWT не пройдёт валидацию, то `JwtAuthenticationMiddleware` не будет передавать обработку запроса дальше, а конвейер обработки запросов сработает по следующему принципу:

![схема работы конвеера в случае неудачной аутентификации](../screenshots/ex01/ex01-07.png)

В случае приведённом на диаграмме выше, пользователь увидит в браузере следующий результат:

![запрос без JWT с сообщенем об ошибке](../screenshots/ex01/ex01-08.png)

Для демонстрации успешного результата работы всего конвейера целиком, сделаем запрос с добавлением валидного JWT в заголовок запроса. Для этого сгенерируем новый JWT ([с помощью онлайн сервиса](https://jwt.io/)) со значениями соответствующими тем что определены в файле *appsettings.json*:

![генерация JWT с помощью онлайн сервиса](../screenshots/ex01/ex01-09.png)

В результате имеем следующий JWT: `eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJleGFtcGxlLTAxLWFzcC1uZXQtY29yZS1wcm9qZWN0IiwiYXVkIjoidGhvc2Utd2hvLXN0dWR5LWRvdG5ldCJ9.wq1zJGOTh4H0JjwGiy8PJ8c--HVskP1OIbHHg4e_6-4`

У данного JWT не истечёт срок действия. Его можно будет скопировать и проверить им работоспособность данного учебного проекта.

Теперь нужно сделать запрос с подстановкой данного JWT в заголовок. Для удобства отправки запроса с нужным нам заголовком, используем вспомогательную программу ([Postman](https://www.postman.com/)). В результате выполнения запроса с JWT подставленным в заголовок, имеем следующий успешный результат:

![запрос с JWT с помощью Postman](../screenshots/ex01/ex01-10.png)

## Порядок добавления middleware компонентов в конвейер
На следующей схеме показан полный конвейер обработки запросов для приложений <span>ASP.NET</span> Core MVC и Razor Pages. На ней демонстрируется порядок размещения middleware, и этапы на которых добавляется пользовательское middleware в стандартном приложении. Разработчик имеет полный контроль над порядком размещения компонентов middleware в конвейере.

![Порядок middleware компонентов в стандартном приложении](../images/middleware-order.svg)

Middleware эндпоинта с предыдущей схемы выполняет конвейер фильтра для соответствующего типа приложения — MVC или Razor Pages. Ниже приведена схема конвейера фильтров:

![Конвейер фильтра для endpoint middleware](../images/mvc-endpoint-pipeline.svg)

Порядок в котором компоненты middleware добавляются в конвейер определяет порядок в котором эти компоненты вызываются при запросе, и имеют обратный порядок при ответе. Соответственно правильный порядок расстановки middleware компонентов **крайне важен** для обеспечения безопасности, производительности и функциональности веб-приложения.

Подробнее о рекомендациях для правильной расстановки middleware компонентов можно прочитать [на сайте документации Microsoft](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0#middleware-order).

## Полезные ссылки
* [Также читайте подробно о middleware компонентах на сайте документации Microsoft](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-5.0)
* [Полный список встроенных middleware компонентов](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view%253Daspnetcore-5.0=&view=aspnetcore-5.0#built-in-middleware)