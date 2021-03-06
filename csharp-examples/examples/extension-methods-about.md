# Методы расширения
12/10/2020 | C# 8
___
## Определение
Методы расширения позволяют "добавить" методы к уже существующим типам без создания производного типа, рекомпиляции, или какой-либо другой модификации оригинального типа.

Методы расширения это статические методы. Но вызываются они из экземпляра расширенного типа так, будто бы они были определены в самом типе. Для клиентского кода написанного на C# нет визуально видимого различия между вызовом метода расширения и вызовом обычного метода что определён внутри данного типа.

Методы расширения бывают особенно полезны когда нам хочется добавить в некоторый тип новый метод, но сам тип (класс или структура) мы изменить не можем, поскольку у нас нет доступа к исходному коду. 

## Пример метода расширения
Метод расширения обязательно должен быть определён в неуниверсальном статическом классе. Для того, чтобы создать метод расширения, вначале нужно создать статический класс, который и будет содержать этот метод. В данном случае это будет класс `MyExtensionMethods`. Затем объявляем статический метод:
```cs
static class MyExtensionMethods
{
    public static string GetFancyDescription(this Array arr)
    {
        return 
            "*** Fancy array description ***\n" +
            $"Type\t\t:\t{arr.GetType().Name}\n" +
            $"Is fixed size\t:\t{arr.IsFixedSize}\n" +
            $"Is readonly\t:\t{arr.IsReadOnly}\n" +
            $"Length\t\t:\t{arr.Length}\n" +
            $"Rank\t\t:\t{arr.Rank}\n";
    }
}
```

Суть данного метода расширения в том чтобы вернуть строку с описанием свойств для текущего экземпляра массива.

В методах расширения первый аргумент всегда уточняет для какого именно типа предназначается данный метод расширения. Параметру всегда обязательно предшествует модификатор `this`. Конструкция имеет следующий синтаксис `this имя-типа название-аргумента`. В нашем случае это `this Array arr`.

Ниже приведён пример использования данного метода расширения:
```cs
var intArray = new int[] {1,3,2};
Console.WriteLine( intArray.GetFancyDescription() );

var stringArray = new string[5,3];
Console.WriteLine( stringArray.GetFancyDescription() );
```

Типы `Int32[]` и `String[]` являются наследниками типа `Array`. Поэтому определённый нами метод расширения может быть вызван для любого типа который наследует `Array`. При этом тип `Array` является абстрактным.
___
Определим ещё один пример использования методов расширения. Имеем следующий класс:
```cs
class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public Person(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}
```

Как видно класс `Person` не имеет никаких методов кроме конструктора. Определим метод расширения для класса `Person`:
```cs
static class MyExtensionMethods
{
    public static string GetFullName(this Person person)
    {
        return $"{person.FirstName} {person.LastName}";
    }
}
```

Суть данного метода расширения в том чтобы вернуть сконкатенированные строковые свойства из класса `Person`. Теперь мы можем вызывать данный метод точно так же если бы он был обычным методом, определённым внутри класса `Person`:
```cs
var person = new Person("Вася", "Петров");
Console.WriteLine( person.GetFullName() );
```

## Полезные ссылки
- Подробнее о методах расширения читайте [на сайте документации Microsoft](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods).
- Также [хорошая статья по данной теме](https://metanit.com/sharp/tutorial/3.18.php) представлена на сайте metanit.com. 