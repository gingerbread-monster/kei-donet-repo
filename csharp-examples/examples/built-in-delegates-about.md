# Встроенные типы делегатов
12/10/2020 | C# 8
___
Чтобы упростить процесс разработки ПО и унифицировать его форму, в платформе .NET уже имеется ряд "встроенных" (определенных платформой) типов делегатов. Рассмотрим наиболее распространенные из них:
* Делегат `Action<>`
* Делегат `Func<>`
* Делегат `Predicate<>`

Их можно использовать вместо определения новых типов делегатов. Между этими типами существуют некоторые различия, связанные с их предназначением.

## Делегат `Action<>`
Делегат `Action<>` используется когда нужно выполнить некоторое действие с использованием аргументов делегата. А метод который он инкапсулирует не возвращает значения.

Делегат `Action<>` является обобщенным. Существует 17 версий данного типа делегата, которые различаются лишь количеством принимаемых аргументов:
* `Action`
* `Action<T>`
* `Action<T1,T2>`
* `Action<T1,T2,T3>`
* ...
* `Action<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>`

Он может не принимать аргументы вовсе (тип `Action`).
Максимальное количество аргументов которое может принимать данный делегат - 16.

Имеем следующий пример использования типа `Action<T>` как аргумента метода:
```cs
public void Run()
{
    string[] names = {"Коваленко", "Трiнько", "Тесленко", "Шевченко"};

    Console.WriteLine("Plain names:");
    foreach (var item in names)
        Console.WriteLine(item);

    void ProcessStringArray(string[] collection, Action<string> del)
    {
        foreach (var item in collection)
        {
            del(item);
        }
    }
    
    Console.WriteLine("\nNames with honorifics:");
    ProcessStringArray(
        names,
        name => Console.WriteLine("Пан " + name));
}
```

Объявляем некоторый массив имён `names`. Далее объявялется локальная функция `ProcessStringArray()` суть которой в обработке принимаемого массива строк (аргумент 1 - `collection`) некоторым принимаемым действием (делегатом, аргумент 2 - `del`). Внутри `ProcessStringArray()` аргумент `del` вызывается для каждого элемента массива `collection`. Затем происходит вызов данной локальной функции с передачей массива имён `names` и лямбда-выражения, которое представляет наше действие над массивом (происходит приведение лямбда-выражения к типу `Action<string>`).

В результате выполнения данного примера массив `names` будет распечатан дважды: сам по себе, и с приставкой "Пан", добавляемой в лямбда-выражении.

Также имеем пример использования делегата `Action` самого по себе, уже не как аргумента метода:
```cs
public void Run()
{
    Action<string, string> myDel = 
        (text1, text2) => Console.WriteLine(text1 + text2);
    myDel("<3 ", "C#");
}
```

В данном примере делегат принимает уже 2 аргумента. Переменная делегата `myDel` инициализируется лямбда-выражением, которое распечатывает принимаемые строки.

## Делегат `Func<>`
Следующий распространенный делегат - `Func<>`. Он возвращает результат действия и может принимать параметры. Аналогично предыдущему делегату он также является обобщённым и имеет несколько форм. Он может принимать от 0 до 16 аргументов:
* `Func<TResult>`
* `Func<T,TResult>`
* `Func<T1,TResult>`
* `Func<T1,T2,TResult>`
* `Func<T1,T2,T3,TResult>`
* ...
* `Func<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,TResult>`

Обратите внимание что последним типом всегда идёт тип возвращаемого результата. `Func<>` обычно используется при наличии преобразования, то есть когда требуется преобразовать аргументы делегата в другой результат. В качестве примера можно привести проекции. Метод, который он инкапсулирует, возвращает указанное значение.

Имеем следующий пример который считает количество гласных букв в строке:
```cs
public void Run()
{
    Func<string, int> countVowels = text => 
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        var vowels = new char[] {'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я'};

        text = text.ToLower();

        int vowelsCount = text.Count(character => 
            vowels.Any(vowel => vowel == character));

        return vowelsCount;
    };

    Console.WriteLine( countVowels("Яблоко") );
}
```

Переменная делегата инициализируется лямбда-выражением. Результатом вызова данного делегата с данным аргументом будет число 3. Аналогично предыдущим примерам данный тип делегата мог бы быть аргументом метода.

Далее имеем следующий пример использования делегата `Func<>`:
```cs
class PetOwner
{
    public string Name { get; set; }
    public ICollection<Animal> Pets { get; set; }
}

class Animal
{
    public string Name { get; set; }
    public string Type { get; set; }
}

public void Run()
{
    var petOwners = new PetOwner[]
    {
        new PetOwner
        {
            Name = "Вася",
            Pets = new List<Animal>
            {
                new Animal
                {
                    Name = "Мармелад",
                    Type = "Кот"
                },
                new Animal
                {
                    Name = "Золотко",
                    Type = "Золотая рыбка"
                }
            }
        },
        new PetOwner
        {
            Name = "Петя",
            Pets = new List<Animal>
            {
                new Animal
                {
                    Name = "Пончик",
                    Type = "Хомячок"
                }
            }
        }
    };

    var petOwnwersWithTheirPetNames = petOwners
        .Select(owner => new {
            owner.Name,
            petNames = owner.Pets.Select(pet => pet.Name)
        });

    foreach (var petOwnerWithHisPetNames in petOwnwersWithTheirPetNames)
    {
        Console.WriteLine($"\nСчастливый {petOwnerWithHisPetNames.Name}");
        Console.WriteLine("и его любимцы:");

        foreach (string petName in petOwnerWithHisPetNames.petNames)
            Console.Write($"{petName} ");

        Console.WriteLine();
    }
}
```

Суть данного примера в том чтобы выбрать имя владельця питомцев и имена питомцев в один анонимный объект.

Определены 2 класса: `PetOwner` и `Animal`. Затем они инициализируются некоторыми значениями. Далее происходит следующее действие: 
```cs
var petOwnwersWithTheirPetNames = petOwners
    .Select(owner => new {
        owner.Name,
        petNames = owner.Pets.Select(pet => pet.Name)
    });
```

Здесь мы с помощью метода расширения `Select()` (который определен в `System.LINQ`), который в качестве аргумента принимает делегат `Func<>`, делаем проекцию и создаём новый анонимный объект. Новый анонимный объект состоит из имени владельца питомцев (`owner.Name`) и коллекции строк с именами питомцев этого владельца (`owner.Pets.Select(pet => pet.Name)`). После этого мы просто распечатываем результат содержащийся в анонимных объектах.

## Делегат `Predicate<>`
Следующий распространенный делегат `Predicate<>`, как правило, используется для сравнения. Возвращаемым типом данного делегата всегда выступает `bool`.

`Predicate<>` используется когда требуется определить удовлетворяет ли аргумент условию делегата. Он также может быть записан как `Func<T, bool>`, что означает, что метод возвращает логическое значение.

Имеем следующий пример:
```cs
public void Run()
{
    var numbers = new List<int> {4,13,3,12,8,65,5,2,1,7,6};

    Console.WriteLine("Все числа:");
    foreach (int number in numbers)
        Console.Write(number);

    
    var evenNumbers = numbers.FindAll(number => number % 2 == 0);

    Console.WriteLine("\nЧётные:");
    foreach (int number in evenNumbers)
        Console.Write(number);
}
```

Метод `FindAll()` принимает делегат типа `Predicate<T>` и возвращает новую коллекцию на основании того на каких элементах коллекции из предиката (делегата) было возвращено `true`. То есть данный метод возвращает все элементы данной коллекции которые удовлетворяют условию которое описано внутри делегата.

Имеем следующий пример:
```cs
class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
}

public void Run()
{
    var students = new List<Student>
    {
        new Student {Name = "Вася", Age = 25},
        new Student {Name = "Петя", Age = 18},
        new Student {Name = "Коля", Age = 16},
        new Student {Name = "Миша", Age = 14}
    };

    string GetStudentInfo(Student student) => 
        $"Имя : {student.Name}, Возраст : {student.Age}";

    Console.WriteLine("\nВсе ученики:");
    foreach (var student in students)
        Console.WriteLine( GetStudentInfo(student) );

    students.RemoveAll(student => student.Age < 18);

    Console.WriteLine("\nСовершеннолетние ученики:");
    foreach (var student in students)
        Console.WriteLine( GetStudentInfo(student) );
}
```

Имеем класс `Student` который описывает имя и возраст ученика. Метод `RemoveAll()` также принимает делегат типа `Predicate<T>`, и удаляет из коллекции все элементы которые удовлетворяют условию описанному внутри делегата.

## Полезные ссылки
- Подробнее о встроенных делегатах читайте [на сайте документации](https://docs.microsoft.com/en-us/dotnet/standard/delegates-lambdas) Microsoft.
- Также [хорошая статья по данной теме](https://metanit.com/sharp/tutorial/3.33.php) представлена на сайте metanit.com. 
- Подробнее о различиях делегатов `Func`, `Action` и `Predicate` читайте [в обсуждении](https://stackoverflow.com/questions/4317479/func-vs-action-vs-predicate). [Одно из лучших объяснений](https://stackoverflow.com/a/4317512) было предоставлено пользователем Jon Skeet.