using PU.MissionGen.Core.GeometryFill;

namespace PU.MissionGen.Core.Data
{
    public class RoomSpecification
    {
        public Box[] RoomShapes { get; }
        public int MinCount { get; }
        public double VolumePerCount { get; }
        public int Priority { get; }
        public RoomType RoomType { get; }

        public RoomSpecification(
            int priority,
            Box[] roomShapes, 
            int minCount, 
            double volumePerCount,
            RoomType roomType)
        {
            RoomShapes = roomShapes;
            MinCount = minCount;
            VolumePerCount = volumePerCount;
            RoomType = roomType;
        }
    }
}
