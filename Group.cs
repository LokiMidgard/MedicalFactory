using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using System.Collections.Generic;

namespace MedicalFactory
{
    public class Group : List<GameObject>, GameObject
    {
        public virtual void LoadContent(ContentManager Content)
        {
            foreach(var gameObject in this) {
                gameObject.LoadContent(Content);
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            foreach(var gameObject in this) {
                gameObject.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var gameObject in this)
            {
                gameObject.Draw(spriteBatch, gameTime);
            }
        }
    }
}