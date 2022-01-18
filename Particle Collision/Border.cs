using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Particle_Collision
{
    public class Border
    {
        public Vector2D Start;
        public Vector2D End;

        public int Width 
        { 
            get 
            {
                return (int)Math.Abs(Start.X - End.X);
            }
        }
        public int Height 
        { 
            get
            {
                return (int)Math.Abs(Start.Y - End.Y);
            }
        }

        public double Hardness { get; set; }
        public double Mass { get; set; }

        public Vector2D Location
        {
            get
            {
                return new Vector2D((int)(Math.Min(Start.X, End.X) + (Width / 2)), (int)(Math.Min(Start.Y, End.Y) + (Height / 2)));
            }
        }

        public bool Filled { get; set; }
        public Color Colour { get; set; }

        public Border(int x, int y, int w, int h, Color c, bool filled = false)
        {
            this.Start = new Vector2D(x, y);
            this.End = new Vector2D(x + w, y + h);
            this.Colour = c;
            this.Filled = filled;
        }
    }
}
