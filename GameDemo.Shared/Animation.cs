using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameDemo.Shared
{
    class Animation
    {
        Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
        }
        /// <summary>
        ///  Each texture is only 1 row so we just devide by the given columns
        /// </summary>
        public int FrameWidth
        {
            get { return Texture.Width / columns; }
        }
        public int totalFrames
        {
            get { return Texture.Width / FrameWidth; }
        }
        float milllisecondPerFrame;
        public float MillisecondPerFrame
        {
            get { return milllisecondPerFrame; }
        }

        int columns;

        public int FrameHeight
        {
            get { return Texture.Height; }
        }
        /// <summary>
        /// Check wether the animation is looping 
        /// </summary>
        public bool isLooping;

        public Animation(Texture2D texture,float millisecondPerFrames,int columns,bool isLooping)
        {
            this.texture = texture;
            this.milllisecondPerFrame = millisecondPerFrames;
            this.columns = columns;
            this.isLooping = isLooping;
        }
    }
}
