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

    private TileModel root = new TileModel(new Terrain(Terrain.TerrainType.Universe), null, 10, zoomable: true);

    private int date = 2030;
    public override void _Ready()
    {
        // universe start
        Model = new MapModel(root);
        root.internalMap = Model;
        CreateSidebar();
        camera = (CameraBuddy)GetNode("CameraBuddy");
        collision = (CollisionShape2D)GetNode("CollisionShape2D");
        CreateTileMap();

        UpdateWholeMapTo(Model.FindHabitablePlanet().parent.internalMap);
        PlaceStartingBuildings();
        root.UpdateHighestTransportInside(false);
        root.CalculateTotalChildResources();

        UpdateWholeMapTo(Model);

        sidebar.SetDateLabelText(date + " AD");
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

            if (sidebar.selectedBuilding == null)
            {

                if (mouseClickEvent.ButtonIndex == (int)ButtonList.Left && !mouseClickEvent.Pressed && Tile.zoomable && Model.GetBuildingAt((int)x, (int)y) == null)
                {
                    ZoomInToInternalMap(Tile);
                }
                if (mouseClickEvent.ButtonIndex == (int)ButtonList.Right && !mouseClickEvent.Pressed)
                {
                    ZoomOutToExternalMap(Tile);
                }
            }
            else if (!mouseClickEvent.Pressed)
            {
                if (mouseClickEvent.ButtonIndex == (int)ButtonList.Left && Model.GetBuildingAt((int)x, (int)y) == null)
                {
                    if (Model.TryPlaceBuildingAt(sidebar.selectedBuilding, ((int)x, (int)y)))
                    {
                        UpdateTileAtLocation(Model.Tiles[(int)x, (int)y], (int)x, (int)y);
                        UpdateSidePanelLabelText();
                        sidebar.SetAvailableBuildingsList(GetAvailableBuildingsList());
                    }
                }
                sidebar.SetSelectedBuilding(null);
            }
        }
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

        sidebar.SetScaleLabelText(DistanceScale.TextForScale(Model.parent.scale));
        UpdateSidePanelLabelText();
        sidebar.SetAvailableBuildingsList(GetAvailableBuildingsList());
        MoveChild(sidebar, GetChildCount());

        camera.SetMapSize(width, height);
        ((RectangleShape2D)collision.Shape).Extents = new Vector2(width * 32, height * 32);
        collision.Position = new Vector2(width * 32, height * 32);
    }

    private void UpdateSidePanelLabelText()
    {
        sidebar.SetSidePanelLabelText(Model.parent.GetAvailableResources().GetResourcesList() + "\nCurrently inside tile of type ", Model.parent.terrain.terrainType, (", " + Model.parent.terrain._debugProps()).TrimEnd(", ".ToCharArray()));
    }

    public List<(BuildingTemplate, bool)> GetAvailableBuildingsList()
    {
        return BuildingTemplateList.buildingTemplates.Where((buildingTemplate) =>
            buildingTemplate.terrainTypes.Intersect(Model.GetTerrainTypes()).Count() > 0
             && FactionList.GetPlayerFaction().techsKnown.Contains(buildingTemplate.technology)
             && buildingTemplate.size == Model.GetTileScale()
        ).Select((buildingTemplate) =>
            (
                buildingTemplate,
                Model.parent.GetAvailableResources().GetAmount(buildingTemplate.cost.resource) >= buildingTemplate.cost.amount
            )
        ).ToList();
    }

    public void CreateSidebar()
    {
        this.sidebar = GD.Load<PackedScene>("res://src/Sidebar.tscn").Instance() as Sidebar;
        AddChild(sidebar);
        sidebar.mapView = this;
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

    private void ZoomInToInternalMap(TileModel Tile)
    {
        if (Tile.internalMap == null)
        {
            Tile.internalMap = new MapModel(Tile);
            Tile.CalculateTotalChildResources();
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

    public void NextTurn()
    {
        root.UpdateHighestTransportInside();
        root.internalMap.NextTurn();
        root.CalculateTotalChildResources();
        date += 1;
        UpdateSidePanelLabelText();
        sidebar.SetAvailableBuildingsList(GetAvailableBuildingsList());
        sidebar.SetDateLabelText(date + " CE");
    }
}
