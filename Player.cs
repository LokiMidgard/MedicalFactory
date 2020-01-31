using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;

namespace MedicalFactory
{
    public class Player : GameObject
    {

        public Sprite ControlledSprite;
        public InputProvider inputProvider;
        public Player(InputProvider inputProvider)
        {
            this.inputProvider = inputProvider;
        }

        public Player(InputProvider inputProvider, Sprite controlledSprite)
        {
            this.inputProvider = inputProvider;
            this.ControlledSprite = controlledSprite;
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {
            inputProvider.Update(gameTime);

            if (ControlledSprite != null)
            {
                ControlledSprite.Position.X += inputProvider.Get(Sliders.LeftStickX);
                ControlledSprite.Position.Y += inputProvider.Get(Sliders.LeftStickY);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }
    }
}