using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public abstract class Patient : Sprite
    {

        public Patient(Texture2D texture, params BodyPart.BodyPartType[] bodyParts) : base(texture)
        {
            foreach (var part in bodyParts)
            {
                var item = new BodyPart(part);
                this.Attach(item);
                item.Scale = new Vector2(0.5f, 0.5f);
                Game1.sprites.Add(item);
            }
        }

        protected abstract int MaximumBodyParts(BodyPart.BodyPartType type);

        public override void Attach(IAttachable toAdd)
        {
            if (toAdd is BodyPart bodyPart)
            {
                var alradyAttached = this.Attached.OfType<BodyPart>().Count(x => x.Type == bodyPart.Type);
                var maximum = this.MaximumBodyParts(bodyPart.Type);

                if (alradyAttached > maximum)
                    return;
            }
            base.Attach(toAdd);
        }


        public override void Update(GameTime gameTime)
        {
            this.Velocity = new Vector2(Game1.game.conveyerBelt.Speed, 0);

            base.Update(gameTime);
            //this.Position = this.Position + new Vector2(Game1.game.conveyerBelt.Speed, 0.0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
