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
            case TerrainType.Filament:
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
            case TerrainType.OuterLunarOrbit:
            case TerrainType.InnerLunarOrbit:
                return "void";
            case TerrainType.CMB:
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
                return "galaxies/" + props[PropKey.GalaxyType].ToString().ToLower();
            case TerrainType.DwarfGalaxy:
                return "galaxies/dwarf_galaxy";
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
                return "stars/" + props[PropKey.SpectralClass].ToString().ToLower();
            case TerrainType.OuterSolarSystem:
            case TerrainType.InnerSolarSystem:
                return "stars/" + props[PropKey.SpectralClass].ToString().ToLower() + "_noletter";
            case TerrainType.OuterSystemBody:
                return "outer_planet";
            case TerrainType.InnerSystemBody:
            case TerrainType.OuterLunarSystem:
            case TerrainType.InnerLunarSystem:
            case TerrainType.TerrestrialPlanet:
                return "inner_planet";
            case TerrainType.AsteroidBeltBodies:
                return "asteroid";
            case TerrainType.Star:
                return "stars/star_" + props[PropKey.SpectralClass].ToString().ToLower();
            case TerrainType.LunarBody:
                return "moon";
            case TerrainType.Land:
                return "land";
            case TerrainType.Ocean:
                return "ocean";
            default:
                return null;
        }
    }

    public enum TerrainType
    {
        // 10   10g ly across
        Universe, InteruniversalSpace,
        // 9    1g ly across
        Filament, Void, CMB, Defect,
        // 8    100m ly across
        GalaxySupercluster, IntersuperclusterVoid,
        // 7    10m ly across
        GalaxyCluster, InterclusterSpace,
        // 6    1m ly across
        GalaxyGroup, IntergroupSpace,
        // 5    100k ly across
        Galaxy, DwarfGalaxy, IntergalacticSpace,
        // 4    10k ly across
        SpiralArm, GalacticCore, GalacticHalo,
        // 3    1k ly across
        StellarBelt,
        // 2    100 ly across
        StellarBubble,
        // 1    10 ly across
        StellarCloud,
        // 0    1 ly across
        SolarSystem, InterstellarSpace,
        // -1   1t km across / 0.1 ly / 60000 AU
        HillsCloud, OortCloudBodies,
        // -2   100g km across / 6000 AU
        ScatteredDisk, HillsCloudBodies,
        // -3   10g km across / 60 AU
        OuterSolarSystem, ScatteredDiskBodies,
        // -4   1g km across / 6 AU
        InnerSolarSystem, OuterSystemOrbit, OuterSystemBody, KuiperBeltBodies,
        // -5   100m km across / 0.6 AU
        Star, InnerSystemOrbit, InnerSystemBody, AsteroidBeltBodies,
        // -6   10m km across
        // -7   1m km across / Earth SOI
        OuterLunarSystem,
        // -8   100k km across
        InnerLunarSystem, OuterLunarOrbit, LunarBody, GasGiant,
        // -9   10k km across
        TerrestrialPlanet, InnerLunarOrbit,
        // -10  1k km across
        Land, Ocean
        // -11  100 km across
        // -12  10 km across
    }

    public enum PlanetType
    {
        Terrestrial, GasGiant
    }
    public enum StarSpectralClass
    {
        O, B, A, F, G, K, M, D
    }

    public enum GalaxyType
    {
        E, S0, S, SB, Irr
    }

    public string _debugProps()
    {
        var s = "";
        foreach (var p in props)
        {
            s += p.Key + ": " + p.Value + ", ";
        }
        return s.TrimEnd(", ".ToCharArray());
    }
}

public enum PropKey
{
    PlanetType,
    SpectralClass,
    GalaxyType
}