using Godot;
public class MapView : Area2D
{
    private CameraBuddy camera;
    private CollisionShape2D collision;
    private MapModel Model;
    private TileMap Tiles;
    private TileMap grid;
    private MapInfoTooltip tooltip;
    public override void _Ready()
    {
        // universe start
        TileModel tile = new TileModel(new Terrain(Terrain.TerrainType.Universe), null, 10, zoomable: true);
        Model = new MapModel(tile);
        tile.internalMap = Model;
        CreateTooltip();
        camera = (CameraBuddy)GetNode("CameraBuddy");
        collision = (CollisionShape2D)GetNode("CollisionShape2D");
        CreateTileMap();
        UpdateWholeMapTo(Model);

        var s = TechTree.techTree; // load tech tree from file

        UpdateWholeMapTo(Model.FindHabitablePlanet().parent.internalMap);
    }

    void CreateTileMap()
    {
        Tiles = new TileMap();
        Tiles.TileSet = CreateTileset();
        AddChild(Tiles);
        grid = new TileMap();
        grid.TileSet = CreateGridTileset();
        AddChild(grid);
    }

    void UpdateTileAtLocation(TileModel newTile, int x, int y)
    {
        Model.Tiles[x, y] = newTile;
        Tiles.SetCell(x, y, Tiles.TileSet.FindTileByName(newTile.image));
        grid.SetCell(x, y, grid.TileSet.FindTileByName("border"));
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

        tooltip.setScaleLabelText(ScaleUtil.TextForScale(Model.parent.scale));
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

    TileSet CreateTileset()
    {
        return (new MapTileset()).tileset;
    }

    TileSet CreateGridTileset()
    {
        var tileset = new TileSet();
        var id = tileset.GetLastUnusedTileId();
        tileset.CreateTile(id);
        tileset.TileSetName(id, "border");
        tileset.TileSetTexture(id, GD.Load<Texture>("res://assets/border.png"));
        tileset.TileSetRegion(id, new Rect2(0, 0, 64, 64));
        return tileset;
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
                ZoomInToInternalMap(Tile);
                tooltip.setSidePanelLabelText("Currently inside tile of type ", Tile.terrain.terrainType, (", " + Tile.terrain._debugProps()).TrimEnd(", ".ToCharArray()));
            }
            if (mouseClickEvent.ButtonIndex == (int)ButtonList.Right && !mouseClickEvent.Pressed)
            {
                ZoomOutToExternalMap(Tile);
                tooltip.setSidePanelLabelText("Currently inside tile of type ", Tile.parent.parent.terrain.terrainType, (", " + Tile.parent.parent.terrain._debugProps()).TrimEnd(", ".ToCharArray()));
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
            grandparentMap.Tiles[RND.Next(0, 10), RND.Next(0, 10)] = Tile.parent;
        }
        UpdateWholeMapTo(Tile.parent.parent.internalMap);
    }

    private TileModel TileAt(int x, int y)
    {
        return Model.Tiles[x, y];
    }
}
