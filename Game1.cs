﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MajorProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            // set lower resolution because I'm worried about my computer

            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 720;


            /*
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 720;
            */
            //graphics.IsFullScreen = true;
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
            // TODO: Add your initialization logic here

            // need to see where the pointer is
            this.IsMouseVisible = true;

            // get the screen dimensions and add it to the screenmanager
            ScreenManager.Instance.Dimensions = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            // initialise sprite batch and pass screenmanager all the graphics devices/helpers
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.GraphicsDevice = GraphicsDevice;
            ScreenManager.Instance.SpriteBatch = spriteBatch;
            ScreenManager.Instance.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                //Exit();

            // TODO: Add your update logic here

            // update the input first so updating game elements can compare
            InputManager.Instance.Update();

            // self-explainatory
            // if a quit is signalled by the input manager, exit
            if (InputManager.Instance.QuitSignaled)
            {

                // save user settings when exiting too
                PlayerPreferences.SavePreferences();

                Exit();
            }

            // update audio manager
            AudioManager.Instance.Update();

            // call the main update loop
            ScreenManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // clear with black background first
            GraphicsDevice.Clear(Color.Black);

            // begin spritebatch with no initial transformations
            spriteBatch.Begin();

            // call main draw loop
            ScreenManager.Instance.Draw(spriteBatch);

            //spriteBatch.Draw(image, destinationRectangle: new Rectangle(graphics.GraphicsDevice.Viewport.Width/2, graphics.GraphicsDevice.Viewport.Height/2, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), null, Color.White, 3.141596f, new Vector2(image.Width/2, image.Height/2), SpriteEffects.None, 1.0f);

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
