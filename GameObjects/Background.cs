using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class Background : Sprite
    {
        public Background(Screen screen) : base("Hintergrund")
        {
            this.Position = new Microsoft.Xna.Framework.Vector2(screen.Width / 2.0f, screen.Height / 2.0f);
        }
    }
}
