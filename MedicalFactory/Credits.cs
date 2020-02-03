using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                using var stream = typeof(Credits).Assembly.GetManifestResourceStream("MedicalFactory.README.md");
                using var reader = new StreamReader(stream);
                Text = reader.ReadToEnd();


            }
            catch (Exception)
            {

            }

            if (string.IsNullOrEmpty(Text))
            {
                try
                {
                    // Fallback
                    Text = File.ReadAllText("./README.md", Encoding.UTF8);
                }
                catch (Exception)
                {
                }
            }

            var end = Text.IndexOf("---");
            if (end != -1)
                Text = Text.Substring(0, end);


        }
    }
}
