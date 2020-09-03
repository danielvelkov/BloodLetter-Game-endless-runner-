using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameDemo.Shared
{
    class AnimationPlayer
    {
        Animation animation;
        public Animation Animation
        {
            get
            {
                return animation;
            }
        }

        public int FrameIndex
        {
            get
            {
                return frameIndex;
            }

        }
        public Vector2 Origin
        {
            get { return new Vector2(this.Animation.FrameWidth/2,this.Animation.FrameHeight/2);  }
            
        }
        int frameIndex;
        /// <summary>

        /// The amount of time in seconds that the current frame has been shown for.

        /// </summary>
        float time;

        /// <summary>

        /// Begins or continues playback of an animation.

        /// </summary>
        public void PlayAnimation(Animation animation)
        {
            if (Animation == animation)
            {
                return;
            }
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0;
                
        }
        /// <summary>

        /// Advances the time position and draws the current frame of the animation.

        /// </summary>

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects,float scale)
        {
            if (Animation == null)

                throw new NotSupportedException("No animation is currently playing.");
            
            // Process passing time.

            time += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            while (time > Animation.MillisecondPerFrame)
            {
                time -= Animation.MillisecondPerFrame;
                
                // Advance the frame index; looping or clamping as appropriate.

                if (Animation.isLooping)
                {
                    frameIndex = (frameIndex + 1) % Animation.totalFrames;
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, Animation.totalFrames - 1);

                }
            }
            
            // Calculate the source rectangle of the current frame.

            Rectangle source = new Rectangle(FrameIndex * Animation.FrameWidth, 0, Animation.FrameWidth, Animation.FrameHeight);
            
            // Draw the current frame.

            spriteBatch.Draw(Animation.Texture, position, source, Color.Aquamarine, 0.0f, Origin, scale, spriteEffects, 0.0f);
            

        }

    }
}
