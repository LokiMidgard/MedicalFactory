using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class HumanPatient : Patient
    {

        public HumanPatient(Texture2D texture) : base(texture)
        {
        }

        protected override void InitOrgans()
        {
            this.Attach(new BodyPart(BodyPart.BodyPartType.HERZ));
            this.Attach(new BodyPart(BodyPart.BodyPartType.LUNGE));
            this.Attach(new BodyPart(BodyPart.BodyPartType.NIERE));

        }

        public override void Update(GameTime gameTime)
        {
            this.Velocity = new Vector2(Game1.game.conveyerBelt.Speed, 0);

            base.Update(gameTime);
            //this.Position = this.Position + new Vector2(Game1.game.conveyerBelt.Speed, 0.0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
