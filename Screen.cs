using System;
using System.Collections.Generic;
using MedicalFactory.GameObjects;
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

        private SpriteBatch screenBatch;
        public RenderTarget2D canvas;
        private Texture2D placeholderBackground;
        private Texture2D overlay;
        private readonly GraphicsDeviceManager graphics;

        private float DisplayNextScore = 10.0f;
        private Score DisplayedScore = null;
        public List<Score> scores = new List<Score>();
        public int Width { get; }
        public int Height { get; }

        private Color tint;
        private Vector2 overlayScale;

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
            this.screenBatch.Begin(blendState: BlendState.NonPremultiplied);
            this.screenBatch.Draw(this.canvas, new Rectangle(0, 0,
                spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth,
                spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            this.screenBatch.Draw(this.overlay, new Rectangle(0, 0,
               spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth,
               spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight), color: this.tint);
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
            this.overlay = game.Content.Load<Texture2D>("Overlay");

            base.LoadContent(game);
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardstate = Keyboard.GetState();

            var isKeyDownFullScreen = GetBlockingConveyer(Keys.F1);
            var isKeyDownDebugGraphic = GetBlockingConveyer(Keys.F2);
            var blockingConveyer = GetBlockingConveyer(Keys.F3);
            var speedUp = GetBlockingConveyer(Keys.F4);


            bool GetBlockingConveyer(Keys key)
            {
                return keyboardstate.IsKeyDown(key) && this.lastState.IsKeyUp(key);
            }


            if (isKeyDownFullScreen)
                this.ToggleFullscreen();

            if (isKeyDownDebugGraphic)
                GameConfig.DrawCollisionGeometry = !GameConfig.DrawCollisionGeometry;

            if (blockingConveyer)
                GameConfig.KeepPlayersToTheirSide = !GameConfig.KeepPlayersToTheirSide;

            if (speedUp)
                Game1.conveyerBelt.MaxSpeed = (Game1.conveyerBelt.MaxSpeed == ConveyerBelt.DefaultSpeed) ? ConveyerBelt.DefaultSpeed * 10f : ConveyerBelt.DefaultSpeed;



            var tintAmount = MathHelper.Clamp(Game1.CountOrgansOnFloor / 10f, 0f, 1f);
            this.tint = Color.Lerp(Color.Transparent, Color.Red, tintAmount);
            //this.tint = Color.Transparent;
            //this.tint = Color.Red;

            this.overlayScale = Vector2.One * (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds);

            this.lastState = keyboardstate;
            base.Update(gameTime);
        }


        private static Random rng = new Random();
        private KeyboardState lastState;

        public static Vector2 GetRandomWorldPos()
        {
            return new Vector2((float)rng.NextDouble() * bigWidth, (float)rng.NextDouble() * bigHeight);
        }
    }
}
