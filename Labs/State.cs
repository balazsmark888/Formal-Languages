using System;
using System.Collections.Generic;

namespace Labs
{
    public class State
    {
        public int Id { get; set; }
        public bool IsTerminal { get; set; }
        public bool IsStartingState { get; set; }
        public bool IsVisited { get; set; }
        public List<Tuple<State, List<char>>> OutgoingStates { get; set; } = new List<Tuple<State, List<char>>>();
        public List<Tuple<State, List<char>>> IncomingStates { get; set; } = new List<Tuple<State, List<char>>>();
        public State Parent { get; set; }
    }
}
