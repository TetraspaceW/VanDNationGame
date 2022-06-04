using Godot;
using System;
public class TileView : Area2D
{
    TileModel _tile;
    AnimatedSprite Sprite;
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
        Sprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }
    private void UpdateSpriteForTile()
    {
        SpriteFrames newFrames = new SpriteFrames();
        newFrames.AddFrame(anim: "default", frame: Tile.imageForTileType());
        Sprite.Frames = newFrames;
    }

    public override void _InputEvent(Godot.Object viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton mouseClickEvent = @event as InputEventMouseButton;
            if (mouseClickEvent.ButtonIndex == (int)ButtonList.Left && !mouseClickEvent.Pressed)
            {
                ZoomIntoInternalMap();
            }
            if (mouseClickEvent.ButtonIndex == (int)ButtonList.Right && !mouseClickEvent.Pressed)
            {
                ZoomOuttoExternalMap();
            }
        }
    }

    private void ZoomIntoInternalMap()
    {
        if (Tile.internalMap == null)
        {
            Tile.internalMap = new MapModel(terrainType: Tile.Terrain, Tile);
        }
        GetParent<MapView>().UpdateWholeMapTo(Tile.internalMap);
    }
    private void ZoomOuttoExternalMap()
    {
        if (Tile.parent.parent == null)
        {
            Tile.parent.parent = new TileModel(type: TileModel.TerrainType.Energy, null, Tile.scale + 2);
        }
        if (Tile.parent.parent.internalMap == null)
        {
            Tile.parent.parent.internalMap = new MapModel(terrainType: Tile.parent.parent.Terrain, Tile.parent.parent);
            MapModel grandparentMap = Tile.parent.parent.internalMap;
            grandparentMap.Tiles[grandparentMap.randomNumber(0, 10), grandparentMap.randomNumber(0, 10)] = Tile.parent;
        }
        GetParent<MapView>().UpdateWholeMapTo(Tile.parent.parent.internalMap);
    }
}