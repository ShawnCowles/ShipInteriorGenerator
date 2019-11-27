using PU.MissionGen.Core.Data;
using PU.MissionGen.Core.GeometryFill;
using System.Collections.Generic;
using System.Numerics;

namespace PU.MissionGen.Core.GeometryGen.Data
{
    public class ShipRole
    {
        public string Name { get; private set; }
        public int MinSize { get; private set; }
        public int MaxSize { get; private set; }
        public int VolPerEngine { get; private set; }
        public int MinEngine { get; private set; }
        public int VolPerHangar { get; private set; }
        public int MinHangar { get; private set; }
        public int MinAirlock { get; private set; }
        public int MaxAirlock { get; private set; }
        public int VolPerReactor { get; private set; }
        public int MinReactor { get; private set; }
        public int VolPerSecondary { get; private set; }
        public int MinSecondaries { get; private set; }
        public int VolPerPrimary { get; private set; }
        public int MinPrimaries { get; private set; }
        public IShipPart BasePart { get; private set; }
        public List<RoomSpecification> RoomsToPlace { get; private set; }

        public static IEnumerable<ShipRole> BuildStandardRoles()
        {
            return new ShipRole[]
            {
                BuildFrigateRole(),
                BuildFreighterRole()
            };
        }


        public static ShipRole BuildFrigateRole()
        {
            var hull = new RelativeBoxShipPart(new FuzzyDimension(0.25f, 0.3f), new FuzzyDimension(0.8f, 1.0f), new FuzzyDimension(0.2f, 0.3f));
            var aftBox = new RelativeBoxShipPart(new FuzzyDimension(0.35f, 0.5f), new FuzzyDimension(0.4f, 0.5f), new FuzzyDimension(0.2f, 0.3f));
            var simpleWing = new RelativeBoxShipPart(new FuzzyDimension(0.7f, 0.9f), new FuzzyDimension(0.2f, 0.3f), new FuzzyDimension(0.1f, 0.2f));
            var enginePodWing = new RelativeBoxShipPart(new FuzzyDimension(0.7f, 0.9f), new FuzzyDimension(0.2f, 0.3f), new FuzzyDimension(0.1f, 0.2f));
            var enginePod = new FixedBoxShipPart(new FuzzyDimension(5, 7), new FuzzyDimension(15, 25), new FuzzyDimension(5, 7))
            {
                Fittings = new[] { ShipFittings.Engines }
            };

            hull.PartNodes.Add(new PartNode(1.0, new FuzzyDimension(0, 0), new FuzzyDimension(-0.2f, 0.5f), new FuzzyDimension(0, 0), new[] { aftBox }));
            hull.PartNodes.Add(new PartNode(1.0, new FuzzyDimension(0, 0), new FuzzyDimension(-0.2f, 0.5f), new FuzzyDimension(0, 0), new[] { simpleWing, enginePodWing }));
            hull.PartNodes.Add(new PartNode(1.0, new FuzzyDimension(0, 0), new FuzzyDimension(-0.4f, 0.2f), new FuzzyDimension(0, 0), new[] { simpleWing }));
            hull.PartNodes.Add(new PartNode(0.5, new FuzzyDimension(0.1f, 0.2f), new FuzzyDimension(0.5f, 0.5f), new FuzzyDimension(0, 0), new[] { enginePod }));
            enginePodWing.PartNodes.Add(new PartNode(1.0, new FuzzyDimension(0.5f, 0.5f), new FuzzyDimension(-0.2f, 0.2f), new FuzzyDimension(0, 0), new[] { enginePod }));

            return new ShipRole
            {
                Name = "Frigate",
                MinSize = 30,
                MaxSize = 45,
                VolPerEngine = 10,
                MinEngine = 1,
                VolPerHangar = int.MaxValue,
                MinHangar = 0,
                MinAirlock = 1,
                MaxAirlock = 3,
                VolPerReactor = 30,
                MinReactor = 1,
                VolPerSecondary = 10,
                MinSecondaries = 1,
                VolPerPrimary = int.MaxValue,
                MinPrimaries = 0,
                BasePart = hull,
                RoomsToPlace = new List<RoomSpecification>
                {
                    new RoomSpecification(10, new [] { new Box(Vector3.Zero, 3, 6, 2)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 2, 2, 1)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 4, 4, 2)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 6, 4, 1)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 4, 6, 2)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 2, 4, 1)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 4, 2, 1)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 6, 10, 3)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 10, 6, 3)}, 0, 20, RoomType.MiscRoom)
                }
            };
        }

        public static ShipRole BuildFreighterRole()
        {
            var body = new RelativeBoxShipPart(new FuzzyDimension(0.2f, 0.3f), new FuzzyDimension(0.8f, 1.0f), new FuzzyDimension(0.2f, 0.3f));
            var bulge = new RelativeBoxShipPart(new FuzzyDimension(0.3f, 0.4f), new FuzzyDimension(0.7f, 0.8f), new FuzzyDimension(0.1f, 0.2f));
            var foreWing = new RelativeBoxShipPart(new FuzzyDimension(0.4f, 0.45f), new FuzzyDimension(0.6f, 0.7f), new FuzzyDimension(0.08f, 0.08f));
            var wing = new RelativeBoxShipPart(new FuzzyDimension(0.5f, 0.6f), new FuzzyDimension(0.2f, 0.3f), new FuzzyDimension(0.08f, 0.08f));
            var enginePod = new FixedBoxShipPart(new FuzzyDimension(7, 7), new FuzzyDimension(10, 15), new FuzzyDimension(7, 7))
            {
                Fittings = new[] { ShipFittings.Engines }
            };

            body.PartNodes.Add(new PartNode(0.8, new FuzzyDimension(0, 0), new FuzzyDimension(0, 0), new FuzzyDimension(0, 0), new[] { bulge }));
            body.PartNodes.Add(new PartNode(0.8, new FuzzyDimension(0, 0), new FuzzyDimension(0, 0), new FuzzyDimension(0, 0), new[] { foreWing }));
            body.PartNodes.Add(new PartNode(0.8, new FuzzyDimension(0, 0), new FuzzyDimension(-0.2f, 0.4f), new FuzzyDimension(0, 0), new[] { wing }));
            wing.PartNodes.Add(new PartNode(0.5, new FuzzyDimension(0.5f, 0.5f), new FuzzyDimension(0, 0), new FuzzyDimension(0, 0), new[] { enginePod }));
            body.PartNodes.Add(new PartNode(1.0, new FuzzyDimension(0, 0), new FuzzyDimension(0.5f, 0.5f), new FuzzyDimension(0, 0), new[] { enginePod }));
            body.PartNodes.Add(new PartNode(0.7, new FuzzyDimension(0, 0), new FuzzyDimension(0.45f, 0.5f), new FuzzyDimension(-0.5f, -0.55f), new[] { enginePod }));
            body.PartNodes.Add(new PartNode(0.7, new FuzzyDimension(0, 0), new FuzzyDimension(0.45f, 0.5f), new FuzzyDimension(0.5f, 0.55f), new[] { enginePod }));

            return new ShipRole
            {
                Name = "Freighter",
                MinSize = 50,
                MaxSize = 70,
                VolPerEngine = 10,
                MinEngine = 1,
                VolPerHangar = int.MaxValue,
                MinHangar = 0,
                MinAirlock = 1,
                MaxAirlock = 3,
                VolPerReactor = 30,
                MinReactor = 1,
                VolPerSecondary = 10,
                MinSecondaries = 1,
                VolPerPrimary = int.MaxValue,
                MinPrimaries = 0,
                BasePart = body,
                RoomsToPlace = new List<RoomSpecification>
                {
                    new RoomSpecification(10, new [] { new Box(Vector3.Zero, 3, 6, 2)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 2, 2, 1)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 4, 4, 2)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 6, 4, 1)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 4, 6, 2)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 2, 4, 1)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 4, 2, 1)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 6, 10, 3)}, 0, 20, RoomType.MiscRoom),
                    new RoomSpecification(20, new [] { new Box(Vector3.Zero, 10, 6, 3)}, 0, 20, RoomType.MiscRoom)
                }
            };
        }
    }
}
