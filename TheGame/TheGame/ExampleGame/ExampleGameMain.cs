/*
 *This game is only for reference, use code examples from the other games in this project 
 * 
 * 
 * Created By Ryan Hatfield
 * 
 * 
 * 
 * 
 */

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

#if ANDROID
using Microsoft.Xna.Framework.Input.Touch;
#endif

namespace TheGame.ExampleGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ExampleGameMain : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        #if ANDROID
        private TouchCollection touch;
        #endif
        private MouseState mouse;
        private Vector2 baseScreenSize;
        private Texture2D background;
        private TheHead[] Heads;
        private int pos = 0;
        private DateTime lastHead;
        private TimeSpan headInterval;
        private Vector2 lastMousePos;
        private Texture2D headTexture;
        private Random random;
        private int score;
        private bool gameOver;
        private SpriteFont font;

        public Rectangle[] positions = new Rectangle[10]
        {
            new Rectangle(85, 20, 231 / 4, 166 / 4), 
            new Rectangle(215, 20, 231 / 4, 166 / 4), 
            new Rectangle(345, 20, 231 / 4, 166 / 4), 
            new Rectangle(55, 70, (int)(231 / 3.5), (int)(166 / 3.5)), 
            new Rectangle(160, 70, (int)(231 / 3.5), (int)(166 / 3.5)), 
            new Rectangle(265, 70, (int)(231 / 3.5), (int)(166 / 3.5)), 
            new Rectangle(370, 70, (int)(231 / 3.5), (int)(166 / 3.5)), 
            new Rectangle(65, 130, (int)(231 / 2.5), (int)(166 / 2.5)), 
            new Rectangle(200, 130, (int)(231 / 2.5), (int)(166 / 2.5)), 
            new Rectangle(330, 130, (int)(231 / 2.5), (int)(166 / 2.5))
        };

        public ExampleGameMain()
        {
            
            #region Giberish you don't need to know about
            graphics = new GraphicsDeviceManager(this);

            this.IsMouseVisible = true;

            Window.AllowUserResizing = true;
            graphics.PreferredBackBufferHeight = 250;
            graphics.PreferredBackBufferWidth = 500;

#if ANDROID
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
#endif
            baseScreenSize = new Vector2(500, 250);

            Content.RootDirectory = "Content";
            #endregion

            random = new Random();

            Heads = new TheHead[10];
            for (int i = 0; i < positions.Length; i++)
            {
                Heads[i] = new TheHead();
                Heads[i].SourceBox = new Rectangle(0, 0, 231, 166);
                Heads[i].DrawBox = positions[i];
                Heads[i].Hit = true;
            }

            headInterval = new TimeSpan(0, 0, 1);
            gameOver = false;
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

            background = Content.Load<Texture2D>("ExampleGame/scene");
            headTexture = Content.Load<Texture2D>("ExampleGame/jeremyhead");
            for (int i = 0; i < positions.Length; i++)
                Heads[i].Head = headTexture;

            font = Content.Load<SpriteFont>("myFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            mouse = Mouse.GetState();
            if (lastMousePos == null || lastMousePos != new Vector2(mouse.X, mouse.Y))
            {
                lastMousePos = new Vector2(mouse.X, mouse.Y);
            }

            Point click = Point.Zero;


            //account for screen size changes
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                click.X = (int)(mouse.X * baseScreenSize.X / Window.ClientBounds.Width);
                click.Y = (int)(mouse.Y * baseScreenSize.Y / Window.ClientBounds.Height);
            }

            #if ANDROID
            touch = TouchPanel.GetState();
            if(touch.Count>0)
            {
                click.X = (int)(touch[0].Position.X * baseScreenSize.X / Window.ClientBounds.Width);
                click.Y = (int)(touch[0].Position.Y * baseScreenSize.Y / Window.ClientBounds.Height);
            }
            #endif

            if (!gameOver)
            {
                int headCount = 0;
                for (int i = 0; i < Heads.Length; i++)
                {
                    if (!Heads[i].Hit)
                        headCount++;
                }

                if (headCount > 9)
                {
                    gameOver = true;
                }

                for (int i = 0; i < Heads.Length; i++)
                {
                    if (Heads[i].CollisionBox.Contains(click))
                    {
                        if (Heads[i].Hit == false)
                        {
                            score++;
                            headInterval -= new TimeSpan(0,0,0,0,10);
                        }
                        Heads[i].Hit = true;
                    }
                }


                //add a head
                TimeSpan delta = DateTime.Now - lastHead;
                if (delta > headInterval)
                {
                    lastHead = DateTime.Now;
                    bool foundOne = false;
                    int i = 0;
                    while (!foundOne)
                    {
                        i = random.Next(0, 10);
                        if (Heads[i].Hit == true)
                            foundOne = true;
                    }

                    Heads[i] = new TheHead();
                    Heads[i].SourceBox = new Rectangle(0, 0, 231, 166);
                    Heads[i].DrawBox = positions[i];
                    Heads[i].Head = headTexture;
                }

            }
            else
            {
                TimeSpan delta = DateTime.Now - lastHead;
                if(delta > new TimeSpan(0,0,10))
                {
                    //reset game
                    Heads = new TheHead[10];
                    for (int i = 0; i < positions.Length; i++)
                    {
                        Heads[i] = new TheHead();
                        Heads[i].SourceBox = new Rectangle(0, 0, 231, 166);
                        Heads[i].DrawBox = positions[i];
                        Heads[i].Hit = true;
                    }

                    headInterval = new TimeSpan(0, 0, 1);
                    gameOver = false;

                    score = 0;
                }
            }
            // TODO: Add your update logic here

            base.Update(gameTime);

        }

        protected Matrix GlobalTransformation
        {
            get
            {
                Vector3 screenScalingFactor;
                float horScaling = (float)GraphicsDevice.PresentationParameters.BackBufferWidth / baseScreenSize.X;
                float verScaling = (float)GraphicsDevice.PresentationParameters.BackBufferHeight / baseScreenSize.Y;
                screenScalingFactor = new Vector3(horScaling, verScaling, 1);

                return Matrix.CreateScale(screenScalingFactor);
            }
        }

        protected Matrix GlobalTransformation2
        {
            get
            {
                Vector3 screenScalingFactor;
                float horScaling = (float)Window.ClientBounds.Width / baseScreenSize.X;
                float verScaling = (float)Window.ClientBounds.Height / baseScreenSize.Y;
                screenScalingFactor = new Vector3(horScaling, verScaling, 1);

                return Matrix.CreateScale(screenScalingFactor);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, GlobalTransformation2);
            // TODO: Add your drawing code here
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            for(int i = 0; i < positions.Length; i++)
                if(!Heads[i].Hit)
                    spriteBatch.Draw(Heads[i].Head, Heads[i].DrawBox, Color.White);

            spriteBatch.DrawString(font, Window.ClientBounds.Width + "," + Window.ClientBounds.Height, Vector2.Zero, Color.White);
            if (gameOver)
            {
                spriteBatch.DrawString(font, "TOO MANY JEREMYS", Vector2.Zero, Color.Black, 0, Vector2.Zero,2.5f, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, "Game Over!!", new Vector2(150, 50), Color.Black, 0, Vector2.Zero, 3f, SpriteEffects.None, 0);
            }
            spriteBatch.DrawString(font, "SCORE: " + score, new Vector2(0, 200), Color.Black, 0, Vector2.Zero, 2.5f, SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
