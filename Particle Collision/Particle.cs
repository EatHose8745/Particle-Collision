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
        public double PullAcceleration { get; set; }
        public double Radius { get; set; }
        public double Mass { get; set; }
        public double Hardness { get; set; }
        public Color Colour { get; set; }
        public bool IsStationary { get; set; }
        public Particle FutureParticle { get { return Update(apply: false); } }

        public Particle(Vector2 location, Vector2 velocity, double pullAcceleration, double radius, double mass, double hardness, Color colour, bool isStationary = false)
        {
            this.Location = location;
            this.Velocity = velocity;
            this.PullAcceleration = pullAcceleration;
            this.Radius = radius;
            this.Mass = mass;
            this.Hardness = hardness;
            this.Colour = colour;
            this.IsStationary = isStationary;
        }

        public Particle Update(double gravity = 0, bool apply = true)
        {
            if (apply)
            {
                this.Location += this.Velocity;
                this.Velocity.Y += gravity;
                return null;
            }
            return new Particle(this.Location + this.Velocity, new Vector2(this.Velocity.X, this.Velocity.Y + gravity), this.PullAcceleration, this.Radius, this.Mass, this.Hardness, this.Colour, this.IsStationary);
        }
    }
}