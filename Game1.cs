using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace monoGameTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private readonly Screen screen;

        private Texture2D stick;
        private Texture2D palaceholderBackground;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            screen = new Screen(_graphics);


            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            this.screen.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            stick = Content.Load<Texture2D>("stick");
            palaceholderBackground = Content.Load<Texture2D>("background");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardstate = Keyboard.GetState();
            var isKeyDown = keyboardstate.IsKeyDown(Keys.F);
            if (isKeyDown)
            {
                screen.ToggleFullscreen();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.screen.PreDraw();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this._spriteBatch.Begin();
            this._spriteBatch.Draw(this.palaceholderBackground, Vector2.Zero, null, Color.White);
            this._spriteBatch.Draw(this.stick, Vector2.Zero, null, Color.White);
            this._spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            this.screen.PostDraw();
        }
    }
}
