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
            var yWidth = Tiles.GetLength(1);
            foreach (var i in Enumerable.Range(0, Tiles.Length).OrderBy(x => RND.Next()))
            {
                var tile = Tiles[i / yWidth, i % yWidth];
                TileModel foundHabitablePlanet = null;
                if (tile.zoomable && tile.scale >= -10)
                {
                    string dicts;
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

    public void PlaceStartingBuildings(List<string> buildings)
    {
        buildings.ForEach((building) =>
        {
            TryPlaceBuildingAt(
                BuildingTemplateList.Get(building),
                GetUnoccupiedTileOfType(Terrain.TerrainType.VerdantTerrain),
                ignoreCost: true
            );
        });
    }

    public bool TryPlaceBuildingAt(BuildingTemplate building, (int, int) coords, bool ignoreCost = false)
    {
        bool canPlaceBuildingHere = building.terrainTypes.Contains(Tiles[coords.Item1, coords.Item2].terrain.terrainType);
        if (canPlaceBuildingHere)
        {
            Buildings.Add(new Building(
                coords,
                building
            ));
            if (!ignoreCost)
            {
                parent.SubtractResource(building.cost.resource, building.cost.amount);
            }
        }
        return canPlaceBuildingHere;

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

    public void NextTurn()
    {
        parent.BuildingResourcesTick();
        foreach (var tile in Tiles)
        {
            if (tile.internalMap != null)
            {
                tile.internalMap.NextTurn();
            }
        }
    }
}
