using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;

namespace MedicalFactory
{
    public class Sprite : GameObject
    {
        private Vector2 Position;
        private Texture2D Texture;

        public void LoadContent(ContentManager Content)
        {
            Texture = Content.Load<Texture2D>("stick");
        }
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White);
        }
    }
}