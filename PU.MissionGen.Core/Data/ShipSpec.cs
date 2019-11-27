using System.Collections.Generic;

namespace PU.MissionGen.Core.Data
{
    public class ShipSpec
    {
        public string RoleName { get; set; }
        public List<HullShape> ShipGeometry { get; set; }
        public int Seed { get; set; }
        public List<RoomSpecification> RoomsToPlace { get; set; }
    }
}
