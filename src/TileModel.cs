using Godot;
public class TileModel
{
    private TerrainType terrain;
    public TerrainType Terrain { get => terrain; }
    public MapModel internalMap;
    public TileModel parent;

    public int scale;

    public TileModel(TerrainType type, TileModel parent, int scale)
    {
        this.terrain = type;
        this.parent = parent;
        this.scale = scale;
    }

    public enum TerrainType
    {
        // 10g ly across
        Universe,
        // 1g ly across
        Space, Void, Energy, Defect,
        // 100m ly across
        GalaxySupercluster, IntersuperclusterVoid,
        // 10m ly across
        GalaxyCluster, InterclusterSpace,
        // 1m ly across
        GalaxyGroup, IntergroupSpace,
        // 100k ly across
        Galaxy, DwarfGalaxy, IntergalacticSpace,
        // 10k ly across
        SpiralArm, GalacticCore,
        // 1k ly across
        StellarBelt,
        // 100 ly across
        StellarBubble,
        // 10 ly across
        StellarCloud,
        // 1 ly across
        SolarSystem, InterstellarSpace,
        // 1t km across / 0.1 ly / 60000 AU
        HillsCloud, OortCloudBodies,
        // 100g km across / 6000 AU
        ScatteredDisk, HillsCloudBodies,
        // 10g km across / 60 AU
        OuterSolarSystem, ScatteredDiskBodies,
        // 1g km across / 6 AU
        InnerSolarSystem, OuterSystemOrbit, OuterSystemBody, KuiperBeltBodies,
        // 100m km across / 0.6 AU
        Star, InnerSystemOrbit, InnerSystemBody, AsteroidBeltBodies
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
            case TerrainType.Universe:
                return GD.Load<Texture>("res://assets/tiles/universe.png");
            default:
                return null;
        }
    }

}
