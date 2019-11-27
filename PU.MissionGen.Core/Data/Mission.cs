using System.Collections.Generic;

namespace PU.MissionGen.Core.Data
{
    public class Mission
    {
        public Tile[,,] Map { get; set; }
        public int Volume { get; set; }
        public int Length { get; set; }
        public string Role { get; set; }
    }
}
