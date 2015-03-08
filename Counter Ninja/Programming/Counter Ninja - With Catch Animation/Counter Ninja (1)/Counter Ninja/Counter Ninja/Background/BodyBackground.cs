using System;
using System.Collections.Generic;
using System.Linq;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CounterNinja;

namespace CounterNinja.Background
{
    public class BodyBackground 
    {
        private Body backgroundBody;
        private Vector2 originA;
        private Texture2D background;
        private CounterNinjaScreen screen;
        private List<BackgroundOverlay> listOverlay;

        public BodyBackground(CounterNinjaScreen screen, string bgFile, string bgBodyFile)
        {
            this.screen = screen;
            listOverlay = new List<BackgroundOverlay>();
            background = ScreenManager.Content.Load<Texture2D>(bgFile);
            Texture2D backgroundB = ScreenManager.Content.Load<Texture2D>(bgBodyFile);
            uint[] dataA = new uint[backgroundB.Width * backgroundB.Height];
            backgroundB.GetData(dataA);
            Vertices textureVerticesA = PolygonTools.CreatePolygon(dataA, backgroundB.Width, true);
            //Vector2 centroidA = -textureVerticesA.GetCentroid();
            //textureVerticesA.Translate(ref centroidA);
            originA = Vector2.Zero;//-centroidA;
            List<Vertices> listA = EarclipDecomposer.ConvexPartition(textureVerticesA);
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1));
            foreach (Vertices vertices in listA)
                vertices.Scale(ref vertScale);

            backgroundBody = BodyFactory.CreateCompoundPolygon(screen.World, listA, 1f, BodyType.Static);
            backgroundBody.Friction = 5f;
            backgroundBody.Position = Vector2.Zero;//new Vector2(ConvertUnits.ToSimUnits(backgroundB.Width/2), ConvertUnits.ToSimUnits(backgroundB.Height/2));
        }

        #region Accessors/Modifiers

        public ScreenManager ScreenManager
        {
            get { return screen.ScreenManager; }
        }

        public Body Body
        {
            get { return backgroundBody; }
        }

        public Texture2D Texture
        {
            get { return background; }
        }

        #endregion

        /// <summary>
        /// Adds an overlay to the background.
        /// </summary>
        /// <param name="overlayFile">Filename of the image to use for overlay</param>
        /// <param name="layerDepth">Layer depth the overlay should be drawn on</param>
        public void AddOverlay(string overlayFile, float layerDepth)
        {
            BackgroundOverlay overlay = new BackgroundOverlay(screen, backgroundBody, originA, overlayFile, layerDepth);
            listOverlay.Add(overlay);
        }

        /// <summary>
        /// Draws this onto the screen.
        /// </summary>
        public void Draw()
        {
            ScreenManager.SpriteBatch.Draw(background, ConvertUnits.ToDisplayUnits(backgroundBody.Position),
                null, Color.White, backgroundBody.Rotation, originA, 1f, SpriteEffects.None, 0f);
            foreach (BackgroundOverlay overlay in listOverlay)
                overlay.Draw();
        }
    }
}
