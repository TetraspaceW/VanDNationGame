using Godot;
public class TileModel
{
    private TerrainType type;

    public TerrainType Type { get => type; }

    public TileModel(TerrainType type)
    {
        this.type = type;
    }

    public enum TerrainType
    {
        Space, Void, Energy, Defect
    }

    public Texture imageForTileType()
    {
        switch (type)
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