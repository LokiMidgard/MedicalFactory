using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class Background : Sprite
    {
        private Random rng = new Random();
        private Texture2D[] bloodSplash;
        private Texture2D[] slimeSplash;

        private Group Decals = new Group();

        public Background(Screen screen) : base("Hintergrund")
        {
            this.Position = new Microsoft.Xna.Framework.Vector2(screen.Width / 2.0f, screen.Height / 2.0f);

            this.bloodSplash = new Texture2D[3];
            this.slimeSplash = new Texture2D[3];
        }

        public override void LoadContent(Game1 game)
        {
            bloodSplash[0] = game.Content.Load<Texture2D>("Blut_Klein");
            bloodSplash[1] = game.Content.Load<Texture2D>("Blut_Mittel");
            bloodSplash[2] = game.Content.Load<Texture2D>("Blut_Gross");

            slimeSplash[0] = game.Content.Load<Texture2D>("Schleim_Klein");
            slimeSplash[1] = game.Content.Load<Texture2D>("Schleim_Mittel");
            slimeSplash[2] = game.Content.Load<Texture2D>("Schleim_Gross");

            base.LoadContent(game);

            Decals.LoadContent(game);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.Draw(spriteBatch, gameTime);
            Decals.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Decals.Update(gameTime);
        }

        public void CleanFloor()
        {
            var looseOrgans = Game1.sprites.OfType<BodyPart>().ToList();
            foreach (var pb in looseOrgans)
                Game1.sprites.Remove(pb);
            Decals.Clear();
        }

        public void AddBloodSplash(Vector2 pos, bool slime = false, bool maxSize = false)
        {
            var texBase = slime ? slimeSplash : bloodSplash;
            int idx = maxSize ? 2 : (int)(rng.NextDouble() * texBase.Length);
            var decal = new Sprite(texBase[idx]);
            decal.Position = pos;
            decal.Scale = maxSize ? Vector2.One * 0.6f : Vector2.One * (float)(rng.NextDouble() * 0.4f);
            Decals.Add(decal);
        }
        public void AddBloodSplash(Vector2 pos, bool slime, float size)
        {
            var texBase = slime ? slimeSplash : bloodSplash;
            var idx = (int)(rng.NextDouble() * texBase.Length);
            var decal = new Sprite(texBase[idx]);
            decal.Position = pos;
            decal.Scale = Vector2.One * size;
            Decals.Add(decal);
        }
    }
}
