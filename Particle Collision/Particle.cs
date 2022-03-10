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
        public Vector2D Location { get; set; }
        public Vector2D Velocity { get; set; }
        public double GravitationalMultiple { get; set; }
        public double Radius { get; set; }
        public double Mass { get; set; }
        public double Hardness { get; set; }
        public Color Colour { get { return Infected ? Color.Red : Color.Blue; } }
        public bool IsStationary { get; set; }
        public bool Infected { get; set; }
        public Vector2D FutureLocationVector { get { return Update(applyExisting: false).Location; } }

        public Particle(Vector2D location, Vector2D velocity, double gravitationalMultiple, double radius, double mass, double hardness, bool infected = false, bool isStationary = false)
        {
            this.Location = location;
            this.Velocity = velocity;
            this.GravitationalMultiple = gravitationalMultiple;
            this.Radius = radius;
            this.Mass = mass;
            this.Hardness = hardness;
            this.Infected = infected;
            this.IsStationary = isStationary;
        }

        public Particle Update(double gravity = 0, bool applyExisting = true)
        {
            if (applyExisting)
            {
                this.Location += this.Velocity;
                this.Velocity.Y += gravity;
                return null;
            }
            return new Particle(this.Location + this.Velocity, new Vector2D(this.Velocity.X, this.Velocity.Y + gravity), this.GravitationalMultiple, this.Radius, this.Mass, this.Hardness, this.IsStationary);
        }
    }
}