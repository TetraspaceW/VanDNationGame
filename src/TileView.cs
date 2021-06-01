using Godot;
public class TileView : AnimatedSprite
{
    TileModel _tile;

    public TileModel tile
    {
        get { return _tile; }
        set
        {
            _tile = value;
            updateSpriteForTile();
        }
    }

    private void updateSpriteForTile()
    {
        SpriteFrames newFrames = new SpriteFrames();
        newFrames.AddFrame(anim: "default", frame: tile.imageForTileType());
        Frames = newFrames;
    }
}