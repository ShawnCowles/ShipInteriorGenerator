using PU.MissionGen.Core.GeometryGen.Data;
using System.Collections.Generic;
using System.Numerics;

namespace PU.MissionGen.Core.Data
{
    public class HullShape : Box
    {
        public IEnumerable<ShipFittings> Fittings { get; set; }

        public HullShape(Vector3 center, float width, float length, float height, IEnumerable<ShipFittings> fittings)
            :base(center, width, length, height)
        {
            Fittings = fittings;
        }
    }
}
