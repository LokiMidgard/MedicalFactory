using MedicalFactory.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalFactory
{

    public class FinishScreen : Group
    {
        private Sprite background;
        private TextBox textbox;
        private float YScroll = 1000;

        public override bool Visible
        {
            get => base.Visible;
            set
            {
                if (!base.Visible && value)
                {
                    Songs.PlayTitleSong();
                    YScroll = 1080;
                }else if(base.Visible && !value)
                {
                    Songs.PlayGameSong();
                }
                base.Visible = value;
            }
        }

        public FinishScreen()
        {
            Visible = false;
            background = new Sprite("FinishedOverlay") { Position = new Vector2(960, 540) };
            Add(background);

            textbox = new TextBox();
            textbox.Position = new Vector2(128, 0);
            textbox.Width = 1920 - 256;
            textbox.Height = 1080;
            textbox.Color = new Color(Color.Wheat, 0.4f);
            Add(textbox);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Visible)
            {
                YScroll -= (float)gameTime.ElapsedGameTime.TotalSeconds * 200f;
                textbox.YScroll = YScroll;

                textbox.Text = Text;
            }
        }

        public string Text =>
            "Good job, fellow robots...\n"
          + "\n\n"
          + "The factory is proud to present today's REPAIRS:\n"
          + "\n"
          + "\n"
          + string.Join("\n\n", Game1.game.Screen.scores.Select(ScoreText))
          + "\n\n"
          + $"You left {Game1.CountOrgansOnFloor} organs lying on the floor.\n\n"
          ;

        private static string ScoreText(Score score)
        {
            return string.Join("\n", score.Text);
        }
    }
}
