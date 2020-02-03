using Microsoft.Xna.Framework;

namespace MedicalFactory
{
    public interface IAttachable
    {
        ICanCarry AttachedTo { get; /*set;*/}
        Vector2 AttachOffset { get; set; }

        void OnAttachChanged();
    }

    public interface IOnlyUseMeIfYouKnowWhatYouAreDoingWithAttachables
    {
        ICanCarry AttachedTo { set; }
    }
}