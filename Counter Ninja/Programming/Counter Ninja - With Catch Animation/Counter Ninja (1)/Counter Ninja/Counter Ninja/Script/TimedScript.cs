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
    public class TimedScript : SmartScript
    {
        private double startTime;
        private double activationTime;
        private bool active;
        private int repeatCount;
        private int repeatAmount;

        /// <summary>
        /// Creates an active TimedScript.
        /// TimedScripts are basically SmartScripts that can do timed actions.
        /// They have a repeat counter too.
        /// </summary>
        /// <param name="scriptType">Type of script</param>
        /// <param name="scriptName">Name of script</param>
        /// <param name="parameters">Parameters of script</param>
        /// <param name="startTime">Time in seconds of when script was created. Use -1 to set time on first update.</param>
        /// <param name="activationTime">Time in seconds after start time that this script will activate.</param>
        /// <param name="repeatAmount"># of times it repeats. -1 to repeat forever.</param>
        public TimedScript(CounterNinjaScreen screen, ScriptCommand scriptCommand, string scriptName, object[] parameters,
            double startTime, double activationTime, int repeatAmount)
            : base(screen, scriptCommand, scriptName, parameters)
        {
            this.startTime = startTime;
            this.activationTime = activationTime;
            this.repeatAmount = repeatAmount;
            active = true;
            repeatCount = 0;
        }

        #region Get/Set Accessor Methods

        /// <summary>
        /// Enable/disable whether this should run.
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        #endregion

        /// <summary>
        /// Update logic of TimedScript.
        /// If it is time to activate, run ProcessScript.
        /// If it is repeatable, repeat and change counter accordingly.
        /// If it's no longer active, remove it from the system.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (startTime == -1)
                startTime = gameTime.TotalGameTime.TotalSeconds;
            if (!active)
                return;
            double elapseTime = gameTime.TotalGameTime.TotalSeconds - startTime;
            //if (gameTime.IsRunningSlowly)
            //    startTime -= Screen.ScreenManager.Game.TargetElapsedTime.TotalSeconds - gameTime.ElapsedGameTime.TotalSeconds;
            //Console.WriteLine(elapseTime);
            if (elapseTime >= activationTime)
            {
                ProcessScript();
                repeatCount++;
                if (repeatAmount == -1 || repeatCount < repeatAmount)
                    startTime = gameTime.TotalGameTime.Seconds;
                else
                {
                    active = false;
                    Dispose();
                }
            }
        }

        /// <summary>
        /// Keep the timing correct when the game is paused.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdatePause(GameTime gameTime)
        {
            if (startTime == -1)
                startTime = gameTime.TotalGameTime.TotalSeconds;
            if (!active)
                return;
            startTime += gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
