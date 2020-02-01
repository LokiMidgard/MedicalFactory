using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using MedicalFactory.GameObjects;

namespace MedicalFactory
{
    /// <summary>
    /// This class links a controller to a sprite
    /// </summary>
    public class Player : IGameObject, IUpdateable
    {
        private float Speed = 1000.0f;
        private float PickupRange = 10.0f;
        private float PickupOffset = 10.0f;

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
                if (InputProvider.WasPressed(inputProvider, PaToRo_Desktop.Engine.Input.Buttons.X))
                {
                    if (ControlledSprite.Attached.Count == 0)
                    {
                        Vector2 PickupPoint = ControlledSprite.Position + (Direction * PickupOffset);
                        var collisions = CollisionManager.GetCollisions(PickupPoint, PickupRange, Game1.sprites);
                        foreach (var collision in collisions)
                        {
                            BodyPart b = collision.spriteB as BodyPart;
                            if (b != null && b.AttachedTo == null)
                            {
                                ControlledSprite.Attach(b);
                            }
                        }
                    } else {
                        ControlledSprite.Detach(ControlledSprite.Attached[0]);
                    }
                }
            }
        }
    }
}