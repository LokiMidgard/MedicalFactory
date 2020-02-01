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
    }

    public enum ParticleMovement
    {
        Static,
        WithEmitter
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
            return new Vector2(this.Scale * (((float)random.NextDouble()) * 2f - 1f), this.Scale * (((float)random.NextDouble()) * 2f - 1f));
        }

    }
    public class ParticleDirectionFix : IParticleDirectionSpawner
    {
        public Vector2 Velocety { get; set; }
        public Vector2 GetVelocety() => Velocety;

    }

    public class ParticleSystem : IUpdateable, IDrawable, ILoadable, IGameObject, IAttachable
    {
        public const int MaxParticles = 1000;
        private readonly string textureName;
        private Texture2D texture;

        public Vector2 Origin { get; set; }
        public TimeSpan SpawnRate { get; set; }
        public bool IsEnabled { get; set; }

        public ParticleMovement Movement { get; set; }

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


        public ParticleSystem(TimeSpan maxAge, string texture)
        {
            this.textureName = texture;
            positions = new Vector2[MaxParticles];
            velocetys = new Vector2[MaxParticles];
            scale = new Vector2[MaxParticles];
            createionTime = new TimeSpan[MaxParticles];
            actives = new bool[MaxParticles];
            fade = new float[MaxParticles];
            this.MaxAge = maxAge;
        }




        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var oldBlendState = spriteBatch.GraphicsDevice.BlendState;
            spriteBatch.GraphicsDevice.BlendState = BlendState.Additive;


            for (int i = 0; i < MaxParticles; i++)
            {
                if (createionTime[i] != default && gameTime.TotalGameTime - createionTime[i] < MaxAge)
                {
                    Vector2 position = positions[i];
                    if (this.Movement == ParticleMovement.WithEmitter)
                        position += this.Position;
                    spriteBatch.Draw(texture, position, origin: this.Origin, color: new Color(this.Tint, fade[i]));
                }
            }
            spriteBatch.GraphicsDevice.BlendState = oldBlendState;



        }

        public void LoadContent(Game1 game)
        {
            texture = game.Content.Load<Texture2D>(textureName);
        }


        private void Spawn(GameTime gameTime)
        {
            if (active < MaxParticles)
            {

                while (createionTime[startIndex] != default && gameTime.TotalGameTime - createionTime[startIndex] < MaxAge)
                {
                    startIndex++;
                    startIndex %= MaxParticles;
                }


                if (this.Movement == ParticleMovement.Static)
                    positions[startIndex] = Position;
                else
                    positions[startIndex] = Vector2.Zero;
                velocetys[startIndex] = this.Spawner?.GetVelocety() ?? Vector2.Zero;
                fade[startIndex] = 1.0f;
                scale[startIndex] = Vector2.One;

                createionTime[startIndex] = gameTime.TotalGameTime;
                actives[startIndex] = true;
                active++;
            }
        }


        public void Update(GameTime gameTime)
        {

            if (AttachedTo != null)
                this.Position = AttachedTo.Position;

            if (IsEnabled)
            {
                spawnBackpack += gameTime.ElapsedGameTime;
                while (spawnBackpack >= this.SpawnRate)
                {
                    spawnBackpack -= this.SpawnRate;
                    this.Spawn(gameTime);
                }

            }

            for (int i = 0; i < MaxParticles; i++)
            {
                positions[i] += velocetys[i];
                if (actives[i])
                {

                    var age = gameTime.TotalGameTime - createionTime[i];

                    var deathBegin = this.MaxAge - DeathDuration;




                    if (age >= MaxAge)
                    {
                        actives[i] = false;
                        active--;
                    }
                    else if (age > deathBegin)
                    {
                        var deathPosition = (age - deathBegin) / DeathDuration;

                        System.Diagnostics.Debug.Assert(deathPosition <= 1 && deathPosition >= 0);

                        if (this.Death.HasFlag(PatricleDeath.Fade))
                        {
                            fade[i] = 1f - (float)deathPosition;
                        }
                        if (this.Death.HasFlag(PatricleDeath.Shrink))
                        {
                            scale[i] = Vector2.One * (1f - (float)deathPosition);
                        }

                    }
                }
            }

        }
    }

}