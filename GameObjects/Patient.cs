using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class Patient : Sprite
    {

        public float ConveyerSpeed = 10.0f;
        public Patient(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            this.Position = this.Position + new Vector2(ConveyerSpeed, 0.0f);
        }
    }
}
