# 01: Конвейер обработки запросов и middleware
06/11/2020 | C# 8 | <span>ASP.NET</span> Core 3.1
___
### Содержание
* [Введение](#Введение)
* [Самый простой middleware компонент](#Самый-простой-middleware-компонент)
* [Последовательная работа нескольких middleware компонентов](#Последовательная-работа-нескольких-middleware-компонентов)
* [Разветвление middleware маршрутами](#Разветвление-middleware-маршрутами)
* [Жизненный цикл middleware](#Жизненный-цикл-middleware)
* [Использование пользовательского middleware класса](#Использование-пользовательского-middleware-класса)
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

Для создания компонентов middleware используется делегат [`RequestDelegate`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.requestdelegate?view=aspnetcore-3.1), который выполняет некоторое действие и принимает контекст запроса (тип [`HttpContext`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext?view=aspnetcore-3.1)):
```cs
public delegate Task RequestDelegate(HttpContext context);
```

Добавление элементов в конвейер происходит в методе `Configure()` класса `Startup`. Компоненты middleware добавляются в конвейер с помощью методов расширения  `Use()`, `Run()`, и `Map()` типа [`IApplicationBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder?view=aspnetcore-3.1).

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
С помощью метода расширения `Map()` возможно определить несколько "ветвей" middleware компонентов для выполнения запросов. Первым аргументов метод `Map()` принимает строку с маршрутом. Вторым аргументом передается делагат с аргументом типа [`IApplicationBuilder`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.iapplicationbuilder?view=aspnetcore-3.1) (для настройки и определения middleware для этого маршрута). Если при сопоставлении запрашиваемого маршрута с переданной в `Map()` строкой совпадут, то будет выполнена часть middleware определенная в `Map()`.

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

## Порядок добавления middleware компонентов в конвейер
На следующей схеме показан полный конвейер обработки запросов для приложений <span>ASP.NET</span> Core MVC и Razor Pages. На ней демонстрируется порядок размещения middleware, и этапы на которых добавляется пользовательское middleware в стандартном приложении. Разработчик имеет полный контроль над порядком размещения компонентов middleware в конвейере.

![Порядок middleware компонентов в стандартном приложении](../images/middleware-order.svg)

Middleware эндпоинта с предыдущей схемы выполняет конвейер фильтра для соответствующего типа приложения — MVC или Razor Pages. Ниже приведена схема конвейера фильтров:

![Конвейер фильтра для endpoint middleware](../images/mvc-endpoint-pipeline.svg)

Порядок в котором компоненты middleware добавляются в конвейер определяет порядок в котором эти компоненты вызываются при запросе, и имеют обратный порядок при ответе. Соответственно правильный порядок расстановки middleware компонентов **крайне важен** для обеспечения безопасности, производительности и функциональности веб-приложения.

Подробнее о рекомендациях для правильной расстановки middleware компонентов можно прочитать [на сайте документации Microsoft](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1#middleware-order).

## Полезные ссылки
* [Также читайте подробно о middleware компонентах на сайте документации Microsoft](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1)
* [Полный список встроенных middleware компонентов](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view%253Daspnetcore-3.1=&view=aspnetcore-3.1#built-in-middleware)