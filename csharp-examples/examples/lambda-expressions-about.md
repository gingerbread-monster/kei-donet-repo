# Лямбда-выражения
06/10/2020 | C# 8
___
## "Эволюция" делегатов в C#
Имеем следующий пример:
```cs
delegate void PrintMessage(string message);

public void Run()
{
    PrintMessage del1 = PrintConsoleMessage;

    PrintMessage del2 = delegate(string m) { Console.WriteLine(m); };

    PrintMessage del3 = (m) => Console.WriteLine(m);

    del1("delegate initialized by method address");
    del2("delegate initialized by anonymous method");
    del3("delegate initialized by lambda expression");
}

void PrintConsoleMessage(string msg)
{
    Console.WriteLine(msg);
}
```

В C# 1.0 мы создавали экземпляр делегата с помощью явной инициализации уже объявленным в коде методом:
```cs
PrintMessage del1 = PrintConsoleMessage;
```

В C# 2.0 были представлены анонимные методы как способ написать безымянный statement блок кода, который может быть выполнен при вызове делегата:
```cs
PrintMessage del2 = delegate(string m) { Console.WriteLine(m); };
```

В C# 3.0 были представлены лябмда-выражения, которые концептуально подобны анонимным методам, но при этом они более выразительны и лаконичны:
```cs
PrintMessage del3 = (m) => Console.WriteLine(m);
```

## Определение лямбда-выражения
Лямбда-выражения, по сути, представляют упрощенную запись анонимных методов. Лямбда-выражения позволяют создать емкие лаконичные методы, которые могут возвращать некоторое значение, и которые можно передать в качестве параметров в другие методы.

Лямбда-выражение является выражением любой из следующих двух форм:
* Лямбда-выражения, имеющая выражение в качестве тела:
```
(input-parameters) => expression
```
* Лямда-оператора, имеющая блок операторов в качестве тела:
```
(input-parameters) => { <sequence-of-statements> }
```

Оператор объявления лямбда-выражения `=>` используется для отделения списка параметров лямбда-выражения от исполняемого кода.

Любое лямбда-выражение может быть преобразовано в делегат (тип делегата). Тип делегата, в которое преобразуется лямбда-выражение, определяется типами его аргументов и возвращаемым значением.

## Примеры лямбда-выражений
Имеем следующий пример:
```cs
delegate void Hello();
delegate bool NullCheck(object obj);
delegate int Sum(int x, int y);

public void Run()
{
    Hello helloDelegate = () => Console.WriteLine("Hello!");
    helloDelegate();

    NullCheck nullCheckDelegate = obj => obj is null;
    Console.WriteLine( nullCheckDelegate(null) );

    Sum sumDelegate = (x, y) => x + y;
    Console.WriteLine( sumDelegate(2, 3) );

    var list = new List<int>() {1,2,3};
    list.ForEach(item => Console.WriteLine(item));
}
```

В примере выше `helloDelegate` инициализируется лямбдой без параметров и возвращаемого значения (в соответствии с типом делегата `Hello`). Обратите внимание, даже в том случае если лябмда-выражение не принимает никаких параметров, то часть синтаксиса с пустыми скобками всё равно обязательна. 

Затем `nullCheckDelegate` инициализируется лябмдой которая принимает 1 параметр типа `object` и возвращает тип `bool` со значением которое зависит от того имеет ли пришедший параметр на самом деле значение `null`. Обратите внимание, в случае если в лямбду передается только 1 параметр, то скобки можно опустить.

Затем `sumDelegate` инициализируется лябмдой которая принимает 2 параметра и возвращает их сумму.

В последнем примере создается экземпляр `List<int>` с коллекцией `int` значений. Затем вызывается метод `ForEach()`, который принимает аргумент типа `Action<T>` (делегат). Как уже было упомянуто выше, лямбда выражения могут преобразовываться к любым делегатам (при условии что их аргументы и возвращаемые типы совпадают). Метод `ForEach()` вызывает переданный ему делегат для каждого элемента из своей коллекции. То есть наше лямбда-выражение будет вызвано для каждого из элементов коллекции. И аргумент `item` будет по очереди равен каждому элементу из коллекции.

## Ещё
- Подробнее [о делегатах](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/delegates/) читайте на сайте документации Microsoft.
- Подробнее [о лямбда-выражениях](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/lambda-expressions) читайте на сайте документации Microsoft.
- Подробнее о различиях делегатов `Func`, `Action` и `Predicate` читайте [в обсуждении](https://stackoverflow.com/questions/4317479/func-vs-action-vs-predicate). [Одно из лучших объяснений](https://stackoverflow.com/a/4317512) было предоставлено by Jon Skeet.