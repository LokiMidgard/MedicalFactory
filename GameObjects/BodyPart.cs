using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class BodyPart : Sprite
    {
        public BodyPart(string TextureName) : base(TextureName)
        {
            AttachOffset = Vector2.UnitY * -48;
        }
    }
}
