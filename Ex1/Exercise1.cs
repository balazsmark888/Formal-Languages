using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using Labs;

namespace Ex1
{
    public class Exercise1
    {
        public static List<State> Automata { get; set; }
        public static List<State> StatesToRemove { get; set; } = new List<State>();

        public static readonly string Path = "D:\\Informatics\\Formal\\lab1\\Labs\\Resources\\test_1.txt";

        public static void Main(string[] args)
        {
            A();
            B();
            Console.WriteLine();
            Methods.PrintAutomata(Automata);
            Console.WriteLine();
            DeleteStatesFromAutomata();
            Methods.PrintAutomata(Automata);
        }

        public static void A()
        {
            Console.WriteLine("a.)");
            Automata = Methods.ReadAutomata(Path);
            foreach (var state in Automata.Where(p => p.IsStartingState))
            {
                Methods.DepthFirstSearch(state);
            }
            StatesToRemove.AddRange(Automata.Where(p => !p.IsVisited));
            foreach (var state in Automata.Where(p => !p.IsVisited))
            {
                Console.WriteLine(state.Id + " is an unreachable state.");
            }
            Console.WriteLine();
        }

        public static void B()
        {
            Console.WriteLine("b.)");
            Automata.ForEach(p => p.IsVisited = false);
            foreach (var state in Automata.Where(p => p.IsTerminal))
            {
                Methods.ReversedDepthFirstSearch(state);
            }
            StatesToRemove.AddRange(Automata.Where(p => !p.IsVisited));
            foreach (var state in Automata.Where(p => !p.IsVisited))
            {
                Console.WriteLine(state.Id + " is a non-productive state.");
            }
        }

        public static void DeleteStatesFromAutomata()
        {
            StatesToRemove.ForEach(p => Methods.DeleteStateFromAutomata(Automata, p));
        }
    }
}
