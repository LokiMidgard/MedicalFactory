using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using MedicalFactory.GameObjects;

namespace MedicalFactory
{
    public class PatientFactory
    {
        double Timer = 2.0;
        Texture2D HumanTexture;

        public void LoadContent(Game1 game)
        {
            HumanTexture = game.Content.Load<Texture2D>("Mensch");
        }
        public void Update(GameTime gameTime)
        {
            if(Timer <= 0.0) {
                Patient patient = new Patient(HumanTexture);
                patient.Position = new Vector2(-120.0f, 540.0f + 30.0f);
                patient.Rotation = MathHelper.PiOver2;
                Game1.game.conveyerBelt.Add(patient);
                Timer = MyMathHelper.Random.NextDouble()*10.0f + 3.0f;
            }
            Timer -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}