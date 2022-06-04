using System;

class TerrainGenerator
{
    private readonly Random _random = new Random();

    public TileModel[,] GenerateTerrainFor(TileModel tile)
    {
        var scale = tile.scale;
        var terrainType = tile.terrain;

        var Tiles = new TileModel[10, 10];

        switch (terrainType)
        {
            case TileModel.TerrainType.InteruniversalSpace:
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        Tiles[x, y] = new TileModel(type: TileModel.TerrainType.InteruniversalSpace, tile, tile.scale - 1);
                    }
                }
                break;
            case TileModel.TerrainType.Universe:
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        if (x == 0 || y == 0 || x == 9 || y == 9)
                        {
                            Tiles[x, y] = new TileModel(type: TileModel.TerrainType.Energy, tile, tile.scale - 1);
                        }
                        else if (_random.Next(0, 2) == 0)
                        {
                            Tiles[x, y] = new TileModel(type: TileModel.TerrainType.Space, tile, tile.scale - 1, zoomable: true);
                        }
                        else
                        {
                            Tiles[x, y] = new TileModel(type: TileModel.TerrainType.Void, tile, tile.scale - 1);
                        }
                    }
                }
                break;
        }

        return Tiles;
    }
}