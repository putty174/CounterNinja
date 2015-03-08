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
    public enum FaceDirection
    {
        Left,
        Right
    }

    public class Player : Unit
    {
        private float runForce = 180.0f;
        private float maxRunVelocity = 25.0f;
        private float stopRate = 1.2f;
        private int jumpDelay = 0;
        private float jumpForce = 5000f;
        private Sprite playerSprite;
        private Body playerBody;
        private Body playerFeet;
        private Vector2 playerGravity;
        private FaceDirection faceDirection;
        private Body holdObject;
        private double pickObjectDist = 5.0;
        private float maxPickUpObjectMass = 1f;
        private PlayerState playerState;
        private bool hasControl = true;
        private int[] fireRate;

        //Keys
        private Keys keyJump = Keys.Up;
        private Keys keyJumpAlt = Keys.Z;
        private Keys keyMoveRight = Keys.Right;
        private Keys keyMoveLeft = Keys.Left;
        private Keys keyFireKunai = Keys.X;
        private Keys keyCatchKunai = Keys.C;
        private Keys keyCrouch = Keys.Down;

        //H.U.D.
        private int kunai;
        private bool unlimited = false;
        private PlayerUI hud;

        //Animated Sprites
        private AnimatedSprite jumpSprite;
        private AnimatedSprite runSprite;
        private AnimatedSprite crouchSprite;
        private AnimatedSprite catchSprite;

        /// <summary>
        /// This class needs clean up later.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="position"></param>
        /// <param name="density"></param>
        public Player(UnitSystem unitSystem)
            : base(unitSystem, TypeId.Player, 3, 5)
        {
            //playerBody = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(30), ConvertUnits.ToSimUnits(110), density);
            playerBody = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(30), ConvertUnits.ToSimUnits(50), 4, 100f);
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.IgnoreGravity = false;
            playerBody.Rotation = 0;
            playerBody.FixedRotation = true;
            playerBody.Mass = 3;
            playerBody.Friction = 0.0f;
            playerFeet = BodyFactory.CreateRectangle(World, ConvertUnits.ToSimUnits(5), ConvertUnits.ToSimUnits(5), 100f);
            playerFeet.BodyType = BodyType.Dynamic;
            playerFeet.IgnoreGravity = false;
            playerFeet.Rotation = 0;
            playerFeet.Mass = 3;
            playerFeet.Friction = 0.0f;
            playerFeet.SleepingAllowed = false;
            playerFeet.IgnoreCollisionWith(playerBody);
            playerBody.IgnoreCollisionWith(playerFeet);

            playerSprite = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/PlayerStand"));
            playerGravity = new Vector2(0, System.Math.Abs(World.Gravity.Y));
            faceDirection = FaceDirection.Right;

            playerState = new PlayerState();
            playerState.AddState(PState.Standing);

            fireRate = new int[2];
            fireRate[0] = 0;
            fireRate[1] = 500;

            //HUD
            kunai = 10;
            hud = new PlayerUI(CounterNinjaScreen.HudSystem);
            hud.KunaiAmount = kunai;
            CounterNinjaScreen.HudSystem.AddHudObject(hud);

            //Animated Sprite
            Sprite[] temp = new Sprite[5];
            temp[0] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Jump/jumpingFrame1"));
            temp[1] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Jump/jumpingFrame2jump1"));
            temp[2] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Jump/jumpingFrame3jump2"));
            temp[3] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Jump/jumpingFrame4UpOrDown"));
            temp[4] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Jump/jumpingFrame5landing"));
            jumpSprite = new AnimatedSprite(ScreenManager, temp, 20, false);

            temp = new Sprite[8];
            temp[0] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Run/RUNNINGCOLOR1"));
            temp[1] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Run/RUNNINGCOLOR2"));
            temp[2] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Run/RUNNINGCOLOR3"));
            temp[3] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Run/RUNNINGCOLOR4"));
            temp[4] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Run/RUNNINGCOLOR5"));
            temp[5] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Run/RUNNINGCOLOR6"));
            temp[6] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Run/RUNNINGCOLOR7"));
            temp[7] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Run/RUNNINGCOLOR8"));
            runSprite = new AnimatedSprite(ScreenManager, temp, 80, true);

            temp = new Sprite[2];
            temp[0] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Crouch/countninjacrouchframe1"));
            temp[1] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Crouch/countninjacrouchframe2"));
            crouchSprite = new AnimatedSprite(ScreenManager, temp, 100, false);

            temp = new Sprite[3];
            temp[0] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Catch/Catch1"));
            temp[1] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Catch/Catch2"));
            temp[2] = new Sprite(ScreenManager.Content.Load<Texture2D>("Player/Catch/Catch3"));
            catchSprite = new AnimatedSprite(ScreenManager, temp, 100, false);

            //Load up sound files on creation
            SoundSystem.LoadSound("SFX/Jump_NinjaStyle_3");
            SoundSystem.LoadSound("SFX/Projectile_v5");
            SoundSystem.LoadSound("SFX/Slide_Short");
        }

        #region Get/Set Accessor Methods
        
        public Camera2D Camera
        {
            get { return CounterNinjaScreen.Camera; }
        }

        public Sprite Sprite
        {
            get { return playerSprite; }
        }

        public Body Body
        {
            get { return playerBody; }
        }

        public Vector2 Gravity
        {
            get { return playerGravity; }
            set { playerGravity = value; }
        }

        public FaceDirection FaceDirection
        {
            get { return faceDirection; }
            set { faceDirection = value; }
        }

        public bool HasControl
        {
            get { return hasControl; }
            set { hasControl = value; }
        }

        #endregion

        public override void AddToUnitSystem(int id)
        {
            base.AddToUnitSystem(id);
            playerBody.UserData = new UnitData(id, TypeId.Player);
        }

        /// <summary>
        /// Checks for collisions below based on body playerFeet.
        /// </summary>
        /// <returns></returns>
        public Boolean HasContactBelow()
        {
            return playerFeet.ContactList != null;
        }

        /// <summary>
        /// Creates dust at the player's feet to simulate dust from jumping.
        /// Uses the particle system.
        /// </summary>
        public void CreateDustJumpEffect()
        {
            ParticleSystem pSystem = CounterNinjaScreen.ParticleSystem;
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                ParticleHandler pHandler = new ParticleHandler(pSystem, null, 0, 0, false,
                   1f, 0f, 0f, false, 255, 10, 0, true, Vector2.Zero, ParticleRemoveType.Fade);
                float sX = (float)rand.NextDouble() / 10f;
                float sY = (float)rand.NextDouble() / -10f;
                if (rand.Next(0, 10) > 5)
                    sX *= -1f;
                pSystem.AddParticle("Particle/dust2", playerFeet.Position, "DustParticle", true,
                    pHandler, new Vector2(sX, sY));
            }
            SoundSystem.PlaySound("SFX/Slide_Short");
        }

        /// <summary>
        /// Creates dust at the player's feet, going the opposite direction of faceDirection.
        /// </summary>
        public void CreateDustRunEffect()
        {
            ParticleSystem pSystem = CounterNinjaScreen.ParticleSystem;
            Random rand = new Random();
            ParticleHandler pHandler = new ParticleHandler(pSystem, null, 0, 0, false,
                   1f, 0f, 0f, false, 255, 10, 0, true, Vector2.Zero, ParticleRemoveType.Fade);
            float sX = (float)rand.NextDouble() / 10f;
            float sY = (float)rand.NextDouble() / -10f;
            if (faceDirection == FaceDirection.Right)
                sX *= -1f;
            pSystem.AddParticle("Particle/dust2", playerFeet.Position, "DustParticle", true,
                pHandler, new Vector2(sX, sY));
        }

        public override void Dispose()
        {
            World.RemoveBody(playerBody);
            World.RemoveBody(playerFeet);
        }

        public void HandleInput(InputHelper input)
        {
            if (CounterNinjaScreen.EnableCameraControl)
                return;
            HandleMovement(input);
            if (input.KeyboardState.IsKeyDown(keyFireKunai) && fireRate[0] <= 0)
                FireKunai();
            if (input.IsNewKeyPress(keyCatchKunai))
            {
                CatchKunai();
            }
            //HandlePickUpObject(input);
        }

        public void FireKunai()
        {
            if (kunai == 0 && !unlimited)
                return;
            BasicProjectile kunaiBullet = new BasicProjectile(UnitSystem, "Player/Kunai", playerBody.Position, 10f, 1, ProjectileType.Player);
            kunaiBullet.Body.IgnoreCollisionWith(playerBody);
            if (faceDirection == FaceDirection.Left)
                kunaiBullet.Body.ApplyForce(new Vector2(-25000, 0));
            else
                kunaiBullet.Body.ApplyForce(new Vector2(25000, 0));
            kunaiBullet.Body.IgnoreGravity = true;
            UnitSystem.AddUnit(kunaiBullet);
            fireRate[0] = fireRate[1];
            SoundSystem.PlaySound("SFX/Projectile_v5");
            if (!unlimited) 
                kunai--;
        }

        public void CatchKunai()
        {

            double distance = pickObjectDist + 1;
            foreach (Body body in World.BodyList)
            {
                double objDistance = Vector2.Distance(body.Position, playerBody.Position);
                if (IsInFront(body.Position) && objDistance <= pickObjectDist
                    && body.UserData != null && body.UserData is UnitData)
                {
                    UnitData data = (UnitData)body.UserData;
                    if (data.TypeId == TypeId.Projectile)
                    {
                        catchSprite.AnimationIndex = 0; //reset animation

                        playerState.ClearStates();
                        playerState.AddState(PState.Catch);

                        UnitSystem.DisposeUnit(data.Id);
                        kunai++;
                    }
                }

            }
        }

        /// <summary>
        /// Handles all the player's basic movements.
        /// Includes handling rotations based on FallDirection, jumping and jump timers, 
        /// and the player's fall direction.
        /// If Keys D is pressed, faceDirection will be Right.
        /// If Keys A is Pressed, faceDirection will be Left.
        /// </summary>
        /// <param name="input"></param>
        public void HandleMovement(InputHelper input)
        {
            //CheckCollision();
            //for debug purposes
            //playerBody.Rotation = 0;
            if (input.IsNewKeyPress(Keys.L) && CounterNinjaScreen.Debug)
                System.Console.WriteLine("X:" + ConvertUnits.ToDisplayUnits(playerBody.Position.X)
                    + " Y:" + ConvertUnits.ToDisplayUnits(playerBody.Position.Y));
            Vector2 force = Vector2.Zero;
            if ((input.KeyboardState.IsKeyDown(keyMoveRight) || input.KeyboardState.IsKeyDown(keyMoveLeft) && hasControl))
            {
                //playerState.ClearStates();
                playerState.AddState(PState.Running);
                playerState.RemoveState(PState.Standing);
            }
            else
            {
                //playerState.ClearStates();
                playerState.AddState(PState.Standing);
                playerState.RemoveState(PState.Running);
            }
            if (input.KeyboardState.IsKeyDown(keyCrouch))
            {
                if (!playerState.HasState(PState.Crouching))
                    crouchSprite.AnimationIndex = 0;
                playerState.AddState(PState.Crouching);
            }
            else
                playerState.RemoveState(PState.Crouching);
            if (input.KeyboardState.IsKeyDown(keyMoveLeft) && hasControl)
            {
                if (faceDirection != FaceDirection.Left && !playerState.HasState(PState.Falling) && !playerState.HasState(PState.Jumping))
                {
                    faceDirection = FaceDirection.Left;
                    CreateDustRunEffect();
                    playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
                }
                else
                    faceDirection = FaceDirection.Left;
            }
            else if (input.KeyboardState.IsKeyDown(keyMoveRight) && hasControl)
            {
                if (faceDirection != FaceDirection.Right && !playerState.HasState(PState.Falling) && !playerState.HasState(PState.Jumping))
                {
                    faceDirection = FaceDirection.Right;
                    CreateDustRunEffect();
                    playerBody.LinearVelocity = new Vector2(0, playerBody.LinearVelocity.Y);
                }
                else
                    faceDirection = FaceDirection.Right;
            }

            //playerFeet is at the bottom of the player's body position. That's 25px down.
            playerFeet.Position = Vector2.Add(playerBody.Position, ConvertUnits.ToSimUnits(new Vector2(0, 20)));
            if (input.KeyboardState.IsKeyDown(keyMoveLeft) && playerBody.LinearVelocity.X > -maxRunVelocity && hasControl)
                force += new Vector2(-runForce, 0);
            else if (input.KeyboardState.IsKeyDown(keyMoveRight) && playerBody.LinearVelocity.X < maxRunVelocity && hasControl)
                force += new Vector2(runForce, 0);
            else if (!playerState.HasState(PState.Falling)
                && !input.KeyboardState.IsKeyDown(keyMoveLeft) && !input.KeyboardState.IsKeyDown(keyMoveRight))
            {
                playerBody.LinearVelocity = new Vector2(playerBody.LinearVelocity.X / stopRate, playerBody.LinearVelocity.Y);
                if (!playerState.HasState(PState.Jumping) && Math.Abs(playerBody.LinearVelocity.Y) < 2
                    && Math.Abs(playerBody.LinearVelocity.X) > maxRunVelocity - 5 && Math.Abs(playerBody.LinearVelocity.X) <= maxRunVelocity)
                    CreateDustRunEffect();
            }
            if ((input.IsNewKeyPress(keyJump) || input.IsNewKeyPress(keyJumpAlt)) && hasControl && jumpDelay >= 5
                && !playerState.HasState(PState.Falling) && !playerState.HasState(PState.Jumping)
                && HasContactBelow())
            {
                force += new Vector2(0, -jumpForce);
                jumpDelay = 0;
                playerState.AddState(PState.Jumping);
                if (jumpSprite.AnimationIndex == 4)
                    jumpSprite.AnimationIndex = 0;
                CreateDustJumpEffect();
                SoundSystem.PlaySound("SFX/Jump_NinjaStyle_3");
            }

            if ((playerState.HasState(PState.Jumping) || playerState.HasState(PState.Falling)) && jumpDelay >= 5)
            {
                jumpDelay = 0;
                if (playerBody.LinearVelocity.Y > 0)
                {
                    playerState.RemoveState(PState.Jumping);
                    playerState.AddState(PState.Falling);
                }
                if (HasContactBelow() && Math.Abs(playerBody.LinearVelocity.Y) < 2 )
                {
                    playerState.RemoveState(PState.Jumping);
                    playerState.RemoveState(PState.Falling);
                }
            }
            if (jumpDelay < 35)
                jumpDelay++;
            if (!HasContactBelow() && !playerState.HasState(PState.Jumping))
                playerState.AddState(PState.Falling);
            if (!playerState.HasState(PState.Jumping) && !playerState.HasState(PState.Falling) && !HasContactBelow())
                playerState.AddState(PState.Falling);
            playerBody.ApplyForce(force);
        }

        /// <summary>
        /// Checks if pos is infront 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool IsInFront(Vector2 pos)
        {
            if (faceDirection == FaceDirection.Right)
                return pos.X > playerBody.Position.X;
            else
                return pos.X < playerBody.Position.X;
        }

        /// <summary>
        /// Picks up the closest object when facing left or right.
        /// TODO.
        /// </summary>
        public void PickUpObject()
        {
            Body tempBody = null;
            double distance = pickObjectDist + 1;
            foreach (Body body in World.BodyList)
            {
                double objDistance = Vector2.Distance(body.Position, playerBody.Position);
                if (IsInFront(body.Position) && objDistance <= pickObjectDist && objDistance < distance
                    && body.Mass <= maxPickUpObjectMass)
                    tempBody = body;

            }
            if (tempBody != null)
            {
                holdObject = tempBody;
                holdObject.IgnoreCollisionWith(playerBody);
                holdObject.IgnoreGravity = true;
            }
        }
                
        public override void Update(GameTime gameTime)
        {
            if (fireRate[0] > 0)
                fireRate[0] -= Convert.ToInt32(gameTime.ElapsedGameTime.TotalMilliseconds);
            hud.Life = Health.Lives;
            hud.Health = Health.Amount;
            hud.KunaiAmount = kunai;


            if (catchSprite.AnimationIndex == 2)
            {
                playerState.ClearStates();
                playerState.AddState(PState.Standing);
            }

            if (playerState.HasState(PState.Jumping) || playerState.HasState(PState.Falling))
                jumpSprite.Update(gameTime);
            else if (playerState.HasState(PState.Running))
                runSprite.Update(gameTime);
            else if (playerState.HasState(PState.Crouching))
                crouchSprite.Update(gameTime);
            else if (playerState.HasState(PState.Catch))
                catchSprite.Update(gameTime);
        }

        /// <summary>
        /// Draws the gravity shift area image and the player's image.
        /// </summary>
        public override void Draw()
        {
            SpriteBatch batch = ScreenManager.SpriteBatch;
            //For debugging playerFeet.
            //batch.Draw(playerSprite.Texture, ConvertUnits.ToDisplayUnits(playerFeet.Position), null,
            //                Color.White, playerFeet.Rotation, playerSprite.Origin, 1f, SpriteEffects.None, 0.1f);

            SpriteEffects spriteEffect = SpriteEffects.None;
            if (faceDirection == FaceDirection.Right)
                spriteEffect = SpriteEffects.FlipHorizontally;
            if (playerState.HasState(PState.Jumping) || playerState.HasState(PState.Falling))
            {
                jumpSprite.Draw(ConvertUnits.ToDisplayUnits(playerBody.Position), playerBody.Rotation, 1f, spriteEffect);
            }
            else if (playerState.HasState(PState.Running))
            {
                runSprite.Draw(ConvertUnits.ToDisplayUnits(playerBody.Position), playerBody.Rotation, 1f, spriteEffect);
            }
            else if (playerState.HasState(PState.Catch))
            {
                catchSprite.Draw(ConvertUnits.ToDisplayUnits(playerBody.Position), playerBody.Rotation, 1f, spriteEffect);
            }
            else if (playerState.HasState(PState.Crouching))
                crouchSprite.Draw(ConvertUnits.ToDisplayUnits(playerBody.Position), playerBody.Rotation, 1f, spriteEffect);
            else
            {
                batch.Draw(playerSprite.Texture, ConvertUnits.ToDisplayUnits(playerBody.Position), null,
                                Color.White, playerBody.Rotation, playerSprite.Origin, 1f, spriteEffect, 0.1f);
            }

            //SpriteEffects spriteEffect = SpriteEffects.None;
            //if (faceDirection == FaceDirection.Left)
            //    spriteEffect = SpriteEffects.FlipHorizontally;
            //if (playerState.HasState(PState.Standing) && !playerState.HasState(PState.Falling) && !playerState.HasState(PState.Jumping))
            //batch.Draw(standAnim.Texture, ConvertUnits.ToDisplayUnits(playerBody.Position), null,
            //                Color.White, playerBody.Rotation, standAnim.Origin, 1f, spriteEffect, 0.5f);
            //if (playerState.HasState(PState.Running) && !playerState.HasState(PState.Falling) && !playerState.HasState(PState.Jumping))
            //{

            //    batch.Draw(runAnim[curAnim].Texture, ConvertUnits.ToDisplayUnits(playerBody.Position), null,
            //                    Color.White, playerBody.Rotation, runAnim[curAnim].Origin, 1f, spriteEffect, 0.5f);
            //    if (curAnimDelay < animDelay)
            //        curAnimDelay++;
            //    else
            //    {
            //        curAnimDelay = 0;
            //        curAnim++; 
            //    }
            //    if (curAnim >= runAnim.Length)
            //        curAnim = 0;
            //}
            //else if (playerState.HasState(PState.Falling) || playerState.HasState(PState.Jumping))
            //    batch.Draw(jumpAnim.Texture, ConvertUnits.ToDisplayUnits(playerBody.Position), null,
            //                    Color.White, playerBody.Rotation, jumpAnim.Origin, 1f, spriteEffect, 0.5f);
        }
    }
}