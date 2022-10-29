using System.Collections.Generic;
using System;
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
                return "star_clusters/dense_stars";
            case TerrainType.GalaxyCluster:
            case TerrainType.StellarBubble:
                return "star_clusters/stars" + (props.ContainsKey(PropKey.SpecialStar) ? "_" + props[PropKey.SpecialStar].ToString().ToLower() : "");
            case TerrainType.GalaxyGroup:
            case TerrainType.StellarCloud:
                return "star_clusters/stars_sparse" + (props.ContainsKey(PropKey.SpecialStar) ? "_" + props[PropKey.SpecialStar].ToString().ToLower() : "");
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
                var planetFileName = "planets/" + props[PropKey.PlanetType].ToString().ToLower();

                switch (Enum.Parse(typeof(SolarSystemGenerator.Hydrosphere), props[PropKey.PlanetHydrosphereType]))
                {
                    case SolarSystemGenerator.Hydrosphere.Liquid: planetFileName += "_ocean"; break;
                    case SolarSystemGenerator.Hydrosphere.IceSheet: planetFileName += "_ice"; break;
                    case SolarSystemGenerator.Hydrosphere.Crustal: planetFileName += "_outer"; break;
                }

                planetFileName += bool.Parse(props[PropKey.PlanetIsLifeBearing]) ? "_life" : "";

                return planetFileName;
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
            case TerrainType.Cytoplasm:
                return "biomolecules/cytoplasm";
            case TerrainType.Nucleolus:
                return "biomolecules/nucleolus";
            case TerrainType.Nucleosome:
                return "biomolecules/nucleotides/nucleosome";
            case TerrainType.Nucleoplasm:
                return "biomolecules/nucleoplasm";
            case TerrainType.Chromatin:
                return "biomolecules/chromatin";
            case TerrainType.ChromatinChain:
                return "biomolecules/chromatin_chain";
            case TerrainType.Nucleotide:
                return "biomolecules/nucleotides/" + props[PropKey.Nucleobase].ToString().ToLower() + "_" + props[PropKey.NucleicBackbone].ToString().ToLower() + props[PropKey.Rotation].ToString().ToLower();
            case TerrainType.NucleotideBlank:
                return "biomolecules/nucleotides/" + props[PropKey.NucleicBackbone].ToString().ToLower() + "_blank" + props[PropKey.Rotation].ToString().ToLower();
            case TerrainType.NucleotideTurnInner:
                return "biomolecules/nucleotides/" + props[PropKey.NucleicBackbone].ToString().ToLower() + "_turn_inner" + props[PropKey.Rotation].ToString().ToLower();
            case TerrainType.NucleotideTurnOuter:
                return "biomolecules/nucleotides/" + props[PropKey.NucleicBackbone].ToString().ToLower() + "_turn_outer" + props[PropKey.Rotation].ToString().ToLower();
            case TerrainType.IntermolecularFluid:
                return "biomolecules/intermolecular_fluid";
            case TerrainType.Atom:
                return "atom/" + props[PropKey.AtomElement].ToString().ToLower();
            case TerrainType.ElectronCloud:
                return "atom/electron_cloud";
            case TerrainType.Nucleus:
                return "atom/nucleus";
            case TerrainType.Proton:
                return "atom/proton";
            case TerrainType.Neutron:
                return "atom/neutron";
            case TerrainType.GluonSea:
                return "atom/quark_gluon_sea";
            case TerrainType.ValenceQuark:
                return "atom/valence_quark_" + props[PropKey.QuarkColour].ToString().ToLower() + "_" + props[PropKey.QuarkFlavour].ToString().ToLower();
            case TerrainType.Quark:
                return "atom/quark_" + props[PropKey.QuarkColour].ToString().ToLower() + "_" + props[PropKey.QuarkFlavour].ToString().ToLower();
            default:
                return null;
        }
    }

    public enum TerrainType
    {
        // 41
        BeyondBubble,
        // 40
        MonocosmCohort,
        // 39
        // 38 / w+1st archverse level
        Monocosm,
        // 37 / wth archverse level
        Omniverse, Godverse,
        // 36
        Ultraverse,
        // 35
        ArchverseChain,
        // 34   8th archverse level
        Yottaverse,
        // 33
        ZettaverseCohort,
        // 32
        // 31   7th archverse level
        Zettaverse,
        // 30
        ExaverseCohort,
        // 29
        // 28   6th archverse level
        Exaverse,
        // 27
        PetaverseCohort,
        // 26
        // 25   5th archverse level
        Petaverse,
        // 24
        TeraverseCohort,
        // 23
        // 22   4th archverse level
        Teraverse,
        // 21
        GigaverseCohort,
        // 20 
        // 19   3rd archverse level
        Gigaverse,
        // 18
        MegaverseCohort,
        // 17
        // 16   2nd archverse level
        Megaverse,
        // 15
        MultiverseCohort,
        // 14
        // 13   1st archverse level
        Multiverse,
        // 12
        UniverseCohort,
        // 11
        // 10   10g ly across / 0th archverse level
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
        Cell,
        // -22  1 um across
        Nucleoplasm, Cytoplasm, Nucleolus, Chromatin,
        // -23  100 nm across
        ChromatinChain,
        // -24  10 nm across
        Nucleosome,
        // -25  1 nm across
        Nucleotide, NucleotideBlank, NucleotideTurnInner, NucleotideTurnOuter, IntermolecularFluid,
        // -26  100 pm across / 1 angstrom
        Atom, IntermolecularSpace,
        // -27  10 pm across
        ElectronCloud,
        // -28  1 pm across
        // -29  100 fm across
        // -30  10 fm across
        Nucleus,
        // -31  1 fm across
        Proton, Neutron,
        // -32  100 am across
        GluonSea, ValenceQuark,
        // -33  10 am across
        // -34  1 am across
        Quark,
        // N/A
        StructureTile,
        Sandbox
    }

    public enum PlanetType
    {
        Chunk, Terrestrial, Jovian
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
        Lithium, Beryllium, Boron, Carbon, Nitrogen, Oxygen, Fluorine, Neon,
        Sodium, Magnesium, Aluminium, Silicon, Phosphorus, Sulfur, Chlorine, Argon,
    }

    public enum QuarkColour
    {
        Red, Green, Blue,
        Antired, Antigreen, Antiblue
    }

    public enum QuarkFlavour
    {
        Up, Down, Strange, Charm, Top, Bottom
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
    Rotation,
    StructureType, StructureShiftX, StructureShiftY,
    QuarkColour, QuarkFlavour,
    AtomElement,
    Nucleobase, NucleicBackbone,
    PlanetHydrosphereCoverage, PlanetHydrosphereType, PlanetRadius, PlanetIsLifeBearing, PlanetTemperature,
    PlanetType,
    SpectralClass,
    SpecialStar,
    GalaxyType
}