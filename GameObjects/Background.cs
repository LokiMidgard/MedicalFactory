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
            Position.X = screen.Width / 2.0f;
            Position.Y = screen.Height / 2.0f;
        }
    }
}
