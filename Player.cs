using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using MedicalFactory.GameObjects;
using System.Linq;

namespace MedicalFactory
{
    /// <summary>
    /// This class links a controller to a sprite
    /// </summary>
    public class Player : IGameObject, IUpdateable
    {
        private float Speed = 1000.0f;
        private float PickupRange = 50.0f;
        private float PickupOffset = 100.0f;

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
                // Handle Directions
                Vector2 Direction = new Vector2(inputProvider.Get(Sliders.LeftStickX), inputProvider.Get(Sliders.LeftStickY));
                ControlledSprite.Velocity = Direction * Speed;
                if (Direction.X + Direction.Y != 0.0f)
                {
                    ControlledSprite.Rotation = MyMathHelper.RightAngleInRadians(new Vector2(0.0f, -1.0f), Vector2.Normalize(Direction));
                }

                // Handle Buttons
                if (InputProvider.WasPressed(inputProvider, PaToRo_Desktop.Engine.Input.Buttons.X))
                {
                    var holdedItem = ControlledSprite.Attached.OfType<BodyPart>().FirstOrDefault();

                    if (holdedItem is null)  // if nothing is attached
                    {
                        Vector2 PickupPoint = ControlledSprite.Position + (Direction * PickupOffset);
                        var collisions = CollisionManager.GetCollisions(PickupPoint, PickupRange, Game1.sprites);
                        // We order all Body parts we colide so demaged will be first.
                        var toTake = collisions.Select(x => x.spriteB).OfType<BodyPart>().OrderBy(x => x.IsDemaged ? 0 : 1).FirstOrDefault();
                        if (toTake != null)
                            ControlledSprite.Attach(toTake);

                    }
                    else
                    {
                        Vector2 PickupPoint = ControlledSprite.Position + (Direction * PickupOffset);
                        var collisions = CollisionManager.GetCollisions(PickupPoint, PickupRange, Game1.conveyerBelt);
                        bool iPutItSomewhere = false;
                        foreach (var collision in collisions)
                        {
                            Patient patient = collision.spriteB as Patient;
                            if (patient != null)
                            {
                                patient.Attach(holdedItem);
                                iPutItSomewhere = true;
                                break;
                            }
                            Recycler recycler = collision.spriteB as Recycler;
                            if (recycler != null)
                            {
                                recycler.PutStuffInside(holdedItem);
                                iPutItSomewhere = true;
                                break;
                            }
                        }
                        if (!iPutItSomewhere)
                        {
                            var bp = holdedItem;
                            if (bp != null)
                            {
                                bp.Velocity = ControlledSprite.Velocity;
                            }

                            ControlledSprite.Detach(holdedItem);
                        }
                    }
                }
            }
        }
    }
}