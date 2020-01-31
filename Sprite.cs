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
        public float Radius;
        public bool CanCollide = false;

        public bool hasCollision;

        private readonly string[] textureNames;
        private Texture2D[] textures;

        public int AnimationFrameTimeInMS { get; set; } = 1000;

        public AnimationMode AnimationMode { get; set; }

        private int animationFrame = -1;    /* hack to trigger initial setter with 0 */
        public int AnimationFrame
        {
            get {
                return animationFrame;
            }
            set {
                if (animationFrame != value)
                {
                    animationFrame = value;

                    Radius = textures[animationFrame].Width / 2.0f;
                }
            }
        }

        private static int PingPong(int value, int length)
        {
            if (value >= length)
            {
                var diff = value - length;
                return length - diff - 2;
            }
            return value;
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
            this.AnimationFrame = this.AnimationMode switch
            {
                AnimationMode.None => this.AnimationFrame,
                AnimationMode.Loop => ((int)Math.Floor(gameTime.TotalGameTime.TotalMilliseconds / AnimationFrameTimeInMS)) % textures.Length,
                AnimationMode.PingPong => PingPong(((int)Math.Floor(gameTime.TotalGameTime.TotalMilliseconds / AnimationFrameTimeInMS)) % (textures.Length + textures.Length - 2), textures.Length),
                _ => throw new NotImplementedException($"AnimationMode {this.AnimationMode}")
            };

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var color = hasCollision ? Color.Red : Color.White;

            spriteBatch.Draw(textures[AnimationFrame], Position-(Vector2.One * Radius), null, color);
        }
    }
}