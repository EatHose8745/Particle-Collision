using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Particle_Collision
{
    public class Particle
    {
        public Vector2 Location { get; set; }
        public Vector2 Velocity { get; set; }
        public double Radius { get; set; }
        public double Mass { get; set; }
        public double Hardness { get; set; }
        public Color Colour { get; set; } 

        public Particle(Vector2 location, Vector2 velocity, double radius, double mass, double hardness, Color colour)
        {
            this.Location = location;
            this.Velocity = velocity;
            this.Radius = radius;
            this.Mass = mass;
            this.Hardness = hardness;
            this.Colour = colour;
        }

        public void Update(double gravity)
        {
            this.Velocity.Y += gravity;

            this.Location += this.Velocity;
        }
    }
}
