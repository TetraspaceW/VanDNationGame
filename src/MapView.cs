using Godot;
public class MapView : Node2D
{
    private MapModel model;
    private TileView[,] tiles;
    public override void _Ready()
    {
        model = new MapModel();
        updateWholeMapTo(model);
        updateTileAtLocation(new TileModel(TileModel.TerrainType.Energy), 8, 8);
    }

    void updateTileAtLocation(TileModel newTile, int x, int y)
    {
        model.tiles[x, y] = newTile;
        tiles[x, y].tile = newTile;
    }

    void updateWholeMapTo(MapModel newModel)
    {
        model = newModel;

        if (tiles != null)
        {
            foreach (TileView tile in tiles)
            {
                RemoveChild(tile);
            }
        }

        tiles = new TileView[10, 10];
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                TileView tileForPosition = GD.Load<PackedScene>("res://src/TileView.tscn").Instance() as TileView;
                AddChild(tileForPosition);
                tileForPosition.tile = newModel.tiles[x, y];
                tileForPosition.Position = positionForCoordinates(x, y);
                tiles[x, y] = tileForPosition;
            }
        }

    }

    Vector2 positionForCoordinates(int x, int y) => new Vector2(x * 64 + 32, y * 64 + 32);
}