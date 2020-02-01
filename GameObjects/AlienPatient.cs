using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class AlienPatient : Patient
    {

        public AlienPatient(Texture2D texture) : base(texture)
        {

            this.Attach(new BodyPart(BodyPart.BodyPartType.HERZ));
            this.Attach(new BodyPart(BodyPart.BodyPartType.HERZ));
            this.Attach(new BodyPart(BodyPart.BodyPartType.LUNGE));
            foreach (var item in this.Attached)
                Game1.sprites.Add(item as BodyPart);
        }

        public override void Update(GameTime gameTime)
        {
            this.Velocity = new Vector2(Game1.game.conveyerBelt.Speed, 0);

            base.Update(gameTime);
            //this.Position = this.Position + new Vector2(Game1.game.conveyerBelt.Speed, 0.0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
