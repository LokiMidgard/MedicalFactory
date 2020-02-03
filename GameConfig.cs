using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory
{
    public static class GameConfig
    {
        public static bool DrawCollisionGeometry = false;
        public static bool KeepPlayersToTheirSide = true;
        public const int NumPlayers = 4;
        private static bool soundEnabled = true;

        public static bool SoundEnabled
        {
            get => soundEnabled; internal set
            {
                if (soundEnabled != value)
                {

                    soundEnabled = value;
                    MediaPlayer.IsMuted = !soundEnabled;
                }
            }
        }
    }
}
