using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class HumanPatient : Patient
    {

        public HumanPatient(Texture2D texture) : base(texture, BodyPart.BodyPartType.HERZ, BodyPart.BodyPartType.LUNGE, BodyPart.BodyPartType.NIERE)
        {
        }

        public override void Update(GameTime gameTime)
        {
            this.Velocity = new Vector2(Game1.game.conveyerBelt.Speed, 0);

            base.Update(gameTime);
            //this.Position = this.Position + new Vector2(Game1.game.conveyerBelt.Speed, 0.0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override int MaximumBodyParts(BodyPart.BodyPartType type)
        {
            return type switch
            {
                BodyPart.BodyPartType.HERZ => 1,
                BodyPart.BodyPartType.NIERE => 1,
                BodyPart.BodyPartType.LUNGE => 1,
                _ => throw new NotSupportedException()
            };
        }

    }
}
