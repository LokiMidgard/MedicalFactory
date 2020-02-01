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

        public bool IsDead
        {
            get
            {
                foreach (var type in Enum.GetValues(typeof(BodyPart.BodyPartType)).OfType<BodyPart.BodyPartType>())
                {
                    if (this.patient.MaximumBodyParts(type) > 0)
                        if (!this.BodyParts.Any(x => x.Type == type))
                            return true;
                }
                return false;
            }
        }

        public (int actual, int maximum) CalculateScore()
        {
            var sum = 0;
            var maximum = 0;
            foreach (var type in Enum.GetValues(typeof(BodyPart.BodyPartType)).OfType<BodyPart.BodyPartType>())
            {
                maximum += 3 * this.patient.MaximumBodyParts(type);
                sum += this.BodyParts.Where(x => x.Type == type).Select(x => x.IsDemaged ? 1 : 3).Sum();
            }
            if (this.IsDead)
                sum = 0;
            return (sum, maximum);
        }


    }
}
