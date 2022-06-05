using System;
public class MapModel
{
    public TileModel[,] Tiles;
    private readonly Random _random = new Random();
    public TileModel parent;

    public MapModel(TileModel parent)
    {
        this.parent = parent;
        Tiles = new TileModel[10, 10];

        var generator = new TerrainGenerator(insideTile: parent);
        Tiles = generator.GenerateTerrain();
    }

    private void fillMapWith(TileModel.TerrainType terrainType)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Tiles[x, y] = new TileModel(terrainType, parent, parent.scale - 1);
            }
        }
    }

    public int randomNumber(int low, int high)
    {
        return _random.Next(low, high);
    }
}
