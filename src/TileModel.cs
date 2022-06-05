using Godot;
public class TileModel
{
    public Terrain terrain;
    public MapModel internalMap;
    public TileModel parent;
    public bool zoomable;
    public int scale;
    private string image;

    public TileModel(Terrain terrain, TileModel parent, int scale, bool zoomable = false)
    {
        this.terrain = terrain;
        this.parent = parent;
        this.scale = scale;
        this.zoomable = zoomable;
        this.image = terrain.filenameForTileType();
    }

    public Texture imageForTileType()
    {
        return GD.Load<Texture>("res://assets/tiles/" + image + ".png");
    }
}
