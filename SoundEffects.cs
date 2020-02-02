using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;

namespace MedicalFactory
{
    public class SoundEffects
    {
        public static SoundEffect FailedPatient;
        public static void LoadContent(Game1 game)
        {
            FailedPatient = game.Content.Load<SoundEffect>("SoundEffects/failed_patient");
        }
    }
}