using System;
using System.IO;
using Labs;

namespace Ex5
{
    public class Exercise5
    {
        public static readonly string Path = "D:\\Informatics\\Formal\\lab1\\Labs\\Resources\\test_3_szamos.txt";
        public static readonly string DestinationPath = "D:\\Informatics\\Formal\\lab1\\Labs\\Resources\\DotCode.txt";

        public static void Main(string[] args)
        {
            var automata = Methods.ReadAutomata(Path);
            var dotCode = Methods.GenerateDotCodeFromAutomata(automata);
            File.WriteAllText(DestinationPath, dotCode);
        }
    }
}
