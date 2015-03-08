using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CounterNinja;

namespace CounterNinja.Background
{
    public class BodylessBackground
    {
        private Texture2D background;
        private Vector2 position;
        private List<BackgroundOverlay> listOverlay;
        private CounterNinjaScreen screen;

        /// <summary>
        /// Creates a BodylessBackground.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="bgFile"></param>
        /// <param name="position"></param>
        public BodylessBackground(CounterNinjaScreen screen, string bgFile, Vector2 position)
        {
            this.screen = screen;
            listOverlay = new List<BackgroundOverlay>();
            background = ScreenManager.Content.Load<Texture2D>(bgFile);
            this.position = position;
        }

        #region Access methods

        public Texture2D Background
        {
            get { return background; }
        }

        public ScreenManager ScreenManager
        {
            get { return screen.ScreenManager; }
        }

        #endregion

        /// <summary>
        /// Adds an overlay to the background.
        /// </summary>
        /// <param name="overlayFile">Filename of the image to use for overlay</param>
        /// <param name="layerDepth">Layer depth the overlay should be drawn on</param>
        public void AddOverlay(string overlayFile, float layerDepth)
        {
            BackgroundOverlay overlay = new BackgroundOverlay(screen, Vector2.Zero, overlayFile, layerDepth, position);
            listOverlay.Add(overlay);
        }

        /// <summary>
        /// Readjust all the background overlays' positions
        /// </summary>
        public void AdjustBackgroundOverlay()
        {
            foreach (BackgroundOverlay overlay in listOverlay)
                overlay.Position = position;
        }

        /// <summary>
        /// Draws the background
        /// </summary>
        public void Draw()
        {
            ScreenManager.SpriteBatch.Draw(background, ConvertUnits.ToDisplayUnits(position),
                    null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            foreach (BackgroundOverlay overlay in listOverlay)
                overlay.Draw();
        }
    }
}
