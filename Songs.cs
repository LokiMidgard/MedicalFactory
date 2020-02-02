using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory
{
    public static class Songs
    {
        private static Song TitleSong;
        private static Song GamePlaySong;

        private static Song CurrentPlaying;

        public static void LoadContent(Game1 game)
        {
            TitleSong = game.Content.Load<Song>("Songs/TitleSong");
            GamePlaySong = game.Content.Load<Song>("Songs/GamePlay");
            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
        }

        private static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
        {
            if (CurrentPlaying != null)
                Play(CurrentPlaying);
        }

        private static void Play(Song song, float? volume = null)
        {
            CurrentPlaying = song;
            if (volume.HasValue)
                MediaPlayer.Volume = volume.Value;
            MediaPlayer.Play(song);
        }

        public static void PlayTitleSong() => Play(TitleSong, 1f);
        public static void PlayGameSong() => Play(GamePlaySong, 0.7f);

    }
}
