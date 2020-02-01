using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;

namespace MedicalFactory
{
    /// <summary>
    /// This class links a controller to a sprite
    /// </summary>
    public class Player : IGameObject, IUpdateable
    {
        private float Speed = 1000.0f;

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

        public void Update(GameTime gameTime)
        {
            if (ControlledSprite != null)
            {
                Vector2 Direction = new Vector2(inputProvider.Get(Sliders.LeftStickX), inputProvider.Get(Sliders.LeftStickY));
                ControlledSprite.Velocity = Direction * Speed;
                if (Direction.X + Direction.Y != 0.0f)
                {
                    ControlledSprite.Rotation = MyMathHelper.RightAngleInRadians(new Vector2(0.0f, -1.0f), Vector2.Normalize(Direction));
                }
            }
        }
    }
}