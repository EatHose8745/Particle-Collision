using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_Collision
{
    public class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2(double speed, double angle, bool usingSpeed)
        {
            this.X = speed * Math.Cos(angle);
            this.Y = speed * Math.Sin(angle);
        }

        public static double Abs(Vector2 vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);

        public static Vector2 operator *(Vector2 a, Vector2 b) => new Vector2(a.X * b.X, a.Y * b.Y);
        public static Vector2 operator *(Vector2 a, double b) => new Vector2(a.X * b, a.Y * b);
    }
}
