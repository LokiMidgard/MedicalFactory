using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MedicalFactory
{
    public class Screen : Group
    {
        private const int bigWidth = 1920;
        private const int bigHeight = 1080;
        private const int smalWidth = 1280;
        private const int smalHeight = 720;
        private bool lastKeyState;
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch screenBatch;
        private RenderTarget2D canvas;
        private Texture2D placeholderBackground;

        public int Width { get; }
        public int Height { get; }

        public Screen(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            this.Width = bigWidth;
            this.Height = bigHeight;
        }

        public void Initialize()
        {
            this.screenBatch = new SpriteBatch(graphics.GraphicsDevice);
            this.canvas = new RenderTarget2D(graphics.GraphicsDevice, Width, Height);

#if DEBUG
            graphics.PreferredBackBufferWidth = smalWidth;
            graphics.PreferredBackBufferHeight = smalHeight;
            graphics.IsFullScreen = false;
#else
            graphics.PreferredBackBufferWidth = bigHeight;
            graphics.PreferredBackBufferHeight = bigHeight;
            graphics.IsFullScreen = true;
#endif
            graphics.ApplyChanges();
        }

        public void PreDraw(SpriteBatch spriteBatch)
        {
            graphics.GraphicsDevice.SetRenderTarget(canvas);
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
        }

        public void PostDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            graphics.GraphicsDevice.SetRenderTarget(null);
            screenBatch.Begin();
            screenBatch.Draw(canvas, new Rectangle(0, 0,
                graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                graphics.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            screenBatch.End();
        }

        private void ToggleFullscreen()
        {
            graphics.PreferredBackBufferWidth = graphics.IsFullScreen ? smalWidth : bigWidth;
            graphics.PreferredBackBufferHeight = graphics.IsFullScreen ? smalHeight : bigHeight;
            graphics.ToggleFullScreen();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.PreDraw(spriteBatch);
            spriteBatch.Draw(placeholderBackground, Vector2.Zero, null, Color.White);
            base.Draw(spriteBatch, gameTime);
            this.PostDraw(spriteBatch);
        }

        public override void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            placeholderBackground = Content.Load<Texture2D>("background");

            base.LoadContent(Content);
        }

        public override void Update(GameTime gameTime)
        {

            var keyboardstate = Keyboard.GetState();
            var isKeyDown = keyboardstate.IsKeyDown(Keys.F1);

            if (isKeyDown && !lastKeyState)
            {
                this.ToggleFullscreen();
            }
            this.lastKeyState = isKeyDown;
            base.Update(gameTime);
        }
    }
}
