using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace GameDemo.Shared
{
    public abstract class Sprite
    {
        Texture2D texture;
        protected Vector2 position;
        int totalFrames;
        protected Point frameSize;
        Point sheetSize;
        protected Vector2 collisionOffset;
        protected Point currentFrame;
        int timeSinceLastFrame;
        protected int millisecondsPerFrame;
        protected Vector2 speed;
        protected float scale;

        public Texture2D Texture
        {
            get { return texture; }
            private set { texture = value; }
        }

        // scales the animation size
        public float Scale{
            get { return scale; }
            set { scale = value; }
        }

        
        const int defaultMillisecondsPerFrame = 16;

        public Sprite(Texture2D texture, Vector2 position, Point frameSize, int totalFrames, Vector2 collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int secondsperFrame,float scale)
        {
            this.texture = texture;
            this.position = position;
            this.frameSize = frameSize;
            this.totalFrames = totalFrames;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = secondsperFrame;
            this.scale = scale;
        }
        public Sprite(Texture2D texture, Vector2 position, Point frameSize, int totalFrames, Vector2 collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,float scale) : this(texture, position, frameSize, totalFrames,
            collisionOffset, currentFrame, sheetSize, speed, defaultMillisecondsPerFrame,scale)
        { }

        // handle the sprite frames (can work with multiple rows and column)
        public virtual void Update(GameTime gametime)
        {
            
            timeSinceLastFrame += gametime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                
                ++currentFrame.X;
                                                            
                if (currentFrame.X >= sheetSize.X || (currentFrame.X*currentFrame.Y*2>totalFrames))
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y )
                        currentFrame.Y = 0;
                }
            }
            
           
        }
        public virtual void Draw(GameTime gametime, SpriteBatch spritebatch,float scale, SpriteEffects spriteEffects)
        {
            
            spritebatch.Draw(texture, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0, Vector2.Zero, scale,spriteEffects, 0);
        }

       // The collision rectangle of each enemy
        public abstract Rectangle collisionRect();
        
        }
    }


