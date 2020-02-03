using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory
{
    public interface IUpdateable
    {
        void Update(GameTime gameTime);
    }
}
