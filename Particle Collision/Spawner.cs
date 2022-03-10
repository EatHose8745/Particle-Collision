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
        public Point Location { get; set; }
        public bool Infectious { get; set; }
        public double Offset { get; set; }
        public double Angle { get; set; }
        public int Frequency { get; set; }
        public int Health { get; set; }

        public Spawner(int x, int y, bool infectious, double offset = 0, double angle = 2 * Math.PI, int frequency = 10, int health = 100)
        {
            this.Location = new Point(x, y);
            this.Infectious = infectious;
            this.Offset = offset;
            this.Angle = angle;
            this.Frequency = frequency;
            this.Health = health;
        }

        public string getSpawnerInfoForSaving()
        {
            string info = "";
            info = Convert.ToString(Location.X) + "*" + Convert.ToString(Location.Y) + "*" + Infectious.ToString() + "*" + Offset.ToString() + "*" + Angle.ToString() + "*" + Frequency.ToString() + "*" + Health.ToString();
            return info;
        }

        public Spawner(string info)//Create a spawner from a string
        {
            string[] infoArray = info.Split('*');
            this.Location = new Point(Convert.ToInt32(infoArray[0]), Convert.ToInt32(infoArray[1]));
            this.Infectious = infoArray[2] == "True" ? true : false;
            this.Offset = Convert.ToDouble(infoArray[3]);
            this.Angle = Convert.ToDouble(infoArray[4]);
            this.Frequency = Convert.ToInt32(infoArray[5]);
            this.Health = Convert.ToInt32(infoArray[6]);
        }
    }
}
