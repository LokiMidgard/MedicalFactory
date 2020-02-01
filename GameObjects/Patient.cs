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

        public string PatientName;
        private bool Scored = false;
        public readonly PatientState State;

        public Patient(Texture2D texture, params BodyPart.BodyPartType[] bodyParts) : base(texture)
        {
            this.State = new PatientState(this);
            foreach (var part in bodyParts)
            {
                var item = new BodyPart(part);
                this.Attach(item);
                item.Scale = new Vector2(0.5f, 0.5f);
                Game1.sprites.Add(item);
            }
        }

        public abstract int MaximumBodyParts(BodyPart.BodyPartType type);

        public override void Attach(IAttachable toAdd)
        {
            if (toAdd is BodyPart bodyPart)
            {
                var alradyAttached = this.Attached.OfType<BodyPart>().Count(x => x.Type == bodyPart.Type);
                var maximum = this.MaximumBodyParts(bodyPart.Type);

                if (alradyAttached >= maximum)
                {
                    if (toAdd.AttachedTo != null)
                        toAdd.AttachedTo.Detach(toAdd);
                    return;
                }
            }
            base.Attach(toAdd);
        }


        public override void Update(GameTime gameTime)
        {
            this.Velocity = new Vector2(Game1.conveyerBelt.Speed, 0);
            if (Position.X > 1800.0f && !Scored)
            {
                Game1.game.Screen.scores.Add(new Score(this));
                Scored = true;
            }

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(Game1.game.Font, PatientName, Position + new Vector2(-2.0f, -151.0f), Color.White, 0.0f, new Vector2(), 1.1f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(Game1.game.Font, PatientName, Position + new Vector2(0.0f, -150.0f), Color.Black, 0.0f, new Vector2(), 1.0f, SpriteEffects.None, 0.0f);
            base.Draw(spriteBatch, gameTime);
        }
    }
}
