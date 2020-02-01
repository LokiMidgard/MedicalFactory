using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MedicalFactory
{
    public static class SpriteBatchExtensions
    {
        public static void Draw(this SpriteBatch batch, Texture2D texture, Vector2 position, Rectangle? sourceRectangle = null, Color? color = null, float rotation = 0f, Vector2 origin = default, Vector2? scale = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0f)
        {
            batch.Draw(texture, position, sourceRectangle, color ?? Color.White, rotation, origin, scale ?? Vector2.One, effects, layerDepth);
        }
    }
}