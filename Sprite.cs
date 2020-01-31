using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;

namespace MedicalFactory
{
    public class Sprite : GameObject
    {
        public Vector2 Position;
        public float Radius;
        public bool CanCollide = false;
        private string TextureName;
        private Texture2D Texture;

        public bool hasCollision;

        public Sprite() : this("stick")
        {
        }

        public Sprite(string TextureName)
        {
            this.TextureName = TextureName;
        }

        public void LoadContent(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>(this.TextureName);
            Radius = Texture.Width / 2.0f;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (hasCollision)
            {
                spriteBatch.Draw(Texture, Position, null, Color.Red);
            }
            else
            {
                spriteBatch.Draw(Texture, Position, null, Color.White);
            }
        }
    }
}