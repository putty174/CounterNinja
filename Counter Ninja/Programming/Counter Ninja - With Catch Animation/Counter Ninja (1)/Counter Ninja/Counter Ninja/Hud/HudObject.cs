using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CounterNinja.Hud
{
    public class HudObject
    {
        private HudSystem hudSystem;

        public HudObject(HudSystem hudSystem)
        {
            this.hudSystem = hudSystem;
        }

        #region Accessor methods

        public HudSystem HudSystem
        {
            get { return hudSystem; }
        }

        public ScreenManager ScreenManager
        {
            get { return hudSystem.ScreenManager; }
        }

        #endregion

        public virtual void Dispose()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void UpdatePause(GameTime gameTime)
        {
        }

        public virtual void Draw()
        {
        }
    }
}
