using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace lab1
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

        public static List<State> ReadAutomata(string path)
        {
            var lines = File.ReadAllText(path).Split('\n').ToList();
            var ids = lines[0].Split(' ').Select(int.Parse).ToList();
            var states = CreateStatesFromIds(ids);
            var chars = lines[1].Split(' ').ToList();
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
            for (var i = 4; i < lines.Count; i++)
            {
                var values = lines[i].Split(' ');

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
