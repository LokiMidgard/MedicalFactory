﻿using MedicalFactory.GameObjects;
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
        private Group players;      // player abstraction, see class Player
        public static Background Background;
        public static Group sprites;

        public readonly Screen Screen;

        private XBoxController xBoxController, xBoxController2;  // => added to controllers
        private Player playerOne, playerTwo;               // => added to players
        private Robot robot1;
        private Robot robot2;

        public PatientFactory patientFactory;
        public ConveyerBelt conveyerBelt;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.DeviceCreated += (s, e) => DebugHelper.GraphicsDevice = _graphics.GraphicsDevice;

            Screen = new Screen(_graphics);

            Background = new Background(Screen);

            controllers = new Group();
            players = new Group();
            sprites = new Group();

            patientFactory = new PatientFactory();
            conveyerBelt = new ConveyerBelt();

            Screen.Add(Background);
            Screen.Add(controllers);
            Screen.Add(conveyerBelt);
            Screen.Add(players);
            Screen.Add(sprites);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Game1.game = this;

            // initialize screen
            this.Screen.Initialize();

            // initialize Input Devices
            xBoxController = new XBoxController(0);
            controllers.Add(xBoxController);
            xBoxController2 = new XBoxController(1);
            controllers.Add(xBoxController2);

            robot1 = new Robot(PlayerColor.Roboter_Blau)
            {
                Position = new Vector2(300, 300)
            };
            robot2 = new Robot(PlayerColor.Roboter_Gelb)
            {
                Position = new Vector2(100, 100)
            };
            sprites.Add(robot1);
            sprites.Add(robot2);

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

            // add bodypartdispensers
            var bpdHeart = new BodyPartDispenser(DispenserType.Herzgerät, 2) { Position = new Vector2(535, 80) };
            conveyerBelt.Add(bpdHeart);

            var bpdLungs = new BodyPartDispenser(DispenserType.Lungengerät, 2) { Position = new Vector2(1350, 80) };
            conveyerBelt.Add(bpdLungs);

            var bpdKidneys = new BodyPartDispenser(DispenserType.Nierengerät, 2) { Position = new Vector2(945, 1035) };
            conveyerBelt.Add(bpdKidneys);

            // add recycler
            var recycler = new Recycler() { Position = new Vector2(1810, 900) };
            recycler.AddDispenser(bpdHeart);
            recycler.AddDispenser(bpdLungs);
            recycler.AddDispenser(bpdKidneys);
            conveyerBelt.Add(recycler);


            // initialize players
            playerOne = new Player(xBoxController, robot1);
            players.Add(playerOne);

            playerTwo = new Player(xBoxController2, robot2);
            players.Add(playerTwo);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Screen.LoadContent(this);
            patientFactory.LoadContent(this);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            patientFactory.Update(gameTime);
            conveyerBelt.Update(gameTime);
            if (InputProvider.WasPressed(xBoxController, PaToRo_Desktop.Engine.Input.Buttons.A))
            {
                if (playerOne.ControlledSprite == robot1)
                {
                    playerOne.ControlledSprite = robot2;
                }
                else
                {
                    playerOne.ControlledSprite = robot1;
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
