using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using System.Collections.Generic;

namespace MedicalFactory
{
    public class Group : List<IGameObject>, IGameObject, ILoadable, IDrawable, IUpdateable
    {
        public virtual bool Visible { get; set; } = true;
        public virtual bool Enabled { get; set; } = true;

        public virtual void LoadContent(Game1 game)
        {
            foreach (var gameObject in this)
            {
                if (gameObject is ILoadable go)
                    go.LoadContent(game);
            }
        }
        public virtual void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                foreach (var gameObject in this)
                {
                    if (gameObject is IUpdateable go)
                        go.Update(gameTime);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Visible)
            {
                foreach (var gameObject in this)
                {
                    if (gameObject is IDrawable go)
                        go.Draw(spriteBatch, gameTime);
                }
            }
        }
    }
}