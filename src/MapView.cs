using Godot;
public class MapView : Node2D
{
    private MapModel Model;
    private TileView[,] Tiles;
    private MapInfoTooltip tooltip;
    public override void _Ready()
    {
        TileModel tile = new TileModel(TileModel.TerrainType.Universe, null, 10);
        Model = new MapModel(TileModel.TerrainType.Universe, tile);
        tile.internalMap = Model;
        CreateTooltip();
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

        if (Tiles != null)
        {
            foreach (TileView tile in Tiles)
            {
                RemoveChild(tile);
            }
        }

        Tiles = new TileView[10, 10];
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                TileView tileForPosition = GD.Load<PackedScene>("res://src/TileView.tscn").Instance() as TileView;
                AddChild(tileForPosition);
                tileForPosition.Tile = newModel.Tiles[x, y];
                tileForPosition.Position = positionForCoordinates(x, y);
                Tiles[x, y] = tileForPosition;
            }
        }

        tooltip.setText(new ScaleUtil(Model.parent.scale).TextForScale());
        MoveChild(tooltip, GetChildCount());
    }

    public void CreateTooltip()
    {
        this.tooltip = GD.Load<PackedScene>("res://src/MapInfoTooltip.tscn").Instance() as MapInfoTooltip;
        AddChild(tooltip);
    }

    Vector2 positionForCoordinates(int x, int y) => new Vector2(x * 64 + 32, y * 64 + 32);
}
