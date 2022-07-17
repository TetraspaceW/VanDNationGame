using System.Linq;
public class MapModel
{
    public TileModel[,] Tiles;
    public TileModel parent;

    public MapModel(TileModel parent)
    {
        this.parent = parent;
        Tiles = new TileModel[10, 10];

        var generator = new TerrainGenerator(insideTile: parent);
        Tiles = generator.GenerateTerrain();
    }

    public MapModel(TileModel parent, TileModel[,] tiles)
    {
        this.parent = parent;
        this.Tiles = tiles;
    }

    public TileModel FindHabitablePlanet()
    {
        if (parent.terrain.terrainType == Terrain.TerrainType.VerdantTerrain) { return parent; }
        else
        {
            foreach (var tile in Tiles)
            {
                TileModel foundHabitablePlanet = null;
                if (tile.zoomable && tile.scale >= -10)
                {
                    string dicts;
                    if (parent.terrain.terrainType == Terrain.TerrainType.DwarfGalaxy) { return null; }
                    if (parent.terrain.props.TryGetValue(PropKey.GalaxyType, out dicts) && dicts != "S") { return null; }
                    if (parent.terrain.props.TryGetValue(PropKey.SpectralClass, out dicts) && dicts != "G") { return null; }
                    if (parent.terrain.props.TryGetValue(PropKey.PlanetIsLifeBearing, out dicts) && !bool.Parse(dicts)) { return null; }

                    if (tile.internalMap == null) { tile.internalMap = new MapModel(tile); }
                    foundHabitablePlanet = tile.internalMap.FindHabitablePlanet();
                }

                if (foundHabitablePlanet != null) { return foundHabitablePlanet; }
            }
            return null;
        }
    }
}
