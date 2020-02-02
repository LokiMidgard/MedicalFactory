using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using PaToRo_Desktop.Engine.Input;
using System.Linq;
using MedicalFactory.GameObjects;
using System;

namespace MedicalFactory
{
    public class CollisionManager
    {
        public static List<Collision> GetCollisions(Vector2 Position, float Radius, Group group)
        {
            List<Collision> Collisions = new List<Collision>();
            for (var i = 0; i < group.Count; ++i)
            {
                var otherSprite = group[i] as Sprite;
                if (otherSprite != null && otherSprite.Visible)
                {
                    Vector2 FromTo = otherSprite.Position - Position;
                    if (FromTo.Length() < Radius + otherSprite.Radius)
                    {
                        Collision collisionAB = new Collision();
                        collisionAB.Distance = (FromTo.Length() - Radius - otherSprite.Radius) * Vector2.Normalize(FromTo);
                        collisionAB.spriteA = null;
                        collisionAB.spriteB = otherSprite;
                        Collisions.Add(collisionAB);
                    }
                }
            }
            return Collisions;
        }
        public static List<Collision> GetCollisions(Sprite sprite, Group group)
        {
            List<Collision> Collisions = new List<Collision>();
            var thisSprite = sprite;
            if (thisSprite != null)
            {
                for (var i = 0; i < group.Count; ++i)
                {
                    var otherSprite = group[i] as Sprite;
                    if (otherSprite != null && otherSprite != sprite && sprite.Visible && otherSprite.Visible)
                    {
                        Vector2 FromTo = otherSprite.Position - thisSprite.Position;
                        if (FromTo.Length() < thisSprite.Radius + otherSprite.Radius)
                        {
                            Collision collisionAB = new Collision();
                            collisionAB.Distance = (FromTo.Length() - thisSprite.Radius - otherSprite.Radius) * Vector2.Normalize(FromTo);
                            collisionAB.spriteA = thisSprite;
                            collisionAB.spriteB = otherSprite;
                            Collisions.Add(collisionAB);
                        }
                    }
                }
            }
            return Collisions;
        }

        public static List<Collision> GetCollisions(Group group)
        {
            List<Collision> Collisions = new List<Collision>();
            for(var i = 0; i<group.Count; ++i)
            {
                var thisSprite = group[i] as Sprite;
                if (thisSprite != null) {
                    for(var j = i+1; j<group.Count; ++j)
                    {
                        var otherSprite = group[j] as Sprite;
                        if (otherSprite != null && thisSprite.Visible && otherSprite.Visible)
                        {
                            Vector2 FromTo = otherSprite.Position - thisSprite.Position;
                            if (FromTo.Length() < thisSprite.Radius + otherSprite.Radius)
                            {
                                Collision collisionAB = new Collision();
                                collisionAB.Distance = (FromTo.Length() - thisSprite.Radius - otherSprite.Radius) * Vector2.Normalize(FromTo);
                                collisionAB.spriteA = thisSprite;
                                collisionAB.spriteB = otherSprite;
                                Collisions.Add(collisionAB);

                                // TODO: not sure if we need this
                                Collision collisionBA = new Collision();
                                collisionBA.Distance = -collisionAB.Distance;
                                collisionBA.spriteA = otherSprite;
                                collisionBA.spriteB = thisSprite;
                                Collisions.Add(collisionBA);
                            }
                        }
                    }
                }
            }
            return Collisions;
        }

        public static void KeepInWorld(Sprite sprite, Action<Recycler> OnRecyclerHit = null)
        {
            // Machine Collision
            IEnumerable<Collision> objColls = CollisionManager.GetCollisions(sprite, Game1.conveyerBelt);
            objColls = objColls.Where(c => (c.spriteB is BodyPartDispenser || c.spriteB is Recycler));
            var recycler = objColls.Where(c => c.spriteB is Recycler).FirstOrDefault();
            if (recycler != null && OnRecyclerHit != null)
            {
                OnRecyclerHit?.Invoke(recycler.spriteB as Recycler);
            }
            else if (objColls.Count() > 0)
            {
                var coll = objColls.First();
                sprite.Position += coll.Distance;
            }

            // Restrict Position
            var yMin = 64f;
            var yMax = 1028f - 16;
            if (GameConfig.KeepPlayersToTheirSide && sprite is Robot robot)
            {
                var isUpperHalf = ((int)(robot.PlayerColor) % 2) == 0;
                yMin = isUpperHalf ? 64 : Game1.conveyerBelt.YPos + 64;
                yMax = isUpperHalf ? Game1.conveyerBelt.YPos - 64 : 1028 - 16;
            }

            sprite.Position = new Vector2(
                MathHelper.Clamp(sprite.Position.X, 32, 1920 - 100),
                MathHelper.Clamp(sprite.Position.Y, yMin, yMax)
            );
        }
    }
}