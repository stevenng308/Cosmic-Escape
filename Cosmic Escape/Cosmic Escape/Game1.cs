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
        Player player;
        public SpriteFont theFont;
        public Vector2 textPos;

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

            base.Initialize();

            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        protected override void LoadContent()
        {
            // default code that came with the project
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spritesheet = Content.Load<Texture2D>("zep spritesheet");
            theFont = Content.Load<SpriteFont>("myFont");
            //platformInfo = System.IO.File.ReadAllText("platform sheet.txt");
            file = new System.IO.StreamReader("platform sheet.txt");
            block = Content.Load<Texture2D>("block1");
            textPos = new Vector2(10, 10);

            // Get the width and height of the window
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            // Start the player off at the middle/bottom of the screen
            Vector2 initialPlayerPos = new Vector2(600, 300);
            // Bring the player to life
            player = new Player(spritesheet, initialPlayerPos, this);
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


            // Note that the Update method of the player MUST have access to the game time
            // to know which image/frame to draw
            player.Update(gameTime, platList);

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
            
            //draw player
            player.Draw(spriteBatch);

            //draw platform
            foreach (Platform p in platList)
            {
                p.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
