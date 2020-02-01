using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            switch (type)
            {
                case DispenserType.Herzgerät: return BodyPartType.HERZ;
                case DispenserType.Lungengerät: return BodyPartType.LUNGE;
                case DispenserType.Nierengerät: return BodyPartType.NIERE;
                default: throw new Exception("Unknown DispenserType");
            }
        }
 
        private DispenserType type;
        private int initialStock;
        private Texture2D bodyPartTex;
        private int count;

        public BodyPartDispenser(DispenserType type, int stock) : base(type.ToString(), "Leeresgerät")
        {
            this.type = type;
            this.initialStock = stock;
            this.count = 0;
        }

        public override void LoadContent(Game1 game)
        {
            base.LoadContent(game);

            this.bodyPartTex = game.Content.Load<Texture2D>(Map(type).ToString());
            
            for (int i = 0; i < initialStock; ++i)
                CreateNew();
        }

        public void CreateNew()
        {
            var part = new BodyPart(Map(type), this.bodyPartTex);
            Attach(part);
            Game1.sprites.Add(part);
            this.count++;
            UpdateAnimFrame();
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
            this.count--;
            UpdateAnimFrame();
        }

        private void UpdateAnimFrame()
        {
            AnimationFrame = this.count == 0 ? 1 : 0;
        }
    }
}
