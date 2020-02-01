using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory.GameObjects
{
    public class Organ : Sprite
    {
        public Organ(OrganType type) : base(type.ToString())
        {

        }
    }
}
