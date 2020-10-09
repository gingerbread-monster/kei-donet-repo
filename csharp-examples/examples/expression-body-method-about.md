# Expression Body Method Example
06/10/2020 | C# 8
___
Существует 2 варианта использования оператора `=>` в C#:
- в лямбда-выражениях
- для сокращённой записи определения методов.

В данном примере показана возможность сокращённой записи тела метода (expression body definition) с помощью оператора `=>`

Следующие определения методов эквивалентны:
```csharp
int GetOne()
{
    return 1;
}

int GetTwo() => 2;
```

Также метод может ничего не возвращать и выполнять одно выражение:
```csharp
void PrintSomething() => Console.WriteLine("Something");
```

Следовательно определение тела-выражения имеет следующий обобщённый синтаксис:
```csharp
member => expression;
```

Возвращаемый тип `expression` (выражения) должен быть неявно приводимым к типу который возвращает `member` (в нашем случае - метод).

Также, если метод ничего не возвращает, то выражение может возвращать любой тип:
```csharp
void Nothing() => string.IsNullOrEmpty(string.Empty);
```

## Ещё
- Читайте [подробнее](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-operator#expression-body-definition) на сайте документации Microsoft.