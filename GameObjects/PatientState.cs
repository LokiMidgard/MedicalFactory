using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MedicalFactory.GameObjects
{
    public class PatientState
    {
        private readonly Patient patient;

        private IEnumerable<Organ> Organs => patient?.Attached.Where(a => a is Organ).Cast<Organ>();

        public PatientState(Patient parent)
        {
            patient = parent;
        }

        public void AddOrgan(Organ organ)
        {
            patient.Attach(organ);
        }

        public void TryRemoveOrgan(OrganType type, ICanCarray newOwner)
        {
            var matching = Organs.Where(o => o.GetType().Name == type.ToString());
            if (matching.Count() > 0)
            {
                newOwner.Attach(matching.First());
            }
        }
    }
}
