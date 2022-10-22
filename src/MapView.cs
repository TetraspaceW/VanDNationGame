using Godot;
using System.Collections.Generic;
using System.Linq;
public class MapView : Area2D
{
    private CameraBuddy camera;
    private CollisionShape2D collision;
    private MapModel Model;
    private TileMap Tiles;
    private TileMap grid;
    private TileMap BuildingTiles;
    private Sidebar sidebar;
    public override void _Ready()
    {
        // universe start
        TileModel tile = new TileModel(new Terrain(Terrain.TerrainType.Universe), null, 10, zoomable: true);
        Model = new MapModel(tile);
        tile.internalMap = Model;
        CreateSidebar();
        camera = (CameraBuddy)GetNode("CameraBuddy");
        collision = (CollisionShape2D)GetNode("CollisionShape2D");
        CreateTileMap();

        var techTree = TechTree.techTree; // load tech tree from file
        var buildings = BuildingTemplateList.buildingTemplates; // load buildings from file

        UpdateWholeMapTo(Model.FindHabitablePlanet().parent.internalMap);
        PlaceStartingBuildings();
        UpdateWholeMapTo(Model);
    }

    void PlaceStartingBuildings()
    {
        Model.PlaceStartingBuildings(FactionList.GetPlayerFaction().startingBuildings);
    }

    void CreateTileMap()
    {
        var mapTileset = new MapTileset();

        Tiles = new TileMap();
        Tiles.TileSet = mapTileset.tileset;
        AddChild(Tiles);

        BuildingTiles = new TileMap();
        BuildingTiles.TileSet = mapTileset.buildingTileset;
        AddChild(BuildingTiles);

        grid = new TileMap();
        grid.TileSet = CreateGridTileset();
        AddChild(grid);
    }

    void UpdateTileAtLocation(TileModel newTile, int x, int y)
    {
        Model.Tiles[x, y] = newTile;
        Tiles.SetCell(x, y, Tiles.TileSet.FindTileByName("tiles/" + newTile.image));

        var building = Model.GetBuildingAt(x, y);
        if (building != null)
        {
            BuildingTiles.SetCell(x, y, BuildingTiles.TileSet.FindTileByName("buildings/" + building.template.image));
        }
        else
        {
            BuildingTiles.SetCell(x, y, -1);
        }

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

        sidebar.SetScaleLabelText(global::Scale.TextForScale(Model.parent.scale));
        sidebar.SetSidePanelLabelText("Currently inside tile of type ", Model.parent.terrain.terrainType, (", " + Model.parent.terrain._debugProps()).TrimEnd(", ".ToCharArray()));
        sidebar.SetAvailableBuildingsList(GetAvailableBuildingsList());
        MoveChild(sidebar, GetChildCount());

        camera.SetMapSize(width, height);
        ((RectangleShape2D)collision.Shape).Extents = new Vector2(width * 32, height * 32);
        collision.Position = new Vector2(width * 32, height * 32);
    }

    public List<BuildingTemplate> GetAvailableBuildingsList()
    {
        return BuildingTemplateList.buildingTemplates.Where((buildingTemplate) =>
        {
            return buildingTemplate.terrainTypes.Intersect(Model.GetTerrainTypes()).Count() > 0
             && FactionList.GetPlayerFaction().techsKnown.Contains(buildingTemplate.technology)
             && buildingTemplate.size == Model.GetTileScale();
        }).ToList();
    }

    public void CreateSidebar()
    {
        this.sidebar = GD.Load<PackedScene>("res://src/Sidebar.tscn").Instance() as Sidebar;
        AddChild(sidebar);
    }

    Vector2 positionForCoordinates(int x, int y) => new Vector2(x * 64 + 32, y * 64 + 32);

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

            if (mouseClickEvent.ButtonIndex == (int)ButtonList.Left && !mouseClickEvent.Pressed && Tile.zoomable && Model.GetBuildingAt((int)x, (int)y) == null)
            {
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
            grandparentMap.Tiles[RND.Next(0, 10), RND.Next(0, 10)] = Tile.parent;
        }
        UpdateWholeMapTo(Tile.parent.parent.internalMap);
    }

    private TileModel TileAt(int x, int y)
    {
        return Model.Tiles[x, y];
    }
}
