using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameDemo.Shared
{
    /// <summary>
    /// different movement for the enemies
    /// </summary>
    public class Movement

    {
        public static Vector2 SinWave(Vector2 position,double magnitude, double frequency,GameTime gameTime,ref float timeOnScreen)
        {
            float transform;
            timeOnScreen -= 0.007F;
            
            transform = (float)(Math.Sin((double)(timeOnScreen*50) * frequency) * magnitude);
            position.Y += transform;
            return position;
        }
        public static Vector2 Bouncing(Vector2 position,float frequency ,GameTime gameTime, ref float timeOnScreen)
        {
            if (position.Y <500)
            {
                timeOnScreen += 0.01F*frequency;
            }
            else if(position.Y >570)
            timeOnScreen -= 0.01F*frequency*2;

            float transform = position.Y * timeOnScreen/30; 
            position.Y = transform;
            return position;
        }
    }
}
