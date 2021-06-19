using System;
public class MapModel
{
    public TileModel[,] Tiles;
    private readonly Random _random = new Random();

    public MapModel(TileModel.TerrainType terrainType)
    {
        Tiles = new TileModel[16, 10];
        switch (terrainType)
        {
            case TileModel.TerrainType.Space:
                fillMapWith(terrainType); break;
            case TileModel.TerrainType.Void:
                fillMapWith(terrainType); break;
            case TileModel.TerrainType.Energy:
                fillMapWith(terrainType); break;
            case TileModel.TerrainType.Defect:
                fillMapWith(terrainType); break;
            default:
                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (x == 0 || y == 0 || x == 9 || y == 9)
                        {
                            Tiles[x, y] = new TileModel(type: TileModel.TerrainType.Energy);
                        }
                        else if (_random.Next(0, 2) == 0)
                        {
                            Tiles[x, y] = new TileModel(type: TileModel.TerrainType.Space);
                        }
                        else
                        {
                            Tiles[x, y] = new TileModel(type: TileModel.TerrainType.Void);
                        }
                    }
                }
                break;
        }
    }

    private void fillMapWith(TileModel.TerrainType terrainType)
    {
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Tiles[x, y] = new TileModel(terrainType);
            }
        }
    }
}