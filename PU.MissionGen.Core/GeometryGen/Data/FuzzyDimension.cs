using System;

namespace PU.MissionGen.Core.GeometryGen.Data
{
    public class FuzzyDimension
    {
        public float Min { get; }
        public float Max { get; }

        public FuzzyDimension(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float Evaluate(Random random)
        {
            return (float)(random.NextDouble() * (Max - Min) + Min);
        }
    }
}
