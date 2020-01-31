using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PaToRo_Desktop.Engine.Input;
using System;

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
        private Player playerOne;
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

            testSprite = new Sprite("Roboter_Blau", "Roboter_Gruen", "Roboter_Gelb", "Roboter_Rot") { AnimationMode = AnimationMode.PingPong };
            gameObjects.Add(testSprite);
            playerOne = new Player(xBoxController, testSprite);
            gameObjects.Add(playerOne);

            this.screen.Add(gameObjects);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            screen.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            xBoxController.Update(gameTime);
            screen.Update(gameTime);

            this.gameObjects.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.screen.Draw(this._spriteBatch, gameTime);
        }
    }
}
