using PU.MissionGen.Core.Data;
using PU.MissionGen.Core.GeometryGen.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace PU.MissionGen.Core.GeometryGen
{
    public class ShipGeometryGenerator
    {
        public ShipSpec BuildShipSpec(int seed)
        {
            var random = new Random(seed);

            var role = random.Choose(ShipRole.BuildStandardRoles());

            var targetLength = random.Next(role.MinSize, role.MaxSize);

            var shipGeometry = new List<HullShape>();

            shipGeometry.AddRange(PlacePart(random, 0, targetLength, targetLength, Vector3.Zero, role.BasePart));
            
            return new ShipSpec
            {
                RoleName = role.Name,
                Seed = seed,
                ShipGeometry = shipGeometry,
                RoomsToPlace = role.RoomsToPlace

            };
        }

        private IEnumerable<HullShape> PlacePart(
            Random random, 
            int iterationDepth, 
            int targetLength,
            int parentDimension, 
            Vector3 root,
            IShipPart part)
        {
            if(iterationDepth > 100)
            {
                return Enumerable.Empty<HullShape>();
            }

            var boxes = new List<HullShape>();

            var shape = part.CreateShape(random, root, targetLength, parentDimension);
            
            boxes.Add(shape);

            if(shape.Center.X != 0)
            {
                boxes.Add(new HullShape(
                    new Vector3(shape.Center.X * -1, shape.Center.Y, shape.Center.Z),
                    shape.Width,
                    shape.Length,
                    shape.Height,
                    shape.Fittings));
            }

            foreach(var node in part.PartNodes)
            {
                if(random.NextDouble() < node.BaseChance && node.ChildParts.Any())
                {
                    var nodePos = root +
                        new Vector3(
                            node.RelativeX.Evaluate(random) * shape.Width,
                            node.RelativeY.Evaluate(random) * shape.Length,
                            node.RelativeZ.Evaluate(random) * shape.Height);

                    var childPart = random.Choose(node.ChildParts);
                    
                    boxes.AddRange(PlacePart(
                        random, 
                        iterationDepth + 1, 
                        targetLength, 
                        (int)Math.Max(shape.Width, Math.Max(shape.Height, shape.Length)), 
                        nodePos, 
                        childPart));
                }
            }

            return boxes;
        }
    }
}
