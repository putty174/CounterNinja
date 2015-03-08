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
    public enum ProjectileType
    {
        Player,
        Enemy
    }

    public class BasicProjectile : Unit
    {
        private ProjectileType type;
        private Body body;
        private Sprite sprite;
        private int startTime;
        private int timeToLive;
        private int damage;
        private bool hasHit;
        
        public BasicProjectile(UnitSystem unitSystem, string spriteFile, Vector2 position, float mass,
            int bWidth, int bHeight, int damage, ProjectileType type, int timeToLive = 10000)
            : base(unitSystem, TypeId.Projectile)
        {
            sprite = new Sprite(ScreenManager.Content.Load<Texture2D>(spriteFile));
            Texture2D texture = sprite.Texture;
            Vertices rect = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(bWidth), ConvertUnits.ToSimUnits(bHeight));
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
            this.damage = damage;
            this.type = type;
            hasHit = false;
        }

        public BasicProjectile(UnitSystem unitSystem, string spriteFile, Vector2 position, float mass, int damage, ProjectileType type, int timeToLive = 10000)
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
            this.damage = damage;
            this.type = type;
            hasHit = false;
        }

        public Body Body
        {
            get { return body; }
        }

        public ProjectileType ProjectileType
        {
            get { return type; }
        }

        public override void Dispose()
        {
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
            {
                UnitSystem.DisposeUnit(Id);
                return;
            }
            if (hasHit)
                return;
            if (body.ContactList != null)
            {
                //startTime = timeToLive - 1000;
                object bt = body.ContactList.Other.UserData;
                if (bt != null && bt is UnitData)
                {
                    UnitData data = (UnitData)bt;
                    if (!UnitSystem.IsDisposingUnit(data.Id))
                    {
                        Unit hitUnit = UnitSystem.Units[data.Id];
                        switch (data.TypeId)
                        {
                            case TypeId.Enemy:
                                if (type == ProjectileType.Player)
                                    hitUnit.Health.TakeDamage(damage);
                                startTime -= timeToLive - 200;
                                hasHit = true;
                                break;
                            case TypeId.Player:
                                if (type == ProjectileType.Enemy)
                                    hitUnit.Health.TakeDamage(damage);
                                startTime -= timeToLive - 200;
                                hasHit = true;
                                break;
                            default:
                                startTime -= timeToLive - 200;
                                break;
                        }
                    }
                    SoundSystem.PlaySound("SFX/Projectile_HitEnemy_1");
                }
                else
                    startTime -= timeToLive - 200;
            }
        }

        public override void Draw()
        {
            SpriteBatch batch = ScreenManager.SpriteBatch;
            SpriteEffects spriteEffect = SpriteEffects.None;
            if (body.LinearVelocity.X > 0)
                spriteEffect = SpriteEffects.FlipHorizontally;
            batch.Draw(sprite.Texture, ConvertUnits.ToDisplayUnits(body.Position), null,
                Color.White, 0, sprite.Origin, 1f, spriteEffect, 0.1f);
        }
    }
}
