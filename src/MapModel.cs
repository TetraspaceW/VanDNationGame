using System;
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
}
