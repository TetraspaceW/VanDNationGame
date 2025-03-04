using System.Collections.Generic;
using System.Linq;
public partial class MapModel
{
	public TileModel[,] Tiles;
	public TileModel parent;
	public List<Building> Buildings = new();

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
		Tiles = tiles;
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
					if (parent.terrain.terrainType == Terrain.TerrainType.Star || parent.terrain.terrainType == Terrain.TerrainType.GasGiant) { return null; }
					if (parent.terrain.props.TryGetValue(PropKey.PlanetIsLifeBearing, out string prop) && !bool.Parse(prop)) { return null; }
					if (parent.terrain.props.TryGetValue(PropKey.PlanetHydrosphereCoverage, out prop) && (double.Parse(prop) < 25 || double.Parse(prop) > 75)) { return null; }

					tile.internalMap ??= new MapModel(tile);
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
			_ = TryPlaceBuildingAt(
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
			_ = parent.CalculateStorageBuildings(false);
			_ = parent.CalculateTotalChildCapacity();
			_ = parent.CalculateTotalChildResources();
			if (!ignoreCost)
			{
				parent.SubtractResource(building.cost.resource, building.cost.amount);
			}
			_ = TileModel.activeTiles.Add(parent);
			_ = parent.UpdateHighestTransportInside(false);
			_ = parent.CalculateStorageBuildings(false);
			_ = parent.CalculateTotalChildCapacity();
			_ = parent.CalculateTotalChildResources();
		}
		return canPlaceBuildingHere;

	}

	public Building GetBuildingAt(int x, int y)
	{
		return Buildings.FirstOrDefault((building) => { return building.coords.Item1 == x && building.coords.Item2 == y; });
	}

	private (int, int) GetUnoccupiedTileOfType(Terrain.TerrainType type)
	{
		var (width, height) = TerrainGenerator.Shape3D(Tiles);
		List<(int, int)> possibleLocations = new();

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
		var (width, height) = TerrainGenerator.Shape3D(Tiles);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				_ = terrainTypes.Add(Tiles[x, y].terrain.terrainType);
			}
		}

		return terrainTypes;
	}

	public void NextTurn()
	{
		parent.BuildingResourcesTick();
	}
}
