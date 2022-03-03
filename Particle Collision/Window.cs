using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

namespace Particle_Collision
{
    public class Window : Border
    {
        public bool Closed { get; set; }
        public double PullMultiple { get; set; }
        public double Range { get; set; }

        [JsonIgnore]
        public int LargestDimention
        {
            get { return Math.Max(Width, Height); }
        }

        public Window(int x, int y, int width, int height, double pullMultiple, Color c, bool closed = false) : base(x, y, width, height, c, closed)
        {
            this.Closed = closed;
            this.PullMultiple = pullMultiple;
        }

        public void CloseToggle() => this.Closed = !this.Closed;

        public string getWindowInfoForSaving()
        {
            string info = "";
            info = Convert.ToString(Start.X) + "*" + Convert.ToString(Start.Y) + "*" + Convert.ToString(End.X) + "*" + Convert.ToString(End.Y) + "*" + Filled.ToString() + "*" + Colour.ToString() + "*" + Convert.ToString(Closed) + "*" + Convert.ToString(PullMultiple) + "*" + Convert.ToString(Range);
            return info;
        }

        public Window(string info) : base(info)//Create a window from a string
        {
            string[] infoArray = info.Split('*');
            this.Closed = Convert.ToBoolean(infoArray[6]);
            this.PullMultiple = Convert.ToDouble(infoArray[7]);
            this.Range = Convert.ToDouble(infoArray[8]);
        }
    }
}
