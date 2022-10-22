using System.Collections.Generic;
using System.Linq;
public class MapModel
{
    public TileModel[,] Tiles;
    public TileModel parent;
    public List<Building> Buildings = new List<Building>();

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
                    if (parent.terrain.props.TryGetValue(PropKey.PlanetHydrosphereCoverage, out dicts) && (double.Parse(dicts) < 25 || double.Parse(dicts) > 75)) { return null; }

                    if (tile.internalMap == null) { tile.internalMap = new MapModel(tile); }
                    foundHabitablePlanet = tile.internalMap.FindHabitablePlanet();
                }

                if (foundHabitablePlanet != null) { return foundHabitablePlanet; }
            }
            return null;
        }
    }

    public void PlaceStartingBuildings()
    {
        Buildings.Add(new Building(
            GetUnoccupiedTileOfType(Terrain.TerrainType.VerdantTerrain),
            BuildingTemplateList.Get("Airport"))
        );
        Buildings.Add(new Building(
            GetUnoccupiedTileOfType(Terrain.TerrainType.VerdantTerrain),
            BuildingTemplateList.Get("Mine"))
        );
    }

    public Building GetBuildingAt(int x, int y)
    {
        return Buildings.FirstOrDefault((building) => { return (building.coords.Item1 == x && building.coords.Item2 == y); });
    }

    private (int, int) GetUnoccupiedTileOfType(Terrain.TerrainType type)
    {
        var (width, height) = TerrainGenerator.Shape(Tiles);
        List<(int, int)> possibleLocations = new List<(int, int)>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (Tiles[x, y].terrain.terrainType == type && GetBuildingAt(x, y) == null) { possibleLocations.Add((x, y)); }
            }
        }

        return possibleLocations[RND.Next(0, possibleLocations.Count)];
    }

    public int GetTileScale()
    {
        return parent.scale - 1;
    }

    public HashSet<Terrain.TerrainType> GetTerrainTypes()
    {
        var terrainTypes = new HashSet<Terrain.TerrainType>();
        var (width, height) = TerrainGenerator.Shape(Tiles);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                terrainTypes.Add(Tiles[x, y].terrain.terrainType);
            }
        }

        return terrainTypes;
    }
}
