using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;
using System.Resources;
using Particle_Collision.Properties;


namespace Particle_Collision
{
    public partial class ParticleEnvironment : Form
    {
        KD_Node KDRootNode = new KD_Node();

        private ResourceManager resourceManager = new ResourceManager("Particle_Collision.Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());

        private List<Particle> particlesReturn = new List<Particle>();
        public List<Particle> Particles = new List<Particle>();
        
        private List<Particle> particlesWithGravity = new List<Particle>();

        string mainDirectory = Directory.GetCurrentDirectory();

        public List<Border> Borders = new List<Border>();
        public List<Window> Windows = new List<Window>();
        public List<Spawner> Spawners = new List<Spawner>();

        //public List<object> selectedObjects = new List<object>();

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
        //private int randomParticlesNumber = 10;
        private string randomSeed = "TestSeed";
        private Random random;

        private Point startSelectMouseLocation;
        private Point endSelectMouseLocation;

        public ParticleEnvironment()
        {
            InitializeComponent();
        }

        private void ParticleEnvironment_Load(object sender, EventArgs e)
        {
            LoadPresets();
            InitiliseRandom(ref random, randomSeed);
            AddParticles();
            InitiliseParticlesWithGravity();
            SetTerminalVelocity();
            TimerToggleButton.PerformClick();
            DrawBox.Refresh();
            refreshItemDesc();
        }

        void LoadPresets()
        {
            Directory.CreateDirectory($@"{mainDirectory}\Presets");
            using (StreamWriter sw = new StreamWriter($@"{mainDirectory}\Presets\preset1.txt"))
                sw.Write(resourceManager.GetString("preset1"));
            using (StreamWriter sw = new StreamWriter($@"{mainDirectory}\Presets\preset2.txt"))
                sw.Write(resourceManager.GetString("preset2"));
            using (StreamWriter sw = new StreamWriter($@"{mainDirectory}\Presets\preset3.txt"))
                sw.Write(resourceManager.GetString("preset3"));
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
            for (int i = 0; i < Spawners.Count; i++)
            {
                if (timerTicks % Spawners[i].Frequency == 0)
                {
                    Vector2D location = new Vector2D(Spawners[i].Location.X, Spawners[i].Location.Y);
                    double angle = Spawners[i].Offset + (random.NextDouble() * Spawners[i].Angle) - (Math.PI / 2);
                    Vector2D velocity = Vector2D.FromSpeedAngle(random.NextDouble() * random.Next(1, 10), angle);

                    double graviationalMultiple = 0;
                    double mass = 1;
                    double hardness = 1;
                    bool infected = Spawners[i].Infectious;
                    double radius = Spawners[i].ParticleRadius;

                    AppendParticle(new Particle(location, velocity, graviationalMultiple, radius, mass, hardness, infected));
                }
            }
            //KDRootNode = KD_Tree.GenerateKDTree(Particles.Select(x => x.Location).ToList());
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
                if (Particles.Count > 1)
                {
                    // NOT USING KD TREE
                    for (int j = i + 1; j < Particles.Count; j++)
                    {
                        CheckForCollsions(Particles[i], Particles[j]);
                    }

                    /* USING KD TREE NOT WORKING BUT WILL FIX
                    if (KDRootNode != null)
                        CheckForCollsions(Particles[i], Particles.Find(x => x.Location == KD_Tree.NearestNeighbour(KDRootNode, Particles[i].Location, 0).Location));
                    */
                }
                //CheckIfInGravitationalParticle(Particles[i]);
                CheckIfInWindow(Particles[i]);
                if (i >= Particles.Count)
                    break;
                CheckIfHittingSpawner(Particles[i]);
                if (i >= Particles.Count)
                    break;
                CheckForBorderIntersect(Particles[i]);
                CheckForWallCollision(Particles[i]);

                if (!Particles[i].IsStationary)
                    Particles[i].Update(gravity: gravity * TickTimer.Interval / 60);
            }
            timerTicks++;
            TimerTicksDisplay.Text = timerTicks.ToString();
            DrawBox.Refresh();
        }

        private void CheckIfHittingSpawner(Particle p)
        {
            if (!p.Infected)
                return;

            foreach (Spawner s in Spawners)
            {
                /*
                if (s.Health <= 0 || Vector2D.AbsoluteSquareDifference(new Vector2D(s.Location.X, s.Location.Y), p.Location) < Vector2D.AbsoluteSquareDifference(new Vector2D(s.Location.X, s.Location.Y), p.FutureLocationVector))
                    continue;
                */

                if (s.Health <= 0)
                {
                    s.Infectious = true;
                    continue;
                }

                int minX = s.Location.X - 25;
                int maxX = s.Location.X + 25;

                int minY = s.Location.Y - 25;
                int maxY = s.Location.Y + 25;

                if (p.Location.Y > minY && p.Location.Y < maxY)
                {
                    if (p.Location.X + p.Radius > minX && p.Location.X < maxX)
                    {
                        s.Health -= 1;
                        continue;
                    }
                    else if (p.Location.X - p.Radius < maxX && p.Location.X > minX)
                    {
                        s.Health -= 1;
                        continue;
                    }
                }

                if (p.Location.X > minX && p.Location.X < maxX)
                {
                    if (p.Location.Y + p.Radius > minY && p.Location.Y < maxY)
                    {
                        s.Health -= 1;
                        continue;
                    }
                    else if (p.Location.Y - p.Radius < maxY && p.Location.Y > minY)
                    {
                        s.Health -= 1;
                        continue;
                    }
                }
            }
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
            DrawBox.Size = new Size(this.Width - 107, this.Height - 71);
            ButtonPanel.Location = new Point(this.Width - 107 + 6, ButtonPanel.Location.Y);
            DescPanel.Location = new Point(DescPanel.Location.X, this.Height - 69);
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
            // Need to add random scene generation
        }

        private void DrawBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            foreach (Particle p in Particles)
            {
                e.Graphics.FillEllipse(new SolidBrush(p.Colour), (float)p.Location.X - (float)p.Radius, (float)p.Location.Y - (float)p.Radius, (float)p.Radius + (float)p.Radius, (float)p.Radius + (float)p.Radius);
                //e.Graphics.DrawLine(new Pen(Color.Black), (float)p.Location.X, (float)p.Location.Y, (float)p.Location.X + (float)p.Velocity.X, (float)p.Location.Y + (float)p.Velocity.Y);
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
                //e.Graphics.FillEllipse(new SolidBrush(s.Colour), (float)s.Location.X, (float)s.Location.Y, (float)5, (float)5);

                // BEN WORNES MODE !DELETE BEFORE HAND-IN!
                //e.Graphics.DrawImage((Image)resourceManager.GetObject("spawnerImage"), (float)s.Location.X - 25, (float)s.Location.Y - 25, (float)50, (float)50);
                e.Graphics.ResetTransform();
                //e.Graphics.TranslateTransform(this.Width / 2, this.Height / 2);
                e.Graphics.TranslateTransform(((float)s.Location.X), ((float)s.Location.Y));
                if (!(s.Offset == 0 && s.Angle == 2 * Math.PI))
                    e.Graphics.RotateTransform((float)((s.Offset + (s.Angle / 2)) * (180 / Math.PI)));
                else
                    e.Graphics.RotateTransform((float)(Math.PI * (180 / Math.PI)));
                //e.Graphics.RotateTransform(45.0F);
                e.Graphics.TranslateTransform(-((float)s.Location.X), -((float)s.Location.Y));
                if (s.Infectious)
                {
                    //e.Graphics.DrawImage((Image)resourceManager.GetObject("infectiousSpawner"), (float)s.Location.X - 25, (float)s.Location.Y - 25, (float)50, (float)50);
                    e.Graphics.DrawImage(Resources.character1SmallInfected, (float)s.Location.X - 25, (float)s.Location.Y - 25, (float)50, (float)50);
                }
                else
                {
                    //e.Graphics.DrawImage((Image)resourceManager.GetObject("normalSpawner"), (float)s.Location.X - 25, (float)s.Location.Y - 25, (float)50, (float)50);
                    e.Graphics.DrawImage(Resources.character1Small, (float)s.Location.X - 25, (float)s.Location.Y - 25, (float)50, (float)50);
                    e.Graphics.FillRectangle(new SolidBrush(Color.Green), new Rectangle(s.Location.X - 25, s.Location.Y - 40, (int)(50 * ((double)s.Health / 100)), 10));
                }
            }
        }

        private void DrawBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && selectedRadio == "NoneRadio")
            {
                startSelectMouseLocation = e.Location;
            }
            this.isDrawing = true;
            switch (selectedRadio)
            {
                case "NoneRadio":
                    isDrawing = false;
                    break;
                case "BorderRadio":
                    nullBorder = new Border(e.Location.X, e.Location.Y, 0, 0, Color.Red, true);
                    nullBorder.Hardness = 1;
                    Borders.Add(nullBorder);
                    break;
                case "WindowRadio":
                    nullWindow = new Window(e.Location.X, e.Location.Y, 0, 0, 10, Color.Blue, true);
                    nullWindow.Hardness = 1;
                    Windows.Add(nullWindow);
                    break;
                case "SpawnerRadio":
                    nullSpawner = new Spawner(e.Location.X, e.Location.Y, false);
                    InputBox spawnerConfig = new InputBox("Offset:", "Angle:", "Frequency:", "Particle Radius:");
                    spawnerConfig.ShowDialog();
                    nullSpawner.Angle = double.TryParse(spawnerConfig.Input2String, out _) ? double.Parse(spawnerConfig.Input2String) * (Math.PI / 180) : 2 * Math.PI;
                    nullSpawner.Offset = double.TryParse(spawnerConfig.InputString, out _) ? double.Parse(spawnerConfig.InputString) * (Math.PI / 180) : 0;
                    nullSpawner.Frequency = int.TryParse(spawnerConfig.Input3String, out _) ?  int.Parse(spawnerConfig.Input3String) : nullSpawner.Frequency;
                    nullSpawner.Infectious = spawnerConfig.Infectious ? true : false;
                    nullSpawner.ParticleRadius = spawnerConfig.Slider1Value;
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
            if (e.Button == MouseButtons.Right && selectedRadio == "NoneRadio")
            {
                endSelectMouseLocation = e.Location;
                if (startSelectMouseLocation != null && endSelectMouseLocation != null)
                    SelectAllObjectsInSelectRadius(startSelectMouseLocation, endSelectMouseLocation);
            }
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

        private void SelectAllObjectsInSelectRadius(Point start, Point end)
        {
            for (int i = 0; i < Borders.Count; i++)
            {
                //if (Borders[i].Location)
            }

            for (int i = 0; i < Windows.Count; i++)
            {
                
            }

            for (int i = 0; i < Spawners.Count; i++)
            {
                
            }
        }

        private void DrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                switch (selectedRadio)
                {
                    case "BorderRadio":
                        Borders[bordersIndex].End = new Point(e.Location.X, e.Location.Y);
                        break;
                    case "WindowRadio":
                        Windows[windowsIndex].End = new Point(e.Location.X, e.Location.Y);
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
                    refreshItemDesc();
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
                    int minX = (int)Spawners[i].Location.X - 50;
                    int maxX = (int)Spawners[i].Location.X + 50;

                    int minY = (int)Spawners[i].Location.Y - 50;
                    int maxY = (int)Spawners[i].Location.Y + 50;

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
            /* bool timerWasRunning = timerRunning;
             TickTimer.Stop();
             timerRunning = false;
             GenerateSaveFolder();
             SaveJsons($"save{Directory.GetFiles($@"{mainDirectory}\Saves").Length + 1}");
             if (timerWasRunning)
             {
                 TickTimer.Start();
                 timerRunning = true;
             }
             */
                    Directory.CreateDirectory($@"{mainDirectory}\Saves");

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = mainDirectory;
            sfd.Filter = "Text files (*.txt)|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string fileName = sfd.FileName;
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine("BORDERS");
                    writer.WriteLine(Convert.ToString(Borders.Count));
                    foreach (Border b in Borders)
                    {
                        writer.WriteLine(b.GetBorderInfoForSaving());
                    }
                    writer.WriteLine("WINDOWS");
                    writer.WriteLine(Convert.ToString(Windows.Count));
                    foreach (Window w in Windows)
                    {
                        writer.WriteLine(w.getWindowInfoForSaving());
                    }
                    writer.WriteLine("SPAWNERS");
                    writer.WriteLine(Convert.ToString(Spawners.Count));
                    foreach (Spawner s in Spawners)
                    {
                        writer.WriteLine(s.getSpawnerInfoForSaving());
                    }
                    writer.WriteLine("FORM SIZE");
                    writer.WriteLine($"{this.Size.Width},{this.Size.Height}");
                }
            }
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            /*//ResetButton.PerformClick();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = $@"{mainDirectory}\Saves";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try { ResetButton.PerformClick(); LoadSave(openFileDialog.FileName); } catch { MessageBox.Show("Something went wrong!"); }
                DrawBox.Refresh();
            } */

            Directory.CreateDirectory($@"{mainDirectory}\Saves");

            // Delete all current items
            ResetButton_Click(sender, e);

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = mainDirectory;
            ofd.Filter = "Text files (*.txt)|*.txt";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string fileName = ofd.FileName;
                using (StreamReader reader = new StreamReader(fileName))
                {
                    reader.ReadLine(); //BORDERS 
                    int numBorders = Convert.ToInt32(reader.ReadLine());
                    for (int i = 0; i < numBorders; i++)
                    {
                        Border b = new Border(reader.ReadLine());
                        Borders.Add(b);
                    }
                    reader.ReadLine(); //WINDOWS
                    int numWindows = Convert.ToInt32(reader.ReadLine());
                    for (int i = 0; i < numWindows; i++)
                    {
                        Window w = new Window(reader.ReadLine());
                        Windows.Add(w);
                    }
                    reader.ReadLine(); //SPAWNERS
                    int numSpawners = Convert.ToInt32(reader.ReadLine());
                    for (int i = 0; i < numSpawners; i++)
                    {
                        Spawner s = new Spawner(reader.ReadLine());
                        Spawners.Add(s);
                    }
                    reader.ReadLine(); //FORM SIZE
                    string[] formSizes = reader.ReadLine().Split(',');
                    this.Width = Convert.ToInt32(formSizes[0]);
                    this.Height = Convert.ToInt32(formSizes[1]);
                    this.CenterToScreen();
                }
                //TickTimer_Tick(sender, new EventArgs());
                Particles.Clear();
                //TickTimer_Tick(sender, new EventArgs());
                DrawBox.Refresh();
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

                //writer.WriteLine(JsonSerializer.Serialize(Particles));
                writer.WriteLine(JsonConvert.SerializeObject(Borders));
                writer.WriteLine(JsonConvert.SerializeObject(Windows));
                writer.WriteLine(JsonConvert.SerializeObject(Spawners));
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

   
        private void LoadSave(string savePath)
        {
            string[] jsonData = File.ReadAllLines(savePath);
            //Particles = JsonSerializer.Deserialize<List<Particle>>(jsonData[0]);
            Borders = JsonConvert.DeserializeObject<BorderData>(jsonData[0]).Data;
            Windows = JsonConvert.DeserializeObject<WindowData>(jsonData[1]).Data;
            Spawners = JsonConvert.DeserializeObject<SpawnerData>(jsonData[2]).Data;

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

        public void refreshItemDesc() 
        {
            if (this.selectedRadio == "NoneRadio")
            {
                ItemDesc.Text = "Click a button or select a tool to create or adjust the environment.";
            }
            else if (this.selectedRadio == "BorderRadio")
            {
                ItemDesc.Text = "BORDER: Click and drag in the environment to create a border, right click a border to remove it.";
            }
            else if (this.selectedRadio == "WindowRadio")
            {
                ItemDesc.Text = "WINDOW: Click and drag in the environment to create a window, right click a window to remove it, double click a window open and close it";
            }
            else if (this.selectedRadio == "SpawnerRadio")
            {
                ItemDesc.Text = "SPAWNER: Tap anywhere on the environment to create a spawner.";
            }
        }
        private void TimerToggleButton_MouseEnter(object sender, EventArgs e)
        {
            ItemDesc.Text = "TOGGLE TIMER: This tool allows you to pause and play the simulation.";
        }
        private void TimerToggleButton_MouseLeave(object sender, EventArgs e){refreshItemDesc();}
        private void ResetButton_MouseEnter(object sender, EventArgs e)
        {
            ItemDesc.Text = "RESET: This tool is used to clear the screen from all objects.";
        }
        private void ResetButton_MouseLeave(object sender, EventArgs e){refreshItemDesc();}
        private void RandomButton_MouseEnter(object sender, EventArgs e)
        {
            ItemDesc.Text = "RANDOM: This button generates a random particles on the screen.";
        }
        private void RandomButton_MouseLeave(object sender, EventArgs e){refreshItemDesc();}
        private void SaveButton_MouseEnter(object sender, EventArgs e)
        {
            ItemDesc.Text = "SAVE: This button saves your current envitronment, but not particles.";
        }
        private void SaveButton_MouseLeave(object sender, EventArgs e){refreshItemDesc();}
        private void LoadButton_MouseEnter(object sender, EventArgs e)
        {
            ItemDesc.Text = "LOAD: This button loads back your saved environments.";
        }
        private void LoadButton_MouseLeave(object sender, EventArgs e){refreshItemDesc();}

        private void TickPerSecondSlider_Scroll(object sender, EventArgs e)
        {

        }

        private void TickPerSecondSlider_ValueChanged(object sender, EventArgs e)
        {
            this.TickTimer.Interval = 1000 / this.TickPerSecondSlider.Value;
            this.TicksPerSecondLabel.Text = $"Ticks/s: {this.TickPerSecondSlider.Value.ToString()}";
        }
    }

    public class BorderData
    {
        public List<Border> Data { get; set; }
    }

    public class WindowData
    {
        public List<Window> Data { get; set; }
    }

    public class SpawnerData
    {
        public List<Spawner> Data { get; set; }
    }
}