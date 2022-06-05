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
            case TerrainType.IntersuperclusterVoid:
            case TerrainType.InterclusterSpace:
            case TerrainType.IntergroupSpace:
            case TerrainType.IntergalacticSpace:
            case TerrainType.InterstellarSpace:
            case TerrainType.OuterSystemOrbit:
            case TerrainType.InnerSystemOrbit:
                return GD.Load<Texture>("res://assets/tiles/void.png");
            case TerrainType.Energy:
            case TerrainType.InteruniversalSpace:
                return GD.Load<Texture>("res://assets/tiles/energy.png");
            case TerrainType.Defect:
                return GD.Load<Texture>("res://assets/tiles/defect.png");
            case TerrainType.Universe:
                return GD.Load<Texture>("res://assets/tiles/universe.png");
            case TerrainType.GalaxySupercluster:
            case TerrainType.SpiralArm:
            case TerrainType.StellarBelt:
            case TerrainType.StellarBubble:
            case TerrainType.StellarCloud:
                return GD.Load<Texture>("res://assets/tiles/dense_stars.png");
            case TerrainType.GalaxyCluster:
            case TerrainType.GalaxyGroup:
                return GD.Load<Texture>("res://assets/tiles/stars.png");
            case TerrainType.Galaxy:
                return GD.Load<Texture>("res://assets/tiles/galaxy.png");
            case TerrainType.DwarfGalaxy:
                return GD.Load<Texture>("res://assets/tiles/dwarf_galaxy.png");
            case TerrainType.GalacticCore:
                return GD.Load<Texture>("res://assets/tiles/core_stars.png");
            case TerrainType.OuterSolarSystem:
            case TerrainType.HillsCloud:
                return GD.Load<Texture>("res://assets/tiles/kuiper_sol.png");
            case TerrainType.OortCloudBodies:
            case TerrainType.HillsCloudBodies:
            case TerrainType.KuiperBeltBodies:
                return GD.Load<Texture>("res://assets/tiles/kuiper.png");
            case TerrainType.SolarSystem:
            case TerrainType.InnerSolarSystem:
                return GD.Load<Texture>("res://assets/tiles/sol.png");
            case TerrainType.OuterSystemBody:
                return GD.Load<Texture>("res://assets/tiles/outer_planet.png");
            case TerrainType.InnerSystemBody:
                return GD.Load<Texture>("res://assets/tiles/inner_planet.png");
            case TerrainType.AsteroidBeltBodies:
                return GD.Load<Texture>("res://assets/tiles/asteroid.png");
            case TerrainType.Star:
                return GD.Load<Texture>("res://assets/tiles/star.png");
            default:
                return null;
        }
    }

}
