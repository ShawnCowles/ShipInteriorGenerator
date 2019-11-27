namespace PU.MissionGen.Core.GeometryFill
{
    public class PlacedRoom
    {
        public int StartBlockX { get; set; }
        public int StartBlockY { get; set; }
        public int BlocksWide { get; set; }
        public int BlocksHigh { get; set; }
        public RoomType RoomType { get; set; }
        public bool OpenToStbd { get; set; }
        public bool OpenToPort { get; set; }
        public bool OpenToAft { get; set; }
        public bool OpenToBow { get; set; }
    }
}
