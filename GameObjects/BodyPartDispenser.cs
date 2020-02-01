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
        private Texture2D shadow;

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.shadow, this.Position + new Vector2(0, 15), null, Color.White, 0, new Vector2(shadow.Width / 2.0f, shadow.Height / 2.0f), new Vector2(0.75f, 0.75f), SpriteEffects.None, 0.0f);
            base.Draw(spriteBatch, gameTime);
        }

        public BodyPartDispenser(DispenserType type, int stock) : base(type.ToString(), "Leeresgerät")
        {
            this.type = type;
            this.initialStock = stock;
            this.count = 0;
        }

        public override void LoadContent(Game1 game)
        {
            this.shadow = game.Content.Load<Texture2D>("Schatten_Oval");
            this.bodyPartTex = game.Content.Load<Texture2D>(Map(type).ToString());

            base.LoadContent(game);
            
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
