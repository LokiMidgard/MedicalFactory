using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PaToRo_Desktop.Engine.Input;

namespace MedicalFactory
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Group gameObjects;
        private XBoxController xBoxController;
        private Vector2 Position;

        private readonly Screen screen;

        private Sprite testSprite;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            screen = new Screen(_graphics);
            gameObjects = new Group();


            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            xBoxController = new XBoxController(0);
            this.screen.Initialize();

            testSprite = new Sprite();
            gameObjects.Add(testSprite);


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            gameObjects.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            xBoxController.Update(gameTime);
            Position.X += xBoxController.Get(Sliders.LeftStickX);
            var keyboardstate = Keyboard.GetState();
            var isKeyDown = keyboardstate.IsKeyDown(Keys.F);
            if (isKeyDown)
            {
                screen.ToggleFullscreen();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.screen.PreDraw();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this._spriteBatch.Begin();
            this.gameObjects.Draw(this._spriteBatch, gameTime);
            this._spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            this.screen.PostDraw();
        }
    }

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

            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
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
