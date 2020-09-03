#region Using Statements
using System;
using System.Threading.Tasks;
using Android.OS;
using GameDemo.Shared.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SQLite;

#endregion

namespace GameDemo.Shared
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        
        // scales the game to any aspect ratio and resolution of a screen
        public static Matrix screenScale = Matrix.Identity;
        
        public Game1()
        {
           

            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
           
            graphics.IsFullScreen = true;
            
            graphics.ApplyChanges();

            
            // add the handler corresponding to the display orientation
            if (this.Window.CurrentOrientation== DisplayOrientation.Portrait)
            this.Window.ClientSizeChanged += WindowSizeChange;
            else
            {
                // no need to change anything
            }
            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
           
            TouchPanel.EnabledGestures = GestureType.Tap  | GestureType.None | GestureType.Flick;
            
            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            //TODO: use this.Content to load your game content here 
        }
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)    // gameTime is actuallly used because every processor speed is different so we need the time that HAS PASSED during the running of the game to code
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (SpriteManager.gameState == GameState.over)
            {
                Process.KillProcess(Process.MyPid());

            }

            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
            // TODO: Add your update logic here			
            base.Update(gameTime);
        }

        // if the resolution is changed (we are working with a 1920,1080 view)
        public void WindowSizeChange(object sender, EventArgs e)
        {
            var bw = GraphicsDevice.PresentationParameters.BackBufferWidth;
            var bh = GraphicsDevice.PresentationParameters.BackBufferHeight;
            screenScale = Matrix.Identity * Matrix.CreateScale(bw / 1080, bh / 1920, 0f);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
           
            //TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, screenScale);
           
            spriteBatch.End();
            base.Draw(gameTime);
            
        }
    }
}
