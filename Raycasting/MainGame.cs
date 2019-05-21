using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Raycasting
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        private List<Boundary> _boundaries;
        private RayViewer _rayViewer;
        private int _width;
        private int _height;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _boundaries = new List<Boundary>();

            _width = GraphicsDevice.Viewport.Width / 2;
            _height = GraphicsDevice.Viewport.Height;

            _boundaries.Add(new Boundary(Vector2.Zero, new Vector2(_width, 0)));
            _boundaries.Add(new Boundary(Vector2.Zero, new Vector2(0, _height)));
            _boundaries.Add(new Boundary(new Vector2(_width, 0), new Vector2(_width, _height)));
            _boundaries.Add(new Boundary(new Vector2(0, _height), new Vector2(_width, _height)));
            /*
            Random rnd = new Random();
            for (int i = 0; i < 7; i++)
            {
                int x1 = (int)(rnd.NextDouble() * _width);
                int y1 = (int)(rnd.NextDouble() * _height);
                int x2 = (int)(rnd.NextDouble() * _width);
                int y2 = (int)(rnd.NextDouble() * _height);
                int red = (int)(rnd.NextDouble() * 255);
                int green = (int)(rnd.NextDouble() * 255);
                int blue = (int)(rnd.NextDouble() * 255);
                _boundaries.Add(new Boundary(new Vector2(x1, y1), new Vector2(x2, y2)) { Color = new Color(red, green, blue) });
            }
            */
            _boundaries.Add(new Boundary(new Vector2(100, 100), new Vector2(150, 100)));
            _boundaries.Add(new Boundary(new Vector2(100, 100), new Vector2(100, 150)));
            _boundaries.Add(new Boundary(new Vector2(150, 100), new Vector2(100, 150)));
            //_rayViewer = new RayViewer(new Vector2(100, 200), 0, 360, 0.1f, Color.White, .03f)
            _rayViewer = new RayViewer(new Vector2(100, 200), 0, 1, 1f, Color.Red, 1f)
            {
                Draw3D = true,
                Area3D = new Rectangle(_width, 0, _width, _height)
            };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() { }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            KeyboardState kb = Keyboard.GetState();
            
            float x = _rayViewer.Position.X;
            float y = _rayViewer.Position.Y;
            if (kb.IsKeyDown(Keys.Left))
                x--;
            if (kb.IsKeyDown(Keys.Right))
                x++;
            if (kb.IsKeyDown(Keys.Up))
                y--;
            if (kb.IsKeyDown(Keys.Down))
                y++;

            x = MathHelper.Clamp(x, 0, _width);
            y = MathHelper.Clamp(y, 0, _height);

            _rayViewer.Position = new Vector2(x, y);

            Vector2 dir = new Vector2(_boundaries[6].A.X, _boundaries[6].A.Y) - _rayViewer.Position;
            //Vector2 dir = (Mouse.GetState().Position.ToVector2() - _rayViewer.Position);
            _rayViewer.Angle = (float)Math.Atan2(dir.Y, dir.X);
            _rayViewer.Update(gameTime, _boundaries);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            for (int i = 0; i < _boundaries.Count; i++)
            {
                Boundary b = _boundaries[i];
                b.Draw(spriteBatch, gameTime);
            }
            _rayViewer.Draw(spriteBatch, gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
