using PU.MissionGen.Core.Data;

namespace PU.MissionGen.Core
{
    public abstract class AbstractTileset
    {
        public abstract string Name { get; }

        public abstract Mission Generate(ShipSpec shipSpec);
    }
}
