using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Particle_Collision
{
    public partial class ParticleEnvironment : Form
    {
        public List<Particle> particles = new List<Particle>();
        private List<Particle> particlesWithGravity = new List<Particle>();

        public List<Border> Borders = new List<Border>();
        public List<Window> Windows = new List<Window>();

        private int bordersIndex = 0;
        private Border nullBorder = null;

        private int windowsIndex = 0;
        private Window nullWindow = null;

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
            particles.Add(new Particle(new Vector2D(50, 50), new Vector2D(10, 0), 0, 20, 10, 1, Color.Red));
            particles.Add(new Particle(new Vector2D(200, 200), new Vector2D(0, 0), 0, 20, 10, 1, Color.Blue, false, true));
            //particles.Add(new Particle(new Vector2(600, 300), new Vector2(-5, 0), 0, 20, 1, 1, Color.Red));
            //particles.Add(new Particle(new Vector2(400, 300), new Vector2(5, 0), 0, 20, 1, 1, Color.Red));
            //particles.Add(new Particle(new Vector2(0, this.Height / 2), new Vector2(0, 0), 10, 0, 0, 0, Color.Transparent, true));
        }

        void SetTerminalVelocity()
        {
            if (autoSetTerminalSpeed)
            {
                double max = 0;
                for (int i = 0; i < particles.Count; i++)
                {
                    if (particles[i].Radius > max)
                        max = particles[i].Radius;
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
            for (int j = 0; j < particles.Count; j++)
            {
                if (particles[j].GravitationalMultiple > 0)
                {
                    particlesWithGravity.Add(particles[j]);
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

            Console.WriteLine($"{Vector2D.Abs(particle.Velocity)}, x: {particle.Velocity.X}, y:{particle.Velocity.Y}");
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
                    particles.Remove(p);
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
                        particles.Remove(p);
                        continue;
                    }
                    else if (p.Location.X - p.Radius < maxX && p.Location.X > minX)
                    {
                        particles.Remove(p);
                        continue;
                    }
                }

                if (p.Location.X > minX && p.Location.X < maxX)
                {
                    if (p.Location.Y + p.Radius > minY && p.Location.Y < maxY)
                    {
                        particles.Remove(p);
                        continue;
                    }
                    else if (p.Location.Y - p.Radius < maxY && p.Location.Y > minY)
                    {
                        particles.Remove(p);
                        continue;
                    }
                }
            }
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                
                for (int x = 0; x < particlesWithGravity.Count; x++)
                {
                    if (!particles[i].IsStationary && particlesWithGravity[x] != particles[i])
                    {
                        //particles[i].Velocity += new Vector2(((particlesWithGravity[x].PullAcceleration / particles[i].Mass) / (Math.Pow(Vector2.Abs(particles[i].Location - particlesWithGravity[x].Location), 2) / UniversalPullStrengthRatio)) * TickTimer.Interval / 60, ReturnRelativeParticleAngle(particles[i], particlesWithGravity[x]), false);
                        particles[i].Velocity += Vector2D.FromSpeedAngle(particlesWithGravity[x].GravitationalMultiple * (G * particlesWithGravity[x].Mass / (Vector2D.AbsoluteDifference(particles[i].Location, particlesWithGravity[x].Location)) * (Vector2D.AbsoluteDifference(particles[i].Location, particlesWithGravity[x].Location))) * TickTimer.Interval / 60, ReturnRelativeParticleAngle(particles[i], particlesWithGravity[x]));
                    }
                }
                for (int x = 0; x < Windows.Count; x++)
                {
                    if (!(particles[i].IsStationary || Windows[x].Closed))
                    {
                        //particles[i].Velocity += new Vector2(((particlesWithGravity[x].PullAcceleration / particles[i].Mass) / (Math.Pow(Vector2.Abs(particles[i].Location - particlesWithGravity[x].Location), 2) / UniversalPullStrengthRatio)) * TickTimer.Interval / 60, ReturnRelativeParticleAngle(particles[i], particlesWithGravity[x]), false);
                        particles[i].Velocity += Vector2D.FromSpeedAngle(Windows[x].PullMultiple * (G * Windows[x].Mass / (Vector2D.AbsoluteDifference(particles[i].Location, Windows[x].Location)) * (Vector2D.AbsoluteDifference(particles[i].Location, Windows[x].Location))) * TickTimer.Interval / 60, Vector2D.ReturnRelativeAngle(particles[i].Location, Windows[x].Location));
                    }
                }
                if (!particles[i].IsStationary)
                    particles[i].Update(gravity: gravity * TickTimer.Interval / 60);
                if (particles.Count > 1)
                {
                    for (int j = i + 1; j < particles.Count; j++)
                    {
                        CheckForCollsions(particles[i], particles[j]);
                    }
                }
                CheckIfInGravitationalParticle(particles[i]);
                CheckForBorderIntersect(particles[i]);
                CheckForWallCollision(particles[i]);
                CheckIfInWindow(particles[i]);
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
            particles.Clear();
            particlesWithGravity.Clear();
            bordersIndex = 0;
            windowsIndex = 0;
            Borders.Clear();
            Windows.Clear();
            AddParticles();
            InitiliseParticlesWithGravity();
            GC.Collect();
            DrawBox.Refresh();
        }

        private void ParticleEnvironment_SizeChanged(object sender, EventArgs e)
        {
            DrawBox.Size = new Size(this.Width - 107, this.Height - 39);
            ButtonPanel.Location = new Point(DrawBox.Width + 6, ButtonPanel.Location.Y);
            DrawBox.Image = new Bitmap(DrawBox.Width, DrawBox.Height);
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
            particles = new List<Particle>();
            particlesWithGravity = new List<Particle>();
            Borders = new List<Border>();
            Windows = new List<Window>();
            bordersIndex = 0;
            windowsIndex = 0;
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
                Color colour = Color.FromArgb(255, random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
                particles.Add(new Particle(location, velocity, pullAcceleration, radius, mass, hardness, colour));
                CheckForWallCollision(particles[i]);
            }
            SetTerminalVelocity();
            InitiliseParticlesWithGravity();
            DrawBox.Refresh();
        }

        private void DrawBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            foreach (Particle p in particles)
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
        }

        private void DrawBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                return;
            this.isDrawing = true;
            switch (selectedRadio)
            {
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
                    InputBox strength = new InputBox("Strength:");
                    strength.ShowDialog();
                    Windows[windowsIndex].PullMultiple = double.TryParse(strength.InputString, out _) ? double.Parse(strength.InputString) : 1;
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
                        Borders.Remove(Windows[i]);
                        windowsIndex--;
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
    }
}