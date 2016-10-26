using System;
using System.IO.Abstractions;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parsing.Parser(new FileSystem());
            var adventure = parser.Parse(@"C:\dev\GamePorts\ColossalCave\ConsoleApplication1\adven.dat").StartGame();

            while (true)
            {
                Console.WriteLine(adventure.CurrentLocationText);

                var input = Console.ReadLine().Trim();

                var responses = adventure.ProcessInput(input);
                foreach (var response in responses)
                {
                    Console.WriteLine(response);
                }
            }
        }
    }
}
