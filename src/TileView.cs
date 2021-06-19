using Godot;
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

    public override void _InputEvent(Object viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton mouseClickEvent = @event as InputEventMouseButton;
            if (mouseClickEvent.ButtonIndex == (int)ButtonList.Left && !mouseClickEvent.Pressed)
            {
                ZoomIntoInternalMap();
            }
        }
    }

    private void ZoomIntoInternalMap()
    {
        if (Tile.internalMap == null)
        {
            Tile.internalMap = new MapModel(terrainType: Tile.Terrain);
        }
        GetParent<MapView>().UpdateWholeMapTo(Tile.internalMap);
    }
}