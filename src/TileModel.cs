using Godot;
public class TileModel
{
    private TerrainType terrain;
    public TerrainType Terrain { get => terrain; }
    public MapModel internalMap;

    public TileModel(TerrainType type)
    {
        this.terrain = type;
    }

    public enum TerrainType
    {
        Universe,
        Space, Void, Energy, Defect
    }

    public Texture imageForTileType()
    {
        switch (terrain)
        {
            case TerrainType.Space:
                return GD.Load<Texture>("res://assets/tiles/space.png");
            case TerrainType.Void:
                return GD.Load<Texture>("res://assets/tiles/void.png");
            case TerrainType.Energy:
                return GD.Load<Texture>("res://assets/tiles/energy.png");
            case TerrainType.Defect:
                return GD.Load<Texture>("res://assets/tiles/defect.png");
            default:
                return null;
        }
    }

}