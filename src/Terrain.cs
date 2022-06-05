using System.Collections.Generic;
public class Terrain
{
    public TerrainType terrainType;
    public Dictionary<PropKey, string> props;

    public Terrain(TerrainType type, Dictionary<PropKey, string> props = null)
    {
        terrainType = type;
        this.props = (props == null) ? new Dictionary<PropKey, string>() : props;
    }

    public string filenameForTileType()
    {
        switch (terrainType)
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
                return "dense_stars";
            case TerrainType.GalaxyCluster:
            case TerrainType.StellarBubble:
                return "stars";
            case TerrainType.GalaxyGroup:
            case TerrainType.StellarCloud:
                return "stars_sparse";
            case TerrainType.Galaxy:
                return "galaxy";
            case TerrainType.DwarfGalaxy:
                return "dwarf_galaxy";
            case TerrainType.GalacticCore:
                return "core_stars";
            case TerrainType.HillsCloud:
            case TerrainType.ScatteredDisk:
                return "stars/kuiper_" + props[PropKey.SpectralClass].ToString().ToLower();
            case TerrainType.OortCloudBodies:
            case TerrainType.HillsCloudBodies:
            case TerrainType.KuiperBeltBodies:
            case TerrainType.ScatteredDiskBodies:
                return "kuiper";
            case TerrainType.SolarSystem:
            case TerrainType.OuterSolarSystem:
            case TerrainType.InnerSolarSystem:
                return "stars/" + props[PropKey.SpectralClass].ToString().ToLower();
            case TerrainType.OuterSystemBody:
                return "outer_planet";
            case TerrainType.InnerSystemBody:
                return "inner_planet";
            case TerrainType.AsteroidBeltBodies:
                return "asteroid";
            case TerrainType.Star:
                return "stars/star_" + props[PropKey.SpectralClass].ToString().ToLower();
            default:
                return null;
        }
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

    public enum StarSpectralClass
    {
        O, B, A, F, G, K, M, D
    }
}

public enum PropKey
{
    SpectralClass
}