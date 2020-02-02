using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MedicalFactory.GameObjects
{
    public enum PlayerColor
    {
        BlauerRoboter, GruenerRoboter, GelberRoboter, RoterRoboter
    }
    public class Robot : Sprite
    {
        private Texture2D shadow;
        private readonly ParticleSystem particles;
        private readonly ParticleSystem particles2;
        private TimeSpan nextSpark;
        private TimeSpan sparkDuration;
        public PlayerColor PlayerColor;
        public Player Player;

        private static readonly Random random = new Random();

        public static Color ToColor(PlayerColor player)
        {
            return player switch
            {
                PlayerColor.BlauerRoboter => new Color(0.235f, 0.384f, 0.875f),
                PlayerColor.GelberRoboter => new Color(0.71f, 0.706f, 0.031f),
                PlayerColor.GruenerRoboter => new Color(0.008f, 0.627f, 0.204f),
                PlayerColor.RoterRoboter => new Color(0.788f, 0.227f, 0.227f),
                _ => throw new NotImplementedException(),
            };
        }

        public Robot(PlayerColor color) : base(color.ToString() + "_1", color.ToString() + "_2", color.ToString() + "_3")
        {
            this.PlayerColor = color;
            this.Origin = new Vector2(30.0f, 90.0f);
            this.AnimationFrameLength = TimeSpan.FromMilliseconds(100);

            var offsetLeft = color switch
            {
                PlayerColor.GruenerRoboter => new Vector2(0, -30),
                PlayerColor.BlauerRoboter => new Vector2(30, 0),
                PlayerColor.GelberRoboter => new Vector2(30, 30),
                PlayerColor.RoterRoboter => new Vector2(30, -30),
                _ => throw new NotSupportedException()
            };
            var offsetRigth = color switch
            {
                PlayerColor.GruenerRoboter => new Vector2(0, 30),
                PlayerColor.BlauerRoboter => new Vector2(-30, 0),
                PlayerColor.GelberRoboter => new Vector2(-30, 30),
                PlayerColor.RoterRoboter => new Vector2(-30, -30),
                _ => throw new NotSupportedException()
            };


            // initilize Particles
            this.particles = new ParticleSystem(TimeSpan.FromSeconds(3), "particle", 100)
            {
                Tint = ToColor(color),
                SpawnRate = TimeSpan.FromSeconds(0.05),
                Spawner = new ParticleDirectionRandom() { Scale = 0.9f },
                DeathDuration = TimeSpan.FromSeconds(0.4),
                MaxAge = TimeSpan.FromSeconds(0.5),
                Death = PatricleDeath.Fade | PatricleDeath.Shrink,
                IsEnabled = true,
                BlendMode = ParticlelBlendMode.Additive,
                Movement = ParticleMovement.Static,
                AttachOffset = offsetLeft,
            };
            this.particles2 = new ParticleSystem(TimeSpan.FromSeconds(3), "particle", 100)
            {
                Tint = ToColor(color),
                SpawnRate = TimeSpan.FromSeconds(0.05),
                Spawner = new ParticleDirectionRandom() { Scale = 0.9f },
                DeathDuration = TimeSpan.FromSeconds(0.4),
                MaxAge = TimeSpan.FromSeconds(0.5),
                Death = PatricleDeath.Fade | PatricleDeath.Shrink,
                IsEnabled = true,
                BlendMode = ParticlelBlendMode.Additive,
                Movement = ParticleMovement.Static,
                AttachOffset = offsetRigth,
            };
            this.Attach(this.particles);
            this.Attach(this.particles2);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Visible)
            {
                spriteBatch.Draw(this.shadow, this.Position + new Vector2(0, 10), null, Color.White, 0, new Vector2(shadow.Width / 2.0f, shadow.Height / 2.0f), new Vector2(0.6f, 0.6f), SpriteEffects.None, 0.0f);
                base.Draw(spriteBatch, gameTime);
                this.particles.Draw(spriteBatch, gameTime);
                this.particles2.Draw(spriteBatch, gameTime);
            }
        }

        public override void LoadContent(Game1 game)
        {
            base.LoadContent(game);
            this.particles.LoadContent(game);
            this.particles2.LoadContent(game);
            this.shadow = game.Content.Load<Texture2D>("Schatten_Oval");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            CollisionManager.KeepInWorld(this);

            // PlayerCollisions
            if (Visible)
            {
                IEnumerable<Collision> objColls = CollisionManager.GetCollisions(this, Game1.sprites);
                objColls = objColls.Where(c => (c.spriteB is Robot));
                if (objColls.Count() > 0)
                {
                    var coll = objColls.First();
                    if (coll.spriteB.Visible)
                    {
                        Position += coll.Distance * 2;
                        coll.spriteB.Position -= coll.Distance * 2;
                        Player?.Rumble();
                        (coll.spriteB as Robot)?.Player?.Rumble();
                    }
                }
            }

            // if transporting body part => spill some blood
            var item = Attached.OfType<IItem>().FirstOrDefault();
            if (item is BodyPart bodyPart)
            {
                if (random.NextDouble() < 0.025)
                    Game1.Background.AddBloodSplash(Position, bodyPart.IsDamaged);
            }
            if (item is null)
                this.AnimationMode = AnimationMode.PingPong;
            else
            {
                this.AnimationMode = AnimationMode.None;
                this.AnimationFrame = 0;
            }

            this.particles.Update(gameTime);
            this.particles2.Update(gameTime);

            if (this.Velocity.Length() > 3)
            {
                this.nextSpark = gameTime.TotalGameTime + TimeSpan.FromSeconds(random.NextDouble() * 3.0 + 1.0);
                this.particles.IsEnabled = true;
                this.particles2.IsEnabled = true;
            }
            else
            {
                this.particles.IsEnabled = false;
                this.particles2.IsEnabled = false;
            }
        }

    }
}
