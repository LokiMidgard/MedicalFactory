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
        private Patient patient;
        public List<string> Text = new List<String>();

        public Score(Patient patient)
        {
            this.patient = patient;
            Text.Add("THE MOTHERFUCKER");
            Text.Add(patient.PatientName);
            Text.Add("DIED");
        }
    }

}
