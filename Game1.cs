using MedicalFactory.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PaToRo_Desktop.Engine.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicalFactory
{
    public class Game1 : Game
    {
        public static Game1 game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Group controllers;  // input devices
        private Group playersGroup;      // player abstraction, see class Player
        public SpriteFont Font;
        public static Background Background;
        public static Group sprites;

        public readonly Screen Screen;

        //private XBoxController xBoxController, xBoxController2;  // => added to controllers
        //private Robot robot1;
        //private Robot robot2;

        public PatientFactory patientFactory;
        public static ConveyerBelt conveyerBelt;
        public static Group TopLayer;

        private Random rng = new Random();


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.DeviceCreated += (s, e) => DebugHelper.GraphicsDevice = _graphics.GraphicsDevice;

            Screen = new Screen(_graphics);

            Background = new Background(Screen);

            controllers = new Group();
            playersGroup = new Group();
            sprites = new Group();

            patientFactory = new PatientFactory();
            conveyerBelt = new ConveyerBelt();
            TopLayer = new Group();

            Screen.Add(Background);
            Screen.Add(controllers);
            Screen.Add(conveyerBelt);
            Screen.Add(playersGroup);
            Screen.Add(sprites);
            Screen.Add(TopLayer);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Game1.game = this;

            // initialize screen
            this.Screen.Initialize();

            // initialize players
            for (int i = 0; i < GameConfig.NumPlayers; ++i)
            {
                var xBoxController = new XBoxController(i);
                controllers.Add(xBoxController);

                var robot = new Robot((PlayerColor)(i % 4)) { Position = new Vector2(300 + (float)(rng.NextDouble() * 1500), i % 2 == 0 ? 300 : 800) };
                sprites.Add(robot);

                var player = new Player(xBoxController, robot);
                playersGroup.Add(player);
            }

            // add bodypartdispensers
            var bpdHeart = new BodyPartDispenser(DispenserType.Herzgerät, 2) { Position = new Vector2(535, 80) };
            conveyerBelt.Add(bpdHeart);

            var bpdLungs = new BodyPartDispenser(DispenserType.Lungengerät, 2) { Position = new Vector2(1350, 80) };
            conveyerBelt.Add(bpdLungs);

            var bpdKidneys = new BodyPartDispenser(DispenserType.Nierengerät, 2) { Position = new Vector2(945, 1035) };
            conveyerBelt.Add(bpdKidneys);

            // add scanner
            var scanner = new Scanner() { Position = new Vector2(1800, Game1.conveyerBelt.YPos) };
            conveyerBelt.Add(scanner.Lower);
            TopLayer.Add(scanner.RightWall);
            TopLayer.Add(scanner.Upper);
            TopLayer.Add(scanner);

            // add recycler
            var recycler = new Recycler() { Position = new Vector2(1810, 900) };
            recycler.AddDispenser(bpdHeart);
            recycler.AddDispenser(bpdLungs);
            recycler.AddDispenser(bpdKidneys);
            conveyerBelt.Add(recycler);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Screen.LoadContent(this);
            patientFactory.LoadContent(this);
            Font = Content.Load<SpriteFont>("PressStart2P");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            patientFactory.Update(gameTime);
            conveyerBelt.Update(gameTime);

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
