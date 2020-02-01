using System;
using System.Linq;
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
        private Texture2D shadow;
        private readonly ParticleSystem particles;
        private TimeSpan nextSpark;
        private TimeSpan sparkDuration;
        private PlayerColor playerColor;

        private static readonly Random random = new Random();
        
        public Robot(PlayerColor color) : base(color.ToString())
        {
            this.playerColor = color;
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
            this.Attach(this.particles);
            this.particles.AttachOffset = new Vector2(30, 30);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(this.shadow, this.Position + new Vector2(0, 10), null, Color.White, 0, new Vector2(shadow.Width / 2.0f, shadow.Height / 2.0f), new Vector2(0.6f, 0.6f), SpriteEffects.None, 0.0f);
            base.Draw(spriteBatch, gameTime);
            this.particles.Draw(spriteBatch, gameTime);
        }

        public override void LoadContent(Game1 game)
        {
            base.LoadContent(game);
            this.particles.LoadContent(game);
            this.shadow = game.Content.Load<Texture2D>("Schatten_Oval");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var isUpperHalf = ((int)(playerColor) % 2) == 0;

            var yMin = isUpperHalf ? 64 : Game1.conveyerBelt.YPos + 64;
            var yMax = isUpperHalf ? Game1.conveyerBelt.YPos - 64 : 1028 - 16;

            if (!GameConfig.KeepPlayersToTheirSide)
            {
                yMin = 64;
                yMax = 1028 - 16;
            }

            Position = new Vector2(
                MathHelper.Clamp(Position.X, 32, 1920 - 32),
                MathHelper.Clamp(Position.Y, yMin, yMax)
            );

            // if transporting body part => spill some blood
            var bp = Attached.FirstOrDefault(a => a is BodyPart) as BodyPart;
            if (bp != null)
            {
                if (random.NextDouble() < 0.025)
                    Game1.Background.AddBloodSplash(Position, bp.IsDemaged);
            }

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
