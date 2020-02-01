using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using System;

namespace MedicalFactory
{
    public class MyMathHelper
    {
        public static float RightAngleInRadians(Vector2 Basis, Vector2 Target)
        {
            float SignedAngleInRadians = Vector2.Dot(Vector2.Normalize(Basis), Vector2.Normalize(Target));

            float AngleInRadians = (float)Math.PI * ((1.0f - SignedAngleInRadians) / 2.0f);

            if ((Basis.X*Target.Y - Basis.Y*Target.X) < 0)
            {
                return 2.0f * (float)Math.PI - AngleInRadians;
            }
            else
            {
                return AngleInRadians;
            }
        }

        // rot clockwise = 0 => upvector (0,-1)
        public static Vector2 RotateBy(Vector2 vec, float radians)
        {
            return Vector2.Transform(vec, Matrix.CreateRotationX(radians));
        }
    }
}