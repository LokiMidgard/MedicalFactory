using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class BodyPart : Sprite
    {
        public Player CarriedBy { get; set; }

        public BodyPart(string TextureName) : base(TextureName)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (CarriedBy != null && CarriedBy.ControlledSprite != null)
            {
                var dist = 32;
                var upVector = -Vector2.UnitY * dist;
                var offset = MyMathHelper.RotateBy(upVector, CarriedBy.ControlledSprite.Rotation);
             
                Position = CarriedBy.ControlledSprite.Position + offset;
                Rotation = CarriedBy.ControlledSprite.Rotation;
            }

            base.Update(gameTime);
        }
    }
}
