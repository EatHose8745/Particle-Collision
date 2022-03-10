using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;


namespace Particle_Collision
{
    public class Border
    {
        public Point Start { get; set; }
        public Point End { get; set; }

        [JsonIgnore]
        public int Width 
        { 
            get 
            {
                return (int)Math.Abs(Start.X - End.X);
            }
        }

        [JsonIgnore]
        public int Height
        { 
            get
            {
                return (int)Math.Abs(Start.Y - End.Y);
            }
        }

        public int Mass
        {
            get { return 1; }
        }
        public int Hardness
        {
            get;
            set;
        }
        [JsonIgnore]
        public Vector2D Location
        {
            
            get
            {
                return new Vector2D((int)(Math.Min(Start.X, End.X) + (Width / 2)), (int)(Math.Min(Start.Y, End.Y) + (Height / 2)));

            }
        }
        public string GetBorderInfoForSaving()
        {
            
            string info = "";
            info = Convert.ToString(Start.X) + "*" + Convert.ToString(Start.Y) + "*" + Convert.ToString(End.X) +"*" + Convert.ToString(End.Y) +"*" + Filled.ToString() + "*" + Colour.ToString();


            return info;
        }
        
        public bool Filled { get; set; }
        public Color Colour { get; set; }

        public Border(int x, int y, int w, int h, Color c, bool filled = false)
        {
            this.Start = new Point(x, y);
            this.End = new Point(x + w, y + h);
            this.Filled = filled;
            this.Colour = c;
        
        }
        public Border(string info)//Create a border from a string
        {
            string[] infoArray = info.Split('*');
            this.Start = new Point(Convert.ToInt32(infoArray[0]), Convert.ToInt32(infoArray[1]));
            this.End = new Point(Convert.ToInt32(infoArray[2]), Convert.ToInt32(infoArray[3]));
            this.Filled = Convert.ToBoolean(infoArray[4]);
            this.Colour = infoArray[5] == "Color [Red]" ? Color.Red : Color.Blue;
        }
    }
}
