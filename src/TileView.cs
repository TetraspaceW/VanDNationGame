using Godot;
using System;
public class TileView : Area2D
{
    TileModel _tile;
    Sprite Sprite;
    public TileModel Tile
    {
        get { return _tile; }
        set
        {
            _tile = value;
            UpdateSpriteForTile();
        }
    }

    public override void _Ready()
    {
        base._Ready();
        Sprite = GetNode<Sprite>("Sprite");
    }
    private void UpdateSpriteForTile()
    {
        Sprite.Texture = Tile.imageForTileType();
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton mouseClickEvent = @event as InputEventMouseButton;
            if (mouseClickEvent.ButtonIndex == (int)ButtonList.Left && !mouseClickEvent.Pressed && Tile.zoomable)
            {
                GD.Print("Entering tile of type " + Tile.terrain);
                ZoomInToInternalMap();
            }
            if (mouseClickEvent.ButtonIndex == (int)ButtonList.Right && !mouseClickEvent.Pressed)
            {
                ZoomOutToExternalMap();
            }
        }
    }

    private void ZoomInToInternalMap()
    {
        if (Tile.internalMap == null)
        {
            Tile.internalMap = new MapModel(Tile);
        }
        GetParent<MapView>().UpdateWholeMapTo(Tile.internalMap);

    }
    private void ZoomOutToExternalMap()
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
        GetParent<MapView>().UpdateWholeMapTo(Tile.parent.parent.internalMap);
    }
}