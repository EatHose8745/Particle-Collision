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

        private bool timerRunning = true;
        private int timerTicks = 0;
        private double gravity = 9.81;
        private double terminalSpeed = 200;
        private double wallHardness = 1;
        private double groundRoofHardness = 1;
        private int randomParticlesNumber = 200;
        private string randomSeed = "Particle";
        private Random random;

        public ParticleEnvironment()
        {
            InitializeComponent();
        }

        private void ParticleEnvironment_Load(object sender, EventArgs e)
        {
            InitiliseRandom(ref random, randomSeed);
            AddParticles();
            DrawBox.Image = new Bitmap(DrawBox.Width, DrawBox.Height);
            TimerToggleButton.PerformClick();
            DrawParticles();
        }

        void InitiliseRandom(ref Random random, string seed)
        {
            random = new Random(seed.GetHashCode());
        }

        void AddParticles()
        {
            particles.Add(new Particle(new Vector2(this.Width / 2 - 100, this.Height / 2), new Vector2(1, 1), 20, 1, 1, Color.Green));
            particles.Add(new Particle(new Vector2(this.Width / 2, this.Height / 2 - 10), new Vector2(-1, -1), 20, 1, 1, Color.Red));
        }

        void DrawParticles()
        {
            var bmp = DrawBox.Image;

            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                foreach (Particle p in particles)
                {
                    g.DrawEllipse(new Pen(p.Colour), (float)p.Location.X - (float)p.Radius, (float)p.Location.Y - (float)p.Radius, (float)p.Radius + (float)p.Radius, (float)p.Radius + (float)p.Radius);
                    g.DrawLine(new Pen(Color.Black), (float)p.Location.X, (float)p.Location.Y, (float)p.Location.X + (float)p.Velocity.X, (float)p.Location.Y + (float)p.Velocity.Y);
                }
            }

            DrawBox.Invalidate();
        }

        bool CheckForCollsions(Particle particle1, Particle particle2)
        {
            if (CircleIntersect(particle1, particle2))
            {
                CollideParticles(particle1, particle2);
                CheckForTerminalVelocity(ref particle1);
                CheckForTerminalVelocity(ref particle2);
                return true;
            }
            return false;
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

            p1.Velocity.X -= (impulse * p2.Mass * collisionVectorNorm.X);
            p1.Velocity.Y -= (impulse * p2.Mass * collisionVectorNorm.Y);
            p2.Velocity.X += (impulse * p1.Mass * collisionVectorNorm.X);
            p2.Velocity.Y += (impulse * p1.Mass * collisionVectorNorm.Y);
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

        bool CircleIntersect(Particle p1, Particle p2)
        {
            double squareDistance = (p1.Location.X - p2.Location.X) * (p1.Location.X - p2.Location.X) + (p1.Location.Y - p2.Location.Y) * (p1.Location.Y - p2.Location.Y);

            return squareDistance <= ((p1.Radius + p2.Radius) * (p1.Radius + p2.Radius));
        }

        void CheckForTerminalVelocity(ref Particle particle)
        {
            //Console.WriteLine($"{Vector2.Abs(particle.Velocity)}, x:{particle.Velocity.X}, y:{particle.Velocity.Y}");
            if (terminalSpeed >= 0 && Vector2.Abs(particle.Velocity) > terminalSpeed)
            {
                double angle = Vector2.Arg(particle.Velocity);
                particle.Velocity = new Vector2(terminalSpeed, angle, true);
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
                particles[i].Update(gravity / (100 / TickTimer.Interval));
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
            for (int i = 0; i < randomParticlesNumber; i++)
            {
                Vector2 location = new Vector2(random.Next(DrawBox.Width), random.Next(DrawBox.Height));
                Vector2 velocity = new Vector2(random.NextDouble() * random.Next(-10, 10), random.NextDouble() * random.Next(-10, 10));
                //double radius = random.Next(10, 31);
                double radius = 20;
                //double mass = random.NextDouble() * random.Next(1, 31);
                double mass = 1;
                //double hardness = random.NextDouble();
                double hardness = 1;
                Color colour = Color.FromArgb(255, random.Next(0, 256), random.Next(0, 256), random.Next(0, 256));
                particles.Add(new Particle(location, velocity, radius, mass, hardness, colour));
                CheckForWallCollision(particles[i]);
            }
            DrawParticles();
        }
    }
}