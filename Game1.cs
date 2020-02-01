using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PaToRo_Desktop.Engine.Input;
using System;
using System.Collections.Generic;

namespace MedicalFactory
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Group controllers;  // input devices
        private Group players;      // player abstraction, see class Player
        private Group sprites;

        private readonly Screen screen;

        private XBoxController xBoxController;  // => added to controllers
        private Player playerOne;               // => added to players

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.DeviceCreated += (s, e) => DebugHelper.GraphicsDevice = _graphics.GraphicsDevice;

            screen = new Screen(_graphics);

            controllers = new Group();
            players = new Group();
            sprites = new Group();

            screen.Add(controllers);
            screen.Add(players);
            screen.Add(sprites);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // initialize screen
            this.screen.Initialize();

            // initialize Input Devices
            xBoxController = new XBoxController(0);
            controllers.Add(xBoxController);

            // initialize sprites
            var testSprite = new Sprite("Roboter_Blau", "Roboter_Gruen", "Roboter_Gelb", "Roboter_Rot") { AnimationMode = AnimationMode.PingPong };
            testSprite.Origin = new Vector2(30.0f, 90.0f);
            var testSprite2 = new Sprite("Roboter_Blau", "Roboter_Gruen", "Roboter_Gelb", "Roboter_Rot") { AnimationMode = AnimationMode.Loop };
            testSprite2.Origin = new Vector2(30.0f, 90.0f);
            testSprite2.Position.X = 100;
            sprites.Add(testSprite);
            sprites.Add(testSprite2);


            // initilize Particles
            var particles = new ParticleSystem(TimeSpan.FromSeconds(3), "Roboter_Rot")
            {
                SpawnRate = TimeSpan.FromSeconds(0.1),
                Velocety = Vector2.One,
                IsEnabled = true
            };
            sprites.Add(particles);

            Random r = new Random();
            for (int i = 0; i < 100; ++i)
            {
                Sprite blub = new Sprite("Roboter_Blau");
                blub.Position.X = (float)r.NextDouble() * 1920;
                blub.Position.Y = (float)r.NextDouble() * 1080;
                blub.Origin = new Vector2(30.0f, 90.0f);
                blub.Rotation = (float)MathHelper.ToRadians(90);
                blub.Rotation = (float)MyMathHelper.RightAngleInRadians(new Vector2(1, 0), new Vector2(-1, 0));
                sprites.Add(blub);
            }

            // initialize players
            playerOne = new Player(xBoxController, testSprite);
            players.Add(playerOne);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            screen.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // detect collisions
            List<Collision> collisions = CollisionManager.GetCollisions(sprites);
            foreach (Collision c in collisions)
            {
                c.spriteA.hasCollision = true;
                c.spriteA.Rotation += 0.01f;
            }

            // update everything (turtles aka gameobjects all the way down)
            screen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.screen.Draw(this._spriteBatch, gameTime);
        }
    }
}
