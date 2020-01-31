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

        public void PreDraw()
        {
            graphics.GraphicsDevice.SetRenderTarget(canvas);
        }

        public void PostDraw()
        {
            graphics.GraphicsDevice.SetRenderTarget(null);
            screenBatch.Begin();
            screenBatch.Draw(canvas, new Rectangle(0, 0,
                graphics.GraphicsDevice.PresentationParameters.BackBufferWidth,
                graphics.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            screenBatch.End();
        }

        private void ToggleFullscreen()
        {
            graphics.PreferredBackBufferWidth = graphics.IsFullScreen ? 1280 : bigWidth;
            graphics.PreferredBackBufferHeight = graphics.IsFullScreen ? smalHeight : bigHeight;
            graphics.ToggleFullScreen();
        }

        internal void Update(GameTime gameTime)
        {
            var keyboardstate = Keyboard.GetState();
            var isKeyDown = keyboardstate.IsKeyDown(Keys.F);

            if (isKeyDown && !lastKeyState)
            {
                this.ToggleFullscreen();
            }
            this.lastKeyState = isKeyDown;
        }
    }
}
