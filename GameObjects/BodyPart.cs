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
        public BodyPartType Type { get; }

        public BodyPart(BodyPartType type) : base(type.ToString())
        {
            AttachOffset = Vector2.UnitY * -48;
            this.Type = type;
        }

        public override ICanCarray AttachedTo
        {
            get => base.AttachedTo; set
            {
                base.AttachedTo = value;
                if (value is Patient)
                {
                    this.AttachOffset = this.Type switch
                    {
                        BodyPartType.HERZ => new Vector2(0, -30),
                        BodyPartType.LUNGE => new Vector2(0, -80),
                        BodyPartType.NIERE => new Vector2(-20, 0),
                        _ => throw new NotImplementedException($"Type {this.Type}")
                    };
                }
            }
        }

    }
}
