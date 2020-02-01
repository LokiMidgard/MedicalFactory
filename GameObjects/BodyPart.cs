using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{

    public class BodyPart : Sprite
    {
        public enum BodyPartType
        {
            HERZ,
            KAPUTTES_HERZ,
            NIERE,
            KAPUTTE_NIERE,
            LUNGE,
            KAPUTTE_LUNGE,
        }

        public static Vector2 DefaultAttachOffset = Vector2.UnitY * -48;
        public BodyPartType Type;

        public BodyPart(BodyPartType type) : base(type.ToString())
        {
            AttachOffset = DefaultAttachOffset;
            this.Type = type;
        }

        public BodyPart(BodyPartType type, Texture2D tex) : base(tex)
        {
            AttachOffset = DefaultAttachOffset;
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
                        BodyPartType.KAPUTTES_HERZ => new Vector2(0, -30),
                        BodyPartType.KAPUTTE_LUNGE => new Vector2(0, -80),
                        BodyPartType.KAPUTTE_NIERE => new Vector2(-20, 0),
                        _ => throw new NotImplementedException($"Type {this.Type}")
                    };


                }

                this.ShouldScaleDown = value is Patient && !(oldValue is Patient);
                this.ShouldScaleUp = !(value is Patient) && (oldValue is Patient);
            }
        }

        private bool ShouldScaleDown;
        private bool ShouldScaleUp;
        private TimeSpan finishedScalingDown;
        private TimeSpan finishedScalingUp;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

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
            if (gameTime.TotalGameTime >= this.finishedScalingDown)
            {
                this.finishedScalingDown = default;
            }
            else if (this.finishedScalingDown != default)
            {
                var scalePosition = (float)((this.finishedScalingDown - gameTime.TotalGameTime) / scalingTime) * 0.5f + 0.5f;
                this.Scale = new Vector2(scalePosition, scalePosition);
            }
            if (gameTime.TotalGameTime >= this.finishedScalingUp)
            {
                this.finishedScalingUp = default;
            }
            else if (this.finishedScalingUp != default)
            {
                var scalePosition = (1f - (float)((this.finishedScalingUp - gameTime.TotalGameTime) / scalingTime)) * 0.5f + 0.5f;
                this.Scale = new Vector2(scalePosition, scalePosition);
            }


        }

    }
}
