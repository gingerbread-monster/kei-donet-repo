using System;
namespace csharp_examples.examples
{
    class AnonTypesExample
    {
        public void Run()
        {
            var anonObject = new {Name = "Vasya", Age = 25};

            Console.WriteLine(anonObject.Name);
            Console.WriteLine(anonObject.Age);

            // Line below will cause an error.
            //anonObject.Age = 1;

            var product = new Product();

            var anotherObject = new {Item = "candy", product.Price};

            Console.WriteLine(
                $"item : {anotherObject.Item}, price : {anotherObject.Price}");
        }

        class Product
        {
            public decimal Price = 10.01m;
        }
    }
}