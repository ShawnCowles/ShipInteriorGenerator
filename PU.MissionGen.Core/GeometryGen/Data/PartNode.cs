namespace PU.MissionGen.Core.GeometryGen.Data
{
    public class PartNode
    {
        public double BaseChance { get; }
        public FuzzyDimension RelativeX { get; }
        public FuzzyDimension RelativeY { get; }
        public FuzzyDimension RelativeZ { get; }
        public IShipPart[] ChildParts { get; }

        public PartNode(
            double baseChance,
            FuzzyDimension relativeX,
            FuzzyDimension relativeY, 
            FuzzyDimension relativeZ,
            IShipPart[] childParts)
        {
            BaseChance = baseChance;
            RelativeX = relativeX;
            RelativeY = relativeY;
            RelativeZ = relativeZ;
            ChildParts = childParts;
        }
    }
}
