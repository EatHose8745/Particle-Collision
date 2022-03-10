using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_Collision
{
    public static class KD_Tree
    {
        public static KD_Node GenerateKDTree(List<Vector2D> particles, int depth = 0)
        {
            if (particles.Count == 0)
                return null;

            KD_Node node = new KD_Node();

            int A = depth % Vector2D.Dimensions;

            particles = A == 0 ? particles.OrderBy(p => p.X).ToList() : particles.OrderBy(p => p.Y).ToList();

            int medianIndex = particles.Count / 2;
            Vector2D median = particles[medianIndex];

            node.Location = median;
            node.LeftChild = KD_Tree.GenerateKDTree(particles.GetRange(0, medianIndex), depth + 1);
            node.RightChild = KD_Tree.GenerateKDTree(particles.GetRange(medianIndex + 1, particles.Count - (medianIndex + 1)), depth + 1);
            return node;
        }

        public static KD_Node NearestNeighbour(KD_Node root, Vector2D target, int depth)
        {
            if (root == null)
                return null;

            KD_Node nextBranch;
            KD_Node otherBranch;

            if (Vector2D.AbsoluteSquareDifference(root.Location, root.LeftChild.Location) > Vector2D.AbsoluteSquareDifference(root.Location, root.RightChild.Location))
            {
                nextBranch = root.RightChild;
                otherBranch = root.LeftChild;
            }
            else
            {
                nextBranch = root.LeftChild;
                otherBranch = root.RightChild;
            }

            KD_Node temp = NearestNeighbour(nextBranch, target, depth + 1);
            KD_Node best = Vector2D.Closest(temp.Location, root.Location, target) == temp.Location ? temp : root;

            double radiusSquared = Vector2D.AbsoluteSquareDifference(target, best.Location);

            double dist = Vector2D.AbsoluteSquareDifference(root.Location, root.LeftChild.Location) - Vector2D.AbsoluteSquareDifference(root.Location, root.RightChild.Location);

            if (radiusSquared >= dist * dist)
            {
                temp = NearestNeighbour(otherBranch, target, depth + 1);
                best = Vector2D.Closest(temp.Location, best.Location, target) == temp.Location ? temp : best;
            }

            return best;
        }
    }
    
    public class KD_Node
    {
        public Vector2D Location { get; set; } = new Vector2D();
        public KD_Node LeftChild { get; set; }
        public KD_Node RightChild { get; set; }
    }
}
