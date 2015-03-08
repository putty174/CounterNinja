using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using CounterNinja;
using CounterNinja.Particle;
using CounterNinja.Hud;

namespace CounterNinja.Unit
{
    public class Enemy : Unit
    {

        private const float MoveSpeed = 128.0f;
        
        private float maxRunVelocity = 25.0f;
        private float stopRate = 1.2f;
        private int jumpDelay = 0;
        private float jumpForce = 5000f;
        private Sprite enemySprite;
        private Body enemyBody;
        private Body enemyFeet;
        private FaceDirection faceDirection;
        private Vector2 enemyOrigin;

        private EnemyState enemyState;
        private List<Body> prevContact;
        private int[] fireRate;
        
        private int kunai;
        private bool unlimited = true;

        private float atkWaitTime;
        private float MaxAtkWait = 0.7f;

        private int moveDist;

        public Enemy(UnitSystem unitSystem, int moveDist = 3)
            : base(unitSystem, TypeId.Enemy, 0, 1)
        {
            enemyBody = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(30), ConvertUnits.ToSimUnits(50), 4, 100f);
            enemyBody.BodyType = BodyType.Dynamic;
            enemyBody.IgnoreGravity = false;
            enemyBody.Rotation = 0;
            enemyBody.FixedRotation = true;
            enemyBody.Mass = 3;
            enemyBody.Friction = 0.0f;
            enemyFeet = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(5), ConvertUnits.ToSimUnits(5), 100f);
            enemyFeet.BodyType = BodyType.Dynamic;
            enemyFeet.IgnoreGravity = false;
            enemyFeet.Rotation = 0;
            enemyFeet.Mass = 3;
            enemyFeet.Friction = 0.0f;
            enemyFeet.SleepingAllowed = false;
            enemyFeet.IgnoreCollisionWith(enemyBody);
            enemyBody.IgnoreCollisionWith(enemyFeet);

            enemySprite = new Sprite(ScreenManager.Content.Load<Texture2D>("Enemy/NinjaBlack"));
            faceDirection = FaceDirection.Left;

            enemyState = new EnemyState();
            enemyState.AddState(EState.Standing);

            enemyOrigin = Vector2.Zero;

            fireRate = new int[2];
            fireRate[0] = 0;
            fireRate[1] = 500;

            this.moveDist = moveDist;
        }
        
        public Sprite Sprite
        {
            get { return enemySprite; }
        }

        public Body Body
        {
            get { return enemyBody; }
        }

        public FaceDirection FaceDirection
        {
            get { return faceDirection; }
            set { faceDirection = value; }
        }
        
        public override void AddToUnitSystem(int id)
        {
            base.AddToUnitSystem(id);
            enemyBody.UserData = new UnitData(id, TypeId.Enemy);
        }

        /// <summary>
        /// Checks for collisions below based on body playerFeet.
        /// </summary>
        /// <returns></returns>
        public Boolean HasContactBelow()
        {
            return enemyFeet.ContactList != null;
        }

        public override void Dispose()
        {
            World.RemoveBody(enemyBody);
            World.RemoveBody(enemyFeet);
        }

        public override void Update(GameTime gameTime)
        {
            if (Health.IsDead)
            {
                if (!UnitSystem.IsDisposingUnit(Id))
                    UnitSystem.DisposeUnit(Id);
                return;
            }
            if (enemyOrigin == Vector2.Zero) enemyOrigin = enemyBody.Position;
            if (fireRate[0] > 0)
                fireRate[0] -= Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds);
            //FireBullet();
            DoAttack(gameTime);
            if (moveDist > 0)
                Move(moveDist);
        }

        public void Move(int radius)
        {
            if (enemyBody.Position.X < (enemyOrigin.X - radius) || enemyBody.LinearVelocity == Vector2.Zero)
                FaceDirection = FaceDirection.Right;
            if (enemyBody.Position.X > (enemyOrigin.X + radius) || enemyBody.LinearVelocity == Vector2.Zero)
                FaceDirection = FaceDirection.Left;
            if (FaceDirection == FaceDirection.Left)
                enemyBody.LinearVelocity = new Vector2(-5, 1);
            else
            {
                enemyBody.LinearVelocity = new Vector2(5, 1);
            }
        }

        public void DoAttack(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (atkWaitTime <= 0)
            {
                FireBullet();
                atkWaitTime = MaxAtkWait;
            }
            else
            {
                atkWaitTime = Math.Max(0.0f, atkWaitTime - (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }

        public void FireBullet()
        {
            if (kunai == 0 && !unlimited)
                return;
            BasicProjectile kunaiBullet = new BasicProjectile(UnitSystem, "Player/Kunai", enemyBody.Position, 10f, 1, ProjectileType.Enemy);
            kunaiBullet.Body.IgnoreCollisionWith(enemyBody);
            if (faceDirection == FaceDirection.Left)
                kunaiBullet.Body.ApplyForce(new Vector2(-25000, 0));
            else
                kunaiBullet.Body.ApplyForce(new Vector2(25000, 0));
            kunaiBullet.Body.IgnoreGravity = true;
            UnitSystem.AddQueuedUnit(kunaiBullet);
            fireRate[0] = fireRate[1];
            if (!unlimited) 
                kunai--;
        }

        public bool IsInFront(Vector2 pos)
        {
            if (faceDirection == FaceDirection.Right)
                return pos.X > enemyBody.Position.X;
            else
                return pos.X < enemyBody.Position.X;
        }

        public override void Draw()
        {
            SpriteBatch batch = ScreenManager.SpriteBatch;
            //For debugging playerFeet.
            //batch.Draw(playerSprite.Texture, ConvertUnits.ToDisplayUnits(playerFeet.Position), null,
            //                Color.White, playerFeet.Rotation, playerSprite.Origin, 1f, SpriteEffects.None, 0.1f);

            SpriteEffects spriteEffect = SpriteEffects.None;
            if (faceDirection == FaceDirection.Right)
                spriteEffect = SpriteEffects.FlipHorizontally;
            batch.Draw(enemySprite.Texture, ConvertUnits.ToDisplayUnits(enemyBody.Position), null,
                            Color.White, enemyBody.Rotation, enemySprite.Origin, 1f, spriteEffect, 0.1f);
        }

    }
}
