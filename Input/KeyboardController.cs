using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XnaInput = Microsoft.Xna.Framework.Input;

namespace PaToRo_Desktop.Engine.Input
{
    public class KeyboardController : InputProvider
    {
        public const int SupportedNumber = 2;
        private int index;
        private XnaInput.KeyboardState st;
        private XnaInput.KeyboardState lastState;


        public KeyboardController(int index)
        {
            if (index >= SupportedNumber)
                throw new ArgumentOutOfRangeException($"Index for Keybord was {index} but maximum is {SupportedNumber - 1}");
            this.index = index;
        }

        public override void Update(GameTime gameTime)
        {
            this.lastState = this.st;
            this.st = XnaInput.Keyboard.GetState();
        }

        private bool GetInternal0(XnaInput.KeyboardState st, Buttons btn)
        {
            return btn switch
            {
                Buttons.A => st.IsKeyDown(XnaInput.Keys.Q),
                Buttons.B => st.IsKeyDown(XnaInput.Keys.E),
                Buttons.Start => st.IsKeyDown(XnaInput.Keys.Enter),
                _ => false,
            };
        }
        private bool GetInternal1(XnaInput.KeyboardState st, Buttons btn)
        {
            return btn switch
            {
                Buttons.A => st.IsKeyDown(XnaInput.Keys.NumPad4),
                Buttons.B => st.IsKeyDown(XnaInput.Keys.NumPad6),
                Buttons.Start => st.IsKeyDown(XnaInput.Keys.NumPad0),
                _ => false,
            };
        }

        public override bool Get(Buttons btn)
        {

            return this.index switch
            {
                0 => this.GetInternal0(this.st, btn),
                1 => this.GetInternal1(this.st, btn),
                _ => false
            };
        }
        public override bool GetLast(Buttons btn)
        {
            return this.index switch
            {
                0 => this.GetInternal0(this.lastState, btn),
                1 => this.GetInternal1(this.lastState, btn),
                _ => false
            };
        }

        private float XAxis0()
        {
            var left = this.st.IsKeyDown(XnaInput.Keys.A) ? -1 : 0;
            var right = this.st.IsKeyDown(XnaInput.Keys.D) ? 1 : 0;
            return left + right;
        }

        private float YAxis0()
        {
            var up = this.st.IsKeyDown(XnaInput.Keys.W) ? -1 : 0;
            var down = this.st.IsKeyDown(XnaInput.Keys.S) ? 1 : 0;
            return up + down;
        }

        private float XAxis1()
        {
            var left = this.st.IsKeyDown(XnaInput.Keys.NumPad1) ? -1 : 0;
            var right = this.st.IsKeyDown(XnaInput.Keys.NumPad3) ? 1 : 0;
            return left + right;
        }

        private float YAxis1()
        {
            var up = this.st.IsKeyDown(XnaInput.Keys.NumPad5) ? -1 : 0;
            var down = this.st.IsKeyDown(XnaInput.Keys.NumPad2) ? 1 : 0;
            return up + down;
        }

        public override float Get(Sliders sldr)
        {
            return this.index switch
            {
                0 => this.Get0(sldr),
                1 => this.Get1(sldr),
                _ => 0f
            };
        }
        private float Get1(Sliders sldr)
        {
            return sldr switch
            {
                Sliders.LeftStickX => this.XAxis1(),
                Sliders.LeftStickY => this.YAxis1(),
                _ => 0f
            };
        }
        private float Get0(Sliders sldr)
        {
            return sldr switch
            {
                Sliders.LeftStickX => this.XAxis0(),
                Sliders.LeftStickY => this.YAxis0(),
                _ => 0f
            };
        }


        public override void Rumble(float low, float high, int ms)
        {
        }
    }
}
