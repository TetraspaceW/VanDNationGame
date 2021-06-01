using Godot;
public class MapView : Node2D
{
    private MapModel model;
    private TileView[,] tiles;
    public override void _Ready()
    {
        model = new MapModel();
        tiles = new TileView[16, 10];
        for (int x = 0; x < 16; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                TileView tileForPosition = new TileView();
                tileForPosition.tile = model.tiles[x, y];
                tileForPosition.Position = positionForCoordinates(x, y);
                AddChild(tileForPosition);
            }
        }
    }

    Vector2 positionForCoordinates(int x, int y) => new Vector2(x * 64 + 32, y * 64 + 32);
}