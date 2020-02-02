using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using PaToRo_Desktop.Engine.Input;
using MedicalFactory.GameObjects;
using System;
using System.Linq;

namespace MedicalFactory
{
    public class PatientFactory
    {
        double Timer = 2.0;
        Patient LastPatient;
        float NextSpawnDistance;

        public int PatientsLeft;
        public int PatientCount = 15;
        Texture2D[] HumanTextures;
        Texture2D[] AlienTextures;
        
        static string bigNameList = "Frank Thorsten Norbert Susi Andrea Carl Dracula Tim Peter James Jordan Lissy Tom Jenny Karla Sahra Brett Harold Kumar Prince Manfred";
        static string[] Names = bigNameList.Split(" ");
        static string GetRandomName()
        {
            int idx = MyMathHelper.Random.Next() % Names.Count();
            return Names[idx];
        }

        public PatientFactory()
        {
        }

        public void Start()
        {
            PatientsLeft = PatientCount;
            LastPatient = null;
        }

        public void LoadContent(Game1 game)
        {
            this.HumanTextures = new Texture2D[]{
                game.Content.Load<Texture2D>("Mensch"),
                game.Content.Load<Texture2D>("ToterMensch"),
                game.Content.Load<Texture2D>("GluecklicherMensch")
            };
            this.AlienTextures = new Texture2D[]{
                game.Content.Load<Texture2D>("Alien"),
                game.Content.Load<Texture2D>("TotesAlien"),
                game.Content.Load<Texture2D>("GluecklichesAlien")
            };
            BodyPart.LoadContent(game.Content);
        }

        private Patient SpawnPatient()
        {
                bool IsHuman = MyMathHelper.Random.NextDouble() < 0.6f;
                Patient patient;
                if (IsHuman)
                {
                    patient = new HumanPatient(this.HumanTextures);
                }
                else
                {
                    patient = new AlienPatient(this.AlienTextures);
                }
                patient.PatientName = GetRandomName();
                int MaxChildren = 7;
                patient.NumberOfChildren = MyMathHelper.Random.Next() % MaxChildren;
                patient.Married = MyMathHelper.Random.NextDouble() < 0.5;
                if (patient.Married)
                {
                    patient.SpouseName = GetRandomName();
                }
                int MaxAge = 120;
                patient.Age = MyMathHelper.Random.Next() % MaxAge;
                patient.LifeExpectancy = MaxAge - patient.Age + (MyMathHelper.Random.Next() % 20) - 10;
                if (patient.LifeExpectancy <= 0) patient.LifeExpectancy = 0;


                var defectOrgans = MyMathHelper.Random.Next(1, patient.Attached.Count + 1);

                foreach (var item in patient.Attached.OrderBy(x => MyMathHelper.Random.NextDouble()).Take(defectOrgans).OfType<BodyPart>())
                    item.IsDamaged = true;


                patient.Position = new Vector2(-420.0f, 540.0f + 30.0f);
                patient.Rotation = MathHelper.PiOver2;
                Game1.conveyerBelt.Add(patient);
                this.Timer = MyMathHelper.Random.NextDouble() * 15.0f + 8.0f;
                
                PatientsLeft -= 1;

                // next patient
                NextSpawnDistance = MyMathHelper.Random.Next()%500 + 90;
                return patient;
        }

        public void Update(GameTime gameTime)
        {
            if (LastPatient == null) {
                LastPatient = SpawnPatient();
            } else if (PatientsLeft > 0 && LastPatient.Position.X > NextSpawnDistance) {
                LastPatient = SpawnPatient();
            }

            this.Timer -= gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}