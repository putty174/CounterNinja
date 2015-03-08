using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Common;
using FarseerPhysics;

namespace CounterNinja.Hud
{
    public class PlayerUI : HudObject
    {
        private SpriteFont font;
        private Sprite kunaiCounter;
        private Sprite playerLife;
        private Sprite[] healthBar;
        private int kunaiAmount;
        private int life;
        private int health;

        public PlayerUI(HudSystem hudSystem)
            : base(hudSystem)
        {
            font = ScreenManager.Content.Load<SpriteFont>("Fonts/SubtitleFont");
            kunaiCounter = new Sprite(ScreenManager.Content.Load<Texture2D>("Hud/KunaiCounter"));
            playerLife = new Sprite(ScreenManager.Content.Load<Texture2D>("Hud/PlayerLifeCounter"));
            healthBar = new Sprite[5];
            healthBar[0] = new Sprite(ScreenManager.Content.Load<Texture2D>("Hud/Health1"));
            healthBar[1] = new Sprite(ScreenManager.Content.Load<Texture2D>("Hud/Health2"));
            healthBar[2] = new Sprite(ScreenManager.Content.Load<Texture2D>("Hud/Health3"));
            healthBar[3] = new Sprite(ScreenManager.Content.Load<Texture2D>("Hud/Health4"));
            healthBar[4] = new Sprite(ScreenManager.Content.Load<Texture2D>("Hud/Health5"));
        }

        public int KunaiAmount
        {
            get { return kunaiAmount; }
            set { kunaiAmount = value; }
        }

        public int Life
        {
            get { return life; }
            set { life = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public override void Draw()
        {
            SpriteBatch batch = ScreenManager.SpriteBatch;

            batch.Draw(kunaiCounter.Texture, new Vector2(40, 460), null, Color.White, 0f, 
                kunaiCounter.Origin, 1f, SpriteEffects.None, 0.1f);
            batch.DrawString(font, "x " + kunaiAmount, new Vector2(70, 442), Color.Yellow,
                0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);

            batch.Draw(playerLife.Texture, new Vector2(200, 460), null, Color.White, 0f,
                playerLife.Origin, 1f, SpriteEffects.None, 0.1f);
            batch.DrawString(font, "x " + life, new Vector2(220, 442), Color.Yellow,
                0f, Vector2.Zero, 1f, SpriteEffects.None, 0.3f);
            switch (health)
            {
                case 1:
                    batch.Draw(healthBar[0].Texture, new Vector2(290, 460), null, Color.White, 0f,
                        healthBar[0].Origin, 1f, SpriteEffects.None, 0.1f);
                    break;
                case 2:
                    batch.Draw(healthBar[1].Texture, new Vector2(290, 460), null, Color.White, 0f,
                        healthBar[1].Origin, 1f, SpriteEffects.None, 0.1f);
                    break;
                case 3:
                    batch.Draw(healthBar[2].Texture, new Vector2(290, 460), null, Color.White, 0f,
                        healthBar[2].Origin, 1f, SpriteEffects.None, 0.1f);
                    break;
                case 4:
                    batch.Draw(healthBar[3].Texture, new Vector2(290, 460), null, Color.White, 0f,
                        healthBar[3].Origin, 1f, SpriteEffects.None, 0.1f);
                    break;
                case 5:
                    batch.Draw(healthBar[4].Texture, new Vector2(290, 460), null, Color.White, 0f,
                        healthBar[4].Origin, 1f, SpriteEffects.None, 0.1f);
                    break;
                default:
                    goto case 5;
            }
        }
    }
}
