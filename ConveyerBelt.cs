using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using MedicalFactory.GameObjects;
using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace MedicalFactory
{
    public class ConveyerBelt : Group
    {
        public float Speed = 10.0f;
        public float XPos = 0;
        public float YPos = 60.0f * 9.5f;

        private List<Sprite> parts = new List<Sprite>();
        private Texture2D[] ConveyorTextures;

        public override void LoadContent(Game1 game)
        {
            ConveyorTextures = new Texture2D[]
            {
                game.Content.Load<Texture2D>("FliessbandAnimiert_1"),
                game.Content.Load<Texture2D>("FliessbandAnimiert_2"),
                game.Content.Load<Texture2D>("FliessbandAnimiert_3")
            };
            
            for (int i = 0; i < 10; ++i)
            {
                Sprite conveyer = new Sprite(ConveyorTextures);
                conveyer.Position = new Vector2(i * 180.0f + 90.0f, YPos);
                // conveyer.AnimationMode = AnimationMode.Loop;
                parts.Add(conveyer);
                Add(conveyer);
            }

            base.LoadContent(game);
        }

        public void Update(GameTime gameTime)
        {
            Speed = ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds)+1.0f)*100.0f;

            XPos += (float)(Speed * gameTime.ElapsedGameTime.TotalSeconds);

            var frameAmount = 180.0 / ConveyorTextures.Length;
            var numTile = XPos % 180;
            var frame = (int)((numTile / frameAmount));

            foreach (var part in parts)
            {
                part.AnimationFrame = frame;
            }

            base.Update(gameTime);
        }

    }
}