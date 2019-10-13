using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labs;

namespace Ex2
{
    public class Exercise2
    {
        public static readonly string Path = "D:\\Informatics\\Formal\\lab1\\Labs\\Resources\\test_3_szamos.txt";

        public static void Main(string[] args)
        {
            var automata = Methods.ReadAutomata(Path);
            Console.WriteLine("Enter word:");
            var word = Console.ReadLine();
            Console.WriteLine(automata.Where(p => p.IsStartingState).Any(p => Methods.IsWordPartOfLanguage(p, word, 0))
                ? "The word is part of the language."
                : "The word is not part of the language.");
        }
    }
}
