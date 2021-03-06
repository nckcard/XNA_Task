using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //menu stuff
        private Texture2D startButton;
        private Texture2D exitButton;
        private Vector2 startButtonPosition;
        private Vector2 exitButtonPosition;
        enum GameState
        {
            StartMenu,
            Playing
        }
        private GameState gameState;
        MouseState mouseState;
        MouseState previousMouseState;

        //timer stuff
        float timer;
        int timecounter;

        SpriteFont font;

        static int WindowWidth = 800;
        static int WindowHeight = 800;
        static int frame = 0;
        static float scale = 0.125f;
        static int scale2 = (int)(1/scale);

        static double refreshRate = 0.0167;
        public double lambda = 0.5;
        public double lambdaRate = 0.1;
        public double gain = 20;
        public double output;
        public double outputOld = 0;
        public double time = 0;
        public float MouseXi, MouseYi;
        public int MaxX;
        public int MinX;
        public int BoxWidth = 500;

        //strings
        public string lambdastr = "lambda";
        public string timestr = "elapsed time";

        //DECLARE DRAWING SUPPORT
        Texture2D circle0;
        Texture2D rect0;
        //TeddyBear balance1;

        Vector2 spritePosition = new Vector2((WindowWidth / 2), (WindowHeight / 2));
        Vector2 spriteSpeed = new Vector2(0.0f, 0.0f);
        Vector2 origin;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Change Resolution to 1080p
            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
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
            //enable mousepointer
            IsMouseVisible = true;

            // set button positions
            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);

            // set gamestate to start menu
            gameState = GameState.StartMenu;

            //get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

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
            font = Content.Load<SpriteFont>("var0");
            startButton = Content.Load<Texture2D>(@"start");
            exitButton = Content.Load<Texture2D>(@"exit");

            // TODO: use this.Content to load your game content here
            // LOAD PICTURES< BUILD RECTANGLES
            circle0 = Content.Load<Texture2D>("circle");
            origin.X = WindowWidth/2 - scale*(circle0.Width / 2);
            origin.Y = WindowHeight/2 - scale*(circle0.Height / 2);
            spritePosition.X = origin.X;
            spritePosition.Y = origin.Y;

            rect0 = new Texture2D(GraphicsDevice, 1, 1);
            rect0.SetData(new[] { Color.White });
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            rect0.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) //GETS CALLED EVERY FRAME
        {
            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            MaxX = (WindowWidth / 2) + (BoxWidth / 2) - ((circle0.Width) / scale2 / 2);
            MinX = (WindowWidth / 2) - (BoxWidth / 2) - ((circle0.Width) / scale2 / 2);

            if (gameState == GameState.Playing)
            {
                //Timer
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                timecounter += (int)timer;
                if (timer >= 1.0F)
                {
                    timer = 0F;
                    lambda += lambdaRate;
                }

                //mouse
                MouseState ms = Mouse.GetState();
                if (time == 0)
                {
                    MouseXi = ms.X;
                    MouseYi = ms.Y;
                }
                float MouseX = ms.X - MouseXi + origin.X + 10;
                float MouseY = ms.Y - MouseYi + origin.Y + 10;
                Vector2 MouseXY = new Vector2(MouseX, 0);

                //unstable system
                spriteSpeed.Y = 0;
                if ((Math.Abs(spritePosition.X - origin.X) - 250) < 0)
                {
                    output = Math.Exp(lambda * refreshRate) * outputOld - (Math.Exp(lambda * refreshRate) - 1) * gain * ((origin.X - MouseX) / (WindowWidth));
                }
                else
                {
                    output = 0;
                }
                float outputF = Convert.ToSingle(output);
                spritePosition.X += outputF;

                // Check for bounce.
                if (spritePosition.X > MaxX)
                {
                    spritePosition.X = origin.X;
                    outputOld = 0;
                    output = 0;
                    lambda = 0.5;
                    gameState = GameState.StartMenu;
                }

                else if (spritePosition.X < MinX)
                {
                    spritePosition.X = origin.X;
                    outputOld = 0;
                    output = 0;
                    lambda = 0.5;
                    gameState = GameState.StartMenu;
                }

                frame++;
                outputOld = output;
                time++;
            }
            //wait for mouseclick
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }

            previousMouseState = mouseState;

 
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);
            
            spriteBatch.Begin();

            // start menu
            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
            }

            // play game
            if (gameState == GameState.Playing)
            {
                spriteBatch.Draw(rect0, new Vector2(150f, 150f), null, Color.WhiteSmoke, 0f, Vector2.Zero, new Vector2(BoxWidth, BoxWidth), SpriteEffects.None, 0f);
                spriteBatch.Draw(rect0, new Vector2(350f, 350f), null, Color.White, 0f, Vector2.Zero, new Vector2(100f, 100f), SpriteEffects.None, 0f);

                if ((spritePosition.X - origin.X > -50) && (spritePosition.X - origin.X < 50))
                {
                    spriteBatch.Draw(circle0, spritePosition, null, Color.Green, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
                else spriteBatch.Draw(circle0, spritePosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

                MouseState ms2 = Mouse.GetState();
                float MouseX2 = ms2.X - MouseXi + origin.X + 10;
                float MouseY2 = ms2.Y - MouseYi + origin.Y + 10;
                Vector2 MouseXY2 = new Vector2(MouseX2, 0);
                spriteBatch.Draw(circle0, MouseXY2, null, Color.Green, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }

            //draw text
            DrawText(lambdastr, lambda, 20, 45);
            DrawText(timestr, timer, 20, 65);

            spriteBatch.End(); 
            base.Draw(gameTime);
        }

        private void DrawText(string title, double output, int xCord, int yCord)
        {
            string outputS = ConvertDoubleString(output);
            spriteBatch.DrawString(font, title + ": " + outputS, new Vector2(xCord, yCord), Color.White);
        }

        public string ConvertDoubleString(double doubleVal)
        {

            string stringVal;

            // A conversion from Double to string cannot overflow.       
            stringVal = System.Convert.ToString(doubleVal);
            System.Console.WriteLine("{0} as a string is: {1}",
                doubleVal, stringVal);

            return stringVal;
        }

        void MouseClicked(int x, int y)
        {
            //creates a rectangle of 10x10 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button
                {
                    gameState = GameState.Playing;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    Exit();
                }
            }
        }
    }
}
