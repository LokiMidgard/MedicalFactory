using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class BodyPart : Sprite, IItem
    {
        public bool Splash { get; private set; }

        public bool Silent { get; set; }

        private TimeSpan DreiSekundenRegel = TimeSpan.FromSeconds(3);
        public bool IsDamaged { get => this.AnimationFrame == 0 ? false : true; set => this.AnimationFrame = value ? 1 : 0; }

        public enum BodyPartType
        {
            HERZ,
            NIERE,
            LUNGE,
        }


        public static string GetDamagedTextureName(BodyPartType type)
        {
            return type switch
            {
                BodyPartType.HERZ => "Kaputtes_Herz",
                BodyPartType.NIERE => "Kaputte_Niere",
                BodyPartType.LUNGE => "Kaputte_Lunge",
                _ => throw new NotSupportedException()
            };
        }

        public static Vector2 DefaultAttachOffset = Vector2.UnitY * -48;
        public BodyPartType Type;
        private readonly ParticleSystem bloodParticle;
        private static Dictionary<BodyPartType, Texture2D> defect = new Dictionary<BodyPartType, Texture2D>();
        private static Dictionary<BodyPartType, Texture2D> correct = new Dictionary<BodyPartType, Texture2D>();
        private static SoundEffect[] splashs;
        private static Texture2D[] blood;
        public static void LoadContent(ContentManager content)
        {
            if (defect.Count == 0)
                foreach (var item in Enum.GetValues(typeof(BodyPartType)).Cast<BodyPartType>())
                {
                    defect.Add(item, content.Load<Texture2D>(GetDamagedTextureName(item)));
                    correct.Add(item, content.Load<Texture2D>(item.ToString()));
                }

            const int numberOfSoundefects = 16;
            splashs = new SoundEffect[numberOfSoundefects];
            for (int i = 1; i <= numberOfSoundefects; i++)
            {
                var name = $"SoundEffects/slime_{ string.Format("{0}", i).PadLeft(2, '0')}";
                splashs[i - 1] = content.Load<SoundEffect>(name);
            }

            blood = new string[] { "Blut_Mittel" }.Select(x => content.Load<Texture2D>(x)).ToArray();
        }

        public BodyPart(BodyPartType type) : base(correct[type], defect[type])
        {
            this.AttachOffset = DefaultAttachOffset;
            this.Type = type;
            this.bloodParticle = new ParticleSystem(TimeSpan.FromSeconds(0.3), blood[0], 3)
            {

                BlendMode = ParticlelBlendMode.None,
                DeathDuration = TimeSpan.FromSeconds(0.5),
                Scale = new Vector2(0.5f, 0.5f),
                Death = PatricleDeath.Fade | PatricleDeath.Grow,
                 Movement = ParticleMovement.WithEmitter
                 
            };
            this.Attach(this.bloodParticle);
        }

        private ICanCarry oldValue;
        public override void OnAttachChanged()
        {
            base.OnAttachChanged();
            var value = this.AttachedTo;
            if (value is HumanPatient)
            {
                this.AttachOffset = this.Type switch
                {
                    BodyPartType.HERZ => new Vector2(15, -60),
                    BodyPartType.LUNGE => new Vector2(0, -90),
                    BodyPartType.NIERE => new Vector2(-20, -20),
                    _ => throw new NotImplementedException($"Type {this.Type}")
                };
            }

            if (value is AlienPatient)
            {
                var isSeccondHath = value.Attached.OfType<BodyPart>().Where(x => x.Type == BodyPartType.HERZ).Count() >= 2;
                this.AttachOffset = this.Type switch
                {
                    BodyPartType.HERZ => isSeccondHath ? new Vector2(-15, -75) : new Vector2(15, -60),
                    BodyPartType.LUNGE => new Vector2(-20, 20),
                    BodyPartType.NIERE => new Vector2(-20, -20),
                    _ => throw new NotImplementedException($"Type {this.Type}")
                };
            }

            if (this.oldValue is Patient || value is Patient && !this.Silent)
            {
                var index = Game1.rng.Next(0, splashs.Length);
                splashs[index].Play();
                this.Splash = true;
            }

            if (this.Type == BodyPartType.HERZ && this.oldValue is AlienPatient)
            {
                var otherHeart = this.oldValue.Attached.OfType<BodyPart>().FirstOrDefault(x => x.Type == BodyPartType.HERZ);
                if (otherHeart != null)
                    otherHeart.AttachOffset = new Vector2(15, -60);
            }

            if (value is Robot)
                this.AttachOffset = DefaultAttachOffset;

            this.ShouldScaleDown = value is Patient && !(this.oldValue is Patient);
            this.ShouldScaleUp = !(value is Patient) && this.Scale.X < 1f;


            this.oldValue = value;
        }

        private bool ShouldScaleDown;
        private bool ShouldScaleUp;


        private TimeSpan finishedScalingDown;
        private TimeSpan finishedScalingUp;
        int SplashCount = 0;
        double NextSplashTime = 0;

        public override void LoadContent(Game1 game)
        {
            LoadContent(game.Content);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            this.bloodParticle.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (this.Splash)
            {

                this.bloodParticle.Spawn(gameTime);
                this.Splash = false;
            }
            this.bloodParticle.Update(gameTime);

            if (this.AttachedTo is null)
            {
                DreiSekundenRegel -= gameTime.ElapsedGameTime;
                if (DreiSekundenRegel.TotalMilliseconds < 0)
                {
                    this.IsDamaged = true;
                    Game1.Background.AddBloodSplash(Position, IsDamaged, 0.4f);
                    DreiSekundenRegel = TimeSpan.FromSeconds(3);
                }
                CollisionManager.KeepInWorld(this, (recycler) => { recycler.PutStuffInside(this); });
            }
            else
            {
                DreiSekundenRegel = TimeSpan.FromSeconds(3);
            }

            var scalingTime = TimeSpan.FromSeconds(1);
            if (this.ShouldScaleDown && this.finishedScalingDown == default && this.Scale.X > 0.5f)
            {
                this.finishedScalingDown = gameTime.TotalGameTime + scalingTime;
                this.ShouldScaleDown = false;
            }
            if (this.ShouldScaleUp && this.finishedScalingUp == default && this.Scale.X < 1.0f)
            {
                this.finishedScalingUp = gameTime.TotalGameTime + scalingTime;
                this.ShouldScaleUp = false;
            }
            if (this.finishedScalingDown != default && gameTime.TotalGameTime >= this.finishedScalingDown)
            {
                this.finishedScalingDown = default;
                this.Scale = new Vector2(0.5f, 0.5f);
            }
            else if (this.finishedScalingDown != default)
            {
                var scalePosition = (float)((this.finishedScalingDown - gameTime.TotalGameTime) / scalingTime) * 0.5f + 0.5f;
                this.Scale = new Vector2(scalePosition, scalePosition);
            }
            if (this.finishedScalingUp != default && gameTime.TotalGameTime >= this.finishedScalingUp)
            {
                this.finishedScalingUp = default;
                this.Scale = Vector2.One;
            }
            else if (this.finishedScalingUp != default)
            {
                var scalePosition = (1f - (float)((this.finishedScalingUp - gameTime.TotalGameTime) / scalingTime)) * 0.5f + 0.5f;
                this.Scale = new Vector2(scalePosition, scalePosition);
            }

            if (Velocity.Length() > 2f)
            {
                if (NextSplashTime <= 0.0f)
                {
                    Game1.Background.AddBloodSplash(Position, IsDamaged, SplashCount * 0.5f / (1.0f + SplashCount));
                    NextSplashTime = 80 / Velocity.Length();
                    SplashCount += 1;
                }
                NextSplashTime -= gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                NextSplashTime = 0;
                SplashCount = 0;
            }
        }

    }
}
