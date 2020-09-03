using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Content;
using Android.Gestures;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameDemo.Shared
{
    class Player : Sprite
    {
        
        bool isDead;
        bool isDucking;
        
        #region Animations
        AnimationPlayer playerAnimate;
        Animation ducking;
        Animation running;
        //Animation dying;
        Animation jumping;
        
        #endregion
        // Constants for controling horizontal movement
        
        public Vector2 Velocity

        {

            get { return velocity; }

            set { velocity = value; }

        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        
        private float startingYpos;
        Vector2 velocity;
        

        // Constants for controlling vertical movement
        
        private const float DuckTime = 500F;

        private const float JumpLaunchVelocity_Y = -40;

        private const float JumpLaunchVelocity_X = 40;

        private const float GravityAcceleration = 3;

        //private static float timeSinceJump = 0;

        private float airdrag = 1;

        TimeSpan elapsedTimeForAnimation= TimeSpan.Zero;

        bool isOnGround;

        public bool IsDead
        {
            get
            {
                return isDead;
            }
            set { isDead = value; }
        }

        public bool IsOnGround
        {

            get
            {
                return isOnGround;
            }
            set { IsOnGround = value; }
        }

        // define the margin of the collision 
        private Rectangle localBounds;

        /// <summary>

        /// Gets a rectangle which bounds this player in world space. No use for the moment

        /// </summary>

        public Rectangle BoundingRectangle
        {
            get
            {
                // smaller hitbox for ducking
                if (isDucking)
                {
                    int left = (int)Math.Round(position.X - playerAnimate.Origin.X) ;

                    int top = (int)Math.Round(position.Y - playerAnimate.Origin.Y)+30 ;
                    return new Rectangle(left, top, (int)((float)playerAnimate.Animation.FrameWidth*scale-(int)collisionOffset.X),
                        (int)((float)playerAnimate.Animation.FrameHeight*scale - (int)collisionOffset.Y));
                }
                else
                {
                    int left = (int)Math.Round(position.X - playerAnimate.Origin.X);

                    int top = (int)Math.Round(position.Y - playerAnimate.Origin.Y)-20;
                    return new Rectangle(left, top, (int)((float)playerAnimate.Animation.FrameWidth*scale - (int)collisionOffset.X),
                        (int)((float)playerAnimate.Animation.FrameHeight*scale - (int)collisionOffset.Y));

                }
            }
        }

        public Player(Texture2D texture, Vector2 position, Point frameSize, int totalFrames, Vector2 collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,float scale)
            : base(texture, position, frameSize, totalFrames, collisionOffset, currentFrame, sheetSize, speed,scale)
        {

            
            startingYpos = position.Y;
            isOnGround = false;
            isDucking = false;
            isDead = false;
        }

        public Player(Texture2D texture, Vector2 position, Point frameSize, int totalFrames, Vector2 collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int secondsperFrame,float scale) 
            : base(texture, position, frameSize, totalFrames, collisionOffset, currentFrame, sheetSize, speed, secondsperFrame,scale)
        {
            localBounds = new Rectangle((int)position.X, (int)position.Y, frameSize.X+(int)collisionOffset.X, frameSize.Y+(int)collisionOffset.Y);
            startingYpos = position.Y;
        }
        public void LoadContent(ContentManager contentManager)
        {
            playerAnimate = new AnimationPlayer();
            running = new Animation(contentManager.Load<Texture2D>("horse_running"), 60, 11, true);
            jumping = new Animation(contentManager.Load<Texture2D>("horse_jump"), 20, 16, false);
            ducking = new Animation(contentManager.Load<Texture2D>("ducking1"), 40, 11, true);
            
        }

        /// <summary> TOUCH IN MONOGAME
        /// https://gregfmartin.com/2017/12/27/monogame-working-with-touch/
        /// </summary>

       
        public override void Update(GameTime gametime)
        {
            
            var gesture = default(GestureSample);
            

                // for some reason in debug its false before the first touch even though we enabled some gestures
            while (TouchPanel.IsGestureAvailable)
            {
                gesture = TouchPanel.ReadGesture();
                TimeSpan time = gesture.Timestamp;

                // JUMPING
                if (gesture.GestureType == GestureType.Flick && gesture.Delta.Y<0)
                {
                    if (isOnGround)
                    {
                        playerAnimate.PlayAnimation(jumping);
                        velocity.Y = JumpLaunchVelocity_Y;
                        velocity.X = JumpLaunchVelocity_X;
                        isOnGround = false;
                        isDucking = false;
                    }
                }
                // DUCKING
                if(gesture.GestureType == GestureType.Tap )
                {
                    if (isOnGround)
                    {
                        isDucking = true;
                        playerAnimate.PlayAnimation(ducking);
                        elapsedTimeForAnimation = TimeSpan.Zero;
                    }
                }
                // SLOWING DOWN
                if (gesture.GestureType == GestureType.Flick && gesture.Delta.Y > 0)
                {
                    velocity.X -= 5;
                }
            }

            #region ducking_handling

            if (isDucking )
            {
                if(elapsedTimeForAnimation==TimeSpan.Zero)
                velocity.X += 10F;
                elapsedTimeForAnimation=elapsedTimeForAnimation.Add(gametime.ElapsedGameTime);
                velocity.X += 1;
            }

            if(elapsedTimeForAnimation.Milliseconds> DuckTime)
            {
                isDucking = false;
                playerAnimate.PlayAnimation(running);
                elapsedTimeForAnimation = TimeSpan.Zero;
            }

            #endregion

            // change the velocity of the player due to factors
            velocity.Y += GravityAcceleration;
            velocity.X -= airdrag;

            // update player position according to his velocity
            position.Y += velocity.Y;
            position.X += velocity.X;

            // So the player doesnt fall off the screen
            if (position.Y >= startingYpos )
            {
                position.Y = startingYpos;
                velocity.Y = 0F;
                isOnGround = true;
                if (isDucking)
                {

                }
                else playerAnimate.PlayAnimation(running);
                
            }
            // Limit the player to certain bounds of the screen so he doesnt run off

            if(position.X < 130 )
            {
                airdrag = 1;
                position.X = 130;
                velocity.X = 0;
            }
            if(position.X> 1300)
            {
                position.X = 1300;
                velocity.X = 0;
            }

            // Limit player speed

            if (velocity.X < -8)
            {
                velocity.X = -5;
            }
            if(velocity.X>10 && isOnGround)
            {
                velocity.X = 10;
            }


        }
        public override void Draw(GameTime gametime, SpriteBatch spritebatch,float scale,SpriteEffects spriteEffects )
        {
            // because at first there is no animation
            if (playerAnimate.Animation == null)
                playerAnimate.PlayAnimation(running);
            if (isDucking)
            {
                // Lower the sprite because its a bit smaller when ducking
                position.Y += 20;
                playerAnimate.Draw(gametime, spritebatch, position, spriteEffects, scale+0.1F);
            }
            else
            playerAnimate.Draw(gametime, spritebatch, position, spriteEffects,scale);
            
        }

        public override Rectangle collisionRect()
        {
            return BoundingRectangle;
        }

        
    }
}
