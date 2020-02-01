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

        public DispenserType type;
        private int initialStock;
        private int count;
        private Texture2D shadow;
        private Texture2D rect;

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.shadow, this.Position + new Vector2(0, 15), null, Color.White, 0, new Vector2(shadow.Width / 2.0f, shadow.Height / 2.0f), new Vector2(0.75f, 0.75f), SpriteEffects.None, 0.0f);
            base.Draw(spriteBatch, gameTime);

            // draw gauge
            var pos = this.Position - new Vector2(20, 20);
            var rect = new Rectangle((int)pos.X, (int)pos.Y, 16, 16 * 4);

            spriteBatch.Draw(this.rect, rect, Color.Black);
            rect.X += 2;
            rect.Y += 2;
            rect.Width -= 4;
            rect.Height -= 4;

            var fill = this.count / (float)this.initialStock;
            var height = (int)(rect.Height * fill);

            rect.Y = rect.Y + (rect.Height - height);
            rect.Height = height;

            spriteBatch.Draw(this.rect, rect, Color.Violet);
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
            this.rect = game.Content.Load<Texture2D>("square");
            BodyPart.LoadContent(game.Content);
            base.LoadContent(game);

            for (int i = 0; i < initialStock; ++i)
                CreateNew();
        }

        public void CreateNew()
        {
            var part = new BodyPart(Map(type));
            Game1.sprites.Add(part);
            Attach(part);
        }

        public override void Attach(IAttachable toAdd)
        {
            if (toAdd == null) return;
            base.Attach(toAdd);
            if (toAdd is Sprite s)
            {
                s.AttachOffset = Vector2.Zero;
                s.Visible = false;
            }
            this.count++;
            if (this.count > this.initialStock)
                initialStock = this.count;
            UpdateAnimFrame();
        }

        public override void Detach(IAttachable toRemove)
        {
            if (toRemove == null) return;
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
