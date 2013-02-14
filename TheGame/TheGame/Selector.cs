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
using System.Threading;


namespace TheGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Selector : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;

        int highlighted;
        List<String> games;
        KeyboardState keyState;
        bool inGame;
        Game gameToRun;
        TimeSpan debounce;
        DateTime lastButton;

        public Selector()
        {
            graphics = new GraphicsDeviceManager(this);

            debounce = new TimeSpan(0, 0, 0, 0, 500);

            Content.RootDirectory = "Content";

            // TODO: Construct any child components here
            highlighted = 0;
            games = new List<String>();
            games.Add("Example Game");
            games.Add("Top-Down");
            games.Add("Tron Game");
            games.Add("Your Game!");
            inGame = false;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);


            font = Content.Load<SpriteFont>("myFont");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Down))
                if (DateTime.Now - lastButton > debounce)
                {
                    lastButton = DateTime.Now;
                    if (highlighted + 1 < games.Count)
                        highlighted++;
                }
            if (keyState.IsKeyDown(Keys.Up))
                if (DateTime.Now - lastButton > debounce)
                {
                    lastButton = DateTime.Now;
                    if (highlighted - 1 >= 0)
                        highlighted--;
                }
            if (keyState.IsKeyDown(Keys.Enter))
            {
                    lastButton = DateTime.Now;
                    Thread thread = new Thread(new ThreadStart(RunGame));
                    thread.Name = "Game";
                    thread.Start();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteBatch.Begin();
            for (int i = 0; i < games.Count; i++)
            {
                Color clr;
                if (i == highlighted)
                    clr = Color.Black;
                else
                    clr = Color.White;
                spriteBatch.DrawString(font, games[i], new Vector2(0, i * 30), clr);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void RunGame()
        {
            try
            {
                if(!inGame)
                    switch(highlighted)
                    {
                        case 0:
                            inGame = true;
                            ExampleGame.ExampleGameMain app0 = new ExampleGame.ExampleGameMain();
                            app0.Run();
                            inGame = false;
                            break;
                        case 1:
                            inGame = true;
                            TopDown.TopDownMain app1 = new TopDown.TopDownMain();
                            app1.Run();
                            inGame = false;
                            break;
                        case 2:
                            inGame = true;
                            Tron.TronMain app2 = new Tron.TronMain();
                            app2.Run();
                            inGame = false;
                            break;
                        case 3:
                            inGame = true;
                            YourGame.YourGameMain app3 = new YourGame.YourGameMain();
                            app3.Run();
                            inGame = false;
                            break;
                    }
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException))
                {
                    //Do exception handling.
                }
            }
        }
    }
}
