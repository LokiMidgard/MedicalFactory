using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MedicalFactory.GameObjects
{
    public class PatientState
    {
        private readonly Patient patient;

        private IEnumerable<BodyPart> BodyParts => patient?.Attached.Where(a => a is BodyPart).Cast<BodyPart>();

        public PatientState(Patient parent)
        {
            patient = parent;
        }

        public void AddBodyPart(BodyPart organ)
        {
            patient.Attach(organ);
        }

        public void TryRemoveBodyPart(BodyPart.BodyPartType type, ICanCarray newOwner)
        {
            var matching = BodyParts.Where(o => o.GetType().Name == type.ToString());
            if (matching.Count() > 0)
            {
                newOwner.Attach(matching.First());
            }
        }
    }
}
