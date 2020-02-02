using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalFactory.GameObjects
{
    class UpperScanner : Sprite
    {
        public UpperScanner() : base("Scanner_Oberseite") { }

        public override Color Color
        {
            get {
                var last = Game1.game.Screen.scores.LastOrDefault();
                if(last != null && last.patient.Position.X < Position.X + 400)
                {
                    if (last.patient.State.IsDead)
                        return Color.Red;
                    if (last.patient.State.IsCompletelyHealed)
                        return Color.Green;
                    return Color.Orange;
                }
                return Color.White;
            }
            set => base.Color = value;
        }
    }

    public class Scanner : IGameObject, IUpdateable
    {
        public Sprite Lower;
        public Sprite Upper;
        public Sprite RightWall;

        public Vector2 Position { set { Lower.Position = value; Upper.Position = value; } }

        public Scanner()
        {
            this.Lower = new Sprite("Scanner_Unterseite");
            this.Upper = new UpperScanner();
            this.RightWall = new Sprite("Hintergrund_Right") { Position = new Vector2(1920 - 30, 540) };
        }

        public void Update(GameTime gameTime)
        {
            if (Game1.game.Screen.scores.Count == Game1.game.patientFactory.PatientCount)
                if(Game1.game.Screen.scores.Last().patient.Position.X > 2200)
                    Game1.game.ShowFinishScreen();

        }
    }
}
