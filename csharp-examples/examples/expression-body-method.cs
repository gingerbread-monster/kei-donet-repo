using System;
namespace csharp_examples.examples
{
    class ExpressionBodyMethodExample
    {
        public void Run()
        {
            Console.WriteLine( GetOne() );
            Console.WriteLine( GetTwo() );

            PrintSomething();

            Nothing();
        }

        int GetOne()
        {
            return 1;
        }

        int GetTwo() => 2;

        void PrintSomething() => Console.WriteLine("Something");

        void Nothing() => string.IsNullOrEmpty(string.Empty);
    }
}