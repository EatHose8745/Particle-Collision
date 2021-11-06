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
        private double gravity = 0.0981;
        private double terminalSpeed = 1;
        private Random random = new Random("test".GetHashCode());

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
            particles.Add(new Particle(new Vector2(200, 200), new Vector2(10, 0), 20, 5, 0.5, Color.Red));
            particles.Add(new Particle(new Vector2(200 + 500, 200 - 20), new Vector2(-10, 0), 20, 1, 1, Color.Red));
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

        void UpdateParticlePosition()
        {
            for (int i = 0; i < particles.Count(); i++)
            {
                particles[i].Location = particles[i].Location + particles[i].Velocity;
            }
        }

        void UpdateParticleVelocity()
        {
            for (int i = 0; i < particles.Count(); i++)
            {
                particles[i].Velocity.Y += gravity;
            }
        }

        void CheckForCollsion()
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
                        particle1.Colour = Color.Blue;
                        particle2.Colour = Color.Blue;
                        
                    }
                    else
                    {
                        particle1.Colour = Color.Red;
                        particle2.Colour = Color.Red;
                    }
                }
            }
        }

        void CollideParticles(Particle p1, Particle p2)
        {
            Vector2 collisionVector = new Vector2(p2.Location.X - p1.Location.X, p2.Location.Y - p1.Location.Y);
            double distance = Math.Sqrt((p2.Location.X - p1.Location.X) * (p2.Location.X - p1.Location.X) + (p2.Location.Y - p1.Location.Y) * (p2.Location.Y - p1.Location.Y));

            Vector2 collisionVectorNorm = new Vector2(collisionVector.X / distance, collisionVector.Y / distance);

            Vector2 relativeVelocityVector = new Vector2(p1.Velocity.X - p2.Velocity.X, p2.Velocity.Y - p1.Velocity.Y);
            double speed = relativeVelocityVector.X * collisionVectorNorm.X + relativeVelocityVector.Y * collisionVectorNorm.Y;

            speed *= Math.Min(p1.Hardness, p2.Hardness);

            double impulse = 2 * speed / (p1.Mass + p2.Mass);

            p1.Velocity.X -= (impulse * p2.Mass * collisionVectorNorm.X);
            p1.Velocity.Y -= (impulse * p2.Mass * collisionVectorNorm.Y);
            p2.Velocity.X += (impulse * p1.Mass * collisionVectorNorm.X);
            p2.Velocity.Y += (impulse * p1.Mass * collisionVectorNorm.Y);
        }

        void CheckForWallCollision()
        {
            Particle particle;
            for (int i = 0; i < particles.Count(); i++)
            {
                particle = particles[i];
                double speedDampen = particle.Hardness;

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

                particles[i] = particle;
            }
        }

        bool CircleIntersect(Particle p1, Particle p2)
        {
            double squareDistance = (p1.Location.X - p2.Location.X) * (p1.Location.X - p2.Location.X) + (p1.Location.Y - p2.Location.Y) * (p1.Location.Y - p2.Location.Y);

            return squareDistance <= ((p1.Radius + p2.Radius) * (p1.Radius + p2.Radius));
        }

        void CheckForTerminalVelocity(ref Particle p)
        {
            if (Vector2.Abs(p.Velocity) > terminalSpeed)
            {
                p.Velocity *= 0;
            }
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            UpdateParticlePosition();
            UpdateParticleVelocity();
            CheckForWallCollision();
            CheckForCollsion();
            DrawParticles();
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
            particles = new List<Particle>();
            AddParticles();
            DrawParticles();
        }
    }
}