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
        private double gravity = 0;
        private double terminalSpeed = 10;
        private double wallHardness = 1;
        private Random random = new Random("testseed".GetHashCode());

        public ParticleEnvironment()
        {
            InitializeComponent();
        }

        private void ParticleEnvironment_Load(object sender, EventArgs e)
        {
            AddParticles();
            DrawBox.Image = new Bitmap(DrawBox.Width, DrawBox.Height);
        }

        void AddParticles()
        {
            particles.Add(new Particle(new Vector2(this.Width / 2 - 250, this.Height / 2), new Vector2(1, 0), 20, 1, 1, Color.Green));
            particles.Add(new Particle(new Vector2(this.Width / 2, this.Height / 2 - 10), new Vector2(-1, 0), 20, 1, 1, Color.Red));
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

        void CheckForCollsions()
        {
            for (int i = 0; i < particles.Count(); i++)
            {
                Particle particle1 = particles[i];
                for (int j = i + 1; j < particles.Count(); j++)
                {
                    Particle particle2 = particles[j];

                    if (CircleIntersect(particle1, particle2))
                    {
                        CollideParticles(particle1, particle2);
                    }
                }
            }
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
            double speedDampen = this.wallHardness;

            if (particle.Location.X < particle.Radius)
            {
                particle.Velocity.X = Math.Abs(particle.Velocity.X) * speedDampen;
                particle.Location.X = particle.Radius;
            }
            else if (particle.Location.X > DrawBox.Width - particle.Radius)
            {
                particle.Velocity.X = -Math.Abs(particle.Velocity.X) * speedDampen;
                particle.Location.X = DrawBox.Width - particle.Radius;
            }

            if (particle.Location.Y < particle.Radius)
            {
                particle.Velocity.Y = Math.Abs(particle.Velocity.Y) * speedDampen;
                particle.Location.Y = particle.Radius;
            }
            else if (particle.Location.Y > DrawBox.Height - particle.Radius)
            {
                particle.Velocity.Y = -Math.Abs(particle.Velocity.Y) * speedDampen;
                particle.Location.Y = DrawBox.Height - particle.Radius;
            }
        }

        bool CircleIntersect(Particle p1, Particle p2)
        {
            double squareDistance = (p1.Location.X - p2.Location.X) * (p1.Location.X - p2.Location.X) + (p1.Location.Y - p2.Location.Y) * (p1.Location.Y - p2.Location.Y);

            return squareDistance <= ((p1.Radius + p2.Radius) * (p1.Radius + p2.Radius));
        }

        void CheckForTerminalVelocity(Particle particle)
        {
            Console.WriteLine($"{Vector2.Abs(particle.Velocity)}, x:{particle.Velocity.X}, y:{particle.Velocity.Y}");
            if (Vector2.Abs(particle.Velocity) > terminalSpeed)
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
                particles[i].Update(gravity / (1000 / TickTimer.Interval));
            }
            CheckForCollsions();
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
            TickTimer.Stop();
            timerRunning = false;
            TimerTicksDisplay.Text = "0";
            timerTicks = 0;
            particles = new List<Particle>();
            AddParticles();
            DrawParticles();
        }

        private void ParticleEnvironment_SizeChanged(object sender, EventArgs e)
        {

            DrawBox.Size = new Size(this.Width - 107, this.Height - 39);
            ButtonPanel.Location = new Point(DrawBox.Width + 6, ButtonPanel.Location.Y);
            DrawBox.Image = new Bitmap(DrawBox.Width, DrawBox.Height);
        }
    }
}