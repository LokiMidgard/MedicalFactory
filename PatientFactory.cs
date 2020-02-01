using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using MedicalFactory.GameObjects;
using System;
using System.Linq;

namespace MedicalFactory
{
    public class PatientFactory
    {
        double Timer = 2.0;
        Texture2D HumanTexture;
        Texture2D AlienTexture;

        private Random random = new Random();

        public void LoadContent(Game1 game)
        {
            this.HumanTexture = game.Content.Load<Texture2D>("Mensch");
            this.AlienTexture = game.Content.Load<Texture2D>("Alien");
            BodyPart.LoadContent(game.Content);
        }
        public void Update(GameTime gameTime)
        {
            if (this.Timer <= 0.0)
            {
                bool IsHuman = MyMathHelper.Random.NextDouble() < 0.6f;
                Patient patient;
                if (IsHuman)
                {
                    patient = new HumanPatient(this.HumanTexture);
                }
                else
                {
                    patient = new AlienPatient(this.AlienTexture);
                }


                var defectOrgans = this.random.Next(1, patient.Attached.Count + 1);

                foreach (var item in patient.Attached.OrderBy(x => this.random.NextDouble()).Take(defectOrgans).OfType<BodyPart>())
                    item.IsDemaged = true;


                patient.Position = new Vector2(-120.0f, 540.0f + 30.0f);
                patient.Rotation = MathHelper.PiOver2;
                Game1.conveyerBelt.Add(patient);
                this.Timer = MyMathHelper.Random.NextDouble() * 10.0f + 3.0f;
            }
            this.Timer -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}