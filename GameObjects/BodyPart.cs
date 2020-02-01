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

        public static Vector2 DefaultAttachOffset = Vector2.UnitY * -48;

        public BodyPart(BodyPartType type) : base(type.ToString())
        {
            AttachOffset = DefaultAttachOffset;
        }
    }
}
