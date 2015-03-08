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
    internal class Sample4 : CounterNinjaScreen
    {
        public Sample4()
            : base()
        {
        }

        public override void LoadContent()
        {
            base.LoadContent();

            Player.Body.Position = ConvertUnits.ToSimUnits(new Vector2(300, 300));
            MusicSystem.Repeat = true;

            //This creates the physical body for the room
            BackgroundSystem.AddScaledBackgroundBody("Level/Sample1/SampleRoom1_Body", 1f);
            //This creates the visual image of the background for you to see
            BackgroundSystem.AddBodylessBackground("Level/Sample1/SampleRoom1");
            
            

            CameraTrackPlayer(false);
            //Camera.Position = new Vector2(320, 240);
            Camera.SetPositionInstant(ConvertUnits.ToSimUnits(new Vector2(320, 240)));
            MusicSystem.PlaySong("Music/CounterNinja_BGM");
            MusicSystem.Repeat = true;
            SpikeCeiling1 sc = new SpikeCeiling1(UnitSystem, new Vector2(320, 48));
            UnitSystem.AddUnit(sc);
            UnitSystem.AddUnit(new BossSamurai(UnitSystem, new Vector2(596, 422), sc));
        }

        public override void GameOver()
        {
            ScreenManager.AddScreen(new GameOverScreen(new Sample4()));
            ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
