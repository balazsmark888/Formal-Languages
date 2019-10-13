using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            BuildProperties(states, lines);
            BuildConnections(states, lines);

            return states;
        }

        private static void BuildProperties(IReadOnlyCollection<State> states, List<string> lines)
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

        private static void BuildConnections(List<State> states, IReadOnlyList<string> lines)
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
