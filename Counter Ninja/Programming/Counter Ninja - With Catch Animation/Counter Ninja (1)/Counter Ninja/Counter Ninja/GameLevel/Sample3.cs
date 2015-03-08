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
    internal class Sample3 : CounterNinjaScreen
    {
        public Sample3()
            : base()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Player.Body.Position = ConvertUnits.ToSimUnits(new Vector2(300, 300));
            MusicSystem.Repeat = true;

            BackgroundSystem.AddScaledBackgroundBody("Level/Sample1/SampleRoom1_Body", 1f);

            BackgroundSystem.AddBodylessBackground("Level/Sample1/SampleRoom1");

            Enemy enemy = new Enemy(UnitSystem);
            UnitSystem.AddUnit(enemy);
            enemy.Body.Position = ConvertUnits.ToSimUnits(new Vector2(400, 400));
            enemy.FaceDirection = FaceDirection.Left;

            GameObject platform1 = new GameObject(UnitSystem, "Object/Platform1", ConvertUnits.ToSimUnits(new Vector2(250, 370)), 1f, 1f);
            UnitSystem.AddUnit(platform1);
            platform1.Body.BodyType = BodyType.Static;

            GameObject platform2 = new GameObject(UnitSystem, "Object/Platform1", ConvertUnits.ToSimUnits(new Vector2(415, 290)), 1f, 1f);
            UnitSystem.AddUnit(platform2);
            platform2.Body.BodyType = BodyType.Static;


            CameraTrackPlayer(false);
            //Camera.Position = new Vector2(320, 240);
            Camera.SetPositionInstant(ConvertUnits.ToSimUnits(new Vector2(320, 240)));
            MusicSystem.PlaySong("Music/CounterNinja_BGM");
            MusicSystem.Repeat = true;
        }

        public override void GameOver()
        {
            ScreenManager.AddScreen(new GameOverScreen(new Sample3()));
            ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
