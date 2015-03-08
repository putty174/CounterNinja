using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterNinja.Unit
{
    public enum PState
    {
        Jumping,
        Standing,
        Falling,
        Running,
        Crouching,
        Catch
    }

    public class PlayerState
    {
        private HashSet<PState> states;

        public PlayerState()
        {
            states = new HashSet<PState>();
        }

        public bool HasState(PState state)
        {
            return states.Contains(state);
        }

        public void RemoveState(PState state)
        {
            states.Remove(state);
        }

        public void AddState(PState state)
        {
            states.Add(state);
        }

        public void ClearStates()
        {
            states.Clear();
        }
    }
}
