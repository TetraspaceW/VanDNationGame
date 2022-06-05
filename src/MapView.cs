using Godot;
public class MapView : Node2D
{
    private CameraBuddy camera;
    private MapModel Model;
    private TileView[,] Tiles;
    private MapInfoTooltip tooltip;
    public override void _Ready()
    {
        TileModel tile = new TileModel(TileModel.TerrainType.Universe, null, 10, zoomable: true);
        Model = new MapModel(tile);
        tile.internalMap = Model;
        CreateTooltip();
        camera = (CameraBuddy)GetNode("CameraBuddy");
        UpdateWholeMapTo(Model);
    }

    void UpdateTileAtLocation(TileModel newTile, int x, int y)
    {
        Model.Tiles[x, y] = newTile;
        Tiles[x, y].Tile = newTile;
    }

    public void UpdateWholeMapTo(MapModel newModel)
    {
        Model = newModel;

        var (width, height) = (newModel.Tiles.GetLength(0), newModel.Tiles.GetLength(1));
        var (viewWidth, viewHeight) = (Tiles != null) ? (Tiles.GetLength(0), Tiles.GetLength(1)) : (0, 0);

        var newShape = ((viewHeight != height) && (viewWidth != width));


        if (newShape)
        {
            if (Tiles != null)
            {
                foreach (TileView tile in Tiles)
                {
                    tile.QueueFree();
                }
            }
            Tiles = new TileView[width, height];
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (newShape)
                {
                    TileView tileForPosition = GD.Load<PackedScene>("res://src/TileView.tscn").Instance() as TileView;
                    AddChild(tileForPosition);
                    tileForPosition.Tile = newModel.Tiles[x, y];
                    tileForPosition.Position = positionForCoordinates(x, y);
                    Tiles[x, y] = tileForPosition;
                }
                else
                {
                    UpdateTileAtLocation(newModel.Tiles[x, y], x, y);
                }
            }
        }

        tooltip.setText(new ScaleUtil(Model.parent.scale).TextForScale());
        MoveChild(tooltip, GetChildCount());

        camera.SetMapSize(width, height);
    }

    public void CreateTooltip()
    {
        this.tooltip = GD.Load<PackedScene>("res://src/MapInfoTooltip.tscn").Instance() as MapInfoTooltip;
        AddChild(tooltip);
    }

    Vector2 positionForCoordinates(int x, int y) => new Vector2(x * 64 + 32, y * 64 + 32);
}
