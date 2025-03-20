using System.Collections.Generic;
using System.Linq.Expressions;


class Chem
{
    private static readonly TerrainRule[] hydr = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 99.99, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Hydrogen.ToString()}
    }), new TerrainRule(Terrain.TerrainType.Atom, true, 0.01, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Deuterium.ToString()}
    })};

    private static readonly TerrainRule[] heli = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Helium.ToString()}
    })};

    private static readonly TerrainRule[] hion = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 99.99, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Hydrogen.ToString()},
        {PropKey.AtomIsIonized, "True"}
    }), new TerrainRule(Terrain.TerrainType.Atom, true, 0.01, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Deuterium.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] heio = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Helium.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] oion = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Oxygen.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] cion = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 99, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Carbon.ToString()},
        {PropKey.AtomIsIonized, "True"}
    }), new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Carbon13.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] feio = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Iron.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] neio = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Neon.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] nion = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 99.6, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Nitrogen.ToString()},
        {PropKey.AtomIsIonized, "True"}
    }), new TerrainRule(Terrain.TerrainType.Atom, true, 0.4, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Nitrogen15.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] siio = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Silicon.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] mgio = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Magnesium.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] sion = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Sulfur.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] carb = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 99, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Carbon.ToString()}
    }), new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Carbon13.ToString()}
    })};
    private static readonly TerrainRule[] nitr = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 99.6, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Nitrogen.ToString()}
    }), new TerrainRule(Terrain.TerrainType.Atom, true, 0.4, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Nitrogen15.ToString()}
    })};
    private static readonly TerrainRule[] oxyg = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Oxygen.ToString()}
    })};
    private static readonly TerrainRule[] silc = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Silicon.ToString()}
    })};
    private static readonly TerrainRule[] phos = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Phosphorus.ToString()}
    })};
    private static readonly TerrainRule[] magn = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Magnesium.ToString()}
    })};
    private static readonly TerrainRule[] alum = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Aluminium.ToString()}
    })};
    private static readonly TerrainRule[] sulf = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Sulfur.ToString()}
    })};
    private static readonly TerrainRule[] calc = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Calcium.ToString()}
    })};
    private static readonly TerrainRule[] iron = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Iron.ToString()}
    })};
    private static readonly TerrainRule[] tita = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Titanium.ToString()}
    })};
    private static readonly TerrainRule[] boro = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Boron.ToString()}
    })};
    private static readonly TerrainRule[] sodi = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Sodium.ToString()}
    })};
    private static readonly TerrainRule[] pota = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Potassium.ToString()}
    })};
    private static readonly TerrainRule[] stro = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Strontium.ToString()}
    })};
    private static readonly TerrainRule[] chlo = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Chlorine.ToString()}
    })};
    private static readonly TerrainRule[] brom = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Bromine.ToString()}
    })};
    private static readonly TerrainRule[] flur = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Fluorine.ToString()}
    })};
    private static readonly TerrainRule[] molc = new[] { new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false, 1) };
    private static readonly TerrainRule[] elec = new[] { new TerrainRule(Terrain.TerrainType.FreeElectron, true, 1) };

    public static readonly Structure HYDROGEN = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, hydr, molc },
        { molc, hydr, molc },
        { null, molc, null }
    }, "hydrogen");

    public static readonly Structure HELIUM = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, heli, molc },
        { null, molc, null },
    }, "helium");

    public static readonly Structure METHANE = new(new TerrainRule[,][] {
        { null, molc, null, molc, null },
        { molc, hydr, molc, hydr, molc },
        { null, molc, carb, molc, null },
        { molc, hydr, molc, hydr, molc },
        { null, molc, null, molc, null }
    }, "methane");

    public static readonly Structure AMMONIA = new(new TerrainRule[,][] {
        { null, molc, null, molc, null },
        { molc, hydr, molc, hydr, molc },
        { null, molc, nitr, molc, null },
        { null, molc, hydr, molc, null },
        { null, null, molc, null, null }
    }, "ammonia");

    public static readonly Structure ETHANE = new(new TerrainRule[,][] {
        { null, null, molc, molc, null, null },
        { null, molc, hydr, hydr, molc, null },
        { molc, hydr, carb, carb, hydr, molc },
        { null, molc, hydr, hydr, molc, null },
        { null, null, molc, molc, null, null }
    }, "ethane");

    public static readonly Structure HYDROGEN_IONIZED = new(new TerrainRule[,][] {
        { hion }
    }, "hydrogen_ionized");

    public static readonly Structure HELIUM_IONIZED = new(new TerrainRule[,][] {
        { heio }
    }, "helium_ionized");

    public static readonly Structure OXYGEN_IONIZED = new(new TerrainRule[,][] {
        { oion }
    }, "oxygen_ionized");

    public static readonly Structure CARBON_IONIZED = new(new TerrainRule[,][] {
        { cion }
    }, "carbon_ionized");

    public static readonly Structure IRON_IONIZED = new(new TerrainRule[,][] {
        { feio }
    }, "iron_ionized");

    public static readonly Structure NEON_IONIZED = new(new TerrainRule[,][] {
        { neio }
    }, "neon_ionized");

    public static readonly Structure NITROGEN_IONIZED = new(new TerrainRule[,][] {
        { nion }
    }, "nitrogen_ionized");

    public static readonly Structure SILICON_IONIZED = new(new TerrainRule[,][] {
        { siio }
    }, "silicon_ionized");

    public static readonly Structure MAGNESIUM_IONIZED = new(new TerrainRule[,][] {
        { mgio }
    }, "magnesium_ionized");

    public static readonly Structure SULFUR_IONIZED = new(new TerrainRule[,][] {
        { sion }
    }, "sulfur_ionized");

    public static readonly Structure FREE_ELECTRON = new(new TerrainRule[,][] {
        { elec }
    }, "free_electron");
    public static readonly Structure SODIUM = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, sodi, molc },
        { null, molc, null }
    }, "sodium");
    public static readonly Structure MAGNESIUM = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, magn, molc },
        { null, molc, null }
    }, "magnesium");
    public static readonly Structure CALCIUM = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, calc, molc },
        { null, molc, null }
    }, "calcium");
    public static readonly Structure POTASSIUM = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, pota, molc },
        { null, molc, null }
    }, "potassium");
    public static readonly Structure STRONTIUM = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, stro, molc },
        { null, molc, null }
    }, "strontium");
    public static readonly Structure CHLORIDE = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, chlo, molc },
        { null, molc, null }
    }, "chloride");
    public static readonly Structure SULFATE = new(new TerrainRule[,][] {
        { null, molc, null, molc, null },
        { molc, oxyg, molc, oxyg, molc },
        { null, molc, sulf, molc, null },
        { molc, oxyg, molc, oxyg, molc },
        { null, molc, null, molc, null }
    }, "sulfate");
    public static readonly Structure BICARBONATE = new(new TerrainRule[,][] {
        { null, molc, null, molc, null, null },
        { molc, oxyg, molc, oxyg, molc, null },
        { null, molc, carb, molc, hydr, molc },
        { null, molc, oxyg, molc, molc, null },
        { null, null, molc, null, null, null }
    }, "bicarbonate");
    public static readonly Structure BROMIDE = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, brom, molc },
        { null, molc, null }
    }, "bromide");
    public static readonly Structure BORATE = new(new TerrainRule[,][] {
        { null, molc, null, molc, null },
        { molc, oxyg, molc, oxyg, molc },
        { null, molc, boro, molc, null },
        { null, molc, oxyg, molc, null },
        { null, null, molc, null, null }
    }, "borate");
    public static readonly Structure FLUORIDE = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, flur, molc },
        { null, molc, null }
    }, "fluoride");
    public static readonly Structure WATER = new(new TerrainRule[,][] {
        { null, molc, null, null },
        { molc, hydr, molc, null },
        { molc, oxyg, hydr, molc },
        { null, molc, molc, null },
    }, "water");
    public static readonly Structure ICE = new(new TerrainRule[,][] {
        { hydr, null },
        { oxyg, hydr },
    }, "ice");
    public static readonly Structure CARBON_DIOXIDE_ICE = new(new TerrainRule[,][] {
        { oxyg, carb, oxyg }
    }, "carbon-dioxide-ice");
    public static readonly Structure NITROGEN_ICE = new(new TerrainRule[,][] {
        { nitr, nitr }
    }, "nitrogen-ice");
    public static readonly Structure METHANE_ICE = new(new TerrainRule[,][] {
        { hydr, null, hydr },
        { null, carb, null },
        { hydr, null, hydr }
    }, "methane-ice");
    public static readonly Structure AMMONIA_ICE = new(new TerrainRule[,][] {
        { hydr, null, hydr },
        { null, nitr, null },
        { null, hydr, null }
    }, "ammonia-ice");
    public static readonly Structure ETHANE_ICE = new(new TerrainRule[,][] {
        { null, hydr, hydr, null },
        { hydr, carb, carb, hydr },
        { null, hydr, hydr, null }
    }, "ethane-ice");
    public static readonly Structure THOLIN = new(new TerrainRule[,][] {
        { hydr, null, nitr, null, hydr },
        { null, carb, null, carb, null },
        { null, carb, null, carb, null },
        { hydr, null, carb, null, hydr },
        { null, null, hydr, null, null }
    }, "tholin");
    public static readonly Structure HYDROXIDE = new(new TerrainRule[,][] {
        { null, molc, null },
        { molc, hydr, molc },
        { molc, oxyg, molc },
        { null, molc, null },
    }, "hydroxide");
    public static readonly Structure HYDRONIUM = new(new TerrainRule[,][] {
        { null, null, molc, null, null },
        { null, molc, hydr, molc, null },
        { molc, hydr, oxyg, hydr, molc },
        { null, molc, molc, molc, null },
    }, "hydronium");
    public static readonly Structure SILICA = new(new TerrainRule[,][] {
        { silc, oxyg },
        { oxyg, null },
    }, "silica");
    public static readonly Structure ANORTHITE = new(new TerrainRule[,][] {
        { oxyg, null, oxyg },
        { null, silc, alum },
        { oxyg, null, oxyg },
        { oxyg, alum, oxyg },
        { null, silc, null },
        { oxyg, calc, oxyg },
    }, "anorthite");
    public static readonly Structure WOLLASTONITE = new(new TerrainRule[,][] {
        { null, null, oxyg },
        { oxyg, silc, null },
        { calc, oxyg, null },
    }, "wollastonite");
    public static readonly Structure ENSTATITE = new(new TerrainRule[,][] {
        { oxyg, magn, oxyg },
        { null, silc, null },
        { null, oxyg, null },
    }, "enstatite");
    public static readonly Structure FERROSILITE = new(new TerrainRule[,][] {
        { oxyg, iron, oxyg },
        { null, silc, null },
        { null, oxyg, null },
    }, "ferrosilite");
    public static readonly Structure FORSTERITE = new(new TerrainRule[,][] {
        { null, magn, null },
        { oxyg, null, oxyg },
        { null, silc, null },
        { oxyg, null, oxyg },
        { null, magn, null },
    }, "forsterite");

    public static readonly Structure FAYALITE = new(new TerrainRule[,][] {
        { oxyg, null, oxyg, iron },
        { null, silc, null, null  },
        { oxyg, null, oxyg, iron }
    }, "fayalite");

    public static readonly Structure ILMENITE = new(new TerrainRule[,][] {
        { null, null, oxyg },
        { iron, tita, oxyg },
        { null, null, oxyg }
    }, "ilmenite");
    public static readonly Structure KAOLINITE = new(new TerrainRule[,][] {
        { hydr, oxyg, alum, oxyg, hydr },
        { null, hydr, oxyg, null, null },
        { oxyg, null, oxyg, null, oxyg },
        { null, silc, null, silc, null },
        { null, oxyg, null, oxyg, null },
        { hydr, oxyg, alum, null, null }
    }, "kaolinite");
    public static readonly Structure TROILITE = new(new TerrainRule[,][] {
        { sulf, iron }
    }, "troilite");
    public static readonly Structure MAGNETITE = new(new TerrainRule[,][] {
        { oxyg, iron, oxyg, iron },
        { iron, oxyg, null, null },
        { oxyg, null, null, null }
    }, "magnetite");
    public static readonly Structure WUESTITE = new(new TerrainRule[,][] {
        { oxyg, iron }
    }, "wuestite");
    public static readonly Structure ADENINE = new(new TerrainRule[,][] {
        { hydr, null, null, null, hydr, null },
        { null, carb, nitr, null, nitr, hydr },
        { null, nitr, null, carb, carb, null }, //attatchment point 1,2
        { null, null, carb, null, null, nitr },
        { null, null, null, nitr, carb, null },
        { null, null, null, null, hydr, null }
    }, "adenine");

    public static readonly Structure GUANINE = new(new TerrainRule[,][] {
        { hydr, null, null, null, null, null },
        { null, carb, nitr, null, null, oxyg },
        { null, nitr, null, carb, carb, null }, //attatchment point 1,2
        { null, null, carb, null, nitr, hydr },
        { null, null, null, nitr, carb, null },
        { null, null, null, null, nitr, hydr },
        { null, null, null, null, hydr, null }
    }, "guanine");
    public static readonly Structure THYMINE = new(new TerrainRule[,][] {
        { null, hydr, hydr, null, null},
        { null, hydr, carb, null, oxyg},
        { hydr, null, carb, carb, null},
        { null, carb, null, nitr, hydr},
        { null, null, nitr, carb, null}, //attatchment point 2,4
        { null, null, null, oxyg, null}
    }, "thymine");
    public static readonly Structure CYTOSINE = new(new TerrainRule[,][] {
        { null, null, null, hydr, null},
        { null, hydr, null, nitr, hydr},
        { hydr, null, carb, carb, null},
        { null, carb, null, null, nitr},
        { null, null, nitr, carb, null}, //attatchment point 2,4
        { null, null, null, null, oxyg}
    }, "cytosine");

    public static readonly Structure DNA_BACKBONE = new(new TerrainRule[,][] {
        { null, oxyg, null, null, null, null },
        { oxyg, phos, oxyg, null, null, null },
        { null, oxyg, null, null, null, null },
        { hydr, carb, hydr, null, null, null },
        { null, hydr, carb, oxyg, null, null }, //attatchment point 5,4
        { null, hydr, carb, null, carb, null },
        { null, oxyg, null, carb, hydr, hydr },
        { oxyg, phos, oxyg, null, hydr, null },
        { null, oxyg, null, null, null, null },
        { null, carb, null, null, null, null }
    }, "dna_backbone");

    public static readonly Structure RNA_BACKBONE = new(new TerrainRule[,][] {
        { null, oxyg, null, null, null, null },
        { oxyg, phos, oxyg, null, null, null },
        { null, oxyg, null, null, null, null },
        { hydr, carb, hydr, null, null, null },
        { null, hydr, carb, oxyg, null, null }, //attatchment point 5,4
        { null, hydr, carb, null, carb, null },
        { null, oxyg, null, carb, hydr, hydr },
        { oxyg, phos, oxyg, null, oxyg, hydr },
        { null, oxyg, null, null, null, null },
        { null, carb, null, null, null, null }
    }, "rna_backbone");
}