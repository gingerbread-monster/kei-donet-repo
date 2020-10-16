using System;
using System.Linq;
using System.Collections.Generic;
namespace csharp_examples.examples
{
    class BuiltInDelegatesExample
    {
        public void Run()
        {
            #region Action<> delegate example
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

                Console.WriteLine();
                Action<string, string> myDel = 
                    (text1, text2) => Console.WriteLine(text1 + text2);
                myDel("<3 ", "C#");
            #endregion

            #region Func<> delegate example
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
            #endregion

            #region Predicate<> delegate example
                var numbers = new List<int> {4,13,3,12,8,65,5,2,1,7,6};

                Console.WriteLine("Все числа:");
                foreach (int number in numbers)
                    Console.Write(number);

                
                var evenNumbers = numbers.FindAll(number => number % 2 == 0);

                Console.WriteLine("\nЧётные:");
                foreach (int number in evenNumbers)
                    Console.Write(number);


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
            #endregion
        }
    }

    #region Func<> delegate example related classes
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
    #endregion

    #region Predicate<> delegate example related classes
        class Student
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }
    #endregion
}