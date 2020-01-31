using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace monoGameTest
{
    public class Screen
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch screenBatch;
        private RenderTarget2D canvas;

        public int Width { get; }
        public int Height { get; }

        public Screen(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
            this.Width = 1920;
            this.Height = 1080;
        }

        public void Initialize()
        {
            this.screenBatch = new SpriteBatch(graphics.GraphicsDevice);
            this.canvas = new RenderTarget2D(graphics.GraphicsDevice, Width, Height);

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;
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

        public void ToggleFullscreen()
        {
            graphics.PreferredBackBufferWidth = graphics.IsFullScreen ? 1280 : 1920;
            graphics.PreferredBackBufferHeight = graphics.IsFullScreen ? 720 : 1080;
            graphics.ToggleFullScreen();
        }
    }
}
