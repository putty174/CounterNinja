using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using CounterNinja;

namespace CounterNinja.Unit
{
    public class SpikeCeiling1 : Unit
    {
        
        private Body body;
        private Sprite sprite;
        private Vector2 moveToPos;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitSystem"></param>
        /// <param name="width">Width in pixels</param>
        public SpikeCeiling1(UnitSystem unitSystem, Vector2 pos)
            : base(unitSystem, TypeId.Object)
        {
            sprite = new Sprite(ScreenManager.Content.Load<Texture2D>("Object/CeilingSpike"));
            Texture2D texture = sprite.Texture;
            Vertices rect = PolygonTools.CreateRectangle(ConvertUnits.ToSimUnits(texture.Width / 2), ConvertUnits.ToSimUnits(texture.Height / 2));
            PolygonShape shape = new PolygonShape(rect, 1f);
            //AssetCreator creator = ScreenManager.Assets;
            body = BodyFactory.CreateBody(World, ConvertUnits.ToSimUnits(pos));
            body.CreateFixture(shape);
            body.BodyType = BodyType.Static;
            moveToPos = Vector2.Zero;
        }

        public Body Body
        {
            get { return body; }
        }

        public Vector2 MoveToPos
        {
            get { return moveToPos; }
            set { moveToPos = value; }
        }

        public override void Dispose()
        {
            World.RemoveBody(body);
        }
        
        public override void Update(GameTime gameTime)
        {
            if (body.ContactList != null)
            {
                FarseerPhysics.Dynamics.Contacts.ContactEdge temp = body.ContactList;
                while (temp != null)
                {
                    object bt = temp.Other.UserData;
                    if (bt != null && bt is UnitData)
                    {
                        UnitData data = (UnitData)bt;
                        if (!UnitSystem.IsDisposingUnit(data.Id))
                        {
                            Unit hitUnit = UnitSystem.Units[data.Id];
                            switch (data.TypeId)
                            {
                                case TypeId.Enemy:
                                    hitUnit.Health.TakeDamage(500);
                                    break;
                                case TypeId.Player:
                                    hitUnit.Health.TakeDamage(500);
                                    break;
                            }
                        }
                    }
                    temp = temp.Next;
                }
            }
            if (moveToPos != Vector2.Zero && moveToPos != body.Position)
            {
                float x = body.Position.X;
                float y = body.Position.Y;
                if (x < moveToPos.X)
                    x++;
                if (x > moveToPos.X)
                    x--;
                if (y < moveToPos.Y)
                    y++;
                if (y > moveToPos.Y)
                    y--;
                body.Position = new Vector2(x, y);
            }
        }

        public override void Draw()
        {
            SpriteBatch batch = ScreenManager.SpriteBatch;
            batch.Draw(sprite.Texture, ConvertUnits.ToDisplayUnits(body.Position), null,
                Color.White, 0, sprite.Origin, 1f, SpriteEffects.None, 0.1f);
        }
    }
}
