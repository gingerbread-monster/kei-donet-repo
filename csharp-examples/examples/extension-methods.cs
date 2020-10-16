using System;

namespace csharp_examples.examples
{
    class ExtensionMethodsExample
    {
        public void Run()
        {
            var intArray = new int[] {1,3,2};
            Console.WriteLine( intArray.GetFancyDescription() );

            var stringArray = new string[5,3];
            Console.WriteLine( stringArray.GetFancyDescription() );

            var person = new Person("Вася", "Петров");
            Console.WriteLine( person.GetFullName() );
        }
    }

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

        public static string GetFullName(this Person person)
        {
            return $"{person.FirstName} {person.LastName}";
        }
    }

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
}