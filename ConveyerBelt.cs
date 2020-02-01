using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using MedicalFactory.GameObjects;

namespace MedicalFactory
{
    public class ConveyerBelt
    {
        public float Speed = 10.0f;
        Texture2D HumanTexture;

        public void LoadContent(Game1 game)
        {
            HumanTexture = game.Content.Load<Texture2D>("Mensch");
            Texture2D ConveyerTexture = game.Content.Load<Texture2D>("Fließband");
            for (int i = 0; i < 10; ++i)
            {
                Sprite conveyer = new Sprite(ConveyerTexture);
                conveyer.Position = new Vector2(i * 180.0f + 90.0f, 60.0f * 9.5f);
                Game1.sprites.Add(conveyer);
            }
        }
        public void Update(GameTime gameTime)
        {
        }

    }
}