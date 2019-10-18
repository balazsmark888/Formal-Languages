using System;
using System.Linq;
using Labs;

namespace Ex2
{
    public class Exercise3
    {
        public static readonly string Path = "D:\\Informatics\\Formal\\lab1\\Labs\\Resources\\test_3_szamos.txt";

        public static void Main(string[] args)
        {
            var automata = Methods.ReadAutomata(Path);
            var word = string.Empty;
            while (word != null && !word.Equals("end", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.WriteLine("Enter word:");
                word = Console.ReadLine();
                Console.WriteLine(automata.Where(p => p.IsStartingState).Any(p => Methods.IsWordPartOfLanguage(p, word, 0))
                    ? "The word is part of the language."
                    : "The word is not part of the language.");
            }
        }
    }
}
