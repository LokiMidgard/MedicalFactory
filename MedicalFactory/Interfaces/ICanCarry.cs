using Microsoft.Xna.Framework;

namespace MedicalFactory
{
    public interface ICanCarry
    {
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        System.Collections.ObjectModel.ReadOnlyCollection<IAttachable> Attached { get; }
        void Attach(IAttachable add);
        void Detach(IAttachable add);
    }
}