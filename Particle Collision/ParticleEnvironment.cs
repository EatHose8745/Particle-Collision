using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Particle_Collision
{
    public partial class ParticleEnvironment : Form
    {
        public List<Particle> particles = new List<Particle>();
        private List<Particle> particlesWithGravity = new List<Particle>();

        private const double G = 10;

        private bool timerRunning = true;
        private int timerTicks = 0;
        private double gravity = 0;
        private double terminalSpeed = -1;
        private bool autoSetTerminalSpeed = false;
        private double wallHardness = 1;
        private double groundRoofHardness = 1;
        private int randomParticlesNumber = 200;
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
            DrawBox.Image = new Bitmap(DrawBox.Width, DrawBox.Height);
            SetTerminalVelocity();
            TimerToggleButton.PerformClick();
            DrawParticles();
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
            particles.Add(new Particle(new Vector2(800, 500), new Vector2(0, 0), 9.81, 20, 1, 1, Color.Green, false));
            particles.Add(new Particle(new Vector2(30, 30), new Vector2(10, 0), 0, 20, 1, 1, Color.Red));
            //particles.Add(new Particle(new Vector2(0, this.Height / 2), new Vector2(0, 0), 10, 0, 0, 0, Color.Transparent, true));
        }

        void DrawParticles()
        {
            var bmp = DrawBox.Image;

            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                foreach (Particle p in particles)
                {
                    //g.DrawEllipse(new Pen(p.Colour), (float)p.Location.X - (float)p.Radius, (float)p.Location.Y - (float)p.Radius, (float)p.Radius + (float)p.Radius, (float)p.Radius + (float)p.Radius);
                    g.FillEllipse(new SolidBrush(p.Colour), (float)p.Location.X - (float)p.Radius, (float)p.Location.Y - (float)p.Radius, (float)p.Radius + (float)p.Radius, (float)p.Radius + (float)p.Radius);
                    g.DrawLine(new Pen(Color.Black), (float)p.Location.X, (float)p.Location.Y, (float)p.Location.X + (float)p.Velocity.X, (float)p.Location.Y + (float)p.Velocity.Y);
                }
            }

            DrawBox.Invalidate();
        }

        void SetTerminalVelocity()
        {
            if (autoSetTerminalSpeed)
            {
                double max = 0;
                for (int i = 0; i < particles.Count(); i++)
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
            Vector2 collisionVector = new Vector2(p2.Location.X - p1.Location.X, p2.Location.Y - p1.Location.Y);
            double distance = Math.Sqrt((p2.Location.X - p1.Location.X) * (p2.Location.X - p1.Location.X) + (p2.Location.Y - p1.Location.Y) * (p2.Location.Y - p1.Location.Y));

            Vector2 collisionVectorNorm = new Vector2(collisionVector.X / distance, collisionVector.Y / distance);

            Vector2 relativeVelocityVector = new Vector2(p1.Velocity.X - p2.Velocity.X, p1.Velocity.Y - p2.Velocity.Y);
            double speed = relativeVelocityVector.X * collisionVectorNorm.X + relativeVelocityVector.Y * collisionVectorNorm.Y;

            speed *= Math.Min(p1.Hardness, p2.Hardness);

            if (speed < 0)
            {
                return;
            }

            double impulse = 2 * speed / (p1.Mass + p2.Mass);

            if (!p1.IsStationary)
                p1.Velocity -= impulse * p2.Mass * collisionVectorNorm.X;
            if (!p2.IsStationary)
                p2.Velocity += impulse * p1.Mass * collisionVectorNorm.X;
        }

        void InitiliseParticlesWithGravity()
        {
            for (int j = 0; j < particles.Count(); j++)
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

            Console.WriteLine($"{Vector2.Abs(particle.Velocity)}, x: {particle.Velocity.X}, y:{particle.Velocity.Y}");
        }

        bool CircleIntersect(Particle p1, Particle p2, bool includeEqual = true)
        {
            double squareDistance = (p1.Location.X - p2.Location.X) * (p1.Location.X - p2.Location.X) + (p1.Location.Y - p2.Location.Y) * (p1.Location.Y - p2.Location.Y);

            if (includeEqual)
            {
                return squareDistance <= ((p1.Radius + p2.Radius) * (p1.Radius + p2.Radius));
            }
            return squareDistance < ((p1.Radius + p2.Radius) * (p1.Radius + p2.Radius));
        }

        void CheckForTerminalVelocity(ref Particle particle)
        {
            //Console.WriteLine($"{Vector2.Abs(particle.Velocity)}, x:{particle.Velocity.X}, y:{particle.Velocity.Y}");
            if (terminalSpeed >= 0 && Vector2.Abs(particle.Velocity) > terminalSpeed)
            {
                particle.Velocity.Absolute = terminalSpeed;
            }
        }

        void CheckIfInGravitationalParticle(Particle p)
        {
            for (int i = 0; i < particlesWithGravity.Count(); i++)
            {
                if (p != particlesWithGravity[i] && CircleIntersect(p, particlesWithGravity[i]))
                {
                    particles.Remove(p);
                }
            }
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < particles.Count(); i++)
            {
                CheckForWallCollision(particles[i]);
                try
                {
                    for (int j = i + 1; j < particles.Count(); j++)
                    {
                        CheckForCollsions(particles[i], particles[j]);
                    }
                }
                catch { }
                particles[i].Update(gravity: gravity * TickTimer.Interval / 60);
                
                for (int x = 0; x < particlesWithGravity.Count(); x++)
                {
                    if (!particles[i].IsStationary && particlesWithGravity[x] != particles[i])
                    {
                        //particles[i].Velocity += new Vector2(((particlesWithGravity[x].PullAcceleration / particles[i].Mass) / (Math.Pow(Vector2.Abs(particles[i].Location - particlesWithGravity[x].Location), 2) / UniversalPullStrengthRatio)) * TickTimer.Interval / 60, ReturnRelativeParticleAngle(particles[i], particlesWithGravity[x]), false);
                        particles[i].Velocity += new Vector2(particlesWithGravity[x].GravitationalMultiple * ((G * particlesWithGravity[x].Mass) / (Vector2.AbsoluteDifference(particles[i].Location, particlesWithGravity[x].Location)) * (Vector2.AbsoluteDifference(particles[i].Location, particlesWithGravity[x].Location))) * TickTimer.Interval / 60, ReturnRelativeParticleAngle(particles[i], particlesWithGravity[x]), false);
                    }
                }
                CheckIfInGravitationalParticle(particles[i]);
            }
            DrawParticles();
            timerTicks++;
            TimerTicksDisplay.Text = timerTicks.ToString();
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
            particles = new List<Particle>();
            particlesWithGravity = new List<Particle>();
            AddParticles();
            DrawParticles();
        }

        private void ParticleEnvironment_SizeChanged(object sender, EventArgs e)
        {
            DrawBox.Size = new Size(this.Width - 107, this.Height - 39);
            ButtonPanel.Location = new Point(DrawBox.Width + 6, ButtonPanel.Location.Y);
            DrawBox.Image = new Bitmap(DrawBox.Width, DrawBox.Height);
            DrawParticles();
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
            for (int i = 0; i < randomParticlesNumber; i++)
            {
                Vector2 location = new Vector2(random.Next(DrawBox.Width), random.Next(DrawBox.Height));
                Vector2 velocity = new Vector2(random.NextDouble() * random.Next(-10, 10), random.NextDouble() * random.Next(-10, 10));
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
            DrawParticles();
        }
    }
}