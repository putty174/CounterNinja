using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using CounterNinja;

namespace CounterNinja.Subtitle
{
    public class BasicSubtitle
    {
        private CounterNinjaScreen screen;
        private SubtitleSystem subtitleSystem;
        private string subtitle;

        /// <summary>
        /// Creates a BasicSubtitle.
        /// You shouldn't be using this class to do anything except extend off it.
        /// </summary>
        /// <param name="screen">Screen that this belongs to</param>
        /// <param name="subtitleSystem">SubtitleSystem that this belongs to</param>
        /// <param name="subtitle">String saying what this subtitle says</param>
        public BasicSubtitle(CounterNinjaScreen screen, SubtitleSystem subtitleSystem, string subtitle)
        {
            this.screen = screen;
            this.subtitleSystem = subtitleSystem;
            this.subtitle = subtitle;
        }

        #region Get/Set Accessor

        /// <summary>
        ///  Get/Set the text of this subtitle
        /// </summary>
        public String Subtitle
        {
            get { return subtitle; }
            set { subtitle = value; }
        }

        /// <summary>
        /// Get the screen this belongs to
        /// </summary>
        public CounterNinjaScreen CounterNinjaScreen
        {
            get { return screen; }
        }

        /// <summary>
        /// Get the subtitle system that handles processing this subtitle
        /// </summary>
        public SubtitleSystem SubtitleSystem
        {
            get { return subtitleSystem; }
        }

        #endregion

        /// <summary>
        /// Dispose of this subtitle from the subtitle system that handles it.
        /// </summary>
        public void Dispose()
        {
            subtitleSystem.DisposeSubtitle(this);
        }

        /// <summary>
        /// method to run when the game is paused.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void UpdatePause(GameTime gameTime)
        {
        }

        /// <summary>
        /// Method to run when game is running.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Method to draw this subtitle.
        /// </summary>
        public virtual void Draw()
        {
        }
    }
}
