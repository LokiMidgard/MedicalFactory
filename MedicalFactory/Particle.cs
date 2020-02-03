using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using System;

namespace MedicalFactory
{
    [Flags]
    public enum PatricleDeath
    {
        None,
        Fade = 1 << 1,
        Shrink = 1 << 2,
        Grow = 1 << 3,
    }

    public enum ParticleMovement
    {
        Static,
        WithEmitter
    }

    public enum ParticlelBlendMode
    {
        None,
        Additive,
    }

    public interface IParticleDirectionSpawner
    {
        Vector2 GetVelocety();
    }
    public class ParticleDirectionRandom : IParticleDirectionSpawner
    {
        private Random random = new Random();
        public float Scale { get; set; } = 1.0f;
        public Vector2 GetVelocety()
        {
            return new Vector2(this.Scale * (((float)this.random.NextDouble()) * 2f - 1f), this.Scale * (((float)this.random.NextDouble()) * 2f - 1f));
        }

    }
    public class ParticleDirectionFix : IParticleDirectionSpawner
    {
        public Vector2 Velocety { get; set; }
        public Vector2 GetVelocety() => this.Velocety;

    }

    public class ParticleSystem : IUpdateable, IDrawable, ILoadable, IGameObject, IAttachable, IOnlyUseMeIfYouKnowWhatYouAreDoingWithAttachables
    {
        private readonly int MaxParticles;
        private readonly string textureName;
        private Texture2D texture;

        public Vector2? Origin { get; set; }
        public TimeSpan SpawnRate { get; set; }
        public bool IsEnabled { get; set; }

        public Vector2 Scale { get; set; } = Vector2.One;

        public ParticleMovement Movement { get; set; }

        public ParticlelBlendMode BlendMode { get; set; }

        private int startIndex;
        private int active;

        public TimeSpan MaxAge { get; set; }
        public TimeSpan DeathDuration { get; set; }

        public Color Tint { get; set; } = Color.White;

        ///<Summary>        
        /// to safe the spawn between updates
        ///</Summary>        
        private TimeSpan spawnBackpack;


        public Vector2 Position { get; set; }
        public IParticleDirectionSpawner Spawner { get; set; }
        public PatricleDeath Death { get; set; }

        private Vector2[] positions;
        private Vector2[] scale;
        private float[] fade;
        private Vector2[] velocetys;
        private TimeSpan[] createionTime;
        private bool[] actives;

        private ICanCarry attachedTo;

        public virtual ICanCarry AttachedTo
        {
            get { return this.attachedTo; }
        }

        public virtual void OnAttachChanged() { }

        ICanCarry IOnlyUseMeIfYouKnowWhatYouAreDoingWithAttachables.AttachedTo { set { this.attachedTo = value; OnAttachChanged(); } }

        public Vector2 AttachOffset { get; set; }

        public ParticleSystem(TimeSpan maxAge, string texture, int maxParticles = 1000)
        {
            this.MaxParticles = maxParticles;
            this.textureName = texture;
            this.positions = new Vector2[this.MaxParticles];
            this.velocetys = new Vector2[this.MaxParticles];
            this.scale = new Vector2[this.MaxParticles];
            this.createionTime = new TimeSpan[this.MaxParticles];
            this.actives = new bool[this.MaxParticles];
            this.fade = new float[this.MaxParticles];
            this.MaxAge = maxAge;
        }
        public ParticleSystem(TimeSpan maxAge, Texture2D texture, int maxParticles = 1000)
        {
            this.MaxParticles = maxParticles;
            this.textureName = texture.Name;
            this.texture = texture;
            this.positions = new Vector2[this.MaxParticles];
            this.velocetys = new Vector2[this.MaxParticles];
            this.scale = new Vector2[this.MaxParticles];
            this.createionTime = new TimeSpan[this.MaxParticles];
            this.actives = new bool[this.MaxParticles];
            this.fade = new float[this.MaxParticles];
            this.MaxAge = maxAge;
            this.Origin = new Vector2(this.texture.Width / 2f, this.texture.Height / 2f);
        }




        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var oldBlendState = spriteBatch.GraphicsDevice.BlendState;
            if (this.BlendMode == ParticlelBlendMode.Additive)
                spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;
            else
                spriteBatch.GraphicsDevice.BlendState = BlendState.NonPremultiplied;


            for (int i = 0; i < this.MaxParticles; i++)
            {
                if (this.createionTime[i] != default && gameTime.TotalGameTime - this.createionTime[i] < this.MaxAge)
                {
                    var position = this.positions[i];
                    if (this.Movement == ParticleMovement.WithEmitter)
                        position += this.Position;
                    spriteBatch.Draw(this.texture, position, origin: this.Origin.Value, color: new Color(this.Tint, this.fade[i]), scale: this.scale[i] * this.Scale);
                }
            }
            spriteBatch.GraphicsDevice.BlendState = oldBlendState;



        }

        public void LoadContent(Game1 game)
        {
            if (texture != null)
                return;
            this.texture = game.Content.Load<Texture2D>(this.textureName);
            this.Origin ??= new Vector2(this.texture.Width / 2f, this.texture.Height / 2f);
        }


        public void Spawn(GameTime gameTime)
        {
            if (this.active < this.MaxParticles)
            {

                while (this.createionTime[this.startIndex] != default && gameTime.TotalGameTime - this.createionTime[this.startIndex] < this.MaxAge)
                {
                    this.startIndex++;
                    this.startIndex %= this.MaxParticles;
                }


                if (this.Movement == ParticleMovement.Static)
                    this.positions[this.startIndex] = this.Position;
                else
                    this.positions[this.startIndex] = Vector2.Zero;
                this.velocetys[this.startIndex] = this.Spawner?.GetVelocety() ?? Vector2.Zero;
                this.fade[this.startIndex] = 1.0f;
                this.scale[this.startIndex] = Vector2.One;

                this.createionTime[this.startIndex] = gameTime.TotalGameTime;
                this.actives[this.startIndex] = true;
                this.active++;
            }
        }


        public void Update(GameTime gameTime)
        {

            if (this.AttachedTo != null)
            {
                Vector2 offset = MyMathHelper.RotateBy(this.AttachOffset, this.AttachedTo.Rotation);

                this.Position = this.AttachedTo.Position + offset;
            }

            if (this.IsEnabled)
            {
                this.spawnBackpack += gameTime.ElapsedGameTime;
                while (this.spawnBackpack >= this.SpawnRate)
                {
                    this.spawnBackpack -= this.SpawnRate;
                    this.Spawn(gameTime);
                }

            }

            for (int i = 0; i < this.MaxParticles; i++)
            {
                this.positions[i] += this.velocetys[i];
                if (this.actives[i])
                {

                    var age = gameTime.TotalGameTime - this.createionTime[i];

                    var deathBegin = this.MaxAge - this.DeathDuration;




                    if (age >= this.MaxAge)
                    {
                        this.actives[i] = false;
                        this.active--;
                    }
                    else if (age > deathBegin)
                    {
                        var deathPosition = (age - deathBegin) / this.DeathDuration;

                        System.Diagnostics.Debug.Assert(deathPosition <= 1 && deathPosition >= 0);

                        if (this.Death.HasFlag(PatricleDeath.Fade))
                        {
                            this.fade[i] = 1f - (float)deathPosition;
                        }
                        if (this.Death.HasFlag(PatricleDeath.Shrink))
                        {
                            this.scale[i] = Vector2.One * (1f - (float)deathPosition);
                        }
                        else if (this.Death.HasFlag(PatricleDeath.Grow))
                        {
                            this.scale[i] = Vector2.One + Vector2.One *2f* ((float)deathPosition);
                        }

                    }
                }
            }

        }
    }

}