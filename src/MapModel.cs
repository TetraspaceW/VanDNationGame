public class MapModel
{
    public TileModel[,] tiles;

    public MapModel()
    {
        // CSharp has gotta have a better way to handle multidimensional arrays
        tiles = new TileModel[16, 10];
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                tiles[x, y] = new TileModel(type: TileModel.TerrainType.Space);
            }
        }
    }
}