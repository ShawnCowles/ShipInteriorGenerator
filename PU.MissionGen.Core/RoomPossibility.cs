namespace PU.MissionGen.Core
{
    public class RoomPossibility
    {
        public int UlX { get; set; }
        public int UlY {get;set;}
        public int Width { get; set; }
        public int Height { get; set; }
        public bool OpenToStbd { get; set; }
        public bool OpenToPort { get; set; }
        public bool OpenToAft { get; set; }
        public bool OpenToBow { get; set; }
    }
}