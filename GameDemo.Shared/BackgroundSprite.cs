using System;
using System.Collections.Generic;
using System.Text;
using Android.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameDemo.Shared
{
    class BackgroundSprite
    {
        Texture2D texture;
        Vector2 position;
        Vector2 size;
        float speed;
        Color color;
        bool loops;
        string name;
        

        public bool Loops
        {
            get { return loops; }
        }

        public int FrameWidth
        {
            get { return (int)size.X; }
        }

        public string Name
        {
            get { return name; }
        }
        public BackgroundSprite(Texture2D texture, Vector2 position, Vector2 size, float speed, Color color,bool loops)
        {
            
            this.texture = texture;
            this.name = texture.Name;
            this.position = position;
            this.size = size;
            this.speed = speed;
            this.color = color;
            this.loops = loops;
        }

        public virtual void Draw(GameTime gametime,SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
        }

        // for looping background
        public virtual void Draw(GameTime gametime, SpriteBatch spriteBatch, int screenWidth) 
        {
            for(int i = 0; i < screenWidth / FrameWidth+1;i++)
            {
                spriteBatch.Draw(texture, new Rectangle((int)position.X+(i*FrameWidth), (int)position.Y, (int)size.X, (int)size.Y), color);
                spriteBatch.Draw(texture, new Rectangle((int)position.X + (i * FrameWidth) + screenWidth, (int)position.Y, (int)size.X, (int)size.Y), color);
            }
           
            
        }
        public virtual void Update(GameTime gametime, int screenWidth)
        {
            // if the player moves forwards the screen should go backwards
            position.X += speed;
            if (position.X <= -screenWidth)
            {
                position.X = 0;
            }
        }
    }
}
