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

            node.particle = median;
            node.leftChild = KD_Tree.GenerateKDTree(particles.GetRange(0, medianIndex), depth + 1);
            node.leftChild = KD_Tree.GenerateKDTree(particles.GetRange(medianIndex + 1, particles.Count - (medianIndex + 1)), depth + 1);

            return node;
        }

        public static KD_Node NearestNeighbourSearch(KD_Node root, KD_Node queryNode)
        {
            KD_Node nearestNode = NNSRecursive(root, queryNode, 0);

            return nearestNode;
        }

        private static KD_Node NNSRecursive(KD_Node current, KD_Node target, int depth)
        {
            int axis = depth * Vector2D.Dimensions;
            double direction = axis == 0 ? target.particle.Location.X - current.particle.Location.X : target.particle.Location.Y - current.particle.Location.Y;
            KD_Node next = direction < 0 ? current.leftChild : current.rightChild;
            KD_Node other = direction < 0 ? current.rightChild : current.leftChild;

            KD_Node best = (next == null) ? current : NNSRecursive(next, target, depth + 1);

            if (Vector2D.AbsoluteSquareDifference(current.particle.Location, target.particle.Location) < Vector2D.AbsoluteSquareDifference(best.particle.Location, target.particle.Location))
            {
                best = current;
            }

            if (!(other is null))
            {
                double currentVerticalToTarget = (axis == 0) ? current.particle.Location.X - target.particle.Location.X : current.particle.Location.Y - target.particle.Location.Y;
                if (currentVerticalToTarget < Vector2D.AbsoluteSquareDifference(best.particle.Location, target.particle.Location))
                {
                    KD_Node possibleBest = NNSRecursive(other, target, depth + 1);
                    if (Vector2D.AbsoluteSquareDifference(possibleBest.particle.Location, target.particle.Location) < Vector2D.AbsoluteSquareDifference(best.particle.Location, target.particle.Location))
                    {
                        best = possibleBest;
                    }
                }
            }

            return best;
        }
    }
    
    public class KD_Node
    {
        public Particle particle;

        public KD_Node leftChild;
        public KD_Node rightChild;
    }
}
