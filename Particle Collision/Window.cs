using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Particle_Collision
{
    public class Window : Border
    {
        public bool Closed { get; set; }
        public double PullMultiple { get; set; }

        public Window(int x, int y, int width, int height, double pullMultiple, Color c, bool closed = false) : base(x, y, width, height, c, closed)
        {
            this.Closed = closed;
            this.PullMultiple = pullMultiple;
        }

        public void CloseToggle() => this.Closed = !this.Closed;
    }
}
