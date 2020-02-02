using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MedicalFactory
{
    public static class Credits
    {
        public static string Text = "";

        public static void LoadContent(Game1 game)
        {
            try
            {
                Text = File.ReadAllText("./README.md");
            } catch(Exception exc)
            {

            }
        }
    }
}
