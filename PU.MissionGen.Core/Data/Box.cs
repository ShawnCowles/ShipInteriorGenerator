using System.Numerics;

namespace PU.MissionGen.Core.Data
{
    public class Box
    {
        public Vector3 Center { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Length { get; set; }
        
        public Box(Vector3 center, float width, float length, float height)
        {
            Center = center;
            Width = width;
            Length = length;
            Height = height;
        }

        public bool TestHull(float x, float y, float z)
        {
            return x >= Center.X - Width / 2
                && x <= Center.X + Width / 2
                && y >= Center.Y - Length / 2
                && y <= Center.Y + Length / 2
                && z >= Center.Z - Height / 2
                && z <= Center.Z + Height / 2;
        }
    }
}
