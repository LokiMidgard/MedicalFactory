using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class TextBox : Sprite
    {
        public int Width = 100;
        public int Height = 100;
        public float FontScale = 1;
        public Color Color = Color.Wheat;
        public float YScroll = 0;

        public string Text;
        private SpriteFont font;

        public TextBox() : base("square")
        {
        }

        public override void LoadContent(Game1 game)
        {
            base.LoadContent(game);
            font = game.Content.Load<SpriteFont>("PressStart2P");
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                var rect = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
                spriteBatch.Draw(this.textures[0], rect, Color);

                var lineHeight = 40 * FontScale;
                var yOffset = YScroll;
                string[] splitString = this.Text.Split("\n", StringSplitOptions.None);
                foreach (var line in splitString)
                {
                    spriteBatch.DrawString(font, line, Position + Vector2.One * 5 + Vector2.UnitY * yOffset, Color.Black, 0, Vector2.Zero, FontScale, SpriteEffects.None, 0);
                    yOffset += lineHeight;
                }
            }
        }
    }
}
