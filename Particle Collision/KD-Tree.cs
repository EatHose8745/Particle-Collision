using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_Collision
{
    public static class KD_Tree
    {
        public static KD_Node GenerateKDTree(List<Particle> particles, int depth = 0)
        {
            if (particles.Count == 0)
                return null;

            KD_Node node = new KD_Node();

            int A = depth % Vector2D.Dimensions;

            particles = A == 0 ? particles.OrderBy(p => p.Location.X).ToList() : particles.OrderBy(p => p.Location.Y).ToList();

            int medianIndex = particles.Count / 2;
            Particle median = particles[medianIndex];

            node.Location = median.Location;
            node.LeftChild = KD_Tree.GenerateKDTree(particles.GetRange(0, medianIndex), depth + 1);
            node.RightChild = KD_Tree.GenerateKDTree(particles.GetRange(medianIndex + 1, particles.Count - (medianIndex + 1)), depth + 1);
            return node;
        }

        public static KD_Node NearestNeighbour(KD_Node root, Vector2D target, int depth)
        {
            // Did work but really slow, gonna fix

            return null;
        }
    }
    
    public class KD_Node
    {
        public Vector2D Location { get; set; }
        public KD_Node LeftChild { get; set; }
        public KD_Node RightChild { get; set; }
    }
}
