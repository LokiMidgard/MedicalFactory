using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{

    public class BodyPart : Sprite
    {
        public enum BodyPartType
        {
            HERZ,
            NIERE,
            LUNGE
        }
        public BodyPart(BodyPartType type) : base(type.ToString())
        {
            AttachOffset = Vector2.UnitY * -48;
        }
    }
}
