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
        public string SpouseName;
        public int NumberOfChildren;
        public bool Married;
        public int Age;
        public int LifeExpectancy;
        private bool Scored = false;
        public readonly PatientState State;

        public Patient(Texture2D[] texture, params BodyPart.BodyPartType[] bodyParts) : base(texture)
        {
            this.State = new PatientState(this);
            foreach (var part in bodyParts)
            {
                var item = new BodyPart(part);
                item.Silent = true;
                this.Attach(item);
                item.Silent = false;
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
            if (this.State.IsDead)
                this.AnimationFrame = 1;
            else if (this.State.IsCompletelyHealed)
                this.AnimationFrame = 2;
            else
                this.AnimationFrame = 0;

            this.Velocity = new Vector2(Game1.conveyerBelt.Speed, 0);
            if (Position.X > 1700.0f && !Scored)
            {
                Game1.game.Screen.scores.Add(new Score(this));
                Scored = true;
                if (GameConfig.SoundEnabled)
                    if (this.State.IsCompletelyHealed)
                    {
                        SoundEffects.SuccessPatient.Play();
                    }
                    else
                    {
                        SoundEffects.FailedPatient.Play();
                    }
            }

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            /*
            Vector2 Offset = new Vector2(0.0f, -250.0f);
            float LineHeight = 20.0f;
            spriteBatch.DrawString(Game1.game.Font, PatientName, Position + Offset, Color.Black, 0.0f, new Vector2(), 0.5f, SpriteEffects.None, 0.0f);
            if(Married) {
                Offset += new Vector2(0.0f, LineHeight);
                spriteBatch.DrawString(Game1.game.Font, "Married to " + SpouseName, Position + Offset, Color.Black, 0.0f, new Vector2(), 0.5f, SpriteEffects.None, 0.0f);
            }
            Offset += new Vector2(0.0f, LineHeight);
            spriteBatch.DrawString(Game1.game.Font, "Age: " + Age + " years", Position + Offset, Color.Black, 0.0f, new Vector2(), 0.5f, SpriteEffects.None, 0.0f);
            Offset += new Vector2(0.0f, LineHeight);
            spriteBatch.DrawString(Game1.game.Font, "Life expectancy: " + LifeExpectancy + " years", Position + Offset, Color.Black, 0.0f, new Vector2(), 0.5f, SpriteEffects.None, 0.0f);
            Offset += new Vector2(0.0f, LineHeight);
            spriteBatch.DrawString(Game1.game.Font, "Has " + NumberOfChildren + " children", Position + Offset, Color.Black, 0.0f, new Vector2(), 0.5f, SpriteEffects.None, 0.0f);
            */
            base.Draw(spriteBatch, gameTime);
        }
    }
}
