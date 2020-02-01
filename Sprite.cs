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
    }

    public interface ICanCarray
    {
        Vector2 Position { get; set; }
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
        public float Rotation;
        public bool CanCollide = false;

        public bool hasCollision;

        private readonly string[] textureNames;
        private Texture2D[] textures;

        public int AnimationFrameTimeInMS { get; set; } = 1000;

        public AnimationMode AnimationMode { get; set; }

        private ICanCarray attachedTo;
        public ICanCarray AttachedTo
        {
            get => attachedTo; set
            {
                if (attachedTo == value)
                    return;

                if (attachedTo != null)
                    attachedTo.Detach(this);
                attachedTo = value;
                attachedTo.Attach(this);
            }
        }

        private readonly List<IAttachable> attached;
        public System.Collections.ObjectModel.ReadOnlyCollection<IAttachable> Attached { get; }
        public void Attach(IAttachable toAdd)
        {
            if (toAdd.AttachedTo == this)
                return;
            if (toAdd.AttachedTo != null)
                throw new ArgumentException("IAttachable may not already be attached.");
            this.attached.Add(toAdd);
            toAdd.AttachedTo = this;
        }
        public void Detach(IAttachable toRemove)
        {
            if (toRemove.AttachedTo is null)
                return;
            if (toRemove.AttachedTo != this)
                throw new ArgumentException("IAttachable must be attached to this");
            this.attached.Remove(toRemove);
            toRemove.AttachedTo = null;
        }




        private int animationFrame;
        public int AnimationFrame
        {
            get
            {
                return animationFrame;
            }
            set
            {
                if (animationFrame != value)
                {
                    animationFrame = value;
                    UpdateRadius();
                }
            }
        }

        private void UpdateRadius()
        {
            Radius = textures[animationFrame].Width / 2.0f;
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
            attached = new List<IAttachable>();
            Attached = attached.AsReadOnly();
            this.textureNames = textureNames;
        }

        public virtual void LoadContent(Game1 game)
        {
            textures = this.textureNames.Select(x => game.Content.Load<Texture2D>(x)).ToArray();
            UpdateRadius();
            if (Origin == default)
            {
                Origin = new Vector2(textures[0].Width / 2.0f, textures[0].Height / 2.0f);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            this.AnimationFrame = this.AnimationMode switch
            {
                AnimationMode.None => this.AnimationFrame,
                AnimationMode.Loop => ((int)Math.Floor(gameTime.TotalGameTime.TotalMilliseconds / AnimationFrameTimeInMS)) % textures.Length,
                AnimationMode.PingPong => PingPong(((int)Math.Floor(gameTime.TotalGameTime.TotalMilliseconds / AnimationFrameTimeInMS)) % (textures.Length + textures.Length - 2), textures.Length),
                _ => throw new NotImplementedException($"AnimationMode {this.AnimationMode}")
            };

            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(textures[AnimationFrame], Position, null, Color.White, Rotation, Origin, 1.0f, SpriteEffects.None, 0.0f);

            if (GameConfig.DrawCollisionGeometry)
            {
                var color = hasCollision ? Color.Red : Color.White;
                DebugHelper.DrawCircle(spriteBatch, Position, Radius, color);
            }
        }
    }
}