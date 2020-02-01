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

        public Patient(Texture2D texture) : base(texture)
        {
            this.InitOrgans();
            foreach (var item in this.Attached.OfType<BodyPart>())
            {
                Game1.sprites.Add(item);
                item.Scale = new Vector2(0.5f, 0.5f);
            }

        }
        protected abstract void InitOrgans();

        public override void Update(GameTime gameTime)
        {
            this.Velocity = new Vector2(Game1.game.conveyerBelt.Speed, 0);

            base.Update(gameTime);
            //this.Position = this.Position + new Vector2(Game1.game.conveyerBelt.Speed, 0.0f) * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
