using Godot;
public class MapView : Area2D
{
    private CameraBuddy camera;
    private CollisionShape2D collision;
    private MapModel Model;
    private TileMap Tiles;
    private MapInfoTooltip tooltip;
    public override void _Ready()
    {
        TileModel tile = new TileModel(new Terrain(Terrain.TerrainType.Universe), null, 10, zoomable: true);
        Model = new MapModel(tile);
        tile.internalMap = Model;
        CreateTooltip();
        camera = (CameraBuddy)GetNode("CameraBuddy");
        collision = (CollisionShape2D)GetNode("CollisionShape2D");
        CreateTileMap();
        UpdateWholeMapTo(Model);
    }

    void CreateTileMap()
    {
        Tiles = new TileMap();
        Tiles.TileSet = CreateTileset();
        AddChild(Tiles);
    }

    void UpdateTileAtLocation(TileModel newTile, int x, int y)
    {
        Model.Tiles[x, y] = newTile;
        Tiles.SetCell(x, y, Tiles.TileSet.FindTileByName(newTile.image));
    }

    public void UpdateWholeMapTo(MapModel newModel)
    {
        Model = newModel;

        var (width, height) = (newModel.Tiles.GetLength(0), newModel.Tiles.GetLength(1));
        var (oldWidth, oldHeight) = (Model.Tiles != null) ? (Model.Tiles.GetLength(0), Model.Tiles.GetLength(1)) : (0, 0);

        var newShape = ((oldHeight != height) && (oldWidth != width));

        if (newShape)
        {
            Tiles.Clear();
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                UpdateTileAtLocation(newModel.Tiles[x, y], x, y);
            }
        }

        tooltip.setText(new ScaleUtil(Model.parent.scale).TextForScale());
        MoveChild(tooltip, GetChildCount());

        camera.SetMapSize(width, height);
        ((RectangleShape2D)collision.Shape).Extents = new Vector2(width * 32, height * 32);
        collision.Position = new Vector2(width * 32, height * 32);
    }

    public void CreateTooltip()
    {
        this.tooltip = GD.Load<PackedScene>("res://src/MapInfoTooltip.tscn").Instance() as MapInfoTooltip;
        AddChild(tooltip);
    }

    Vector2 positionForCoordinates(int x, int y) => new Vector2(x * 64 + 32, y * 64 + 32);

    public TileSet CreateTileset()
    {
        return (new MapTileset()).tileset;
    }

    // Handling zooming in and out
    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton mouseClickEvent = @event as InputEventMouseButton;

            var pos = mouseClickEvent.Position - GetGlobalTransformWithCanvas().origin;
            var (x, y) = (pos.x / 64, pos.y / 64);
            var Tile = TileAt((int)x, (int)y);

            if (mouseClickEvent.ButtonIndex == (int)ButtonList.Left && !mouseClickEvent.Pressed && Tile.zoomable)
            {
                GD.Print("Entering tile of type ", Tile.terrain.terrainType, (", " + Tile.terrain._debugProps()).TrimEnd(", ".ToCharArray()));
                ZoomInToInternalMap(Tile);
            }
            if (mouseClickEvent.ButtonIndex == (int)ButtonList.Right && !mouseClickEvent.Pressed)
            {
                ZoomOutToExternalMap(Tile);
            }
        }
    }

    private void ZoomInToInternalMap(TileModel Tile)
    {
        if (Tile.internalMap == null)
        {
            Tile.internalMap = new MapModel(Tile);
        }
        UpdateWholeMapTo(Tile.internalMap);

    }
    private void ZoomOutToExternalMap(TileModel Tile)
    {
        if (Tile.parent.parent == null)
        {
            Tile.parent.parent = new TileModel(new Terrain(Terrain.TerrainType.InteruniversalSpace), null, Tile.scale + 2, zoomable: true);
        }
        if (Tile.parent.parent.internalMap == null)
        {
            Tile.parent.parent.internalMap = new MapModel(Tile.parent.parent);
            MapModel grandparentMap = Tile.parent.parent.internalMap;
            grandparentMap.Tiles[grandparentMap.randomNumber(0, 10), grandparentMap.randomNumber(0, 10)] = Tile.parent;
        }
        UpdateWholeMapTo(Tile.parent.parent.internalMap);
    }

    private TileModel TileAt(int x, int y)
    {
        return Model.Tiles[x, y];
    }
}
