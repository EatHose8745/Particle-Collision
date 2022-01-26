using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particle_Collision
{
    public static class KD_Tree
    {
        public static KD_Node GenerateKDTree(List<Particle> points, int depth = 0)
        {
            if (points.Count == 0)
                return null;

            KD_Node node = new KD_Node();

            int A = depth % Vector2D.Dimensions;

            points = A == 0 ? points.OrderBy(p => p.Location.X).ToList() : points.OrderBy(p => p.Location.Y).ToList();

            int medianIndex = points.Count / 2;
            Particle median = points[medianIndex];

            node.Location = median.Location;
            node.LeftChild = KD_Tree.GenerateKDTree(points.GetRange(0, medianIndex), depth + 1);
            node.RightChild = KD_Tree.GenerateKDTree(points.GetRange(medianIndex + 1, points.Count - (medianIndex + 1)), depth + 1);
            return node;
        }

        public static KD_Node NearestNeighbour(KD_Node root, Vector2D target, int depth)
        {
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
