using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameDemo.Shared.Menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using SQLite;

namespace GameDemo.Shared
{
    class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        SpriteBatch spriteBatch;
        Player player;
        double score;
        List<BackgroundSprite> spriteList = new List<BackgroundSprite>();
        List<Enemy> enemies = new List<Enemy>();

        public const float groundLevel= 800;
        public const float airLevel = 560;

        public TimeSpan previousSpawnTime;
        public TimeSpan enemySpawnTime;
        float timeonscreenMS;
        Random random;
        
        float rotationAngle = 0;

        Enemy e;
        int current_enemy;

        public static GameState gameState;
        MainMenu menu;
        SpriteFont sf;
        string name="";
        

        public SpriteManager(Game game) : base(game)
        {
           
            random = new Random();
            gameState = GameState.mainMenu;
            menu = new MainMenu();
            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // Scales and offsets of the player and enemy animation boundaries
            float playerscale = 2F;
            float enemyscale = 3;
            Vector2 offset_player = new Vector2(100, 70);

            sf = Game.Content.Load<SpriteFont>("font");

            // Load the buttons
            menu.LoadContent(Game.Content, new Size(1080, 1920));
            
            // Load the player,background and enemies
            player = new Player(Game.Content.Load<Texture2D>("horse_running"), new Vector2(130,groundLevel-99), new Point(184, 116), 11, offset_player, new Point(1, 1), new Point(3, 4),new Vector2(0,0),50,playerscale);
            player.LoadContent(Game.Content);
            spriteList.Add((new BackgroundSprite(Game.Content.Load<Texture2D>("parallax-mountain-bg"), new Vector2(0, 0), new Vector2(Game.Window.ClientBounds.Height, Game.Window.ClientBounds.Width), 0, Color.White, false)));
            spriteList.Add((new BackgroundSprite(Game.Content.Load<Texture2D>("montain-far"), new Vector2(0,-200), new Vector2(Game.Window.ClientBounds.Height, Game.Window.ClientBounds.Width), 0,Color.Green,false)));
            spriteList.Add((new BackgroundSprite(Game.Content.Load<Texture2D>("mountains"), Vector2.Zero, new Vector2(Game.Window.ClientBounds.Height, Game.Window.ClientBounds.Width), 0, Color.Yellow,false)));
            spriteList.Add((new BackgroundSprite(Game.Content.Load<Texture2D>("foreground-trees"),new Vector2(0,groundLevel-300), new Vector2(1920, 300), -1, Color.ForestGreen,true)));
            spriteList.Add(new BackgroundSprite(Game.Content.Load<Texture2D>("mountainGround"), new Vector2(0, groundLevel), new Vector2(200, 200), -10, Color.Gray,true));
            spriteList.Add(new BackgroundSprite(Game.Content.Load<Texture2D>("mountainTile"), new Vector2(0, groundLevel+200), new Vector2(200, 180), -10, Color.Gray,true));
            enemies.Add(new Enemy(ENEMY_TYPES.AXE,Game.Content.Load<Texture2D>("battleaxe-sheet"), new Vector2(2000, airLevel+50), new Point(16, 16), 8, Vector2.Zero, new Point(1, 1), new Point(8, 1), new Vector2(5, 0), 100, enemyscale+0.5F));
            enemies.Add(new Enemy(ENEMY_TYPES.SKELETON,Game.Content.Load<Texture2D>("useful skele"),new Vector2(2000,groundLevel-32*enemyscale), new Point(32, 32), 5,new Vector2(10,0), new Point(1, 1), new Point(5, 1), new Vector2(5,0),100,enemyscale));
            enemies.Add(new Enemy(ENEMY_TYPES.BOULDER, Game.Content.Load<Texture2D>("boulder2"), new Vector2(2000, groundLevel-140), new Point(76, 73), 1, Vector2.Zero, new Point(1, 1), new Point(1, 1), new Vector2(8, 0), 10, 1.5f));
            enemies.Add(new Enemy(ENEMY_TYPES.GHOST, Game.Content.Load<Texture2D>("ghost"), new Vector2(2000, airLevel), new Point(25, 35), 10, Vector2.Zero, new Point(1, 1), new Point(10, 1), new Vector2(5, 0), 100, enemyscale));
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)

        {
            if (gameState == GameState.mainMenu)
            {
                score = 0;
                menu.Update();
            }
                

            if (gameState == GameState.enterName)
            {
                if (KeyboardInput.IsVisible)
                {
                    gameState = GameState.viewLeaderboards;
                }
                // respawn the enemy
                e.Spawn(gameTime);
                // revive the player
                player.IsDead = false;
                // reset the score
                
                player.Position = new Vector2(130, groundLevel - 99);
                return;
            }

            if (gameState == GameState.viewLeaderboards)
            {
                menu.Update();
            }

            if (gameState==GameState.inGame)
            {
                score += 2;
                
                // Update player    
                player.Update(gameTime);

                // Update all background sprites   
                foreach (BackgroundSprite s in spriteList)
                {
                    s.Update(gameTime, 1920);
                }


                // Handle the spawning of enemies  (Currently: 1 enemy per screen)
                if (gameTime.TotalGameTime - previousSpawnTime > enemySpawnTime)
                {
                    current_enemy = random.Next(0, 4);
                    e = enemies[current_enemy];
                    
                    timeonscreenMS = (1400 / (e.Speed.X * 60))*10;
                    previousSpawnTime = e.Spawn(gameTime);

                    int spawnSeconds = random.Next(6, 7); // random should be a member of the class
                    enemySpawnTime = TimeSpan.FromSeconds(spawnSeconds);

                }
                
                e.Behaviour(gameTime, ref timeonscreenMS);
                e.Update(gameTime);

                // If the player hits an enemy show the keyboard
                if (Collide(e))
                {
                    player.IsDead = true;
                    if (!KeyboardInput.IsVisible)
                        NewKeyboard();
                    gameState = GameState.enterName;
                    return;
                }

            }

            base.Update(gameTime);
        }

        

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Game1.screenScale);

            if (gameState == GameState.mainMenu)
            {
                foreach (BackgroundSprite s in spriteList) { s.Draw(gameTime, spriteBatch, 1920); }
                menu.Draw(spriteBatch);

            }

            if (gameState == GameState.enterName)
            {
                spriteBatch.End();
                return;
            }
            
            if(gameState == GameState.viewLeaderboards)
            {
                spriteBatch.Draw(Game.Content.Load<Texture2D>("leaderboards"), new Rectangle(720, 100, 500, 900), Color.White);
                List<Scores> scores = Database.getAllScores();

                for (int i = 0, offset = 330; i < scores.Count; i++)
                {
                    Scores s = scores[i];
                    offset += 90;
                    spriteBatch.DrawString(sf, i+1 + ".   " + s.Name + "    " + s.score, new Vector2(800, offset), Color.Crimson);
                }
               
                menu.Draw(spriteBatch);
            }

            if (gameState == GameState.inGame)
            {
                if (player.IsDead) { GraphicsDevice.Clear(Color.Black); spriteBatch.End(); return; }

                rotationAngle -= 0.2F;
                

                // Draw backgroud
                foreach (BackgroundSprite s in spriteList)
                {
                    if(s.Loops)
                        s.Draw(gameTime, spriteBatch, 1920);
                    else
                    s.Draw(gameTime, spriteBatch);
                }

                // Draw player
                Rectangle rect = player.collisionRect();
                //spriteBatch.Draw(Game.Content.Load<Texture2D>("dummytexture"), rect, Color.AliceBlue);
                player.Draw(gameTime, spriteBatch, player.Scale, SpriteEffects.FlipHorizontally);
                
                spriteBatch.DrawString(sf, "Score: " + score, new Vector2(1600, 200), Color.Beige);
                // Draw current enemy

                //spriteBatch.Draw(Game.Content.Load<Texture2D>("dummytexture"), e.collisionRect(), Color.AliceBlue);
                if (e.Type == ENEMY_TYPES.BOULDER)
                    e.DrawRotating(gameTime, spriteBatch, e.Scale, SpriteEffects.None, rotationAngle);
                else if (e.Type == ENEMY_TYPES.GHOST)
                    e.Draw(gameTime, spriteBatch, e.Scale, SpriteEffects.FlipHorizontally);
                else
                    e.Draw(gameTime, spriteBatch, e.Scale, SpriteEffects.None);
                
            }
            spriteBatch.End();
        }
       
        protected bool Collide(Enemy enemy)
        {
            return player.collisionRect().Intersects(enemy.collisionRect());
        }

        public async void NewKeyboard()
        {
            await ShowKeyboard();
        }
        private async Task ShowKeyboard()
        {
            await Task.Run(async () =>
            {
                var result = await KeyboardInput.Show("enter your name", "name", "", false);
                if (null != result)
                {
                    //your method to set text goes here
                    name = result;
                    if (name == "")
                    {
                        name = "ANONYMOUS";
                    }
                    Scores score_player= new Scores();
                    score_player.Name = name;
                    score_player.score = score;
                    Database.SaveScore(score_player);
                   
                }
            });
        }
       
    }
}
