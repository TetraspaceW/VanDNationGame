using System.Collections.Generic;


class Chem
{
    private static readonly TerrainRule[] hydr = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Hydrogen.ToString()}
    })};
    private static readonly TerrainRule[] heli = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Helium.ToString()}
    })};

    private static readonly TerrainRule[] hion = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Hydrogen.ToString()},
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
    private static readonly TerrainRule[] cion = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Carbon.ToString()},
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
    private static readonly TerrainRule[] nion = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Nitrogen.ToString()},
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
    private static readonly TerrainRule[] carb = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Carbon.ToString()}
    })};
    private static readonly TerrainRule[] nitr = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Nitrogen.ToString()}
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
    private static readonly TerrainRule[] alum = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Aluminium.ToString()}
    })};
    private static readonly TerrainRule[] iron = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Iron.ToString()}
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

    public static readonly Structure WATER = new(new TerrainRule[,][] {
        { molc, molc, molc, null },
        { molc, hydr, molc, molc },
        { molc, oxyg, hydr, molc },
        { molc, molc, molc, molc }, }, "water");
    public static readonly Structure ICE = new(new TerrainRule[,][] {
         { hydr, molc },
         { oxyg, hydr }, }, "ice");
    public static readonly Structure HYDROXIDE = new(new TerrainRule[,][] {
        { molc, molc, molc },
        { molc, hydr, molc },
        { molc, oxyg, molc },
        { molc, molc, molc }, }, "hydroxide");
    public static readonly Structure HYDRONIUM = new(new TerrainRule[,][] {
        { null, molc, molc, molc, null },
        { molc, molc, hydr, molc, molc },
        { molc, hydr, oxyg, hydr, molc },
        { molc, molc, molc, molc, molc }, }, "hydronium");
    public static readonly Structure SILICA = new(new TerrainRule[,][] {
        { silc, oxyg },
        { oxyg, molc }, }, "silica");
    public static readonly Structure ADENINE = new(new TerrainRule[,][] {
        { hydr, null, null, null, hydr, null },
        { null, carb, nitr, null, nitr, hydr },
        { null, nitr, null, carb, carb, null }, //attatchment point 1,2
        { null, null, carb, null, null, nitr },
        { null, null, null, nitr, carb, null },
        { null, null, null, null, hydr, null } }, "adenine");

    public static readonly Structure GUANINE = new(new TerrainRule[,][] {
        { hydr, null, null, null, null, null },
        { null, carb, nitr, null, null, oxyg },
        { null, nitr, null, carb, carb, null }, //attatchment point 1,2
        { null, null, carb, null, nitr, hydr },
        { null, null, null, nitr, carb, null },
        { null, null, null, null, nitr, hydr },
        { null, null, null, null, hydr, null } }, "guanine");
    public static readonly Structure THYMINE = new(new TerrainRule[,][] {
        { null, hydr, hydr, null, null},
        { null, hydr, carb, null, oxyg},
        { hydr, null, carb, carb, null},
        { null, carb, null, nitr, hydr},
        { null, null, nitr, carb, null}, //attatchment point 2,4
        { null, null, null, oxyg, null} }, "thymine");
    public static readonly Structure CYTOSINE = new(new TerrainRule[,][] {
        { null, null, null, hydr, null},
        { null, hydr, null, nitr, hydr},
        { hydr, null, carb, carb, null},
        { null, carb, null, null, nitr},
        { null, null, nitr, carb, null}, //attatchment point 2,4
        { null, null, null, null, oxyg} }, "cytosine");

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
        { null, carb, null, null, null, null } }, "dna_backbone");

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
        { null, carb, null, null, null, null } }, "rna_backbone");
}