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
        public bool Paused;
        private GameTime MyGameTime = new GameTime();

        public static Random rng = new Random();


        public static Game1 game;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Group controllers;  // input devices
        private Group playersGroup;      // player abstraction, see class Player
        public SpriteFont Font;
        public static Background Background;
        public static TextBox TextBoxLeft;
        public static Group sprites;

        public readonly Screen Screen;

        public PatientFactory patientFactory;
        public static ConveyerBelt conveyerBelt;
        public static Group TopLayer;

        public FinishScreen FinishScreen;
        public StartScreen StartScreen { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.DeviceCreated += (s, e) => DebugHelper.GraphicsDevice = _graphics.GraphicsDevice;

            Screen = new Screen(_graphics);

            Background = new Background(Screen);

            controllers = new Group();
            playersGroup = new Group();
            sprites = new Group() { Enabled = false };

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

        public static int CountOrgansOnFloor
        {
            get
            {
                return sprites.Count(s => s is BodyPart && (s as BodyPart).AttachedTo == null);
            }
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

                var pos = (i % 4) switch
                {
                    0 => new Vector2(300, 300),
                    1 => new Vector2(300, 800),
                    2 => new Vector2(1400, 300),
                    3 => new Vector2(1400, 800),
                    _ => throw new NotImplementedException()
                };

                var robot = new Robot((PlayerColor)(i % 4)) { Position = pos };
                sprites.Add(robot);

                var player = new Player(xBoxController, robot);
                playersGroup.Add(player);
            }

            // add bodypartdispensers
            var bpdHeart = new BodyPartDispenser(DispenserType.Herzgerät, 2) { Position = new Vector2(945, 1035) };
            conveyerBelt.Add(bpdHeart);

            var bpdLungs = new BodyPartDispenser(DispenserType.Lungengerät, 2) { Position = new Vector2(1350, 80) };
            conveyerBelt.Add(bpdLungs);

            var bpdKidneys = new BodyPartDispenser(DispenserType.Nierengerät, 2) { Position = new Vector2(535, 80) };
            conveyerBelt.Add(bpdKidneys);

            // add scanner
            var scanner = new Scanner() { Position = new Vector2(1800, Game1.conveyerBelt.YPos) };
            conveyerBelt.Add(scanner.Lower);
            TopLayer.Add(scanner.RightWall);
            TopLayer.Add(scanner.Upper);
            TopLayer.Add(scanner);

            // prepare FinishScreen
            this.FinishScreen = new FinishScreen();
            TopLayer.Add(FinishScreen);

            // prepare StartScreen
            this.StartScreen = new StartScreen(playersGroup.OfType<Player>()) { Visible = true };
            TopLayer.Add(this.StartScreen);


            // add recycler
            var recycler = new Recycler() { Position = new Vector2(1810, 900) };
            recycler.AddDispenser(bpdHeart);
            recycler.AddDispenser(bpdLungs);
            recycler.AddDispenser(bpdKidneys);
            conveyerBelt.Add(recycler);

            // add Textboxes
            TextBoxLeft = new TextBox() { Position = new Vector2(30, 30), Width = 100, Height = 100 };
            TextBoxLeft.Text = "";
            TextBoxLeft.FontScale = 0.5f;
            TopLayer.Add(TextBoxLeft);

            base.Initialize();
        }

        public void ShowFinishScreen()
        {
            if (this.FinishScreen.Visible)
                return;
            this.FinishScreen.Visible = true;
            sprites.Enabled = false;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Songs.LoadContent(this);
            Credits.LoadContent(this);

            Songs.PlayTitleSong();

            Screen.LoadContent(this);
            patientFactory.LoadContent(this);
            Font = Content.Load<SpriteFont>("PressStart2P");
            SoundEffects.LoadContent(game);
        }

        protected override void Update(GameTime gameTime)
        {
            if (!Paused)
            {
                MyGameTime.TotalGameTime += gameTime.ElapsedGameTime;
                MyGameTime.ElapsedGameTime = gameTime.ElapsedGameTime;
            } else
            {
                MyGameTime.ElapsedGameTime = TimeSpan.Zero;
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (this.StartScreen.Visible)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    this.RestartGame();
                }
            }

            if (this.FinishScreen.Visible)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Enter))
                    this.RestartGame();
            }

            patientFactory.Update(MyGameTime);
            conveyerBelt.Update(MyGameTime);

            // detect collisions
            List<Collision> collisions = CollisionManager.GetCollisions(sprites);

            // update everything (turtles aka gameobjects all the way down)
            Screen.Update(MyGameTime);

            base.Update(MyGameTime);
        }

        private void RestartGame()
        {
            this.FinishScreen.Visible = false;
            this.StartScreen.Visible = false;
            Background.CleanFloor();
            conveyerBelt.ResetAll();
            this.patientFactory.Start();
            this.Screen.scores.Clear();
            sprites.Enabled = true;
        }

        protected override void Draw(GameTime gameTime)
        {
                this.Screen.Draw(this._spriteBatch, gameTime);
        }
    }
}
