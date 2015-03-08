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
using CounterNinja.Script;

namespace FarseerPhysics.GameLevel
{
    internal class Sample2 : CounterNinjaScreen
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

        public Sample2()
            : base()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();
            //Moves the Player to (100,100)px
            Player.Body.Position = ConvertUnits.ToSimUnits(new Vector2(300, 300));

            MusicSystem.Repeat = true;

            //This creates the physical body for the room
            BackgroundSystem.AddScaledBackgroundBody("Level/Sample2/Room1_Body", 1f);
            //This creates the visual image of the background for you to see
            BackgroundSystem.AddBodylessBackground("Level/Sample2/Room1");

            GameObject platform1 = new GameObject(UnitSystem, "Object/Platform1", ConvertUnits.ToSimUnits(new Vector2(1201, 289)), 1f, 1f);
            UnitSystem.AddUnit(platform1);
            platform1.Body.BodyType = BodyType.Static;

            Enemy enemy = new Enemy(UnitSystem, 0);
            UnitSystem.AddUnit(enemy);
            enemy.Body.Position = ConvertUnits.ToSimUnits(new Vector2(1061, 334));
            enemy.FaceDirection = FaceDirection.Left;

            enemy = new Enemy(UnitSystem, 0);
            UnitSystem.AddUnit(enemy);
            enemy.Body.Position = ConvertUnits.ToSimUnits(new Vector2(978, 365));
            enemy.FaceDirection = FaceDirection.Left;
            
            CameraTrackPlayer(true);
            object[] param = new object[1];
            param[0] = new LoadingScreen(new Sample4(), 3);
            ScriptSystem.AddScript(new TriggerScript(this, ScriptCommand.LoadNextLevel, "NextLevel", param, Player.Body, ConvertUnits.ToSimUnits(new Vector2(2274, 365)), 2, 1));
            MusicSystem.PlaySong("Music/CounterNinja_BGM");
            MusicSystem.Repeat = true;

            //Bottomless pits
            UnitSystem.AddUnit(new BottomlessPitDamage(UnitSystem, 50, new Vector2(829, 467)));
            UnitSystem.AddUnit(new BottomlessPitDamage(UnitSystem, 50, new Vector2(930, 469)));
            UnitSystem.AddUnit(new BottomlessPitDamage(UnitSystem, 50, new Vector2(1024, 468)));
            UnitSystem.AddUnit(new BottomlessPitDamage(UnitSystem, 50, new Vector2(1610, 469)));
            UnitSystem.AddUnit(new BottomlessPitDamage(UnitSystem, 50, new Vector2(1512, 471)));
            UnitSystem.AddUnit(new BottomlessPitDamage(UnitSystem, 50, new Vector2(1408, 471)));
            UnitSystem.AddUnit(new BottomlessPitDamage(UnitSystem, 300, new Vector2(1212, 470)));            
        }
        
        public override void GameOver()
        {
            ScreenManager.AddScreen(new GameOverScreen(new Sample2()));
            ExitScreen();
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
