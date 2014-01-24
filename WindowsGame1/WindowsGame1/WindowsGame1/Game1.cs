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

        SpriteFont font;

        static int WindowWidth = 800;
        static int WindowHeight = 800;
        static int frame = 0;
        static float scale = 0.125f;
        static int scale2 = (int)(1/scale);

        static double refreshRate = 0.0167;
        public double lambda = 1;
        public double output;
        public double outputOld = 0;

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

            // TODO: use this.Content to load your game content here
            // LOAD PICTURES< BUILD RECTANGLES
            circle0 = Content.Load<Texture2D>("circle");
            origin.X = circle0.Width / 2;
            origin.Y = circle0.Height / 2;
            spritePosition.X = (WindowWidth/2 - scale*(origin.X));
            spritePosition.Y = (WindowHeight/2 - scale*(origin.Y));

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

            // TODO: Add your update logic here
            //update balance
            //balance1.Update();

            int MaxX = WindowWidth - ((circle0.Width)/scale2);
            int MinX = 0;

            MouseState ms = Mouse.GetState();
            float MouseX = ms.X;
            float MouseY = ms.Y;
            Vector2 MouseXY = new Vector2(ms.X, 0);

            //spritePosition += spriteSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            //spriteSpeed.X = (spritePosition.X - MouseXY.X) / 100 + (spritePosition.X - WindowWidth / 2)/100;
            spriteSpeed.Y = 0;
            //spritePosition += spriteSpeed;

            output = Math.Exp(lambda * refreshRate)*outputOld + (Math.Exp(lambda * refreshRate)-1)*((-1*MouseX + origin.X)/50);
            float outputF = Convert.ToSingle(output);
            spritePosition.X += outputF;

            // Check for bounce.
            if (spritePosition.X > MaxX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MaxX;
            }

            else if (spritePosition.X < MinX)
            {
                spriteSpeed.X *= -1;
                spritePosition.X = MinX;
            }

            frame++;
            outputOld = output;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);
            
            // TODO: Add your drawing code here

     
            //Draw balance
            spriteBatch.Begin();
            //spriteBatch.Draw(balance0, drawRectangle0, Color.White);
            //balance1.Draw(spriteBatch);
            spriteBatch.Draw(rect0, new Vector2(150f, 150f), null, Color.WhiteSmoke, 0f, Vector2.Zero, new Vector2(500f, 500f), SpriteEffects.None, 0f);
            spriteBatch.Draw(rect0, new Vector2(350f, 350f), null, Color.White, 0f, Vector2.Zero, new Vector2(100f, 100f), SpriteEffects.None, 0f);

            if (spritePosition.X > ((WindowWidth / 2) - 20))
            {
                if (spritePosition.X < ((WindowWidth / 2) + 20))
                {
                    spriteBatch.Draw(circle0, spritePosition, null, Color.Green, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                }
                else spriteBatch.Draw(circle0, spritePosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else spriteBatch.Draw(circle0, spritePosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            DrawText(output);
            spriteBatch.End(); 
            base.Draw(gameTime);
        }

        private void DrawText(double output)
        {
            string outputS = ConvertDoubleString(output);
            spriteBatch.DrawString(font, outputS, new Vector2(20, 45), Color.White);
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
    }
}
