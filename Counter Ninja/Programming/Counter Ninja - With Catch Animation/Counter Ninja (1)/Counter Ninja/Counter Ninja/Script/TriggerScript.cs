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
    public class TriggerScript : SmartScript
    {
        private Body triggerBody;
        private Vector2 position;
        private float triggerDistance;
        private int repeatCount;
        private int repeatAmount;

        /// <summary>
        /// Creates a TriggerScript, which is basically 
        /// a SmartScript that can be triggered by a physics body
        /// that comes in range of it.
        /// </summary>
        /// <param name="screen">screen this belongs to.</param>
        /// <param name="scriptSystem"></param>
        /// <param name="scriptCommand"></param>
        /// <param name="scriptName"></param>
        /// <param name="parameter"></param>
        /// <param name="triggerBody"></param>
        /// <param name="position"></param>
        /// <param name="triggerDistance"></param>
        /// <param name="repeatAmount"></param>
        public TriggerScript(CounterNinjaScreen screen, ScriptCommand scriptCommand, string scriptName, object[] parameter, 
            Body triggerBody, Vector2 position, float triggerDistance, int repeatAmount)
            : base(screen, scriptCommand, scriptName, parameter)
        {
            this.triggerBody = triggerBody;
            this.position = position;
            this.triggerDistance = triggerDistance;
            this.repeatAmount = repeatAmount;
            repeatCount = 0;
        }

        /// <summary>
        /// Handles the basic logic of this TriggerScript.
        /// If the body is within triggering distance, trigger the script.
        /// If repeatable, repeat as it should.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Vector2.Distance(position, triggerBody.Position) <= triggerDistance)
            {
                repeatCount++;
                ProcessScript();
                if (repeatCount >= repeatAmount && repeatAmount != -1)
                    Dispose();
            }
        }
    }
}
