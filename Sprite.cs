using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using System.Linq;
using System;

namespace MedicalFactory
{

    public enum AnimationMode
    {
        None,
        Loop,
        PingPong
    }

    public class Sprite : GameObject
    {

        public Vector2 Position;
        private readonly string[] textureNames;
        private Texture2D[] textures;
        private int animationFrame;

        public int AnimationFrameTimeInMS { get; set; } = 1000;

        public AnimationMode AnimationMode { get; set; }
        public int AnimationFrame
        {
            get => animationFrame; set
            {
                if (value >= this.textures.Length)
                {
                    var diff = value - this.textures.Length;
                    animationFrame = this.textures.Length - diff - 2;
                }
                else
                {
                    animationFrame = value;
                }
            }
        }

        [Obsolete]
        public Sprite() : this("stick")
        {

        }

        public Sprite(params string[] textureNames)
        {
            this.textureNames = textureNames;
        }

        public void LoadContent(ContentManager Content)
        {
            textures = this.textureNames.Select(x => Content.Load<Texture2D>(x)).ToArray();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.AnimationFrame = this.AnimationMode switch
            {
                AnimationMode.None => this.AnimationFrame,
                AnimationMode.Loop => ((int)Math.Floor(gameTime.TotalGameTime.TotalMilliseconds / AnimationFrameTimeInMS)) % textures.Length,
                AnimationMode.PingPong => ((int)Math.Floor(gameTime.TotalGameTime.TotalMilliseconds / AnimationFrameTimeInMS)) % (textures.Length + textures.Length - 2),
                _ => throw new NotImplementedException($"AnimationMode {this.AnimationMode}")
            };


            spriteBatch.Draw(textures[this.AnimationFrame], Position, null, Color.White);
        }
    }
}