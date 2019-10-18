using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.String;

namespace Labs
{
    public static class Methods
    {
        public static void DepthFirstSearch(State startState)
        {
            startState.IsVisited = true;
            var possibleStates = startState.OutgoingStates.Where(p => !p.Item1.IsVisited).Select(p => p.Item1).ToList();
            foreach (var state in possibleStates.Where(state => !state.IsVisited))
            {
                state.Parent = startState;
                DepthFirstSearch(state);
            }
        }

        public static void ReversedDepthFirstSearch(State startState)
        {
            startState.IsVisited = true;
            var possibleStates = startState.IncomingStates.Where(p => !p.Item1.IsVisited).Select(p => p.Item1).ToList();
            foreach (var state in possibleStates.Where(state => !state.IsVisited))
            {
                state.Parent = startState;
                ReversedDepthFirstSearch(state);
            }
        }

        public static string GenerateDotCodeFromAutomata(List<State> automata)
        {
            return $@"digraph G{{
rankstep = 0.5;
nodestep = 0.5;
rankdir = LR;
node [shape = circle, fontsize = 16];
fontsize = 10;
compound = true;

{TerminalStatesDotTemplate(automata)}

{StateConnectionsDotTemplate(automata)}
}}";
        }

        public static string TerminalStatesDotTemplate(List<State> automata)
        {
            return automata.Where(p => p.IsTerminal)
                .Aggregate(Empty, (current, state) => $@"{current}
{state.Id} [shape = doublecircle];");
        }

        public static string StateConnectionsDotTemplate(List<State> automata)
        {
            var template = StartingStateConnectionsDotTemplate(automata);
            foreach (var state in automata)
            {
                foreach (var (outgoingState, possibleCharacters) in state.OutgoingStates)
                {
                    template = possibleCharacters.Aggregate(template, (current, c) => $@"{current}
{state.Id} -> {outgoingState.Id} [label = {c}]");
                }
            }

            return template;
        }

        public static string StartingStateConnectionsDotTemplate(List<State> automata)
        {
            return automata.Where(p => p.IsStartingState)
                .Aggregate(Empty, (current, state) => $@"{current}
i{state.Id} [shape = point, style = invis];
i{state.Id} -> {state.Id}");
        }

        public static bool IsWordPartOfLanguage(State currentState, string word, int index)
        {
            if (index == word.Length)
            {
                return currentState.IsTerminal;
            }
            var possibleStates = currentState.OutgoingStates.Where(p => p.Item2.Contains(word[index])).ToList();
            return possibleStates.Any(possibleState => IsWordPartOfLanguage(possibleState.Item1, word, index + 1));
        }

        public static List<State> ReadAutomata(string path)
        {
            var lines = File.ReadAllText(path).Split('\n').ToList();
            var states = CreateStatesFromIds(lines[0].Split(' ').Select(int.Parse).ToList());
            BuildPropertiesOfStates(states, lines);
            BuildConnectionsOfStates(states, lines);

            return states;
        }

        public static void DeleteStateFromAutomata(List<State> automata, State state)
        {
            automata.Remove(state);
            automata.ForEach(p =>
            {
                if (p.OutgoingStates.Any(pp => pp.Item1.Id == state.Id))
                {
                    p.OutgoingStates.Remove(p.OutgoingStates.Find(pp => pp.Item1.Id == state.Id));
                }
                if (p.IncomingStates.Any(pp => pp.Item1.Id == state.Id))
                {
                    p.IncomingStates.Remove(p.IncomingStates.Find(pp => pp.Item1.Id == state.Id));
                }
            });
        }

        public static void PrintAutomata(List<State> automata)
        {
            var possibleChars = new List<char>();
            foreach (var state in automata)
            {
                Console.Write(state.Id + " ");
                possibleChars.AddRange(state.OutgoingStates.SelectMany(p => p.Item2));
                possibleChars = possibleChars.Distinct().ToList();
            }
            Console.WriteLine();
            possibleChars.ForEach(p => Console.Write(p + " "));
            Console.WriteLine();
            foreach (var state in automata.Where(p => p.IsStartingState))
            {
                Console.Write(state.Id + " ");
            }
            Console.WriteLine();
            foreach (var state in automata.Where(p => p.IsTerminal))
            {
                Console.Write(state.Id + " ");
            }
            Console.WriteLine();
            foreach (var state in automata)
            {
                foreach (var (outState, charList) in state.OutgoingStates)
                {
                    foreach (var character in charList)
                    {
                        Console.WriteLine($"{state.Id} {character} {outState.Id}");
                    }
                }
            }
        }

        private static void BuildPropertiesOfStates(IReadOnlyCollection<State> states, List<string> lines)
        {
            if (lines == null) throw new ArgumentNullException(nameof(lines));
            var startingIds = lines[2].Split(' ').Select(int.Parse).ToList();
            var terminalIds = lines[3].Split(' ').Select(int.Parse).ToList();
            foreach (var state in states.Where(p => startingIds.Contains(p.Id)))
            {
                state.IsStartingState = true;
            }
            foreach (var state in states.Where(p => terminalIds.Contains(p.Id)))
            {
                state.IsTerminal = true;
            }
        }

        private static void BuildConnectionsOfStates(List<State> states, IReadOnlyList<string> lines)
        {
            for (var i = 4; i < lines.Count; i++)
            {
                var values = lines[i].Split(' ');
                var sourceState = states.Find(p => p.Id == int.Parse(values[0]));
                var destState = states.Find(p => p.Id == int.Parse(values[2]));
                if (sourceState.OutgoingStates.All(p => p.Item1.Id != destState.Id))
                {
                    sourceState.OutgoingStates.Add(new Tuple<State, List<char>>(destState, new List<char>()));
                    destState.IncomingStates.Add(new Tuple<State, List<char>>(sourceState, new List<char>()));

                }
                sourceState.OutgoingStates.Find(p => p.Item1.Id == destState.Id).Item2.Add(values[1][0]);
                destState.IncomingStates.Find(p => p.Item1.Id == sourceState.Id).Item2.Add(values[1][0]);
            }
        }

        private static List<State> CreateStatesFromIds(IEnumerable<int> ids)
        {
            return ids.Select(p => new State
            {
                Id = p,
            }).ToList();
        }
    }
}
