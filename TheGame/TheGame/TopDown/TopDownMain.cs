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
using Microsoft.Xna.Framework.Input.Touch; //needed for android
#endif
using Newtonsoft.Json;

namespace TheGame.TopDown
{

    public class TopDownMain : Microsoft.Xna.Framework.Game
    {
#if ANDROID
        TouchCollection touch;
#endif
        Vector2 baseScreenSize;
        Matrix scaleMatrix;
        SpriteBatch spriteBatch;
        GraphicsDeviceManager graphics;
        SpriteFont font;
        MouseState mouse;
        KeyboardState keyboard;
        Map myMap;

        public TopDownMain()
        {
            graphics = new GraphicsDeviceManager(this);
            Window.AllowUserResizing = true;
            Content.RootDirectory = "Content";
            baseScreenSize = new Vector2(800, 600);

            this.IsMouseVisible = true;

            graphics.PreferredBackBufferHeight = (int)baseScreenSize.Y;
            graphics.PreferredBackBufferWidth = (int)baseScreenSize.X;

#if ANDROID
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
#endif

            this.Window.ClientSizeChanged += delegate { Window_ClientSizeChanged(); };

            myMap = new Map("Zelda");

            
        }

        void Window_ClientSizeChanged()
        {
            //handle the window resize
            scaleMatrix = Helper.GlobalTransformation(new Vector2((float)GraphicsDevice.PresentationParameters.BackBufferWidth, (float)GraphicsDevice.PresentationParameters.BackBufferHeight), baseScreenSize);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("myFont");
            myMap.sprite = Content.Load<Texture2D>("TopDown/Maps/" + myMap.MapName + "/map");

            
            base.LoadContent();
        }

        protected override void Initialize()
        {
            //set window size
            Window_ClientSizeChanged();

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            #region Read Mouse or Touch Panel
            mouse = Mouse.GetState();

            Point[] click = new Point[1] { Point.Zero };

            //account for screen size changes
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                click[0].X = (int)(mouse.X * baseScreenSize.X / Window.ClientBounds.Width);
                click[0].Y = (int)(mouse.Y * baseScreenSize.Y / Window.ClientBounds.Height);
            }

#if ANDROID
            touch = TouchPanel.GetState();
            click = new Point[touch.Count];
            for (int i = 0; i < touch.Count; i++)
            {
                click[i].X = (int)(touch[0].Position.X * baseScreenSize.X / Window.ClientBounds.Width);
                click[i].Y = (int)(touch[0].Position.Y * baseScreenSize.Y / Window.ClientBounds.Height);
            }
#endif
            #endregion

            // click is an array of points, adjusted to your screen size ready to use!
            /* save for later
            string json = JsonConvert.SerializeObject(myMap,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            string path = Content.RootDirectory + "/TopDown/Maps/" + myMap.MapName + "/map.json";
            System.IO.TextWriter tw = new System.IO.StreamWriter(path);
            tw.Write(json);
            tw.Close();
             */

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, scaleMatrix);

            for (int x = 0; x < myMap.MapArray.Length; x++)
            {
                for (int y = 0; y < myMap.MapArray[x].Length; y++)
                {
                    spriteBatch.Draw(myMap.sprite, new Rectangle(x * 50, y * 50, 50, 50), new Rectangle((int)(myMap.MapArray[x][y].X * myMap.TileSize.X), (int)(myMap.MapArray[x][y].Y * myMap.TileSize.Y), (int)myMap.TileSize.X, (int)myMap.TileSize.Y), Color.White);
                }
            }
            spriteBatch.DrawString(font, "Your Game!", new Vector2(100, 100), Color.White);




            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
