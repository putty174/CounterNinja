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
    public class BottomlessPitDamage : Unit
    {
        private Body body;
        private Sprite debugSprite;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitSystem"></param>
        /// <param name="width">Width in pixels</param>
        public BottomlessPitDamage(UnitSystem unitSystem, int width, Vector2 pos)
            : base(unitSystem, TypeId.Object)
        {
            body = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(width), ConvertUnits.ToSimUnits(5), 4, 100f);
            body.BodyType = BodyType.Static;
            debugSprite = new Sprite(ScreenManager.Assets.TextureFromShape(body.FixtureList[0].Shape,
                MaterialType.Squares, Color.White, 1f));
            body.Position = ConvertUnits.ToSimUnits(pos);
        }

        public Body Body
        {
            get { return body; }
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
        }

        //public override void Draw()
        //{
        //    SpriteBatch batch = ScreenManager.SpriteBatch;
        //    batch.Draw(debugSprite.Texture, ConvertUnits.ToDisplayUnits(body.Position), null,
        //        Color.White, 0, debugSprite.Origin, 1f, SpriteEffects.None, 0.1f);
        //}
    }
}
