using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalFactory.GameObjects
{

    public class BodyPart : Sprite
    {

        public bool IsDemaged { get => this.AnimationFrame == 0 ? false : true; set => this.AnimationFrame = value ? 1 : 0; }

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

        private static Dictionary<BodyPartType, Texture2D> defect = new Dictionary<BodyPartType, Texture2D>();
        private static Dictionary<BodyPartType, Texture2D> correct = new Dictionary<BodyPartType, Texture2D>();

        public static void LoadContent(ContentManager content)
        {
            if (defect.Count == 0)
                foreach (var item in Enum.GetValues(typeof(BodyPartType)).Cast<BodyPartType>())
                {
                    defect.Add(item, content.Load<Texture2D>(GetDamagedTextureName(item)));
                    correct.Add(item, content.Load<Texture2D>(item.ToString()));
                }
        }

        public BodyPart(BodyPartType type) : base(correct[type], defect[type])
        {
            this.AttachOffset = DefaultAttachOffset;
            this.Type = type;
        }

        public override ICanCarray AttachedTo
        {
            get => base.AttachedTo; set
            {
                var oldValue = base.AttachedTo;
                base.AttachedTo = value;
                if (value is Patient)
                {
                    this.AttachOffset = this.Type switch
                    {
                        BodyPartType.HERZ => new Vector2(15, -60),
                        BodyPartType.LUNGE => new Vector2(0, -90),
                        BodyPartType.NIERE => new Vector2(-20, -20),
                        _ => throw new NotImplementedException($"Type {this.Type}")
                    };


                }
                if (value is Robot)
                    this.AttachOffset = DefaultAttachOffset;

                this.ShouldScaleDown = value is Patient && !(oldValue is Patient);
                this.ShouldScaleUp = !(value is Patient) && this.Scale.X < 1f;
            }
        }

        private bool ShouldScaleDown;
        private bool ShouldScaleUp;
        private TimeSpan finishedScalingDown;
        private TimeSpan finishedScalingUp;

        public override void LoadContent(Game1 game)
        {
            LoadContent(game.Content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.AttachedTo is null)
                this.IsDemaged = true;


            var scalingTime = TimeSpan.FromSeconds(1);
            if (this.ShouldScaleDown && this.finishedScalingDown == default)
            {
                this.finishedScalingDown = gameTime.TotalGameTime + scalingTime;
                this.ShouldScaleDown = false;
            }
            if (this.ShouldScaleUp && this.finishedScalingUp == default)
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


        }

    }
}
