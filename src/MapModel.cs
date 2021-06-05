public class MapModel
{
    public TileModel[,] Tiles;

    public MapModel()
    {
        // CSharp has gotta have a better way to handle multidimensional arrays
        Tiles = new TileModel[16, 10];
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                Tiles[x, y] = new TileModel(type: TileModel.TerrainType.Space);
            }
        }
    }
}