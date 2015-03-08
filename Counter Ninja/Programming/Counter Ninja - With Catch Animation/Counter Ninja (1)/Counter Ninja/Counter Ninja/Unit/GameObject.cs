using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CounterNinja.Unit
{
    public class GameObject : Unit
    {
        private Sprite sprite;
        private Body body;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitSystem">UnitSystem this belongs to.</param>
        /// <param name="spriteFile">filename of sprite</param>
        /// <param name="position">In sumulation units</param>
        /// <param name="mass">Mass of this object</param>
        /// <param name="friction">amount of friction</param>
        /// <param name="allowSleep">Allows body to sleep.</param>
        public GameObject(UnitSystem unitSystem, string spriteFile, Vector2 position, float mass, float friction, bool allowSleep = true)
            : base(unitSystem, TypeId.Object)
        {
            sprite = new Sprite(ScreenManager.Content.Load<Texture2D>(spriteFile));
            Texture2D texture = sprite.Texture;
            Vertices rect = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(texture.Width / 2), ConvertUnits.ToSimUnits(texture.Height / 2));
            PolygonShape shape = new PolygonShape(rect, 1f);
            //AssetCreator creator = ScreenManager.Assets;
            body = BodyFactory.CreateBody(World, position);
            body.CreateFixture(shape);
            body.Friction = friction;
            body.BodyType = BodyType.Dynamic;
            body.Mass = mass;
            body.SleepingAllowed = allowSleep;
        }

        public GameObject(UnitSystem unitSystem, Sprite sprite, Body body)
            : base(unitSystem, TypeId.Object)
        {
            this.sprite = sprite;
            this.body = body;
        }

        public Body Body
        {
            get { return body; }
        }

        /// <summary>
        /// Removes the body from world
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            World.RemoveBody(body);
        }

        public override void AddToUnitSystem(int id)
        {
            base.AddToUnitSystem(id);
            body.UserData = new UnitData(id, TypeId.Object);
        }

        public override void Draw()
        {
            base.Draw();
            SpriteBatch batch = ScreenManager.SpriteBatch;
            batch.Draw(sprite.Texture, ConvertUnits.ToDisplayUnits(body.Position), null,
                Color.White, body.Rotation, sprite.Origin, 1f, SpriteEffects.None, 0.1f);
        }
    }
}
