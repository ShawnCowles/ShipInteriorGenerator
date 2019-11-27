using System.Numerics;

namespace PU.MissionGen.Core.GeometryFill
{
    public interface IHullShape
    {
        Vector3 Center { get; set; }
        float Width { get; set; }
        float Height { get; set; }
        float Length { get; set; }

        bool TestHull(float x, float y, int z);
    }
}