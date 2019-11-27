using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using PU.MissionGen.Core.Data;

namespace PU.MissionGen.Core.GeometryGen.Data
{
    public class RelativeBoxShipPart : IShipPart
    {
        public FuzzyDimension Width { get; }
        public FuzzyDimension Length { get; }
        public FuzzyDimension Height { get; }
        
        public List<PartNode> PartNodes { get; }

        public IEnumerable<ShipFittings> Fittings { get; set; }

        public RelativeBoxShipPart(FuzzyDimension width, FuzzyDimension length, FuzzyDimension height)
        {
            Width = width;
            Length = length;
            Height = height;
            Fittings = Enumerable.Empty<ShipFittings>();
            PartNodes = new List<PartNode>();
        }

        public HullShape CreateShape(Random random, Vector3 center, int targetLength, int parentDimension)
        {
            return new HullShape(
                center,
                Width.Evaluate(random) * targetLength,
                Length.Evaluate(random) * targetLength,
                Height.Evaluate(random) * targetLength,
                Fittings);
        }
    }
}
