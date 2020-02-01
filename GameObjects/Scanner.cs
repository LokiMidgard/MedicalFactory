using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class Scanner : IGameObject, IUpdateable, IDrawable
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

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            var pationt = ConveyerBelt.GetPatientUnderScanner();
            if (pationt != null)
                spriteBatch.DrawString(Game1.game.Font, pationt.patient.State.CalculateScore().actual.ToString(), this.Upper.Position - new Vector2(15, 0), Color.Black, 0.0f, new Vector2(), 1.0f, SpriteEffects.None, 0.0f);
        }

    }
}
