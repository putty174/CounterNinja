using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics;
using CounterNinja;
using CounterNinja.Unit;

namespace CounterNinja.Level
{
    public class LevelMap
    {
        private CounterNinjaScreen screen;
        private Sprite map;
        private Sprite dot;
        private Rectangle rectangle;
        private double mapToMainRatio;
        private Vector2 position;

        /// <summary>
        /// Creates an map on the screen that tracks where the player is.
        /// </summary>
        /// <param name="screen">The Game Screen that this map belongs to</param>
        /// <param name="mapFile">Map file name that will be displayed for this</param>
        /// <param name="mapToMainRatio">ratio in pixels showing how small/big this map is compared to the actual map.</param>
        public LevelMap(CounterNinjaScreen screen, string mapFile, double mapToMainRatio)
        {
            this.screen = screen;
            this.mapToMainRatio = mapToMainRatio;
            map = new Sprite(screen.ScreenManager.Content.Load<Texture2D>(mapFile));
            //Vertices verts = PolygonTools.CreateCircle(0.2f, 10);
            dot = new Sprite(screen.ScreenManager.Content.Load<Texture2D>("Hud/reddot"));//new Sprite(screen.ScreenManager.Assets.TextureFromVertices(verts, MaterialType.Pavement, Color.Red, 2f));
            position = new Vector2(Settings2.Default.screenWidth - 125, 125);
            rectangle = new Rectangle((int)position.X, (int)position.Y, 250, 250);
        }

        /// <summary>
        /// Draws the map, and the dot representing the player on the screen.
        /// </summary>
        public void Draw()
        {
            if (!screen.CameraTrackingPlayer)
                return;
            //int halfWidth = rectangle.Width / 2;
            //int halfHeight = rectangle.Height / 2;
            //int halfScreenWidth = Settings2.Default.screenWidth / 2;
            //int halfScreenHeight = Settings2.Default.screenHeight / 2;
            Vector2 dotPos = Vector2.Zero;//Vector2.Add(position, new Vector2(-halfWidth, -halfWidth));
            Vector2 playerPixelPos = ConvertUnits.ToDisplayUnits(screen.Player.Body.Position);
            double xRatio = playerPixelPos.X / (map.Texture.Width * mapToMainRatio);
            double yRatio = playerPixelPos.Y / (map.Texture.Height * mapToMainRatio);

            dotPos = Vector2.Add(position, new Vector2(rectangle.Width / 2, rectangle.Height / 2));
            dotPos = Vector2.Add(dotPos, new Vector2(-(float)(rectangle.Width * xRatio), -(float)(rectangle.Height * yRatio)));
            screen.ScreenManager.SpriteBatch.Draw(map.Texture, rectangle, null,
                Color.White * 0.8f, screen.Camera.Rotation, map.Origin, SpriteEffects.None, 0.998f);
            screen.ScreenManager.SpriteBatch.Draw(dot.Texture, dotPos, null,
                Color.White * 0.9f, 0f, dot.Origin, 1f, SpriteEffects.None, 0.999f);
        }
    }
}
