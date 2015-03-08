using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterNinja.Unit
{
    public enum EState
    {
        Walking,
        Attacking,
        Jumping,
        Standing,
        Falling,
        Running
    }

    public class EnemyState
    {
        private HashSet<EState> states;

        public EnemyState()
        {
            states = new HashSet<EState>();
        }

        public bool HasState(EState state)
        {
            return states.Contains(state);
        }

        public void RemoveState(EState state)
        {
            states.Remove(state);
        }

        public void AddState(EState state)
        {
            states.Add(state);
        }

        public void ClearStates()
        {
            states.Clear();
        }
    }
}
