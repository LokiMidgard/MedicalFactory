using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory
{
    public class BodyPart : Sprite
    {
        public Player CarriedBy { get; set; }

        public BodyPart() : base()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (CarriedBy != null && CarriedBy.ControlledSprite != null)
            {
                var dist = 32;
                var rot = CarriedBy.ControlledSprite.Rotation;
                var upVector = -Vector2.UnitY;
                var forwardDirection = Vector2.TransformNormal(upVector, Matrix.CreateRotationX(rot));
                var offset = forwardDirection * dist;
                Position = CarriedBy.ControlledSprite.Position + offset;
                Rotation = CarriedBy.ControlledSprite.Rotation;
            }

            base.Update(gameTime);
        }
    }
}
