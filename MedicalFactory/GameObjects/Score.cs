using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MedicalFactory.GameObjects
{

    public class Score
    {
        public readonly Patient patient;
        public List<string> Text = new List<String>();

        public Score(Patient patient)
        {
            this.patient = patient;

            if (patient.State.IsDead)
            {
                Text.Add($"Today you lost {patient.PatientName}");
                Text.Add($"{patient.PatientName} leaves {patient.NumberOfChildren} children behind.");
            } else if(patient.State.IsCompletelyHealed)
            {
                Text.Add($"Today you completely healed {patient.PatientName}.");
            } else
            {
                Text.Add($"Today you somehow kept {patient.PatientName} alive.");
            }
        }
    }

}
