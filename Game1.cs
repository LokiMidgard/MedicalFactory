using MedicalFactory.GameObjects;
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
        private Background bg;
        public static Group sprites;
        private Sprite testSprite, testSprite2;

        public readonly Screen Screen;

        private XBoxController xBoxController;  // => added to controllers
        private Player playerOne;               // => added to players


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.DeviceCreated += (s, e) => DebugHelper.GraphicsDevice = _graphics.GraphicsDevice;

            Screen = new Screen(_graphics);

            bg = new Background(Screen);

            controllers = new Group();
            players = new Group();
            sprites = new Group();

            Screen.Add(bg);
            Screen.Add(controllers);
            Screen.Add(players);
            Screen.Add(sprites);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // initialize screen
            this.Screen.Initialize();

            // initialize Input Devices
            xBoxController = new XBoxController(0);
            controllers.Add(xBoxController);

            // initialize sprites
            testSprite = new Sprite("Roboter_Blau", "Roboter_Gruen", "Roboter_Gelb", "Roboter_Rot") { AnimationMode = AnimationMode.PingPong };
            testSprite.Origin = new Vector2(30.0f, 90.0f);
            testSprite.Position = new Vector2(300, 300);
            testSprite2 = new Sprite("Roboter_Blau", "Roboter_Gruen", "Roboter_Gelb", "Roboter_Rot") { AnimationMode = AnimationMode.Loop };
            testSprite2.Origin = new Vector2(30.0f, 90.0f);
            testSprite2.Position = new Vector2(100, 100);
            sprites.Add(testSprite);
            sprites.Add(testSprite2);

            for (int i = 0; i < 10; ++i)
            {
                Sprite conveyer = new Sprite("Fließband");
                conveyer.Position = new Vector2(i*180.0f + 90.0f, 60.0f*9.5f);
                sprites.Add(conveyer);
            }

            // initilize Particles
            var particles = new ParticleSystem(TimeSpan.FromSeconds(3), "particle")
            {
                Tint = Color.Red,
                SpawnRate = TimeSpan.FromSeconds(0.1),
                Velocety = Vector2.One,
                DeathDuration = TimeSpan.FromSeconds(1),
                Death = PatricleDeath.Fade,
                IsEnabled = true
            };
            sprites.Add(particles);

            /*
            Random r = new Random();
            for (int i = 0; i < 100; ++i)
            {
                Sprite blub = new Sprite("Roboter_Blau");
                blub.Position = Screen.GetRandomWorldPos();
                blub.Rotation = (float)MathHelper.ToRadians(90);
                blub.Rotation = (float)MyMathHelper.RightAngleInRadians(new Vector2(1, 0), new Vector2(-1, 0));
                sprites.Add(blub);
            }
            */

            // add some bodyparts
            for (int i = 0; i < 5; ++i)
            {
                var bodyPart = new BodyPart("Lunge");
                bodyPart.Position = Screen.GetRandomWorldPos();
                sprites.Add(bodyPart);
            }

            // initialize players
            playerOne = new Player(xBoxController, testSprite);
            players.Add(playerOne);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Screen.LoadContent(this);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (InputProvider.WasPressed(xBoxController, PaToRo_Desktop.Engine.Input.Buttons.A)) {
                if (playerOne.ControlledSprite == testSprite) {
                    playerOne.ControlledSprite = testSprite2;
                }
                else
                {
                    playerOne.ControlledSprite = testSprite;
                }
            }

            // detect collisions
            List<Collision> collisions = CollisionManager.GetCollisions(sprites);

            // update everything (turtles aka gameobjects all the way down)
            Screen.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.Screen.Draw(this._spriteBatch, gameTime);
        }
    }
}
