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
        private Particle _particle;

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
            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;
            Random rnd = new Random();
            for (int i = 0; i < 7; i++)
            {
                int x1 = (int)(rnd.NextDouble() * width);
                int y1 = (int)(rnd.NextDouble() * height);
                int x2 = (int)(rnd.NextDouble() * width);
                int y2 = (int)(rnd.NextDouble() * height);
                _boundaries.Add(new Boundary(new Vector2(x1, y1), new Vector2(x2, y2)));
            }
            _particle = new Particle(new Vector2(100,200), -22, 23, 1);
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

            float x = _particle.Position.X;
            float y = _particle.Position.Y;
            if (kb.IsKeyDown(Keys.Left))
                x--;
            if (kb.IsKeyDown(Keys.Right))
                x++;
            if (kb.IsKeyDown(Keys.Up))
                y--;
            if (kb.IsKeyDown(Keys.Down))
                y++;

            _particle.Position = new Vector2(x, y);
            //_particle.LookAt(Mouse.GetState().Position.ToVector2());
            Vector2 dir = (Mouse.GetState().Position.ToVector2() - _particle.Position);
            _particle.Angle = (float)Math.Atan2(dir.Y, dir.X);
            _particle.Update(gameTime, _boundaries);

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
            _particle.Draw(spriteBatch, gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
