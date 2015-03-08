using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Collision.Shapes;
using CounterNinja;


namespace CounterNinja.Unit
{
    public class ProjectileWithBody : Unit
    {
        private Body body;
        private Sprite sprite;
        private int startTime;
        private int timeToLive;

        public ProjectileWithBody(UnitSystem unitSystem, Vector2 pos)
            : base(unitSystem, TypeId.Projectile)
        {
            body = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(2), ConvertUnits.ToSimUnits(2), 4);
            body.BodyType = BodyType.Dynamic;
            body.IgnoreGravity = true;
            body.Rotation = 0;
            body.Mass = 3;
            body.Friction = 0.0f;
            body.Position = pos;
            sprite = new Sprite(ScreenManager.Assets.TextureFromShape(body.FixtureList[0].Shape,
                                                                                MaterialType.Squares,
                                                                                Color.Orange, 1f));
            startTime = -1;
            timeToLive = 200;
        }

        public ProjectileWithBody(UnitSystem unitSystem, string spriteFile, Vector2 position, float mass, int timeToLive = 10000)
            : base(unitSystem, TypeId.Projectile)
        {
            sprite = new Sprite(ScreenManager.Content.Load<Texture2D>(spriteFile));
            Texture2D texture = sprite.Texture;
            Vertices rect = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(texture.Width / 2), ConvertUnits.ToSimUnits(texture.Height / 2));
            PolygonShape shape = new PolygonShape(rect, 1f);
            //AssetCreator creator = ScreenManager.Assets;
            body = BodyFactory.CreateBody(World, position);
            body.CreateFixture(shape);
            body.Friction = 0f;
            body.BodyType = BodyType.Dynamic;
            body.Mass = mass;
            body.SleepingAllowed = false;
            startTime = -1;
            this.timeToLive = timeToLive;
        }

        public Body Body
        {
            get { return body; }
        }

        public override void Dispose()
        {
            base.Dispose();
            World.RemoveBody(body);
        }

        public override void AddToUnitSystem(int id)
        {
            base.AddToUnitSystem(id);
            body.UserData = new UnitData(id, TypeId.Projectile);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsBeingDisposed)
                return;
            if (startTime == -1)
                startTime = Convert.ToInt32(gameTime.TotalGameTime.TotalMilliseconds);
            if (startTime + timeToLive < gameTime.TotalGameTime.TotalMilliseconds)
                UnitSystem.DisposeUnit(Id);
            if (body.ContactList != null)
            {
                UnitSystem.DisposeUnit(Id);
                //startTime = timeToLive - 1000;
                object bt = body.ContactList.Other.UserData;
                if (bt != null && bt is UnitData)
                    UnitSystem.DisposeUnit(((UnitData)bt).Id);
            }
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
            SpriteBatch batch = ScreenManager.SpriteBatch;
            SpriteEffects spriteEffect = SpriteEffects.None;
            if (body.LinearVelocity.X > 0)
                spriteEffect = SpriteEffects.FlipHorizontally;
            batch.Draw(sprite.Texture, ConvertUnits.ToDisplayUnits(body.Position), null,
                Color.White, 0, sprite.Origin, 1f, spriteEffect, 0.1f);
        }
    }
}
