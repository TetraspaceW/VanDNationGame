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
            case TerrainType.SystemOrbit:
            case TerrainType.LunarOrbit:
            case TerrainType.IntermolecularSpace:
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
            case TerrainType.FarfarfarSystemBody:
            case TerrainType.FarfarSystemBody:
            case TerrainType.FarSystemBody:
            case TerrainType.OuterSystemBody:
                return "outer_planet";
            case TerrainType.InnerSystemBody:
            case TerrainType.OuterLunarSystem:
            case TerrainType.InnerLunarSystem:
            case TerrainType.TerrestrialPlanet:
            case TerrainType.GasGiant:
            case TerrainType.LunarBody:
                return "planets/" + props[PropKey.PlanetType].ToString().ToLower();
            case TerrainType.AsteroidBeltBodies:
                return "asteroid";
            case TerrainType.EpistellarSolarSystem:
            case TerrainType.EpiepistellarSolarSystem:
            case TerrainType.EpiepiepistellarSolarSystem:
            case TerrainType.Star:
                return "stars/star_" + props[PropKey.SpectralClass].ToString().ToLower();
            case TerrainType.StarSurface:
                return "stars/starstuff";
            case TerrainType.VerdantTerrain:
                return "land";
            case TerrainType.Ocean:
                return "ocean";
            case TerrainType.IceSheet:
                return "ice";
            case TerrainType.BarrenTerrain:
                return "barren";
            case TerrainType.HydrogenAtom:
                return "atom/hydrogen";
            case TerrainType.OxygenAtom:
                return "atom/oxygen";
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
        HillsCloud, OortCloudBodies, FarfarfarSystemBody, SystemOrbit,
        // -2   100g km across / 6000 AU
        ScatteredDisk, HillsCloudBodies, FarfarSystemBody,
        // -3   10g km across / 60 AU
        OuterSolarSystem, ScatteredDiskBodies, FarSystemBody,
        // -4   1g km across / 6 AU
        InnerSolarSystem, OuterSystemBody, KuiperBeltBodies,
        // -5   100m km across / 0.6 AU
        Star, InnerSystemBody, AsteroidBeltBodies, EpistellarSolarSystem,
        // -6   10m km across
        // -7   1m km across / Earth SOI
        OuterLunarSystem, EpiepistellarSolarSystem, StarSurface,
        // -8   100k km across
        InnerLunarSystem, LunarOrbit, LunarBody, GasGiant, EpiepiepistellarSolarSystem,
        // -9   10k km across
        TerrestrialPlanet,
        // -10  1k km across
        BarrenTerrain, VerdantTerrain, Ocean, IceSheet,
        // -11  100 km across
        // -12  10 km across
        // -13  1 km across
        // -14  100 m across
        // -15  10 m across
        // -16  1 m across
        // -17  10 cm across
        // -18  1 cm across
        // -19  1 mm across
        // -20  100 um across
        // -21  10 um across
        // -22  1 um across
        // -23  100 nm across
        // -24  10 nm across
        // -25  1 nm across
        // -26  100 pm across
        HydrogenAtom, OxygenAtom, IntermolecularSpace,
        // -27  10 pm across
        ElectronCloud,
        // -28  1 pm across
        // -29  100 fm across
        // -30  10 fm across
        Nucleus,
        // -31  1 fm across
        Proton, Neutron,
        // -32  100 am across
        GluonSea, ValenceUpQuark, ValenceDownQuark,
        // -33  10 am across
        // -34  1 am across
        UpQuark, DownQuark
    }

    public enum PlanetType
    {
        Chunk,
        // Terrestrial
        Terrestrial,
        // Jovian
        Jovian
    }
    public enum StarSpectralClass
    {
        O, B, A, F, G, K, M, D,
        MIII, KI
    }

    public enum GalaxyType
    {
        E, S0, S, SB, Irr
    }

    public enum AtomElement
    {
        Hydrogen, Helium,
        Lithium, Beryllium, Boron, Carbon, Nitrogen, Oxygen, Fluorine, Neon
    }

    public enum QuarkColour
    {
        Red, Green, Blue,
        Antired, Antigreen, Antiblue
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
    PlanetHydrosphereCoverage, PlanetHydrosphereType, PlanetRadius, PlanetIsLifeBearing,
    PlanetType,
    SpectralClass,
    GalaxyType
}