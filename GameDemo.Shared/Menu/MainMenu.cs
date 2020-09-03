using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameDemo.Shared.Menu
{
    struct Size
    {
        public int height { get; private set; }

        public int width { get; private set; }

        public Size(int width, int height) : this()
        {
            this.height = height;
            this.width = width;

        }

    }
    /// <summary>
    /// Enum used to determine the current GameState
    /// </summary>
    public enum GameState { mainMenu, howTo, inGame, enterName, viewLeaderboards,over }

    class MainMenu
    {
       
        private List<List<MenuOption>> menus;

        public MainMenu()
        {
            //Adds lists of GUI elements to the menu list
            menus = new List<List<MenuOption>>
            {   //MainMenu
                (new List<MenuOption>
                {
                    new MenuOption("playButton",2),
                    new MenuOption("scoresButton",2),
                    new MenuOption("quitButton",2)
                }),
                //Enter name menu
                (new List<MenuOption>
                {
                    new MenuOption("doneButton",2),
                })

            };
            //Sets the OnClick event on all menu items
            for (int i = 0; i < menus.Count; i++)
            {
                foreach (MenuOption button in menus[i])
                {
                    button.clickEvent += OnClick;
                }
            }

        }

        public void LoadContent(ContentManager content, Size windowSize)
        {
            
            //Loads the content of all other GUI elements
            for (int i = 0; i < menus.Count; i++)
            {
                foreach (MenuOption button in menus[i])
                {
                    button.LoadContent(content);
                    button.CenterElement(windowSize);
                }
            }
            //Sets offsets of the buttons
            menus[0].Find(x => x.ElementName == "playButton").MoveElement(-170, -350);
            menus[0].Find(x => x.ElementName == "scoresButton").MoveElement(-170, -100);
            menus[0].Find(x => x.ElementName == "quitButton").MoveElement(-170, 150);
            menus[1].Find(x => x.ElementName == "doneButton").MoveElement(-150, 300);
        }

        /// <summary>
        /// Updates all our GUI element(Mainly checks if any button is pressed)
        /// </summary>
        public void Update()
        {
            //Update each element according to the current gameState
            switch (SpriteManager.gameState)
            {

                //If you are the mainMenu screen handle the current button input
                case GameState.mainMenu:
                    var gesture = default(GestureSample);
                    while (TouchPanel.IsGestureAvailable)
                    {
                        gesture = TouchPanel.ReadGesture();
                        foreach (MenuOption button in menus[0]) //MainMenu
                        {
                            
                            button.Update(gesture);
                        }
                    }
                    break;
               
                //if you are at the leaderboards screen handle the current button input
                case GameState.viewLeaderboards:
                    gesture = default(GestureSample);
                    while (TouchPanel.IsGestureAvailable)
                    {
                        gesture = TouchPanel.ReadGesture();
                        foreach (MenuOption button in menus[1]) //MainMenu
                        {

                            button.Update(gesture);
                        }
                    }
                    break;
                case GameState.over:
                    break;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draws each element according to the current gameState
            switch (SpriteManager.gameState)
            {
                case GameState.mainMenu:
                    foreach (MenuOption button in menus[0]) //MainMenu
                    {
                        button.Draw(spriteBatch);
                    }
                    break;
                case GameState.viewLeaderboards:
                    foreach (MenuOption button in menus[1])//leaderboards menu
                    {
                        button.Draw(spriteBatch);
                    }
                    break;
                case GameState.inGame://Ingame GUI
                    break;
            }

        }

        /// <summary>
        /// Method called every time a GUI element is clicked
        /// </summary>
        /// <param name="element"></param>
        public void OnClick(string element)
        {
            if (element == "playButton")//PlayButton
            {
                SpriteManager.gameState = GameState.inGame;
            }
            if (element == "scoresButton")//scores button
            {
                SpriteManager.gameState = GameState.viewLeaderboards;
            }
            if (element == "quitButton")//Quit button
            {
                SpriteManager.gameState = GameState.over; 
            }
            if( element== "doneButton")
            {
                SpriteManager.gameState = GameState.mainMenu;
            }

        }
    }
}
