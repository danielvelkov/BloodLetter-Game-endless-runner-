using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameDemo.Shared.Menu
{
    class MenuOption
    {
        int scale;

        /// <summary>
        /// The GUIElement's texture
        /// </summary>
        private Texture2D guiTexture;

        /// <summary>
        /// The GUIElement's rectangle, this is used to determine if the lement is clicked
        /// </summary>
        private Rectangle guiRectangle;

        /// <summary>
        /// The name of the GUIElement, we need this to select a specific element
        /// </summary>
        private string elementName;

        /// <summary>
        /// Proptery to get the element's name
        /// </summary>
        public string ElementName
        {
            get { return elementName; }
        }

        /// <summary>
        /// Delegate used to the Element clicked event
        /// </summary>
        /// <param name="element">Name of the clicked element</param>
        public delegate void ElementClicked(string element);

        /// <summary>
        /// Event triggered every time an element is clicked
        /// </summary>
        public event ElementClicked clickEvent;

        /// <summary>
        /// The GUIElements constructor
        /// </summary>
        /// <param name="name">name of the element(also name of the texture)</param>
        public MenuOption(string name,int scale)
        {

            this.scale = scale;
            elementName = name;

        }

        /// <summary>
        /// Loads the element's texture
        /// </summary>
        /// <param name="content"></param>
        public virtual void LoadContent(ContentManager content)
        {
            guiTexture = content.Load<Texture2D>(elementName);
        }

        /// <summary>
        /// The update checks if the GUIElement is clicked
        /// </summary>
        public virtual void Update(GestureSample gesture)
        {

           
                if (guiRectangle.Contains(new Point((int)gesture.Position.X, (int)gesture.Position.Y)) && gesture.GestureType == GestureType.Tap)
                {

                    //This element was clicked
                    clickEvent(elementName);

                }
        }
        

        /// <summary>
        /// Draws the GUIElement
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(guiTexture, guiRectangle, Color.White);
        }

        /// <summary>
        /// Centers the GUIElement in the GameWindow
        /// </summary>
        /// <param name="windowSize"></param>
        public void CenterElement(Size windowSize)
        {
            guiRectangle = new Rectangle
                (
                    (windowSize.height / 2) - (this.guiTexture.Width / 2),
                    (windowSize.width / 2) - (this.guiTexture.Height / 2),
                    guiTexture.Width*scale,
                    guiTexture.Height*scale
                );
        }

        /// <summary>
        /// Move the GUIElement
        /// </summary>
        /// <param name="x">X-Offset</param>
        /// <param name="y">Y-Offset</param>
        public void MoveElement(int x, int y)
        {
            guiRectangle = new Rectangle
                (
                    guiRectangle.X + x,
                    guiRectangle.Y + y,
                    guiRectangle.Width,
                    guiRectangle.Height
                );
        }
    }
}

