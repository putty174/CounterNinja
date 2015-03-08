using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using CounterNinja;

namespace CounterNinja.Script
{
    public class ScriptSystem
    {
        private CounterNinjaScreen screen;
        private Dictionary<string, SmartScript> scripts;
        private HashSet<string> dispose;
        private List<SmartScript> scriptQueue;
        private int scriptCount;

        /// <summary>
        /// Creates a scriptsystem that handles all smartscripts in it.
        /// </summary>
        /// <param name="screen">Screen this belongs to.</param>
        public ScriptSystem(CounterNinjaScreen screen)
        {
            this.screen = screen;
            scripts = new Dictionary<string, SmartScript>();
            dispose = new HashSet<string>();
            scriptQueue = new List<SmartScript>();
            scriptCount = 0;
        }

        public void UnloadContent()
        {
            scripts.Clear();
            dispose.Clear();
            scriptQueue.Clear();
        }

        public void AddScript(SmartScript script)
        {
            script.ScriptName += "_" + scriptCount;
            scripts.Add(script.ScriptName, script);
            scriptCount++;
        }

        public void AddScriptQueue(SmartScript script)
        {
            scriptQueue.Add(script);
        }

        /// <summary>
        /// Adds the script to the dispose list.
        /// </summary>
        /// <param name="scriptName"></param>
        public void Dispose(string scriptName)
        {
            dispose.Add(scriptName);
        }

        /// <summary>
        /// Checks to see if there are scripts that have completed and need disposal.
        /// </summary>
        public void CheckDispose()
        {
            foreach (string scriptName in dispose)
            {
                //Console.WriteLine("Disposed SmartScript: " + scriptName);
                scripts.Remove(scriptName);
            }
            dispose.Clear();
        }

        public void CheckQueue()
        {
            foreach (SmartScript script in scriptQueue)
                AddScript(script);
            scriptQueue.Clear();
        }

        public void Update(GameTime gameTime)
        {
            CheckDispose();
            CheckQueue();
            foreach (KeyValuePair<string, SmartScript> scriptPair in scripts)
                scriptPair.Value.Update(gameTime);
        }

        public void UpdatePause(GameTime gameTime)
        {
            CheckDispose();
            CheckQueue();
            foreach (KeyValuePair<string, SmartScript> scriptPair in scripts)
                scriptPair.Value.UpdatePause(gameTime);
        }
    }
}
