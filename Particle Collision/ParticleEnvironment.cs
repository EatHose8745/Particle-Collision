using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;

namespace Particle_Collision
{
    public partial class ParticleEnvironment : Form
    {
        //KD_Node KDTree = new KD_Node();

        private List<Particle> particlesReturn = new List<Particle>();
        public List<Particle> Particles = new List<Particle>();
        
        private List<Particle> particlesWithGravity = new List<Particle>();

        string mainDirectory = Directory.GetCurrentDirectory();

        public List<Border> Borders = new List<Border>();
        public List<Window> Windows = new List<Window>();
        public List<Spawner> Spawners = new List<Spawner>();

        private int bordersIndex = 0;
        private Border nullBorder = null;

        private int windowsIndex = 0;
        private Window nullWindow = null;

        private int spawnersIndex = 0;
        private Spawner nullSpawner = null;

        private bool isDrawing = false;
        private string selectedRadio = "NoneRadio";

        private const double G = 10;

        private bool timerRunning = true;
        private int timerTicks = 0;
        private double gravity = 0;
        private double terminalSpeed = 50;
        private bool autoSetTerminalSpeed = false;
        private double wallHardness = 1;
        private double groundRoofHardness = 1;
        private int randomParticlesNumber = 10;
        private string randomSeed = "TestSeed";
        private Random random;

        public ParticleEnvironment()
        {
            InitializeComponent();
        }

        private void ParticleEnvironment_Load(object sender, EventArgs e)
        {
            InitiliseRandom(ref random, randomSeed);
            AddParticles();
            InitiliseParticlesWithGravity();
            SetTerminalVelocity();
            TimerToggleButton.PerformClick();
            DrawBox.Refresh();
        }

        void InitiliseRandom(ref Random random, string seed)
        {
            if (seed != "Random")
                random = new Random(seed.GetHashCode());
            else
                random = new Random();
        }

        void AddParticles()
        {
            //particles.Add(new Particle(new Vector2(800, 500), new Vector2(0, 0), 0, 20, 1000000, 1, Color.Green, false));
            //AppendParticle(new Particle(new Vector2D(70, 20), new Vector2D(10, 0), 0, 20, 10, 1));
            //AppendParticle(new Particle(new Vector2D(50, 40), new Vector2D(10, 0), 0, 20, 10, 1));
            //AppendParticle(new Particle(new Vector2D(90, 60), new Vector2D(10, 0), 0, 20, 10, 1));
            //AppendParticle(new Particle(new Vector2D(40, 70), new Vector2D(10, 0), 0, 20, 10, 1));
            //AppendParticle(new Particle(new Vector2D(80, 10), new Vector2D(10, 0), 0, 20, 10, 1));
            //AppendParticle(new Particle(new Vector2D(20, 30), new Vector2D(10, 0), 0, 20, 10, 1));
            //particles.Add(new Particle(new Vector2D(200, 200), new Vector2D(0, 0), 0, 20, 10, 1, Color.Blue, false, true));
            //particles.Add(new Particle(new Vector2(600, 300), new Vector2(-5, 0), 0, 20, 1, 1, Color.Red));
            //particles.Add(new Particle(new Vector2(400, 300), new Vector2(5, 0), 0, 20, 1, 1, Color.Red));
            //particles.Add(new Particle(new Vector2(0, this.Height / 2), new Vector2(0, 0), 10, 0, 0, 0, Color.Transparent, true));
        }

        private void AppendParticle(Particle p)
        {
            Particles.Add(p);
        }

        private void RemoveParticle(Particle p)
        {
            Particles.Remove(p);
        }

        void SetTerminalVelocity()
        {
            if (autoSetTerminalSpeed)
            {
                double max = 0;
                for (int i = 0; i < Particles.Count; i++)
                {
                    if (Particles[i].Radius > max)
                        max = Particles[i].Radius;
                }
                this.terminalSpeed = max;
            }
        }

        bool CheckForCollsions(Particle particle1, Particle particle2, bool apply = true)
        {
            if (CircleIntersect(particle1, particle2))
            {
                if (apply)
                {
                    CollideParticles(particle1, particle2);
                    CheckForTerminalVelocity(ref particle1);
                    CheckForTerminalVelocity(ref particle2);
                }
                return true;
            }
            return false;
        }

        double ReturnRelativeParticleAngle(Particle p1, Particle p2)
        {
            return Math.Atan2(p2.Location.Y - p1.Location.Y, p2.Location.X - p1.Location.X);
        }

        void CollideParticles(Particle p1, Particle p2)
        {
            //https://spicyyoghurt.com/tutorials/html5-javascript-game-development/collision-detection-physics

            if (p1.Infected || p2.Infected)
            {
                p1.Infected = true;
                p2.Infected = true;
            }    

            Vector2D collisionVector = new Vector2D(p2.Location.X - p1.Location.X, p2.Location.Y - p1.Location.Y);
            double distance = Math.Sqrt((p2.Location.X - p1.Location.X) * (p2.Location.X - p1.Location.X) + (p2.Location.Y - p1.Location.Y) * (p2.Location.Y - p1.Location.Y));

            Vector2D collisionVectorNorm = Vector2D.Normalise(collisionVector);

            Vector2D relativeVelocityVector = new Vector2D(p1.Velocity.X - p2.Velocity.X, p1.Velocity.Y - p2.Velocity.Y);
            double speed = relativeVelocityVector.X * collisionVectorNorm.X + relativeVelocityVector.Y * collisionVectorNorm.Y;

            speed *= Math.Min(p1.Hardness, p2.Hardness);

            if (speed < 0)
            {
                return;
            }

            double impulse = 2 * speed / (p1.Mass + p2.Mass);

            if (!p1.IsStationary)
            {
                p1.Velocity -= impulse * p2.Mass * collisionVectorNorm;
            }
            if (!p2.IsStationary)
            {
                p2.Velocity += impulse * p1.Mass * collisionVectorNorm;
            }
        }

        void InitiliseParticlesWithGravity()
        {
            for (int j = 0; j < Particles.Count; j++)
            {
                if (Particles[j].GravitationalMultiple > 0)
                {
                    particlesWithGravity.Add(Particles[j]);
                }
            }
        }

        void CheckForWallCollision(Particle particle)
        {
            double speedDampenWall = this.wallHardness;
            double speedDampenGround = this.groundRoofHardness;

            if (particle.Location.X < particle.Radius)
            {
                particle.Velocity.X = Math.Abs(particle.Velocity.X) * speedDampenWall;
                particle.Location.X = particle.Radius;
            }
            else if (particle.Location.X > DrawBox.Width - particle.Radius)
            {
                particle.Velocity.X = -Math.Abs(particle.Velocity.X) * speedDampenWall;
                particle.Location.X = DrawBox.Width - particle.Radius;
            }

            if (particle.Location.Y < particle.Radius)
            {
                particle.Velocity.Y = Math.Abs(particle.Velocity.Y) * speedDampenGround;
                particle.Location.Y = particle.Radius;
            }
            else if (particle.Location.Y > DrawBox.Height - particle.Radius)
            {
                particle.Velocity.Y = -Math.Abs(particle.Velocity.Y) * speedDampenGround;
                particle.Location.Y = DrawBox.Height - particle.Radius;
            }

            CheckForTerminalVelocity(ref particle);

            //Console.WriteLine($"{Vector2D.Abs(particle.Velocity)}, x: {particle.Velocity.X}, y:{particle.Velocity.Y}");
        }

        bool CircleIntersect(Particle p1, Particle p2, bool includeEqual = true)
        {
            double squareDistance = Vector2D.AbsSquared(p1.Location - p2.Location);

            if (includeEqual)
            {
                return squareDistance <= ((p1.Radius + p2.Radius) * (p1.Radius + p2.Radius));
            }
            return squareDistance < ((p1.Radius + p2.Radius) * (p1.Radius + p2.Radius));
        }

        void CheckForBorderIntersect(Particle p)
        {
            foreach (Border b in Borders)
            {
                int minX = (int)Math.Min(b.Start.X, b.End.X);
                int maxX = (int)Math.Max(b.Start.X, b.End.X);

                int minY = (int)Math.Min(b.Start.Y, b.End.Y);
                int maxY = (int)Math.Max(b.Start.Y, b.End.Y);

                if (p.Location.Y > minY && p.Location.Y < maxY)
                {
                    if (p.Location.X + p.Radius > minX && p.Location.X < maxX)
                    {
                        p.Velocity.X = -Math.Abs(p.Velocity.X);
                        p.Location.X = minX - p.Radius;
                    }
                    else if (p.Location.X - p.Radius < maxX && p.Location.X > minX)
                    {
                        p.Velocity.X = Math.Abs(p.Velocity.X);
                        p.Location.X = maxX + p.Radius;
                    }
                }

                if (p.Location.X > minX && p.Location.X < maxX)
                {
                    if (p.Location.Y + p.Radius > minY && p.Location.Y < maxY)
                    {
                        p.Velocity.Y = -Math.Abs(p.Velocity.Y);
                        p.Location.Y = minY - p.Radius;
                    }
                    else if (p.Location.Y - p.Radius < maxY && p.Location.Y > minY)
                    {
                        p.Velocity.Y = Math.Abs(p.Velocity.Y);
                        p.Location.Y = maxY + p.Radius;
                    }
                }
            }
            CheckForTerminalVelocity(ref p);
        }

        void CheckForTerminalVelocity(ref Particle particle)
        {
            //Console.WriteLine($"{Vector2.Abs(particle.Velocity)}, x:{particle.Velocity.X}, y:{particle.Velocity.Y}");
            if (terminalSpeed >= 0 && Vector2D.AbsSquared(particle.Velocity) > terminalSpeed * terminalSpeed)
            {
                particle.Velocity.Absolute = terminalSpeed;
            }
        }

        void CheckIfInGravitationalParticle(Particle p)
        {
            for (int i = 0; i < particlesWithGravity.Count; i++)
            {
                if (p != particlesWithGravity[i] && CircleIntersect(p, particlesWithGravity[i]))
                {
                    RemoveParticle(p);
                }
            }
        }

        void CheckIfInWindow(Particle p)
        {
            foreach (Window w in Windows)
            {
                if (w.Closed)
                    continue;

                int minX = (int)Math.Min(w.Start.X, w.End.X);
                int maxX = (int)Math.Max(w.Start.X, w.End.X);

                int minY = (int)Math.Min(w.Start.Y, w.End.Y);
                int maxY = (int)Math.Max(w.Start.Y, w.End.Y);

                if (p.Location.Y > minY && p.Location.Y < maxY)
                {
                    if (p.Location.X + p.Radius > minX && p.Location.X < maxX)
                    {
                        RemoveParticle(p);
                        continue;
                    }
                    else if (p.Location.X - p.Radius < maxX && p.Location.X > minX)
                    {
                        RemoveParticle(p);
                        continue;
                    }
                }

                if (p.Location.X > minX && p.Location.X < maxX)
                {
                    if (p.Location.Y + p.Radius > minY && p.Location.Y < maxY)
                    {
                        RemoveParticle(p);
                        continue;
                    }
                    else if (p.Location.Y - p.Radius < maxY && p.Location.Y > minY)
                    {
                        RemoveParticle(p);
                        continue;
                    }
                }
            }
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine(isDrawing);
            for (int i = 0; i < Spawners.Count; i++)
            {
                if (timerTicks % Spawners[i].Frequency == 0)
                {
                    bool infectious = Spawners[i].Colour == Color.Red ? true : false;
                    AppendParticle(new Particle(Spawners[i].Location, Vector2D.FromSpeedAngle(random.NextDouble() * random.Next(100), random.NextDouble() * Spawners[i].Angle + Spawners[i].Offset), 0, 10, 1, 1, infectious));
                }
            }
            for (int i = 0; i < Particles.Count; i++)
            {
                for (int x = 0; x < particlesWithGravity.Count; x++)
                {
                    if (!Particles[i].IsStationary && particlesWithGravity[x] != Particles[i])
                    {
                        //particles[i].Velocity += new Vector2(((particlesWithGravity[x].PullAcceleration / particles[i].Mass) / (Math.Pow(Vector2.Abs(particles[i].Location - particlesWithGravity[x].Location), 2) / UniversalPullStrengthRatio)) * TickTimer.Interval / 60, ReturnRelativeParticleAngle(particles[i], particlesWithGravity[x]), false);
                        Particles[i].Velocity += Vector2D.FromSpeedAngle(particlesWithGravity[x].GravitationalMultiple * (G * particlesWithGravity[x].Mass / (Vector2D.AbsoluteDifference(Particles[i].Location, particlesWithGravity[x].Location)) * (Vector2D.AbsoluteDifference(Particles[i].Location, particlesWithGravity[x].Location))) * TickTimer.Interval / 60, ReturnRelativeParticleAngle(Particles[i], particlesWithGravity[x]));
                    }
                }
                for (int x = 0; x < Windows.Count; x++)
                {
                    if (!(Particles[i].IsStationary || Windows[x].Closed || Vector2D.AbsoluteDifference(Particles[i].Location, Windows[x].Location) > Windows[x].LargestDimention + Windows[x].Range))
                    {
                        //particles[i].Velocity += new Vector2(((particlesWithGravity[x].PullAcceleration / particles[i].Mass) / (Math.Pow(Vector2.Abs(particles[i].Location - particlesWithGravity[x].Location), 2) / UniversalPullStrengthRatio)) * TickTimer.Interval / 60, ReturnRelativeParticleAngle(particles[i], particlesWithGravity[x]), false);
                        Particles[i].Velocity += Vector2D.FromSpeedAngle(Windows[x].PullMultiple * (G * Windows[x].Mass / (Vector2D.AbsoluteDifference(Particles[i].Location, Windows[x].Location)) * (Vector2D.AbsoluteDifference(Particles[i].Location, Windows[x].Location))) * TickTimer.Interval / 60, Vector2D.ReturnRelativeAngle(Particles[i].Location, Windows[x].Location));
                    }
                }
                if (!Particles[i].IsStationary)
                    Particles[i].Update(gravity: gravity * TickTimer.Interval / 60);
                if (Particles.Count > 1)
                {
                    for (int j = i + 1; j < Particles.Count; j++)
                    {
                        CheckForCollsions(Particles[i], Particles[j]);
                    }

                    //CheckForCollsions(Particles[i], Particles.Find(p => p.Location == KD_Tree.NearestNeighbour(KDTree, Particles[i].Location, 0).Location));
                }
                CheckIfInGravitationalParticle(Particles[i]);
                CheckForBorderIntersect(Particles[i]);
                CheckForWallCollision(Particles[i]);
                CheckIfInWindow(Particles[i]);
            }
            timerTicks++;
            TimerTicksDisplay.Text = timerTicks.ToString();
            DrawBox.Refresh();
        }

        private void TimerToggleButton_Click(object sender, EventArgs e)
        {
            if (timerRunning)
            {
                TickTimer.Stop();
                timerRunning = false;
            }
            else
            {
                TickTimer.Start();
                timerRunning = true;
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetTimer();
            InitiliseRandom(ref random, randomSeed);
            Particles.Clear();
            particlesWithGravity.Clear();
            bordersIndex = 0;
            windowsIndex = 0;
            spawnersIndex = 0;
            Borders.Clear();
            Windows.Clear();
            Spawners.Clear();
            AddParticles();
            InitiliseParticlesWithGravity();
            GC.Collect();
            DrawBox.Refresh();
        }

        private void ParticleEnvironment_SizeChanged(object sender, EventArgs e)
        {
            DrawBox.Size = new Size(this.Width - 107, this.Height - 39);
            ButtonPanel.Location = new Point(this.Width - 107 + 6, ButtonPanel.Location.Y);
            try { DrawBox.Image = new Bitmap(this.Width - 107, this.Height - 39); } catch { }
            isDrawing = false;
            DrawBox.Refresh();
        }

        void ResetTimer()
        {
            TickTimer.Stop();
            timerRunning = false;
            TimerTicksDisplay.Text = "0";
            timerTicks = 0;
        }

        private void RandomButton_Click(object sender, EventArgs e)
        {
            ResetTimer();
            Particles = new List<Particle>();
            particlesWithGravity = new List<Particle>();
            Borders = new List<Border>();
            Windows = new List<Window>();
            Spawners = new List<Spawner>();
            bordersIndex = 0;
            windowsIndex = 0;
            spawnersIndex = 0;
            for (int i = 0; i < randomParticlesNumber; i++)
            {
                Vector2D location = new Vector2D(random.Next(DrawBox.Width), random.Next(DrawBox.Height));
                Vector2D velocity = new Vector2D(random.NextDouble() * random.Next(-10, 10), random.NextDouble() * random.Next(-10, 10));
                //Vector2 velocity = new Vector2();
                double pullAcceleration = 0;
                //double radius = random.Next(10, 31);
                double radius = 20;
                //double mass = random.NextDouble() * random.Next(1, 31);
                double mass = 1;
                //double hardness = random.NextDouble();
                double hardness = 1;
                Color colour = Color.FromArgb(255, random.Next(256), random.Next(256), random.Next(256));
                AppendParticle(new Particle(location, velocity, pullAcceleration, radius, mass, hardness));
                CheckForWallCollision(Particles[i]);
            }
            SetTerminalVelocity();
            InitiliseParticlesWithGravity();
            
            DrawBox.Refresh();
        }

        private void DrawBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            foreach (Particle p in Particles)
            {
                e.Graphics.FillEllipse(new SolidBrush(p.Colour), (float)p.Location.X - (float)p.Radius, (float)p.Location.Y - (float)p.Radius, (float)p.Radius + (float)p.Radius, (float)p.Radius + (float)p.Radius);
                e.Graphics.DrawLine(new Pen(Color.Black), (float)p.Location.X, (float)p.Location.Y, (float)p.Location.X + (float)p.Velocity.X, (float)p.Location.Y + (float)p.Velocity.Y);
            }
            foreach (Border b in Borders)
            {
                Rectangle rect = new Rectangle((int)Math.Min(b.Start.X, b.End.X), (int)Math.Min(b.Start.Y, b.End.Y), (int)Math.Abs(b.Start.X - b.End.X), (int)Math.Abs(b.Start.Y - b.End.Y));
                if (b.Filled)
                {
                    e.Graphics.FillRectangle(new SolidBrush(b.Colour), rect);
                }
                else
                {
                    e.Graphics.DrawRectangle(new Pen(b.Colour), rect);
                }
            }
            foreach (Window w in Windows)
            {
                Rectangle rect = new Rectangle((int)Math.Min(w.Start.X, w.End.X), (int)Math.Min(w.Start.Y, w.End.Y), (int)Math.Abs(w.Start.X - w.End.X), (int)Math.Abs(w.Start.Y - w.End.Y));
                e.Graphics.DrawRectangle(new Pen(w.Colour), rect);

                if (w.Closed)
                {
                    e.Graphics.DrawLine(new Pen(w.Colour), (float)w.Start.X, (float)w.Start.Y, (float)w.End.X, (float)w.End.Y);
                    e.Graphics.DrawLine(new Pen(w.Colour), (float)w.Start.X, (float)w.End.Y, (float)w.End.X, (float)w.Start.Y);
                }
            }
            foreach (Spawner s in Spawners)
            {
                e.Graphics.FillEllipse(new SolidBrush(s.Colour), (float)s.Location.X, (float)s.Location.Y, (float)5, (float)5);
            }
        }

        private void DrawBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;
            this.isDrawing = true;
            switch (selectedRadio)
            {
                case "NoneRadio":
                    isDrawing = false;
                    break;
                case "BorderRadio":
                    nullBorder = new Border(e.Location.X, e.Location.Y, 0, 0, Color.Red, true);
                    nullBorder.Hardness = 1;
                    nullBorder.Mass = 1;
                    Borders.Add(nullBorder);
                    break;
                case "WindowRadio":
                    nullWindow = new Window(e.Location.X, e.Location.Y, 0, 0, 10, Color.Blue, true);
                    nullWindow.Hardness = 1;
                    nullWindow.Mass = 1;
                    Windows.Add(nullWindow);
                    break;
                case "SpawnerRadio":
                    nullSpawner = new Spawner(e.Location.X, e.Location.Y, Color.Black);
                    InputBox spawnerConfig = new InputBox("Offset:", "Angle:", "Frequency:");
                    spawnerConfig.ShowDialog();
                    nullSpawner.Angle = double.TryParse(spawnerConfig.Input2String, out _) ? double.Parse(spawnerConfig.Input2String) * (Math.PI / 180) : 2 * Math.PI;
                    nullSpawner.Offset = double.TryParse(spawnerConfig.InputString, out _) ? double.Parse(spawnerConfig.InputString) * (Math.PI / 180) : 0;
                    nullSpawner.Frequency = int.TryParse(spawnerConfig.Input3String, out _) ?  int.Parse(spawnerConfig.Input3String) : 200;
                    nullSpawner.Colour = spawnerConfig.Infectious ? Color.Red : Color.Blue;
                    spawnerConfig.Dispose();
                    Spawners.Add(nullSpawner);
                    spawnersIndex++;
                    nullSpawner = null;
                    isDrawing = false;
                    NoneRadio.Checked = true;
                    DrawBox.Refresh();
                    break;
            }
        }

        private void DrawBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;
            this.isDrawing = false;
            switch (selectedRadio)
            {
                case "BorderRadio":
                    bordersIndex++;
                    nullBorder = null;
                    break;
                case "WindowRadio":
                    InputBox strength = new InputBox("Strength:", "Range:");
                    strength.ShowDialog();
                    Windows[windowsIndex].PullMultiple = double.TryParse(strength.InputString, out _) ? double.Parse(strength.InputString) : 1;
                    Windows[windowsIndex].Range = double.TryParse(strength.Input2String, out _) && double.Parse(strength.Input2String) > Windows[windowsIndex].LargestDimention ? double.Parse(strength.Input2String) : Windows[windowsIndex].LargestDimention + 13.5;
                    strength.Dispose();
                    windowsIndex++;
                    nullWindow = null;
                    break;
            }
            NoneRadio.Checked = true;
            DrawBox.Refresh();
        }

        private void DrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                switch (selectedRadio)
                {
                    case "BorderRadio":
                        Borders[bordersIndex].End.X = e.Location.X;
                        Borders[bordersIndex].End.Y = e.Location.Y;
                        break;
                    case "WindowRadio":
                        Windows[windowsIndex].End.X = e.Location.X;
                        Windows[windowsIndex].End.Y = e.Location.Y;
                        break;
                }
                DrawBox.Refresh();
            }
        }

        private void Radio_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var control in ButtonPanel.Controls)
            {
                RadioButton radio = control as RadioButton;

                if (radio != null && radio.Checked)
                {
                    this.selectedRadio = radio.Name;
                }
            }
        }

        private void DrawBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < Borders.Count; i++)
                {
                    int minX = (int)Math.Min(Borders[i].Start.X, Borders[i].End.X);
                    int maxX = (int)Math.Max(Borders[i].Start.X, Borders[i].End.X);

                    int minY = (int)Math.Min(Borders[i].Start.Y, Borders[i].End.Y);
                    int maxY = (int)Math.Max(Borders[i].Start.Y, Borders[i].End.Y);

                    if (minX <= e.X && maxX >= e.X && minY <= e.Y && maxY >= e.Y)
                    {
                        Borders.Remove(Borders[i]);
                        bordersIndex--;
                    }
                }

                for (int i = 0; i < Windows.Count; i++)
                {
                    int minX = (int)Math.Min(Windows[i].Start.X, Windows[i].End.X);
                    int maxX = (int)Math.Max(Windows[i].Start.X, Windows[i].End.X);

                    int minY = (int)Math.Min(Windows[i].Start.Y, Windows[i].End.Y);
                    int maxY = (int)Math.Max(Windows[i].Start.Y, Windows[i].End.Y);

                    if (minX <= e.X && maxX >= e.X && minY <= e.Y && maxY >= e.Y)
                    {
                        Windows.Remove(Windows[i]);
                        windowsIndex--;
                    }
                }

                for (int i = 0; i < Spawners.Count; i++)
                {
                    int minX = (int)Spawners[i].Location.X - 5;
                    int maxX = (int)Spawners[i].Location.X + 5;

                    int minY = (int)Spawners[i].Location.Y - 5;
                    int maxY = (int)Spawners[i].Location.Y + 5;

                    if (minX <= e.X && maxX >= e.X && minY <= e.Y && maxY >= e.Y)
                    {
                        Spawners.Remove(Spawners[i]);
                        spawnersIndex--;
                    }
                }

                DrawBox.Refresh(); 
            }
        }


        private void DrawBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < Windows.Count; i++)
            {
                int minX = (int)Math.Min(Windows[i].Start.X, Windows[i].End.X);
                int maxX = (int)Math.Max(Windows[i].Start.X, Windows[i].End.X);

                int minY = (int)Math.Min(Windows[i].Start.Y, Windows[i].End.Y);
                int maxY = (int)Math.Max(Windows[i].Start.Y, Windows[i].End.Y);

                if (minX <= e.X && maxX >= e.X && minY <= e.Y && maxY >= e.Y)
                {
                    Windows[i].CloseToggle();
                }
            }

            DrawBox.Refresh();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            bool timerWasRunning = timerRunning;
            TickTimer.Stop();
            timerRunning = false;
            GenerateSaveFolder();
            SaveJsons($"save{Directory.GetFiles($@"{mainDirectory}\Saves").Count() + 1}");
            if (timerWasRunning)
            {
                TickTimer.Start();
                timerRunning = true;
            }
        }

        private void GenerateSaveFolder()
        {
            if (!Directory.Exists($@"{mainDirectory}\Saves"))
                Directory.CreateDirectory($@"{mainDirectory}\Saves");
        }

        private void SaveJsons(string folderName)
        {
            //Directory.CreateDirectory($@"{mainDirectory}\Saves\{folderName}");

            if (!File.Exists($@"{mainDirectory}\Saves\{folderName}.json"))
            {
                var file = File.Create($@"{mainDirectory}\Saves\{folderName}.json");
                file.Close();
            }
            using (StreamWriter writer = new StreamWriter($@"{mainDirectory}\Saves\{folderName}.json"))
            {
                //string poo = JsonSerializer.Serialize(Particles);
                //Console.WriteLine(poo);
                writer.WriteLine(JsonSerializer.Serialize(Particles));
                writer.WriteLine(JsonSerializer.Serialize(Borders));
                writer.WriteLine(JsonSerializer.Serialize(Windows));
                writer.WriteLine(JsonSerializer.Serialize(Spawners));
                /*writer.WriteLine("Particles");
                string[] particlesToJSON = new string[Particles.Count];
                for (int i = 0; i < Particles.Count; i++)
                {
                    particlesToJSON[i] = JsonSerializer.Serialize(Particles[i]);
                }
                writer.WriteLine(String.Join(Environment.NewLine, particlesToJSON));
                writer.WriteLine("Borders");
                string[] bordersToJSON = new string[Borders.Count];
                for (int i = 0; i < Borders.Count; i++)
                {
                    bordersToJSON[i] = JsonSerializer.Serialize(Borders[i]);
                }
                writer.WriteLine(String.Join(Environment.NewLine, bordersToJSON));
                writer.WriteLine("Borders");
                string[] windowsToJSON = new string[Windows.Count];
                for (int i = 0; i < Windows.Count; i++)
                {
                    windowsToJSON[i] = JsonSerializer.Serialize(Windows[i]);
                }
                writer.WriteLine(String.Join(Environment.NewLine, windowsToJSON));
                writer.WriteLine("Spawners");
                string[] spawnersToJSON = new string[Spawners.Count];
                for (int i = 0; i < Spawners.Count; i++)
                {
                    spawnersToJSON[i] = JsonSerializer.Serialize(Spawners[i]);
                }
                writer.WriteLine(String.Join(Environment.NewLine, spawnersToJSON));*/
                writer.Close();
            }

            /*for (int i = 0; i < Particles.Count; i++)
            {
                File.WriteAllText($@"{mainDirectory}\Saves\{folderName}\Particles\particle{i}.json", JsonSerializer.Serialize(Particles[i]));
            }
            for (int i = 0; i < Borders.Count; i++)
            {
                File.WriteAllText($@"{mainDirectory}\Saves\{folderName}\Borders\border{i}.json", JsonSerializer.Serialize(Borders[i]));
            }
            for (int i = 0; i < Windows.Count; i++)
            {
                File.WriteAllText($@"{mainDirectory}\Saves\{folderName}\Windows\window{i}.json", JsonSerializer.Serialize(Windows[i]));
            }
            for (int i = 0; i < Spawners.Count; i++)
            {
                File.WriteAllText($@"{mainDirectory}\Saves\{folderName}\Spawners\spawner{i}.json", JsonSerializer.Serialize(Spawners[i]));
            }*/
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            //ResetButton.PerformClick();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = $@"{mainDirectory}\Saves";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try { ResetButton.PerformClick(); LoadSave(openFileDialog.FileName);  } catch { MessageBox.Show("Something went wrong!"); }
                DrawBox.Refresh();
            }
        }

        private void LoadSave(string savePath)
        {
            string[] jsonData = File.ReadAllLines(savePath);
            Particles = JsonSerializer.Deserialize<List<Particle>>(jsonData[0]);
            Borders = JsonSerializer.Deserialize<List<Border>>(jsonData[1]);
            Windows = JsonSerializer.Deserialize<List<Window>>(jsonData[2]);
            Spawners = JsonSerializer.Deserialize<List<Spawner>>(jsonData[3]);

            /*foreach (string file in Directory.GetFiles($@"{savePath}\Particles"))
            {
                Particles.Add(JsonSerializer.Deserialize<Particle>(File.ReadAllText(file)));
            }
            foreach (string file in Directory.GetFiles($@"{savePath}\Borders"))
            {
                Borders.Add(JsonSerializer.Deserialize<Border>(File.ReadAllText(file)));
            }
            foreach (string file in Directory.GetFiles($@"{savePath}\Windows"))
            {
                Windows.Add(JsonSerializer.Deserialize<Window>(File.ReadAllText(file)));
            }
            foreach (string file in Directory.GetFiles($@"{savePath}\Spawners"))
            {
                Spawners.Add(JsonSerializer.Deserialize<Spawner>(File.ReadAllText(file)));
            }*/
        }
    }
}