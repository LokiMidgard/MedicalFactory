using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using static MedicalFactory.GameObjects.BodyPart;

namespace MedicalFactory.GameObjects
{
    public enum DispenserType
    {
        Herzgerät, Lungengerät, Nierengerät
    }

    public class BodyPartDispenser : Sprite
    {
        public static BodyPartType Map(DispenserType type)
        {
            switch(type)
            {
                case DispenserType.Herzgerät: return BodyPartType.HERZ;
                case DispenserType.Lungengerät: return BodyPartType.LUNGE;
                case DispenserType.Nierengerät: return BodyPartType.NIERE;
                default: throw new Exception("Unknown DispenserType");
            }
        }

        private DispenserType type;
        private int initialStock;

        public BodyPartDispenser(DispenserType type, int stock) : base(type.ToString())
        {
            this.type = type;
            this.initialStock = stock;

            for (var i = 0; i < initialStock; ++i)
            {
                var part = new BodyPart(Map(type));
                Attach(part);
                Game1.sprites.Add(part);
            }
        }

        public override void Attach(IAttachable toAdd)
        {
            base.Attach(toAdd);
            if (toAdd is Sprite s)
            {
                s.AttachOffset = Vector2.Zero;
                s.Visible = false;
            }
        }

        public override void Detach(IAttachable toRemove)
        {
            if (toRemove is Sprite s)
            {
                s.AttachOffset = BodyPart.DefaultAttachOffset;
                s.Visible = true;
            }
            base.Detach(toRemove);
        }
    }
}
