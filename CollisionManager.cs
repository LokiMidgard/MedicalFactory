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
        public static List<Collision> GetCollisions(Group group)
        {
            List<Collision> Collisions = new List<Collision>();
            foreach(GameObject thisgo in group) {
                if (thisgo is Sprite) {
                    Sprite thisSprite = thisgo as Sprite;
                    foreach (GameObject othergo in group)
                    {
                        if (othergo is Sprite)
                        {
                            if (othergo == thisgo) continue;
                            Sprite otherSprite = othergo as Sprite;
                            Vector2 FromTo = otherSprite.Position - thisSprite.Position;
                            if (FromTo.Length() < thisSprite.Radius + otherSprite.Radius)
                            {
                                Collision collision = new Collision();
                                collision.Direction = FromTo * (FromTo.Length() - thisSprite.Radius - otherSprite.Radius);
                                collision.spriteA = thisSprite;
                                collision.spriteB = otherSprite;
                                Collisions.Add(collision);
                            }
                        }
                    }
                }
            }
            return Collisions;
        }
    }
}