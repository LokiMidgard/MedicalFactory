using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using MedicalFactory.GameObjects;
using System;
using System.Diagnostics;

namespace MedicalFactory
{
    public class ConveyerBelt : Group
    {
        public float Speed = 3.0f;
        public float YPos = 60.0f * 9.5f;
        Texture2D HumanTexture;

        public override void LoadContent(Game1 game)
        {
            HumanTexture = game.Content.Load<Texture2D>("Mensch");
            Texture2D ConveyerTexture1 = game.Content.Load<Texture2D>("FliessbandAnimiert_1");
            Texture2D ConveyerTexture2 = game.Content.Load<Texture2D>("FliessbandAnimiert_2");
            Texture2D ConveyerTexture3 = game.Content.Load<Texture2D>("FliessbandAnimiert_3");
            
            for (int i = 0; i < 10; ++i)
            {
                Sprite conveyer = new Sprite(ConveyerTexture1, ConveyerTexture2, ConveyerTexture3);
                conveyer.Position = new Vector2(i * 180.0f + 90.0f, YPos);
                conveyer.AnimationMode = AnimationMode.Loop;
                Add(conveyer);
            }

            base.LoadContent(game);
        }

        public void Update(GameTime gameTime)
        {
            Speed = ((float)Math.Sin(gameTime.TotalGameTime.TotalSeconds)+1.0f)*30.0f;

            //Debugger.Log(0, "", Speed + "\n");
            base.Update(gameTime);
        }

    }
}