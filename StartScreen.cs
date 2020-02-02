using MedicalFactory.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace MedicalFactory
{
    public class StartScreen : Group
    {
        private readonly Player yellowPlayer;
        private readonly Player greenPlayer;
        private readonly Sprite background;
        private readonly Sprite green;
        private readonly Sprite yellow;
        private readonly Sprite blue;
        private readonly Sprite red;
        private readonly Player redPlayer;
        private readonly Player bluePlayer;

        public StartScreen(System.Collections.Generic.IEnumerable<Player> enumerable)
        {
            this.redPlayer = enumerable.FirstOrDefault(x => x.ControlledSprite.PlayerColor == PlayerColor.RoterRoboter);
            this.bluePlayer = enumerable.FirstOrDefault(x => x.ControlledSprite.PlayerColor == PlayerColor.BlauerRoboter);
            this.yellowPlayer = enumerable.FirstOrDefault(x => x.ControlledSprite.PlayerColor == PlayerColor.GelberRoboter);
            this.greenPlayer = enumerable.FirstOrDefault(x => x.ControlledSprite.PlayerColor == PlayerColor.GruenerRoboter);

            this.background = new Sprite("StartScreen/HintergrundMitSchatten")
            {
                Origin = new Vector2(1258 + 600 / 2, 285 + 754 / 2),
            };
            this.green = new Sprite("StartScreen/GruenerRoboterWolke")
            {
                Origin = new Vector2(429, 932),
            };
            this.yellow = new Sprite("StartScreen/GelberRoboterWolke")
            {
                Origin = new Vector2(1174, 719),
            };
            this.red = new Sprite("StartScreen/RoterRoboterWolke")
            {
                Origin = new Vector2(867, 841),
            };
            this.blue = new Sprite("StartScreen/BlauerRoboterWolke")
            {
                Origin = new Vector2(1470, 952),
            };

            this.Add(this.background);
            this.Add(this.yellow);
            this.Add(this.red);
            this.Add(this.green);
            this.Add(this.blue);

            foreach (var item in this.OfType<Sprite>())
            {
                item.Position = item.Origin;
            }
        }

        public override bool Visible
        {
            get => base.Visible; set
            {
                if (base.Visible != value)
                {
                    base.Visible = value;
                    if (this.Visible)
                        Songs.PlayTitleSong();
                    else
                        Songs.PlayGameSong();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            var interpolation = (float)Math.Sin(gameTime.TotalGameTime / TimeSpan.FromSeconds(1.5));
            var activeinterpolation = (float)Math.Sin(gameTime.TotalGameTime / TimeSpan.FromSeconds(0.2));

            const float normal = MathHelper.PiOver4 / 24;
            const float active = MathHelper.PiOver4 / 24;
            if (this.bluePlayer.Active)
                this.blue.Rotation = MathHelper.Lerp(-active, active, activeinterpolation);
            else
                this.blue.Rotation = MathHelper.Lerp(-normal, normal, interpolation);

            if (this.redPlayer.Active)
                this.red.Rotation = MathHelper.Lerp(-active, active, activeinterpolation);
            else
                this.red.Rotation = MathHelper.Lerp(-normal, normal, interpolation);

            if (this.yellowPlayer.Active)
                this.yellow.Rotation = MathHelper.Lerp(-active, active, -activeinterpolation);
            else
                this.yellow.Rotation = MathHelper.Lerp(-normal, normal, -interpolation);

            if (this.greenPlayer.Active)
                this.green.Rotation = MathHelper.Lerp(-active, active, -activeinterpolation);
            else
                this.green.Rotation = MathHelper.Lerp(-normal, normal, -interpolation);


            base.Update(gameTime);
        }
    }
}
