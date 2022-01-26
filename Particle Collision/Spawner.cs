using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Particle_Collision
{
    public class Spawner
    {
        public Vector2D Location { get; set; }
        public Color Colour { get; set; }
        public double Offset { get; set; }
        public double Angle { get; set; }
        public int Frequency { get; set; }

        public Spawner(int x, int y, Color c, double offset = 0, double angle = 2 * Math.PI, int frequency = 200)
        {
            this.Location = new Vector2D(x, y);
            this.Colour = c;
            this.Offset = offset;
            this.Angle = angle;
            this.Frequency = frequency;
        }
    }
}
