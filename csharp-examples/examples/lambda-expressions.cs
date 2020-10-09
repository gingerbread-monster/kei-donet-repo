using System;
namespace csharp_examples.examples
{
    class LambdaExpressionsExample
    {
        // Evolution of delegates region
        delegate void PrintMessage(string message);
        void PrintConsoleMessage(string msg)
        {
            System.Console.WriteLine(msg);
        }

        public void Run()
        {
            #region Evolution of delegates
                PrintMessage del1 = PrintConsoleMessage;

                PrintMessage del2 = delegate(string m) { Console.WriteLine(m); };

                PrintMessage del3 = (m) => Console.WriteLine(m);

                del1("delegate initialized by method address");
                del2("delegate initialized by anonymous method");
                del3("delegate initialized by lambda expression");
            #endregion

            #region More of lambdas
                Hello helloDelegate = () => Console.WriteLine("Hello!");
                helloDelegate();

                NullCheck nullCheckDelegate = obj => obj is null;
                Console.WriteLine( nullCheckDelegate(null) );

                Sum sumDelegate = (x, y) => x + y;
                Console.WriteLine( sumDelegate(2, 3) );

                var list = new System.Collections.Generic.List<int>() {1,2,3};
                list.ForEach(item => Console.WriteLine(item));
            #endregion
        }

        // More of lambdas region
        delegate void Hello();
        delegate bool NullCheck(object obj);
        delegate int Sum(int x, int y);
    }
}