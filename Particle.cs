using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using System;

namespace MedicalFactory
{

    public enum PatricleDeath
    {
        None,
        Fade,
        Shrink,
    }



    public struct RandomDirection
    {

    }
    public struct FixDirection 
    {

    }

    public class ParticleSystem : IUpdateable, IDrawable, ILoadable, IGameObject, IAttachable
    {
        private const int MaxParticles = 1000;
        private readonly string textureName;
        private Texture2D texture;

        public Vector2 Origin { get; set; }
        public TimeSpan SpawnRate { get; set; }
        public bool IsEnabled { get; set; }

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
        public Vector2 Velocety { get; set; }
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
                    spriteBatch.Draw(texture, positions[i], origin: this.Origin, color: new Color(this.Tint, fade[i]));
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



                positions[startIndex] = Position;
                velocetys[startIndex] = Velocety;
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

                    }
                }
            }

        }
    }

}