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
        private Texture2D palaceholderBackground;

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
            palaceholderBackground = Content.Load<Texture2D>("background");
            // TODO: use this.Content to load your game content here
            gameObjects.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            xBoxController.Update(gameTime);
            Position.X += xBoxController.Get(Sliders.LeftStickX);
            screen.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.screen.PreDraw();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            this._spriteBatch.Begin();

            this._spriteBatch.Draw(this.palaceholderBackground, Vector2.Zero, null, Color.White);

            this.gameObjects.Draw(this._spriteBatch, gameTime);

            this._spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            this.screen.PostDraw();
        }
    }
}
