using Microsoft.Xna.Framework;

namespace MedicalFactory.GameObjects
{
    public enum PlayerColor
    {
        Roboter_Blau, Roboter_Gruen, Roboter_Gelb, Roboter_Rot
    }
    public class Robot : Sprite
    {

        public Robot(PlayerColor color) : base(color.ToString())
        {
            this.Origin = new Vector2(30.0f, 90.0f);
        }
    }
}
