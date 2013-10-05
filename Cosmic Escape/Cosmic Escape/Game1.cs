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

        Texture2D enemySprite;

        public int screenWidth, screenHeight;
        Player player;
        Enemy enemy;
        public SpriteFont theFont;
        public Vector2 textPos;
        Song bgsong;
        bool songstart;

        //background variables
        const int BACKGROUND_RATE = 60;     //controls speed of space and stars background outside the ship
        Texture2D spaceBackgroundTex;
        Texture2D shipBackgroundTex;

        Space background_space;
        Background background_ship;
        

        //platform variables
        string platformInfo;
        System.IO.StreamReader file;
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
            // default code that came with the project
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spritesheet = Content.Load<Texture2D>("zep spritesheet");
            enemySprite = Content.Load<Texture2D>("enemy_sprite");
            theFont = Content.Load<SpriteFont>("myFont");
            //load song
            bgsong = Content.Load<Song>("SECRET IV - Adaptation");
            MediaPlayer.IsRepeating = true;
            //read block sheet
            file = new System.IO.StreamReader("Content\\platformsheet.txt");
            block = Content.Load<Texture2D>("block1");
            textPos = new Vector2(10, 10);

            //Load Backgrounds and Initialize
            spaceBackgroundTex = Content.Load<Texture2D>("space_bg");                   
            shipBackgroundTex = Content.Load<Texture2D>("background_ship");             
            background_space = new Space(spaceBackgroundTex);
            background_ship = new Background(shipBackgroundTex);

            // Get the width and height of the window
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            // Start the player off at the middle/bottom of the screen
            Vector2 initialPlayerPos = new Vector2(400, 300);
            // Test enemy start position
            Vector2 initialEnemyPos = new Vector2(250, 200);
            // Bring the player to life
            player = new Player(spritesheet, initialPlayerPos, this);
            // Bring enemy to life
            enemy = new Enemy(enemySprite, initialEnemyPos, this);
            // Platform list created
            platList = new List<Platform>();
            //Generate platforms
            while ((platformInfo = file.ReadLine()) != null)
            {
                string[] tempStringArray = platformInfo.Split(',');
                Vector2 tempVect = new Vector2(float.Parse(tempStringArray[0]), float.Parse(tempStringArray[1]));
                Platform plat = new Platform(block, tempVect);
                platList.Add(plat);
            }
            
        }

        // Basically, just tell the player to update.
        // Again, remember that this function is called 60 times per second.
        protected override void Update(GameTime gameTime)
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
            textPos.Y = camera.getCamera().Y;

            //Enemy update method. Deals with enemy movements, status, etc.
            enemy.Update(gameTime);

           

            //update camera movement
            camera.Update(gameTime, 32, player);
            
            base.Update(gameTime);
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

            //draw player
            player.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
