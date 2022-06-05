using Godot;
public class TileModel
{
    public TerrainType terrain;
    public TerrainType Terrain { get => terrain; }
    public MapModel internalMap;
    public TileModel parent;
    public bool zoomable;

    public int scale;

    public TileModel(TerrainType type, TileModel parent, int scale, bool zoomable = false)
    {
        this.terrain = type;
        this.parent = parent;
        this.scale = scale;
        this.zoomable = zoomable;
    }

    public enum TerrainType
    {
        // 10g ly across
        Universe, InteruniversalSpace,
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
        SpiralArm, GalacticCore, GalacticHalo,
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

    private string filenameForTileType()
    {
        switch (terrain)
        {
            case TerrainType.Space:
                return "space";
            case TerrainType.Void:
            case TerrainType.IntersuperclusterVoid:
            case TerrainType.InterclusterSpace:
            case TerrainType.IntergroupSpace:
            case TerrainType.IntergalacticSpace:
            case TerrainType.GalacticHalo:
            case TerrainType.InterstellarSpace:
            case TerrainType.OuterSystemOrbit:
            case TerrainType.InnerSystemOrbit:
                return "void";
            case TerrainType.Energy:
            case TerrainType.InteruniversalSpace:
                return "energy";
            case TerrainType.Defect:
                return "defect";
            case TerrainType.Universe:
                return "universe";
            case TerrainType.GalaxySupercluster:
            case TerrainType.SpiralArm:
            case TerrainType.StellarBelt:
            case TerrainType.StellarBubble:
                return "dense_stars";
            case TerrainType.GalaxyCluster:
            case TerrainType.GalaxyGroup:
            case TerrainType.StellarCloud:
                return "stars";
            case TerrainType.Galaxy:
                return "galaxy";
            case TerrainType.DwarfGalaxy:
                return "dwarf_galaxy";
            case TerrainType.GalacticCore:
                return "core_stars";
            case TerrainType.HillsCloud:
            case TerrainType.ScatteredDisk:
                return "kuiper_sol";
            case TerrainType.OortCloudBodies:
            case TerrainType.HillsCloudBodies:
            case TerrainType.KuiperBeltBodies:
            case TerrainType.ScatteredDiskBodies:
                return "kuiper";
            case TerrainType.SolarSystem:
            case TerrainType.OuterSolarSystem:
            case TerrainType.InnerSolarSystem:
                return "sol";
            case TerrainType.OuterSystemBody:
                return "outer_planet";
            case TerrainType.InnerSystemBody:
                return "inner_planet";
            case TerrainType.AsteroidBeltBodies:
                return "asteroid";
            case TerrainType.Star:
                return "star";
            default:
                return null;
        }
    }
    public Texture imageForTileType()
    {
        return GD.Load<Texture>("res://assets/tiles/" + filenameForTileType() + ".png");
    }

}
