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

    public class Sprite : IGameObject, IUpdateable, IDrawable, ILoadable, IAttachable, IOnlyUseMeIfYouKnowWhatYouAreDoingWithAttachables, ICanCarry
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;

        public Vector2 Origin;
        public float Radius;
        public float Drag = 10.0f;
        public Vector2 Velocity;
        public float Rotation { get; set; }
        public bool CanCollide = false;

        public bool hasCollision;

        private readonly string[] textureNames;
        protected Texture2D[] textures;

        public TimeSpan AnimationFrameLength { get; set; } = TimeSpan.FromSeconds(1);

        public AnimationMode AnimationMode { get; set; }

        #region Attaching
        private ICanCarry attachedTo;

        public virtual ICanCarry AttachedTo
        {
            get { return this.attachedTo; }
        }

        public virtual void OnAttachChanged()
        {
            if (this.AttachedTo != null)
                this.Velocity = Vector2.Zero;
        }

        ICanCarry IOnlyUseMeIfYouKnowWhatYouAreDoingWithAttachables.AttachedTo { set { this.attachedTo = value; OnAttachChanged(); } }

        public Vector2 AttachOffset { get; set; }

        private readonly List<IAttachable> attached;
        public System.Collections.ObjectModel.ReadOnlyCollection<IAttachable> Attached { get; }

        public virtual void Attach(IAttachable toAdd)
        {
            if (!(toAdd is IOnlyUseMeIfYouKnowWhatYouAreDoingWithAttachables impl))
                throw new NotSupportedException();

            if (toAdd.AttachedTo == this)
                return;

            if (toAdd.AttachedTo != null)
                toAdd.AttachedTo.Detach(toAdd);

            this.attached.Add(toAdd);
            impl.AttachedTo = this;
        }
        public virtual void Detach(IAttachable toRemove)
        {
            if (!(toRemove is IOnlyUseMeIfYouKnowWhatYouAreDoingWithAttachables impl))
                throw new NotSupportedException();

            this.attached.Remove(toRemove);
            impl.AttachedTo = null;
        }
        #endregion

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


        public bool Visible { get; set; } = true;

        private Sprite()
        {
            this.attached = new List<IAttachable>();
            this.Attached = this.attached.AsReadOnly();
        }


        public Sprite(params Texture2D[] textures) : this()
        {
            this.textures = textures;
            this.textureNames = new string[] { "" };
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
                AnimationMode.Loop => ((int)Math.Floor(gameTime.TotalGameTime / this.AnimationFrameLength)) % this.textures.Length,
                AnimationMode.PingPong => PingPong(((int)Math.Floor(gameTime.TotalGameTime / this.AnimationFrameLength)) % (this.textures.Length + this.textures.Length - 2), this.textures.Length),
                _ => throw new NotImplementedException($"AnimationMode {this.AnimationMode}")
            };

            if (this.AttachedTo == null)
            {
                this.Position += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (Velocity.Length() != 0.0f)
                {
                    this.Velocity *= 0.96f;
                    if (Velocity.Length() < 0.1f)
                    {
                        Velocity = new Vector2(0.0f);
                    }
                }
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
            if (Visible)
            {
                spriteBatch.Draw(this.textures[this.AnimationFrame], this.Position, rotation: this.Rotation, origin: this.Origin, scale: this.Scale);
                if (GameConfig.DrawCollisionGeometry)
                {
                    var color = this.hasCollision ? Color.Red : Color.White;
                    DebugHelper.DrawCircle(spriteBatch, this.Position, this.Radius, color);
                }
            }
        }
    }
}