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
        private float Speed = 600.0f;
        private float PickupRange = 50.0f;
        private float PickupOffset = 120.0f;

        public bool Active => this.ControlledSprite != null;

        public Robot ControlledSprite
        {
            get => this.controlledSprite; private set
            {
                if (value is null)
                {
                    if (this.controlledSprite != null)
                        this.controlledSprite.Player = null;
                }
                else
                {
                    if (value.Player != null)
                        return;
                }
                this.controlledSprite = value;
                if (this.controlledSprite != null)
                    this.controlledSprite.Player = this;
            }
        }

        public bool SignalStart { get; private set; }

        public InputProvider inputProvider;
        private Robot controlledSprite;

        public Player(InputProvider inputProvider)
        {
            this.inputProvider = inputProvider;
        }

        public void Rumble()
        {
            this.inputProvider?.Rumble(1f, 1f, 250);
        }

        public void Update(GameTime gameTime)
        {
            if (!this.Active
                && (InputProvider.WasPressed(this.inputProvider, PaToRo_Desktop.Engine.Input.Buttons.X)
                    || InputProvider.WasPressed(this.inputProvider, PaToRo_Desktop.Engine.Input.Buttons.A)
                    || InputProvider.WasPressed(this.inputProvider, PaToRo_Desktop.Engine.Input.Buttons.B)
                    || InputProvider.WasPressed(this.inputProvider, PaToRo_Desktop.Engine.Input.Buttons.Y)

                    || InputProvider.WasPressed(this.inputProvider, PaToRo_Desktop.Engine.Input.Buttons.Start)

                    || this.inputProvider.Get(Sliders.LeftStickX) != 0
                    || this.inputProvider.Get(Sliders.LeftStickY) != 0
                ))
            {
                var availablePlayer = Game1.sprites.OfType<Robot>().FirstOrDefault(x => x.Player is null);
                this.ControlledSprite = availablePlayer;
            }

            this.SignalStart = InputProvider.WasPressed(this.inputProvider, PaToRo_Desktop.Engine.Input.Buttons.Start);

            if (this.ControlledSprite != null)
            {
                this.ControlledSprite.Visible = this.Active;
                // Handle Directions
                Vector2 Direction = new Vector2(this.inputProvider.Get(Sliders.LeftStickX), this.inputProvider.Get(Sliders.LeftStickY));
                this.ControlledSprite.Velocity = Direction * this.Speed;
                if (Direction.X + Direction.Y != 0.0f)
                {
                    this.ControlledSprite.Rotation = MyMathHelper.RightAngleInRadians(new Vector2(0.0f, -1.0f), Vector2.Normalize(Direction));
                }

                // Handle Buttons
                bool wasButtonPressed = InputProvider.WasPressed(this.inputProvider, PaToRo_Desktop.Engine.Input.Buttons.A)
                    || InputProvider.WasPressed(this.inputProvider, PaToRo_Desktop.Engine.Input.Buttons.B)
                    || InputProvider.WasPressed(this.inputProvider, PaToRo_Desktop.Engine.Input.Buttons.X)
                    || InputProvider.WasPressed(this.inputProvider, PaToRo_Desktop.Engine.Input.Buttons.Y);

                if (wasButtonPressed)
                {
                    var holdedItem = this.ControlledSprite.Attached.OfType<IItem>().FirstOrDefault();

                    if (holdedItem is null)  // if nothing is attached
                    {
                        Vector2 PickupPoint = this.ControlledSprite.Position + (Direction * this.PickupOffset);
                        var collisions = CollisionManager.GetCollisions(PickupPoint, this.PickupRange, Game1.sprites);
                        // We order all Body parts we colide so demaged will be first.
                        var toTake = collisions.Select(x => x.spriteB).OfType<IItem>().OrderBy(x =>
                        {
                            if (x is BodyPart bodyPart)
                                return bodyPart.IsDamaged ? 0 : 1;
                            return 2;
                        }).FirstOrDefault();
                        if (toTake != null)
                            this.ControlledSprite.Attach(toTake);

                    }
                    else
                    {
                        Vector2 PickupPoint = this.ControlledSprite.Position + (Direction * this.PickupOffset);
                        var collisions = CollisionManager.GetCollisions(PickupPoint, this.PickupRange, Game1.conveyerBelt);
                        bool iPutItSomewhere = false;
                        if (holdedItem is BodyPart bodyPart)
                            foreach (var collision in collisions)
                            {
                                Patient patient = collision.spriteB as Patient;
                                if (patient != null)
                                {
                                    patient.Attach(bodyPart);
                                    iPutItSomewhere = true;
                                    break;
                                }
                                Recycler recycler = collision.spriteB as Recycler;
                                if (recycler != null)
                                {
                                    recycler.PutStuffInside(bodyPart);
                                    iPutItSomewhere = true;
                                    break;
                                }
                            }

                        if (!iPutItSomewhere)
                        {
                            if (holdedItem is Sprite sprite)
                            {
                                var direction = MyMathHelper.RotateBy(-Vector2.UnitY, this.ControlledSprite.Rotation);
                                sprite.Velocity = direction * 800f + this.ControlledSprite.Velocity;
                            }

                            this.ControlledSprite.Detach(holdedItem);
                        }
                    }
                }
            }
        }
    }
}