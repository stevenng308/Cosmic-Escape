using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Cosmic_Escape
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D spritesheet;

        public int screenWidth, screenHeight;
        GameObject player;
        GameObject enemy;
        public SpriteFont theFont;
        public Vector2 textPos, healthPos;
        Song bgsong;
        bool songstart;

        public Texture2D startButton;
        public Texture2D exitButton;
        public Texture2D pauseButton;
        public Texture2D resumeButton;

        public Vector2 startButtonPos;
        public Vector2 exitButtonPos;
        public Vector2 resumeButtonPos;

        

        //enemy variables
        string enemyInfo;
        System.IO.StreamReader enemyFile;
        Texture2D enemySprite;
        public List<GameObject> enemyList;


        //background variables
        const int BACKGROUND_RATE = 60;     //controls speed of space and stars background outside the ship
        Texture2D spaceBackgroundTex;
        Texture2D shipBackgroundTex;

        Space background_space;
        Background background_ship;

        MenuComponent menuComponent;

        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        GameScreen activeScreen;
        StartScreen startScreen;
        ActionScreen actionScreen;

        //platform variables
        string platformInfo;
        System.IO.StreamReader platFile;
        Texture2D block;
        List<Platform> platList;

        //camera variables
        Camera camera;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            camera = new Camera(GraphicsDevice.Viewport);
            songstart = false;

            base.Initialize();

            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        protected override void LoadContent()
        {
            string[] menuItems = { "Start Game", "High Scores", "End Game" };


            // default code that came with the project
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spritesheet = Content.Load<Texture2D>("zep spritesheet");
            theFont = Content.Load<SpriteFont>("myFont");

            //menuComponent = new MenuComponent(this, spriteBatch, Content.Load<SpriteFont>("myfont"), menuItems);
            

            startScreen = new StartScreen(this, spriteBatch, Content.Load<SpriteFont>("myfont"), Content.Load<Texture2D>("StartScreen"));
            Components.Add(startScreen);
            startScreen.Hide();

            actionScreen = new ActionScreen(this, spriteBatch, Content.Load<Texture2D>("background_ship"));
            Components.Add(actionScreen);
            actionScreen.Hide();

            activeScreen = startScreen;
            activeScreen.Show();

            //Components.Add(menuComponent);

            //load enemies from file
            enemyFile = new System.IO.StreamReader("Content\\enemyPosList.txt");
            enemySprite = Content.Load<Texture2D>("enemy_sprite");

            //load song
            bgsong = Content.Load<Song>("SECRET IV - Adaptation");
            MediaPlayer.IsRepeating = true;

            //read block sheet
            platFile = new System.IO.StreamReader("Content\\platformsheet.txt");
            block = Content.Load<Texture2D>("block1");
            textPos = new Vector2(10, 30);
            healthPos = new Vector2(10, 10);

            //Load Backgrounds and Initialize
            spaceBackgroundTex = Content.Load<Texture2D>("space_bg");                   
            shipBackgroundTex = Content.Load<Texture2D>("background_ship");             
            background_space = new Space(spaceBackgroundTex);
            background_ship = new Background(shipBackgroundTex);
            this.IsMouseVisible = true;
            Mouse.SetPosition(400, 240);

            // Get the width and height of the window
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            enemyList = new List<GameObject>();
            // Start the player off at the middle/bottom of the screen
            Vector2 initialPlayerPos = new Vector2(0, 300);
            
            
            
            
            // Test enemy start position
            //Vector2 initialEnemyPos = new Vector2(250, 200);
            
            
            
            // Bring the player to life
            player = new Player(spritesheet, initialPlayerPos, this);
            // Bring enemy to life
            //enemy = new Enemy(enemySprite, initialEnemyPos, this, player);
            //enemyList.Add(enemy);

            // Platform list created
            platList = new List<Platform>();
            //Generate platforms
            while ((platformInfo = platFile.ReadLine()) != null)
            {
                string[] tempStringArray = platformInfo.Split(',');
                Vector2 tempVect = new Vector2(float.Parse(tempStringArray[0]), float.Parse(tempStringArray[1]));
                Platform plat = new Platform(block, tempVect);
                platList.Add(plat);
            }

            //Generate Enemies
            while ((enemyInfo = enemyFile.ReadLine()) != null)
            {
                string[] tempStringArray = enemyInfo.Split(',');
                Vector2 tempVect = new Vector2(float.Parse(tempStringArray[0]), float.Parse(tempStringArray[1]));
                enemy = new Enemy(enemySprite, tempVect, this, player);
                enemyList.Add(enemy);
            }
            
        }

        // Basically, just tell the player to update.
        // Again, remember that this function is called 60 times per second.
        protected override void Update(GameTime gameTime)
        {
           
            //update camera movement
            camera.Update(this, screenWidth, player);


            keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (activeScreen == startScreen)
            {
                if (CheckKey(Keys.Enter))
                {
                    if (startScreen.SelectedIndex == 0)
                    {
                        activeScreen.Hide();
                        activeScreen = actionScreen;
                        activeScreen.Show();
                    }
                    if (startScreen.SelectedIndex == 1)
                    {
                        this.Exit();
                    }
                }
            }

            if (activeScreen == actionScreen)
            {
                // Allows the game to exit. Again, I added the escape key
                if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                    (Keyboard.GetState().IsKeyDown(Keys.Escape)))
                    this.Exit();

                //moves space background
                background_space.MoveBackground((float)gameTime.ElapsedGameTime.TotalMilliseconds, player);

                //update ship background
                background_ship.Update(player);
                //start song
                if (!songstart)
                {
                    MediaPlayer.Play(bgsong);
                    songstart = true;
                }

                // Note that the Update method of the player MUST have access to the game time
                // to know which image/frame to draw
                player.Update(gameTime, platList);
                textPos.X = camera.getCamera().X + 25.0f;
                textPos.Y = camera.getCamera().Y + 25.0f;

                healthPos.X = camera.getCamera().X + 25.0f;
                //Enemy update method. Deals with enemy movements, status, etc.

                foreach (Enemy e in enemyList)
                {
                    e.Update(gameTime, platList);
                }
            }


            base.Update(gameTime);
            oldKeyboardState = keyboardState;
          
            base.Update(gameTime);
        }

        private bool CheckKey(Keys theKey)
{
	return keyboardState.IsKeyUp(theKey) &&
		oldKeyboardState.IsKeyDown(theKey);
}

        // Basically, just tell the player to draw itself.
        // This is called 60 times per second as well.  
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);

            //draw backgrounds      
            background_space.Draw(spriteBatch);
            background_ship.Draw(spriteBatch);

            //draw platform
            foreach (Platform p in platList)
            {
                p.Draw(spriteBatch, theFont);
            }

            foreach (Enemy e in enemyList)
            {
                e.Draw(spriteBatch);
            }

            //draw player
            player.Draw(spriteBatch);
            //enemy.Draw(spriteBatch);
            //spriteBatch.End();
            
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
