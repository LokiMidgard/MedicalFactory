using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class Recycler : Sprite
    {

        Dictionary<BodyPart.BodyPartType, List<BodyPartDispenser>> dispensers = new Dictionary<BodyPart.BodyPartType, List<BodyPartDispenser>>();

        public Recycler() : base("Recyclingtonne")
        {

        }

        public void AddDispenser(BodyPartDispenser dispenser)
        {
            DispenserType type = dispenser.type;
            BodyPart.BodyPartType bodyPartType;
            switch(type) {
                case DispenserType.Herzgerät: bodyPartType = BodyPart.BodyPartType.HERZ; break;
                case DispenserType.Lungengerät: bodyPartType = BodyPart.BodyPartType.LUNGE; break;
                case DispenserType.Nierengerät: bodyPartType = BodyPart.BodyPartType.NIERE; break;
                default: bodyPartType = BodyPart.BodyPartType.HERZ; break;
            }
            if (!dispensers.ContainsKey(bodyPartType)) {
                dispensers[bodyPartType] = new List<BodyPartDispenser>();
            }
            dispensers[bodyPartType].Add(dispenser);
        }

        public void PutStuffInside(BodyPart bodyPart)
        {
            List<BodyPartDispenser> dispenser = dispensers[bodyPart.Type];
            if (dispenser != null) {
                int Count = dispenser.Count;
                if (Count > 0) {
                    int Selection = MyMathHelper.Random.Next()%Count;
                    bodyPart.IsDamaged = false;
                    dispenser[Selection].Attach(bodyPart);
                }
            }
        }
    }
}
