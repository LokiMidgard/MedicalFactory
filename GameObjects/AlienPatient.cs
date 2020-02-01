using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class AlienPatient : Patient
    {

        public AlienPatient(Texture2D texture) : base(texture, BodyPart.BodyPartType.HERZ, BodyPart.BodyPartType.HERZ, BodyPart.BodyPartType.LUNGE)
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
                BodyPart.BodyPartType.HERZ => 2,
                BodyPart.BodyPartType.NIERE => 0,
                BodyPart.BodyPartType.LUNGE => 1,
                _ => throw new NotSupportedException()
            };
        }

    }
}
