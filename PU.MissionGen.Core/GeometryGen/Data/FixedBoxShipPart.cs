using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using PU.MissionGen.Core.Data;

namespace PU.MissionGen.Core.GeometryGen.Data
{
    public class FixedBoxShipPart : IShipPart
    {
        public FuzzyDimension Width { get; }
        public FuzzyDimension Length { get; }
        public FuzzyDimension Height { get; }
        
        public List<PartNode> PartNodes { get; }

        public IEnumerable<ShipFittings> Fittings { get; set; }

        public FixedBoxShipPart(FuzzyDimension width, FuzzyDimension length, FuzzyDimension height)
        {
            Width = width;
            Length = length;
            Height = height;

            PartNodes = new List<PartNode>();
            Fittings = Enumerable.Empty<ShipFittings>();
        }

        public HullShape CreateShape(Random random, Vector3 center, int targetLength, int parentDimension)
        {
            return new HullShape(
                center,
                Width.Evaluate(random),
                Length.Evaluate(random),
                Height.Evaluate(random),
                Fittings);
        }
    }
}
