using Godot;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

public partial class MapView : Area2D
{
	private CameraBuddy camera;
	private CollisionShape2D collision;
	private MapModel Model;
	private TileMap Tiles;
	private TileMap grid;
	private TileMap BuildingTiles;
	private Sidebar sidebar;

	private TileModel root;

	private int date = 2030;
	private double turns = 0;
	private double localEpoch = 2030;
	private double localYearLength = 1.0; // Default to Earth's year (1 turn per year)
	public override void _Ready()
	{
		TileModel startingTile = new TileModel(new Terrain(Terrain.TerrainType.InteruniversalSpace), null, 11, zoomable: true);

		// universe start
		Model = new MapModel(startingTile);
		startingTile.internalMap = Model;
		CreateSidebar();
		camera = (CameraBuddy)GetNode("CameraBuddy");
		collision = (CollisionShape2D)GetNode("CollisionShape2D");
		CreateTileMap();

		// Find habitable planet and set local year length
		TileModel habitablePlanet = Model.FindHabitablePlanet();
		if (double.TryParse(habitablePlanet.parent.terrain.props[PropKey.OrbitalPeriod], out double period) && period > 0)
		{
			localYearLength = period;
			localEpoch = RND.Next(1, 10001);
			date = (int)(localEpoch / localYearLength);
		}

		UpdateWholeMapTo(habitablePlanet.parent.internalMap);

		PlaceStartingBuildings();
		SetRootTo(startingTile);

		UpdateWholeMapTo(Model);

		sidebar.SetDateLabelText(date + " CE");
	}

	// Handling zooming in and out
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton)
		{
			InputEventMouseButton mouseClickEvent = @event as InputEventMouseButton;

			var pos = mouseClickEvent.Position - GetGlobalTransformWithCanvas().Origin;
			var (x, y) = (pos.X / 64, pos.Y / 64);
			if (x < Model.Tiles.GetLength(0) && y < Model.Tiles.GetLength(1))
			{
				if (sidebar.selectedBuilding == null)
				{

					if (mouseClickEvent.ButtonIndex == MouseButton.Left)
					{
						var Tile = TileAt((int)x, (int)y);
						if (!mouseClickEvent.Pressed && Tile.zoomable && Model.GetBuildingAt((int)x, (int)y) == null)
						{
							ZoomInToInternalMap(Tile);
						}
					}
					if (mouseClickEvent.ButtonIndex == MouseButton.Right && !mouseClickEvent.Pressed)
					{
						var Tile = TileAt((int)x, (int)y);
						ZoomOutToExternalMap(Tile);
					}
				}
				else if (!mouseClickEvent.Pressed)
				{
					if (mouseClickEvent.ButtonIndex == MouseButton.Left && Model.GetBuildingAt((int)x, (int)y) == null)
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
	}

	void SetRootTo(TileModel newRoot)
	{
		root = newRoot;
		root.UpdateHighestTransportInside(false);
		root.CalculateTotalChildResources();
		root.CalculateTotalChildCapacity();
		root.CalculateStorageBuildings();
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
		Tiles.SetCell(0, new Vector2I(x, y), FindTileByName(Tiles.TileSet, "tiles/" + newTile.image), Vector2I.Zero);
		var building = Model.GetBuildingAt(x, y);
		if (building != null)
		{
			BuildingTiles.SetCell(0, new Vector2I(x, y), FindTileByName(BuildingTiles.TileSet, "buildings/" + building.template.image), Vector2I.Zero);
		}
		else
		{
			BuildingTiles.SetCell(0, new Vector2I(x, y), -1);
		}

		grid.SetCell(0, new Vector2I(x, y), FindTileByName(grid.TileSet, "border"), Vector2I.Zero);
	}

	public static int FindTileByName(TileSet tileSet, string name)
	{
		for (int i = 0; i < tileSet.GetSourceCount(); i++)
		{
			if (tileSet.GetSource(i).ResourceName == name)
			{
				return i;
			}
		}
		return -1;
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
		((RectangleShape2D)collision.Shape).Size = new Vector2(width * 64, height * 64);
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
		this.sidebar = GD.Load<PackedScene>("res://src/Sidebar.tscn").Instantiate() as Sidebar;
		AddChild(sidebar);
		sidebar.mapView = this;
	}

	TileSet CreateGridTileset()
	{
		var tileset = new TileSet();
		var s = new TileSetAtlasSource();
		s.Texture = GD.Load<Texture2D>("res://assets/border.png");
		s.TextureRegionSize = new Vector2I(64, 64);
		s.CreateTile(Vector2I.Zero, new Vector2I(1, 1));
		s.ResourceName = "border";
		tileset.AddSource(s, -1);
		tileset.TileSize = new Vector2I(64, 64);
		return tileset;
	}

	private void ZoomInToInternalMap(TileModel Tile)
	{
		if (Tile.internalMap == null)
		{
			Tile.internalMap = new MapModel(Tile);
		}
		Tile.UpdateHighestTransportInside();
		Tile.CalculateStorageBuildings();
		Tile.CalculateTotalChildResources();
		Tile.CalculateTotalChildCapacity();
		UpdateWholeMapTo(Tile.internalMap);
	}

	private void ZoomOutToExternalMap(TileModel Tile)
	{
		if (Tile.parent.parent == null)
		{
			Tile.parent.parent = new TileModel(new Terrain(Terrain.TerrainType.InteruniversalSpace), null, Tile.scale + 2, zoomable: true);
			SetRootTo(Tile.parent.parent);
		}
		if (Tile.parent.parent.internalMap == null)
		{
			Tile.parent.parent.internalMap = new MapModel(Tile.parent.parent);
			MapModel grandparentMap = Tile.parent.parent.internalMap;
			grandparentMap.Tiles[RND.Next(0, 10), RND.Next(0, 10)] = Tile.parent;
		}
		Tile.parent.parent.UpdateHighestTransportInside();
		Tile.parent.parent.CalculateStorageBuildings();
		Tile.parent.parent.CalculateTotalChildResources();
		Tile.parent.parent.CalculateTotalChildCapacity();
		UpdateWholeMapTo(Tile.parent.parent.internalMap);
	}

	private TileModel TileAt(int x, int y)
	{
		return Model.Tiles[x, y];
	}

	public void NextTurn()
	{
		foreach (TileModel tile in TileModel.activeTiles)
		{
			tile.UpdateHighestTransportInside();
			tile.CalculateTotalChildResources();
			tile.CalculateTotalChildCapacity();
			tile.internalMap.NextTurn();
		}
		Model.parent.UpdateHighestTransportInside();
		Model.parent.CalculateTotalChildResources();
		Model.parent.CalculateTotalChildCapacity();

		// Increment total turns
		turns += 1.0;

		// Calculate the current year based on turns and local year length
		date = (int)((localEpoch + turns) / localYearLength);

		UpdateSidePanelLabelText();
		sidebar.SetAvailableBuildingsList(GetAvailableBuildingsList());
		sidebar.SetDateLabelText(date + " CE");
	}
}
