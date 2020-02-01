using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using System;

namespace MedicalFactory
{
    public class ParticleSystem : IUpdateable, IDrawable, ILoadable
    {
        private const int MaxParticles = 1000;
        private readonly string textureName;
        private Texture2D texture;
        public Vector2 Origin { get; set; }
        public TimeSpan SpawnRate { get; set; }
        public bool IsEnabled { get; set; }

        private int startIndex;
        private int active;

        private readonly TimeSpan maxAge;

        ///<Summary>        
        /// to safe the spawn between updates
        ///</Summary>        
        private TimeSpan spawnBackpack;


        public Vector2 Position { get; set; }
        public Vector2 Velocety { get; set; }


        private Vector2[] positions;
        private Vector2[] velocetys;
        private TimeSpan[] createionTime;
        private bool[] actives;




        public ParticleSystem(TimeSpan maxAge, string texture)
        {
            this.textureName = texture;
            positions = new Vector2[MaxParticles];
            velocetys = new Vector2[MaxParticles];
            createionTime = new TimeSpan[MaxParticles];
            actives = new bool[MaxParticles];
            this.maxAge = maxAge;
        }




        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < MaxParticles; i++)
            {
                if (createionTime[i] != default && gameTime.TotalGameTime - createionTime[i] < maxAge)
                {
                    spriteBatch.Draw(texture, positions[i], origin: this.Origin);
                }
            }

        }

        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>(textureName);
        }


        private void Spawn(GameTime gameTime)
        {
            if (active < MaxParticles)
            {

                while (createionTime[startIndex] != default && gameTime.TotalGameTime - createionTime[startIndex] < maxAge)
                    startIndex++;

                positions[startIndex] = Position;
                velocetys[startIndex] = Velocety;
                createionTime[startIndex] = gameTime.TotalGameTime;
                actives[startIndex] = true;
                active++;
            }
        }


        public void Update(GameTime gameTime)
        {

            if (IsEnabled)
            {
                spawnBackpack += gameTime.ElapsedGameTime;
                while (spawnBackpack >= this.SpawnRate)
                {
                    spawnBackpack -= this.SpawnRate;
                    this.Spawn(gameTime);
                }

            }

            for (int i = 0; i < MaxParticles; i++)
            {
                positions[i] += velocetys[i];
                if (actives[i] && (gameTime.TotalGameTime - createionTime[startIndex] >= maxAge))
                {
                    actives[i] = false;
                    active--;
                }
            }

        }
    }

}