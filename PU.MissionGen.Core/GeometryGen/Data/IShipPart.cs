using PU.MissionGen.Core.Data;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace PU.MissionGen.Core.GeometryGen.Data
{
    public interface IShipPart
    {
        FuzzyDimension Width { get; }
        FuzzyDimension Length { get; }
        FuzzyDimension Height { get; }

        List<PartNode> PartNodes { get; }

        IEnumerable<ShipFittings> Fittings { get; set; }

        HullShape CreateShape(Random random, Vector3 center, int targetLength, int parentDimension);
    }
}
