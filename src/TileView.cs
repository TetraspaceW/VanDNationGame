using Godot;
public class TileView : Area2D
{
    TileModel _tile;
    AnimatedSprite sprite;
    CollisionShape2D collisionShape;

    public TileModel tile
    {
        get { return _tile; }
        set
        {
            _tile = value;
            updateSpriteForTile();
        }
    }

    public override void _Ready()
    {
        base._Ready();
        sprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }
    private void updateSpriteForTile()
    {
        SpriteFrames newFrames = new SpriteFrames();
        newFrames.AddFrame(anim: "default", frame: tile.imageForTileType());
        sprite.Frames = newFrames;
    }

    public override void _InputEvent(Object viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton mouseClickEvent = @event as InputEventMouseButton;
            if (mouseClickEvent.ButtonIndex == (int)ButtonList.Left && !mouseClickEvent.Pressed)
            {
                GD.Print($"Clicked on tile with type {tile.Type}!");
            }
        }
    }
}