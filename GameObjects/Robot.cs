using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MedicalFactory.GameObjects
{
    public enum PlayerColor
    {
        Roboter_Blau, Roboter_Gruen, Roboter_Gelb, Roboter_Rot
    }
    public class Robot : Sprite
    {
        private readonly ParticleSystem particles;
        private TimeSpan nextSpark;
        private TimeSpan sparkDuration;
        private static readonly Random random = new Random();
        public Robot(PlayerColor color) : base(color.ToString())
        {
            this.Origin = new Vector2(30.0f, 90.0f);

            // initilize Particles
            this.particles = new ParticleSystem(TimeSpan.FromSeconds(3), "particle", 100)
            {
                Tint = Color.Yellow,
                SpawnRate = TimeSpan.FromSeconds(0.05),
                Spawner = new ParticleDirectionRandom() { Scale = 0.9f },
                DeathDuration = TimeSpan.FromSeconds(0.4),
                MaxAge = TimeSpan.FromSeconds(0.5),
                Death = PatricleDeath.Fade | PatricleDeath.Shrink,
                IsEnabled = true,
                BlendMode = ParticlelBlendMode.Additive,
                Movement = ParticleMovement.Static,
            };
            this.particles.AttachedTo = this;
            this.particles.AttachOffset = new Vector2(30, 30);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            this.particles.Draw(spriteBatch, gameTime);
        }

        public override void LoadContent(Game1 game)
        {
            base.LoadContent(game);
            this.particles.LoadContent(game);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.particles.Update(gameTime);

            if (this.Velocity.Length() > 3)
            {
                //if (this.nextSpark < gameTime.TotalGameTime)
                {
                    this.nextSpark = gameTime.TotalGameTime + TimeSpan.FromSeconds(random.NextDouble() * 3.0 + 1.0);
                    this.particles.IsEnabled = true;
                    
                }
                //else
                {
                    //this.particles.IsEnabled = false;
                }
            }
            else
                this.particles.IsEnabled = false;

        }

    }
}
