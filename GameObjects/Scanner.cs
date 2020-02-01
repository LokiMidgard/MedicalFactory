using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class Scanner
    {
        public Sprite Lower;
        public Sprite Upper;

        public Vector2 Position { set { Lower.Position = value; Upper.Position = value; } }

        public Scanner()
        {
            this.Lower = new Sprite("Scanner_Unterseite");
            this.Upper = new Sprite("Scanner_Oberseite");
        }
    }
}
