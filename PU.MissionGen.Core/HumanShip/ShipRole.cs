namespace PU.MissionGen.Core.HumanShip
{
    public class ShipRole
    {
        public static readonly ShipRole[] StandardRoles = new ShipRole[]
        {
            new ShipRole
            {
                Name = "Scout",
                MinSize = 0,
                MaxSize = 15,
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
                MinPrimaries = 0
            },
            new ShipRole
            {
                Name = "Cruiser",
                MinSize = 10,
                MaxSize = 25,
                VolPerEngine = 15,
                MinEngine = 1,
                VolPerHangar = 15,
                MinHangar = 0,
                MinAirlock = 3,
                MaxAirlock = 5,
                VolPerReactor = 30,
                MinReactor = 1,
                VolPerSecondary = 10,
                MinSecondaries = 1,
                VolPerPrimary = 15,
                MinPrimaries = 9
            },
            new ShipRole
            {
                Name = "Carrier",
                MinSize = 20,
                MaxSize = int.MaxValue,
                VolPerEngine = 15,
                MinEngine = 1,
                VolPerHangar = 10,
                MinHangar = 1,
                MinAirlock = 4,
                MaxAirlock = 8,
                VolPerReactor = 30,
                MinReactor = 1,
                VolPerSecondary = 15,
                MinSecondaries = 1,
                VolPerPrimary = int.MaxValue,
                MinPrimaries = 0
            },
            new ShipRole
            {
                Name = "Battleship",
                MinSize = 25,
                MaxSize = int.MaxValue,
                VolPerEngine = 15,
                MinEngine = 1,
                VolPerHangar = 20,
                MinHangar = 0,
                MinAirlock = 3,
                MaxAirlock = 5,
                VolPerReactor = 30,
                MinReactor = 1,
                VolPerSecondary = 10,
                MinSecondaries = 1,
                VolPerPrimary = 10,
                MinPrimaries = 9
            },
        };

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
    }
}
