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
        public Texture2D heartTex;

        Texture2D exitSign;
        Vector2 exitSignPos;
        Texture2D exitDoor;
        Vector2 exitDoorPos;

        public int screenWidth, screenHeight;
        GameObject player;
        GameObject enemy, invisibleEnemy;
        public SpriteFont theFont;
        public Vector2 textPos, textPos2, healthPos, powerPos;
        Song bgsong;
        Song menuSong;
        bool songstart;

        public bool isPaused;
        Rectangle rectPause;
        Texture2D pauseBackground;

        //Menu Screen variables
        /*public Texture2D startButton;
        public Texture2D exitButton;
        public Texture2D pauseButton;
        public Texture2D resumeButton;

        public Vector2 startButtonPos;
        public Vector2 exitButtonPos;
        public Vector2 resumeButtonPos;
        
        MenuComponent menuComponent;
        */
        KeyboardState keyboardState;
        KeyboardState oldKeyboardState;

        GameScreen activeScreen;
        StartScreen startScreen;
        ActionScreen actionScreen;

        //enemy variables
        string enemyInfo;
        System.IO.StreamReader enemyFile;
        Texture2D enemySprite;
        Texture2D enemySpriteInvisible;
        public Texture2D dot;
        public List<GameObject> enemyList;


        //background variables
        const int BACKGROUND_RATE = 60;     //controls speed of space and stars background outside the ship
        Texture2D spaceBackgroundTex;
        Texture2D shipBackgroundTex;
        Space background_space;
        Background background_ship;

        //platform variables
        string platformInfo;
        System.IO.StreamReader platFile;
        Texture2D block;
        List<Platform> platList;

        //camera variables
        public Camera camera;

        //cursor variables
        Texture2D cursor;
        public Cursor mouse;
        Vector2 mousePos;

        //interactable objects
        string objectInfo;
        System.IO.StreamReader objectFile;
        Texture2D barrelTex;
        public List<GameObject> objectList;
        GameObject actionObject;

        //explosion variables
        Texture2D explodeTex;
        Vector2 explodePos, explodeVel;
        public Random generator;
        List<Explosion> explosionList;
        SoundEffect explodeSound;

        public SoundEffect bumpSound;
        //public SoundEffect powerSound;
        public bool gameEnd = false;

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
            isPaused = false;

            base.Initialize();

            //graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }

        protected override void LoadContent()
        {
            // default code that came with the project
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spritesheet = Content.Load<Texture2D>("zep spritesheet");
            heartTex = Content.Load<Texture2D>("heart");
            theFont = Content.Load<SpriteFont>("myFont");
            exitSign = Content.Load<Texture2D>("ExitSign");
            exitDoor = Content.Load<Texture2D>("exitDoor");
         

            //load menu screen
            startScreen = new StartScreen(this, spriteBatch, Content.Load<SpriteFont>("myfont"), Content.Load<Texture2D>("menu"));
            Components.Add(startScreen);
            startScreen.Hide();

            actionScreen = new ActionScreen(this, spriteBatch, Content.Load<Texture2D>("background_ship"));
            Components.Add(actionScreen);
            actionScreen.Hide();

            activeScreen = startScreen;
            activeScreen.Show();

            //load enemies from file
            enemyFile = new System.IO.StreamReader("Content\\enemyPosList.txt");
            enemySprite = Content.Load<Texture2D>("enemy");
            enemySpriteInvisible = Content.Load<Texture2D>("invisEnemy2");
            dot = Content.Load<Texture2D>("effector");

            //load song
            menuSong = Content.Load<Song>("menuSong"); 
            bgsong = Content.Load<Song>("SECRET IV - Adaptation");
            MediaPlayer.Play(menuSong);
            MediaPlayer.IsRepeating = true;

            //read block sheet
            platFile = new System.IO.StreamReader("Content\\platformsheet.txt");
            block = Content.Load<Texture2D>("block1");
            textPos = new Vector2(10, 30);
            textPos = new Vector2(10, 50);
            healthPos = new Vector2(10, 10);
            powerPos = new Vector2(200, 10);
            exitSignPos = new Vector2(-70, 200);
            exitDoorPos = new Vector2(3925, 70);

            //Load Backgrounds and Initialize
            spaceBackgroundTex = Content.Load<Texture2D>("space_bg");                   
            shipBackgroundTex = Content.Load<Texture2D>("background_ship");             
            background_space = new Space(spaceBackgroundTex);
            background_ship = new Background(shipBackgroundTex);
            //this.IsMouseVisible = true;
            //Mouse.WindowHandle = Window.Handle;
            Mouse.SetPosition(400, 300);

            // Get the width and height of the window
            screenWidth = 800;
            screenHeight = 600;
            
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();

            // Start the player off at the middle/bottom of the screen
            Vector2 initialPlayerPos = new Vector2(0, 380);

            // Bring the player to life
            player = new Player(spritesheet, initialPlayerPos, this);

            // Platform list created
            platList = new List<Platform>();
            //Generate platforms
            while ((platformInfo = platFile.ReadLine()) != null)
            {
                string[] tempStringArray = platformInfo.Split(',');
                Vector2 tempVect = new Vector2(float.Parse(tempStringArray[0]), float.Parse(tempStringArray[1]));
                Platform plat = new Platform(block, tempVect, this);
                platList.Add(plat);
            }

            //Generate Enemies
            enemyList = new List<GameObject>();
            while ((enemyInfo = enemyFile.ReadLine()) != null)
            {
                string[] tempStringArray = enemyInfo.Split(',');
                Vector2 tempVect = new Vector2(float.Parse(tempStringArray[1]), float.Parse(tempStringArray[2]));
                switch (Int32.Parse(tempStringArray[0]))
                {
                    case 0: enemy = new Enemy(enemySprite, tempVect, this, player);
                        enemyList.Add(enemy);
                        break;
                    case 1:
                        Texture2D enemySprite2 = Content.Load<Texture2D>("invisEnemy");
                        invisibleEnemy = new InvisibleEnemy(enemySprite2, enemySpriteInvisible, tempVect, this, player);
                        enemyList.Add(invisibleEnemy);
                        break;
                };
            }

            //cursor stuff
            cursor = Content.Load<Texture2D>("mouse");
            mousePos = new Vector2(0, 0);
            mouse = new Cursor(cursor, mousePos, this, player, enemyList);
            
            //load interactable objects
            barrelTex = Content.Load<Texture2D>("barrel");
            objectList = new List<GameObject>();
            //Vector2 tempVect2 = new Vector2(300, 25);
            objectFile = new System.IO.StreamReader("Content\\objectsheet.txt");
            while ((objectInfo = objectFile.ReadLine()) != null)
            {
                string[] tempStringArray = objectInfo.Split(',');
                Vector2 tempVect2 = new Vector2(float.Parse(tempStringArray[0]), float.Parse(tempStringArray[1]));
                actionObject = new Barrel(barrelTex, tempVect2, player, this);
                objectList.Add(actionObject);
            }
            
            //load explosion objects
            explodeTex = Content.Load<Texture2D>("fire");
            generator = new Random();
            explosionList = new List<Explosion>();
            explodePos = new Vector2();
            explodeVel = new Vector2();
            explodeSound = Content.Load<SoundEffect>("explosion");
            bumpSound = Content.Load<SoundEffect>("bump");
            //powerSound = Content.Load<SoundEffect>("sweep");
        }

        // Basically, just tell the player to update.
        // Again, remember that this function is called 60 times per second.

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit. Again, I added the escape key
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                (Keyboard.GetState().IsKeyDown(Keys.Escape)))
                this.Exit();
            //game menu update logic
            keyboardState = Keyboard.GetState();

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

                if(CheckKey(Keys.P))
                {
                    if (isPaused == false)
                    {
                        isPaused = true;
                        //MediaPlayer.Pause();
                        MediaPlayer.Volume = 0.20f;
                    }
                        
                    else
                    {
                        isPaused = false;
                        MediaPlayer.Volume = 1.0f;
                        //MediaPlayer.Resume();
                    }
                }

                if (isPaused == false)
                {
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
                    if (!gameEnd)
                    {
                        player.Update(gameTime, platList);
                    }
                    textPos.X = camera.getCamera().X + 25.0f;
                    textPos.Y = camera.getCamera().Y + 25.0f;

                    //Enemy update method. Deals with enemy movements, status, etc.

                    foreach (Enemy e in enemyList)
                    {
                        e.Update(gameTime, platList);
                    }

                    if (player.getHealth() == 0)
                    {
                        activeScreen.Hide();

                        activeScreen = startScreen;
                        activeScreen.Show();
                        MediaPlayer.Stop();
                        MediaPlayer.Play(menuSong);
                        //player.reset(); 
                        //Reset();


                        if (CheckKey(Keys.Enter))
                        {
                            if (startScreen.SelectedIndex == 0)
                            {
                                activeScreen.Hide();
                                activeScreen = actionScreen;
                                activeScreen.Show();
                                Reset();

                            }
                            if (startScreen.SelectedIndex == 1)
                            {
                                this.Exit();
                            }
                        }
                    }
                

                    //update the interactable objects in game
                    foreach (GameObject o in objectList)
                    {
                        o.Update(gameTime, platList);
                    }
                    for (int i = objectList.Count - 1; i >= 0; i--)//removes interactable objects. cannot be in barrel class
                    {
                        GameObject tempBarrel = objectList[i];
                        if (tempBarrel.getHealth() <= 0)
                        {
                            explodePos.X = tempBarrel.getPos().X;
                            explodePos.Y = tempBarrel.getPos().Y;
                            objectList.Remove(tempBarrel);
                            for (int j = 0; j < 8; j++)
                            {
                                /*vel.X = (float)generator.NextDouble() - 0.5f;
                                vel.Y = (float)generator.NextDouble() - 0.5f;
                                vel *= 1.7f;*/
                                double angle = generator.NextDouble() * (Math.PI * 2);
                                explodeVel.X = (float)Math.Cos(angle) * 1.8f;
                                explodeVel.Y = (float)Math.Sin(angle) * 1.3f;

                                Explosion e = new Explosion(explodeTex, explodePos, explodeVel);
                                explosionList.Add(e);
                            }
                            explodeSound.Play();
                        }
                        /*//update each particle
                        foreach (Explosion e in explosionList)
                        {
                            e.Update();
                        }
                        //remove explosions
                        for (int k = explosionList.Count - 1; k >= 0; k--)
                        {
                            Explosion tempParticle = explosionList[k];
                            if (!tempParticle.isAlive)
                            {
                                explosionList.Remove(tempParticle);
                                //explodeStart = false;
                            }
                        }*/
                    }

                    if (explosionList.Count() > 0)
                    {
                        //update each particle
                        foreach (Explosion e in explosionList)
                        {
                            e.Update();
                        }
                        //remove explosions
                        for (int k = explosionList.Count - 1; k >= 0; k--)
                        {
                            Explosion tempParticle = explosionList[k];
                            if (!tempParticle.isAlive)
                            {
                                explosionList.Remove(tempParticle);
                                //explodeStart = false;
                            }
                        }
                    }
                }
            }
            /*
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
            */
            
            //update mouse
            mouse.Update();

            //get last key press
            oldKeyboardState = keyboardState;

            //update camera movement
            camera.Update(this, 350, player);
            healthPos.X = camera.getCamera().X + 25.0f;
            powerPos.X = camera.getCamera().X + 550.0f;

            base.Update(gameTime);
        }

        // Basically, just tell the player to draw itself.
        // This is called 60 times per second as well.  
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if(isPaused == true)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, null, null, null, null, camera.transform);
            }

            if(isPaused == false)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.transform);
            }
            //spriteBatch.Begin();
            //draw backgrounds      
            background_space.Draw(spriteBatch);
            background_ship.Draw(spriteBatch);

            spriteBatch.Draw(exitSign,exitSignPos, Color.White);
            spriteBatch.Draw(exitDoor, exitDoorPos, Color.White);
            
            spriteBatch.DrawString(theFont, "Select Telekinesis and drag this\nenemy to the topmost platform.", new Vector2 (600, 430), Color.White);
            spriteBatch.DrawString(theFont, "Invisible enemy detected.\nThese enemy will only\nchase if you are within\na certain distance.", new Vector2(1500, 250), Color.White);
            spriteBatch.DrawString(theFont, "Drag these barrels onto\nthe enemies. Use wisely.\nThey can only be used 3 times.", new Vector2(1850, 400), Color.White);
            spriteBatch.DrawString(theFont, "Select the Gravity Flip power\nto get to these platforms.\nBe careful. Everything else\nwill float, too.", new Vector2(2700,390), Color.White);
            //draw platform
            foreach (Platform p in platList)
            {
                p.Draw(spriteBatch, theFont);
            }
            //draw enemies
            foreach (Enemy e in enemyList)
            {
                e.Draw(spriteBatch);
            }
            //draw interactable objects
            foreach (GameObject o in objectList)
            {
                o.Draw(spriteBatch);
            }
            //draw player
            player.Draw(spriteBatch);

            //draw mouse
            mouse.Draw(spriteBatch);

            //draw explosions
            foreach (Explosion e in explosionList)
            {
                e.Draw(spriteBatch);
            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        //was button released
        private bool CheckKey(Keys theKey)
        {
            return keyboardState.IsKeyUp(theKey) &&
                oldKeyboardState.IsKeyDown(theKey);
        }

        public void Reset()
        {
            // Start the player off at the middle/bottom of the screen
            Vector2 initialPlayerPos = new Vector2(0, 380);

            // Bring the player to life
            player = new Player(spritesheet, initialPlayerPos, this);

            //Enemy reset code
            enemyFile = new System.IO.StreamReader("Content\\enemyPosList.txt");

            enemyList = new List<GameObject>();
            while ((enemyInfo = enemyFile.ReadLine()) != null)
            {
                string[] tempStringArray = enemyInfo.Split(',');
                Vector2 tempVect = new Vector2(float.Parse(tempStringArray[1]), float.Parse(tempStringArray[2]));
                switch (Int32.Parse(tempStringArray[0]))
                {
                    case 0: enemy = new Enemy(enemySprite, tempVect, this, player);
                        enemyList.Add(enemy);
                        break;
                    case 1: invisibleEnemy = new InvisibleEnemy(enemySprite, enemySpriteInvisible, tempVect, this, player);
                        enemyList.Add(invisibleEnemy);
                        break;
                };
            }

            invisibleEnemy = new InvisibleEnemy(enemySprite, enemySpriteInvisible, new Vector2(215, 330), this, player);

            //load interactable objects
            barrelTex = Content.Load<Texture2D>("barrel");
            objectList = new List<GameObject>();
            Vector2 tempVect2 = new Vector2(320, 25);
            actionObject = new Barrel(barrelTex, tempVect2, player, this);
            objectList.Add(actionObject);

            //cursor stuff
            cursor = Content.Load<Texture2D>("mouse");
            mousePos = new Vector2(0, 0);
            mouse = new Cursor(cursor, mousePos, this, player, enemyList);
            Mouse.SetPosition(400, 300);

            MediaPlayer.Stop();
            MediaPlayer.Play(bgsong);
        }
    }
}
