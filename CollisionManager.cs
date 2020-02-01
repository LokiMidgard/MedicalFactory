using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using PaToRo_Desktop.Engine.Input;

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
                if (otherSprite != null)
                {
                    Vector2 FromTo = otherSprite.Position - Position;
                    if (FromTo.Length() < Radius + otherSprite.Radius)
                    {
                        Collision collisionAB = new Collision();
                        collisionAB.Direction = FromTo * (FromTo.Length() - Radius - otherSprite.Radius);
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
                    if (otherSprite != null)
                    {
                        Vector2 FromTo = otherSprite.Position - thisSprite.Position;
                        if (FromTo.Length() < thisSprite.Radius + otherSprite.Radius)
                        {
                            Collision collisionAB = new Collision();
                            collisionAB.Direction = FromTo * (FromTo.Length() - thisSprite.Radius - otherSprite.Radius);
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
                        if (otherSprite != null)
                        {
                            Vector2 FromTo = otherSprite.Position - thisSprite.Position;
                            if (FromTo.Length() < thisSprite.Radius + otherSprite.Radius)
                            {
                                Collision collisionAB = new Collision();
                                collisionAB.Direction = FromTo * (FromTo.Length() - thisSprite.Radius - otherSprite.Radius);
                                collisionAB.spriteA = thisSprite;
                                collisionAB.spriteB = otherSprite;
                                Collisions.Add(collisionAB);

                                // TODO: not sure if we need this
                                Collision collisionBA = new Collision();
                                collisionBA.Direction = -collisionAB.Direction;
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
    }
}