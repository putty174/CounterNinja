using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using CounterNinja;

namespace CounterNinja.Unit
{
    public class UnitSystem
    {
        private CounterNinjaScreen screen;
        private Dictionary<int, Unit> units;
        private HashSet<int> dispose;
        private int counter;
        private List<Unit> unitToAdd;

        public UnitSystem(CounterNinjaScreen screen)
        {
            this.screen = screen;
            units = new Dictionary<int, Unit>();
            dispose = new HashSet<int>();
            unitToAdd = new List<Unit>();
            counter = 0;
        }

        #region Accessor methods

        public HashSet<int> Dispose
        {
            get { return dispose; }
        }

        public CounterNinjaScreen CounterNinjaScreen
        {
            get { return screen; }
        }

        public World World
        {
            get { return screen.World; }
        }

        public ScreenManager ScreenManager
        {
            get { return screen.ScreenManager; }
        }

        public Dictionary<int, Unit> Units
        {
            get { return units; }
        }

        #endregion

        public void UnloadContent()
        {
            foreach (KeyValuePair<int, Unit> kvp in units)
                kvp.Value.Dispose();
            units.Clear();
            dispose.Clear();
        }

        /// <summary>
        /// Adds a unit to the system to handle.
        /// Increment the counter.
        /// This cannot be used when the UnitSystem is processing its Unit list. Use AddQueuedUnit instead.
        /// </summary>
        /// <param name="unit">Unit to add</param>
        public void AddUnit(Unit unit)
        {
            unit.AddToUnitSystem(counter);
            units.Add(counter++, unit);
        }

        /// <summary>
        /// This queues a Unit adding to the next update cycle, allowing
        /// new Units to be added during Update.
        /// </summary>
        /// <param name="unit"></param>
        public void AddQueuedUnit(Unit unit)
        {
            unitToAdd.Add(unit);
        }

        /// <summary>
        /// Places the unit on the list to be disposed on next ProcessDispose
        /// </summary>
        /// <param name="id"></param>
        public void DisposeUnit(int id)
        {
            dispose.Add(id);
        }

        public bool IsDisposingUnit(int id)
        {
            return dispose.Contains(id) || !units.ContainsKey(id);
        }

        /// <summary>
        /// Processes all to-be-disposed units.
        /// if these units' Dispose method disposes other units,
        /// they will be disposed here too in the next dispose round.
        /// </summary>
        public void ProcessDispose()
        {
            //We use a temp list incase one of these units
            //has some extra unit it disposes on dispose
            List<int> tempDispose = new List<int>(dispose);
            foreach (int toRemove in tempDispose)
            {
                if (units.ContainsKey(toRemove))
                {
                    units[toRemove].Dispose();
                    units.Remove(toRemove);
                }
                dispose.Remove(toRemove);
            }
        }

        /// <summary>
        /// Calls all the Update methods of units in this system. If paused,
        /// it calls the UpdatePause method. 
        /// If there are Units to add from AddUnitDelayed,
        /// it will be added in this cycle.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="isPaused"></param>
        public void Update(GameTime gameTime, bool isPaused)
        {
            ProcessDispose();
            if (unitToAdd.Count > 0)
            {
                foreach (Unit unit in unitToAdd)
                    AddUnit(unit);
                unitToAdd.Clear();
            }
            foreach (KeyValuePair<int, Unit> kvp in units)
                if (!isPaused)
                    kvp.Value.Update(gameTime);
                else
                    kvp.Value.UpdatePause(gameTime);
        }

        /// <summary>
        /// This calls all the Draw methods of units handled by this system.
        /// </summary>
        public void Draw()
        {
            foreach (KeyValuePair<int, Unit> kvp in units)
                kvp.Value.Draw();
        }
    }
}
