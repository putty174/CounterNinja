using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using CounterNinja;
using CounterNinja.Unit;
using CounterNinja.Level;

namespace FarseerPhysics.GameLevel
{
    internal class Level1 : CounterNinjaScreen 
    {
        #region IDemoScreen Members

        public string GetTitle()
        {
            return "Level Sample";
        }

        public string GetDetails()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("This is a test of the engine and level.");
            sb.AppendLine(string.Empty);
            return sb.ToString();
        }

        #endregion

        public Level1()
            : base()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            Player.Body.Position = ConvertUnits.ToSimUnits(new Vector2(951, 2015));

            MusicSystem.Repeat = true;
            //ObjectSystem.AddObject("object/white_box_medium", "medium_block", Player.Body.Position, 0.9f, 1f);
            BackgroundSystem.AddScaledBackgroundBody("Level/Level2/Level2_v2_Body", 2f);
            BackgroundSystem.AddBodylessBackgroundSet("Level/Level2/Background/Level2_v2", 1, 16, 4);

            CreatePyramid(ConvertUnits.ToSimUnits(new Vector2(900, 1900)), 15, 1, ConvertUnits.ToSimUnits(5f));
            CreateRectangle(ConvertUnits.ToSimUnits(new Vector2(950, 1800)), ConvertUnits.ToSimUnits(30f),
                ConvertUnits.ToSimUnits(50f));
            
            //MusicSystem.PlaySong("Music/2DPolygon___Welcome_In_Uto_2");
            //MusicSystem.AddSong("Music/CelestialTearsF6");
            CameraTrackPlayer(true);
        }


        /// <summary>
        /// Creates a simple rectangle.
        /// </summary>
        /// <param name="position">position in simulation unit</param>
        /// <param name="width">width in simulation unit</param>
        /// <param name="height">hight in simulation unit</param>
        public void CreateRectangle(Vector2 position, float width, float height)
        {
            Body body = BodyFactory.CreateRectangle(World, width, height, 1f);
            Sprite boxSprite = new Sprite(ScreenManager.Assets.TextureFromShape(body.FixtureList[0].Shape,
                MaterialType.Squares, Color.White, 1f));
            body.BodyType = BodyType.Dynamic;
            body.Position = position;
            body.SleepingAllowed = true;
            body.Mass = 1;
            UnitSystem.AddUnit(new GameObject(UnitSystem, boxSprite, body));
        }

        /// <summary>
        /// Creates a pyramid of squares. Old stuff for testing.
        /// It doesn't create correctly to "pixel" size, so this is only for testing
        /// lots of boxes.
        /// use CreateRectangle for creating a pixel accurate box/rect.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="count"></param>
        /// <param name="density"></param>
        /// <param name="size"></param>
        public void CreatePyramid(Vector2 position, int count, float density, float size)
        {
            Vertices rect = PolygonTools.CreateRectangle(size, size);
            PolygonShape shape = new PolygonShape(rect, density);

            Vector2 rowStart = position;
            rowStart.Y -= 0.5f + count * 1.1f;

            Vector2 deltaRow = new Vector2(-0.625f, 1.1f);
            const float spacing = 1.25f;

            AssetCreator creator = ScreenManager.Assets;
            Sprite boxSprite = new Sprite(creator.TextureFromVertices(rect, MaterialType.Dots, Color.LightGray, 2f));

            for (int i = 0; i < count; ++i)
            {
                Vector2 pos = rowStart;
                for (int j = 0; j < i + 1; ++j)
                {
                    Body body = BodyFactory.CreateBody(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = pos;
                    body.CreateFixture(shape);
                    body.SleepingAllowed = true;
                    UnitSystem.AddUnit(new GameObject(UnitSystem, boxSprite, body));
                    pos.X += spacing;
                }
                rowStart += deltaRow;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
