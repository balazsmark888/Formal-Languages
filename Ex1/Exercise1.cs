using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labs;

namespace Ex1
{
    public class Exercise1
    {
        public static readonly string Path = "D:\\Informatics\\Formal\\lab1\\Ex1\\Resources\\test_1.txt";

        public static void Main(string[] args)
        {
            A();
            B();

        }

        public static void A()
        {
            Console.WriteLine("a.)");
            var automata = Methods.ReadAutomata(Path);
            foreach (var state in automata.Where(p => p.IsStartingState))
            {
                Methods.DepthFirstSearch(state);
            }

            foreach (var state in automata.Where(p => !p.IsVisited))
            {
                Console.WriteLine(state.Id + " is an unreachable state.");
            }
            Console.WriteLine();
        }

        public static void B()
        {
            Console.WriteLine("b.)");
            var automata = Methods.ReadAutomata(Path);
            foreach (var state in automata.Where(p => p.IsTerminal))
            {
                Methods.ReversedDepthFirstSearch(state);
            }

            foreach (var state in automata.Where(p => !p.IsVisited))
            {
                Console.WriteLine(state.Id + " is a non-productive state.");
            }
        }
    }
}
