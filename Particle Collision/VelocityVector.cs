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
        public double Absolute { get { { return Abs(this); } } }
        public double Angle { get { return Arg(this); } set { double abs = this.Absolute; this.X = abs * Math.Cos(value); this.Y = abs * Math.Sin(value); } }

        public Vector2(double x = 0, double y = 0)
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

        public static double AbsPow2(Vector2 vector)
        {
            return vector.X * vector.X + vector.Y * vector.Y;
        }

        public static double Arg(Vector2 vector)
        {
            return Math.Atan2(vector.Y, vector.X);
        }

        public static double AbsoluteDifference(Vector2 a, Vector2 b)
        {
            return Abs(a - b);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator +(Vector2 a, double b) => new Vector2(a.X + b, a.Y + b);
        public static Vector2 operator +(double b, Vector2 a) => new Vector2(a.X + b, a.Y + b);

        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator -(Vector2 a, double b) => new Vector2(a.X - b, a.Y - b);
        public static Vector2 operator -(double b, Vector2 a) => new Vector2(b - a.X, b - a.Y);

        public static double operator *(Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;
        public static Vector2 operator *(Vector2 a, double b) => new Vector2(a.X * b, a.Y * b);
        public static Vector2 operator *(double b, Vector2 a) => new Vector2(a.X * b, a.Y * b);

        public static Vector2 operator /(Vector2 a, Vector2 b) => new Vector2(a.X / b.X, a.Y / b.Y);
        public static Vector2 operator /(double b, Vector2 a) => new Vector2(b / a.X, b / a.Y);
        public static Vector2 operator /(Vector2 a, double b) => new Vector2(a.X / b, a.Y / b);
    }
}