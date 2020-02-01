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
        private bool lastFullScreenKey;
        private bool lastFullDebugGraficKey;

        private SpriteBatch screenBatch;
        public RenderTarget2D canvas;
        private Texture2D placeholderBackground;
        private readonly GraphicsDeviceManager graphics;

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
            this.screenBatch = new SpriteBatch(this.graphics.GraphicsDevice);
            this.canvas = new RenderTarget2D(this.graphics.GraphicsDevice, this.Width, this.Height);

#if DEBUG
            this.graphics.PreferredBackBufferWidth = smalWidth;
            this.graphics.PreferredBackBufferHeight = smalHeight;
            this.graphics.IsFullScreen = false;
#else
            graphics.PreferredBackBufferWidth = bigHeight;
            graphics.PreferredBackBufferHeight = bigHeight;
            graphics.IsFullScreen = true;
#endif
            this.graphics.ApplyChanges();
        }

        public void PreDraw(SpriteBatch spriteBatch)
        {

            spriteBatch.GraphicsDevice.SetRenderTarget(this.canvas);
            spriteBatch.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(sortMode: SpriteSortMode.Immediate);
        }

        public void PostDraw(SpriteBatch spriteBatch)
        {
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            this.screenBatch.Begin();
            this.screenBatch.Draw(this.canvas, new Rectangle(0, 0,
                spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth,
                spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            this.screenBatch.End();
        }

        private void ToggleFullscreen()
        {
            this.graphics.PreferredBackBufferWidth = this.graphics.IsFullScreen ? smalWidth : bigWidth;
            this.graphics.PreferredBackBufferHeight = this.graphics.IsFullScreen ? smalHeight : bigHeight;
            this.graphics.ToggleFullScreen();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.PreDraw(spriteBatch);
            spriteBatch.Draw(this.placeholderBackground, Vector2.Zero, null, Color.White);
            base.Draw(spriteBatch, gameTime);
            this.PostDraw(spriteBatch);
        }

        public override void LoadContent(Game1 game)
        {
            this.placeholderBackground = game.Content.Load<Texture2D>("background");

            base.LoadContent(game);
        }

        public override void Update(GameTime gameTime)
        {

            var keyboardstate = Keyboard.GetState();
            var isKeyDownFullScreen = keyboardstate.IsKeyDown(Keys.F1);
            var isKeyDownDebugGraphic = keyboardstate.IsKeyDown(Keys.F2);

            if (isKeyDownFullScreen && !this.lastFullScreenKey)
                this.ToggleFullscreen();

            if (isKeyDownDebugGraphic && !this.lastFullDebugGraficKey)
                GameConfig.DrawCollisionGeometry = !GameConfig.DrawCollisionGeometry;

            this.lastFullScreenKey = isKeyDownFullScreen;
            this.lastFullDebugGraficKey = isKeyDownDebugGraphic;
            base.Update(gameTime);
        }

        private static Random rng = new Random();
        public static Vector2 GetRandomWorldPos()
        {
            return new Vector2((float)rng.NextDouble() * bigWidth, (float)rng.NextDouble() * bigHeight);
        }
    }
}
