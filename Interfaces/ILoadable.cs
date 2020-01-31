using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace MedicalFactory
{
    public interface ILoadable
    {
        void LoadContent(ContentManager Content);
    }
}
