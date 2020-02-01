using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory
{
    public static class DebugHelper
    {
        private static GraphicsDevice graphicsDevice;
        public static GraphicsDevice GraphicsDevice
        {
            get { return graphicsDevice; }
            set
            {
                if (value != graphicsDevice && value != null)
                {
                    graphicsDevice = value;
                    foreach (var radius in circleCache.Keys)
                    {
                        circleCache[radius].Dispose();
                        CreateCircleTex(radius);
                    }
                }
            }
        }

        private static Dictionary<int, Texture2D> circleCache = new Dictionary<int, Texture2D>();

        private static bool CreateCircleTex(int radius)
        {
            if (GraphicsDevice == null)
                return false;

            int diameter = radius * 2;
            Texture2D texture = new Texture2D(GraphicsDevice, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];

            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = y * diameter + x;
                    float len = new Vector2(x - radius, y - radius).Length();
         
                    if ((len > radius-2 && len <= radius) ||
                        (x == radius && y<radius)
                        )
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }

            texture.SetData(colorData);
            circleCache.Add(radius, texture);

            return true;
        }

        public static void DrawCircle(SpriteBatch spriteBatch, Vector2 position, float radius, Color color)
        {
            var idxRadius = (int)radius;

            if (radius <= 0)
                return;

            if (!circleCache.ContainsKey(idxRadius))
                CreateCircleTex(idxRadius);

            if (circleCache.ContainsKey(idxRadius))
            {
                var circle = circleCache[idxRadius];
                spriteBatch.Draw(circle, position - (Vector2.One * radius), null, color);
            }
        }
    }
}
