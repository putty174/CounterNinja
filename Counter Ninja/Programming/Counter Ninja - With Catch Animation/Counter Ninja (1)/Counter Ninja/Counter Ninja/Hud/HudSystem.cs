using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CounterNinja;

namespace CounterNinja.Hud
{
    public class HudSystem
    {
        private CounterNinjaScreen screen;
        private List<HudObject> hudObjects;

        public HudSystem(CounterNinjaScreen screen)
        {
            this.screen = screen;
            hudObjects = new List<HudObject>();
        }

        #region Get/Accessor methods

        public CounterNinjaScreen CounterNinjaScreen
        {
            get { return screen; }
        }

        public ScreenManager ScreenManager
        {
            get { return screen.ScreenManager; }
        }

        #endregion

        public void UnloadContent()
        {
            foreach (HudObject hudObj in hudObjects)
                hudObj.Dispose();
            hudObjects.Clear();
        }

        /// <summary>
        /// Adds a HudObject to be handled.
        /// </summary>
        /// <param name="hudObj"></param>
        public void AddHudObject(HudObject hudObj)
        {
            hudObjects.Add(hudObj);
        }

        public void Update(GameTime gameTime, bool isPaused)
        {
            foreach (HudObject hudObj in hudObjects)
            {
                if (isPaused)
                    hudObj.UpdatePause(gameTime);
                else
                    hudObj.Update(gameTime);
            }
        }

        public void Draw()
        {
            foreach (HudObject hudObj in hudObjects)
                hudObj.Draw();
        }
    }
}
