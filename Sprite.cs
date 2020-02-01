using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MedicalFactory
{

    public enum AnimationMode
    {
        None,
        Loop,
        PingPong
    }


    public interface IAttachable
    {
        ICanCarray AttachedTo { set; get; }
        Vector2 AttachOffset { get; set; }
    }

    public interface ICanCarray
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        System.Collections.ObjectModel.ReadOnlyCollection<IAttachable> Attached { get; }
        void Attach(IAttachable add);
        void Detach(IAttachable add);
    }

    public class Sprite : IGameObject, IUpdateable, IDrawable, ILoadable, IAttachable, ICanCarray
    {
        public Vector2 Position { get; set; }
        public Vector2 Origin;
        public float Radius;
        public Vector2 Velocity;
        public float Rotation { get; set; }
        public bool CanCollide = false;

        public bool hasCollision;

        private readonly string[] textureNames;
        private Texture2D[] textures;

        public int AnimationFrameTimeInMS { get; set; } = 1000;

        public AnimationMode AnimationMode { get; set; }

        private ICanCarray attachedTo;
        public virtual ICanCarray AttachedTo
        {
            get => this.attachedTo; set
            {
                var newValue = value;
                var oldValue = this.attachedTo;
                this.attachedTo = newValue;
                if (newValue == oldValue)
                    return;

                if (oldValue != null)
                    oldValue.Detach(this);
                if (newValue != null)
                    newValue.Attach(this);
            }
        }

        public Vector2 AttachOffset { get; set; }

        private readonly List<IAttachable> attached;
        public System.Collections.ObjectModel.ReadOnlyCollection<IAttachable> Attached { get; }
        public void Attach(IAttachable toAdd)
        {
            if (toAdd.AttachedTo == this)
                return;
            if (toAdd.AttachedTo != null)
                toAdd.AttachedTo = null;
            this.attached.Add(toAdd);
            toAdd.AttachedTo = this;
        }
        public void Detach(IAttachable toRemove)
        {
            this.attached.Remove(toRemove);
            toRemove.AttachedTo = null;
        }

        private int animationFrame;
        public int AnimationFrame
        {
            get
            {
                return this.animationFrame;
            }
            set
            {
                if (this.animationFrame != value)
                {
                    this.animationFrame = value;
                    this.UpdateRadius();
                }
            }
        }

        private void UpdateRadius()
        {
            this.Radius = this.textures[this.animationFrame].Width / 2.0f;
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


        private Sprite()
        {
            this.attached = new List<IAttachable>();
            this.Attached = this.attached.AsReadOnly();
        }

        public Sprite(Texture2D texture) : this()
        {
            this.textures = new Texture2D[] { texture };
            this.textureNames = new string[] { "" };
            Init();
        }

        public Sprite(params Texture2D[] textures) :this()
        {
            this.textures = textures;
            this.textureNames = new string[]{""};
            Init();
        }


        public Sprite(params string[] textureNames) : this()
        {
            this.textureNames = textureNames;
            this.textures = new Texture2D[textureNames.Length];
        }

        public virtual void Init()
        {
            this.UpdateRadius();
            if (this.Origin == default)
            {
                this.Origin = new Vector2(this.textures[0].Width / 2.0f, this.textures[0].Height / 2.0f);
            }

        }

        public virtual void LoadContent(Game1 game)
        {
            for (var i = 0; i < textures.Length; ++i)
            {
                if (textures[i] == null && !string.IsNullOrEmpty(textureNames[i]))
                {
                    textures[i] = game.Content.Load<Texture2D>(textureNames[i]);
                }
            }
            Init();
        }

        public virtual void Update(GameTime gameTime)
        {
            this.AnimationFrame = this.AnimationMode switch
            {
                AnimationMode.None => this.AnimationFrame,
                AnimationMode.Loop => ((int)Math.Floor(gameTime.TotalGameTime.TotalMilliseconds / this.AnimationFrameTimeInMS)) % this.textures.Length,
                AnimationMode.PingPong => PingPong(((int)Math.Floor(gameTime.TotalGameTime.TotalMilliseconds / this.AnimationFrameTimeInMS)) % (this.textures.Length + this.textures.Length - 2), this.textures.Length),
                _ => throw new NotImplementedException($"AnimationMode {this.AnimationMode}")
            };

            if (this.AttachedTo == null)
            {
                this.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                Vector2 offset = MyMathHelper.RotateBy(this.AttachOffset, this.AttachedTo.Rotation);

                this.Position = this.AttachedTo.Position + offset;
                this.Rotation = this.AttachedTo.Rotation;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.textures[this.AnimationFrame], this.Position, null, Color.White, this.Rotation, this.Origin, 1.0f, SpriteEffects.None, 0.0f);

            if (GameConfig.DrawCollisionGeometry)
            {
                var color = this.hasCollision ? Color.Red : Color.White;
                DebugHelper.DrawCircle(spriteBatch, this.Position, this.Radius, color);
            }
        }
    }
}