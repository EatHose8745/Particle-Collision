using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Particle_Collision
{
    public class Vector2D
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Absolute
        {
            get
            {
                return Abs(this);
            }
            set
            {
                double angle = this.Angle;
                this.X = value * Math.Cos(angle);
                this.Y = value * Math.Sin(angle);
            }
        }
        public double Angle
        {
            get
            {
                return Arg(this);
            }
            set
            {
                double abs = this.Absolute;
                this.X = abs * Math.Cos(value);
                this.Y = abs * Math.Sin(value);
            }
        }

        public static int Dimensions { get { return 2; } }

        public Vector2D(double x = double.PositiveInfinity, double y = double.PositiveInfinity)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2D FromSpeedAngle(double speed, double angle)
        {
            return new Vector2D(speed * Math.Cos(angle), speed * Math.Sin(angle));
        }

        public static double Abs(Vector2D vector)
        {
            return Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
        }

        public static double AbsSquared(Vector2D vector)
        {
            return vector.X * vector.X + vector.Y * vector.Y;
        }

        public static double Arg(Vector2D vector)
        {
            return Math.Atan2(vector.Y, vector.X);
        }

        public static double AbsoluteDifference(Vector2D a, Vector2D b)
        {
            return Abs(a - b);
        }

        public static double RelativeDirectionSign(Vector2D a, Vector2D b)
        {
            return Math.Sign(a.Absolute - b.Absolute);
        }

        public static double AbsoluteSquareDifference(Vector2D a, Vector2D b)
        {
            return AbsSquared(a - b);
        }

        public static Vector2D Closest(Vector2D a, Vector2D b, Vector2D target)
        {
            Vector2D result = AbsoluteSquareDifference(a, target) > AbsoluteDifference(b, target) ? b : a;
            return result;
        }

        public static Vector2D Normalise(Vector2D vector)
        {
            vector.Absolute = 1;
            return vector;
        }

        public static double ReturnRelativeAngle(Vector2D a, Vector2D b)
        {
            return Math.Atan2(b.Y - a.Y, b.X - a.X);
        }

        public virtual bool Equals(Vector2D vector1, Vector2D vector2)
        {
            return vector1.Absolute == vector2.Absolute && vector1.Angle == vector2.Angle;
        }

        public static Vector2D operator +(Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y);
        public static Vector2D operator +(Vector2D a, double b) => new Vector2D(a.X + b, a.Y + b);
        public static Vector2D operator +(double b, Vector2D a) => new Vector2D(a.X + b, a.Y + b);

        public static Vector2D operator -(Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
        public static Vector2D operator -(Vector2D a, double b) => new Vector2D(a.X - b, a.Y - b);
        public static Vector2D operator -(double b, Vector2D a) => new Vector2D(b - a.X, b - a.Y);

        public static double operator *(Vector2D a, Vector2D b) => a.X * b.X + a.Y * b.Y;
        public static Vector2D operator *(Vector2D a, double b) => new Vector2D(a.X * b, a.Y * b);
        public static Vector2D operator *(double b, Vector2D a) => new Vector2D(a.X * b, a.Y * b);

        public static Vector2D operator /(Vector2D a, Vector2D b) => new Vector2D(a.X / b.X, a.Y / b.Y);
        public static Vector2D operator /(double b, Vector2D a) => new Vector2D(b / a.X, b / a.Y);
        public static Vector2D operator /(Vector2D a, double b) => new Vector2D(a.X / b, a.Y / b);
    }
}