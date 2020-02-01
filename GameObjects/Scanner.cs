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
        public Sprite RightWall;

        public Vector2 Position { set { Lower.Position = value; Upper.Position = value; } }

        public Scanner()
        {
            this.Lower = new Sprite("Scanner_Unterseite");
            this.Upper = new Sprite("Scanner_Oberseite");
            this.RightWall = new Sprite("Hintergrund_Right") { Position = new Vector2(1920 - 30, 540) };
        }
    }
}
