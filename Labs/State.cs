using System;
using System.Collections.Generic;
using System.Text;

namespace lab1
{
    public class State
    {
        public int Id { get; set; }
        public bool IsTerminal { get; set; }
        public bool IsStartingState { get; set; }
        public bool IsVisited { get; set; }
        public List<Tuple<State, char>> OutgoingStates { get; set; }
        public List<Tuple<State, char>> IncomingStates { get; set; }
        public State Parent { get; set; }
    }
}
