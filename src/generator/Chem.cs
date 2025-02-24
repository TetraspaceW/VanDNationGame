using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Chem
{
    private static readonly TerrainRule[] hydr = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Hydrogen.ToString()},
        {PropKey.AtomIsIonized, "False"}
    })};
    private static readonly TerrainRule[] heli = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Helium.ToString()},
        {PropKey.AtomIsIonized, "False"}
    })};
    
    private static readonly TerrainRule[] hydrIon = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Hydrogen.ToString()},
        {PropKey.AtomIsIonized, "True"}
    })};
    private static readonly TerrainRule[] heliIon = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Helium.ToString()},
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
    private static readonly TerrainRule[] molc = new[] { new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false, 1) };
    private static readonly TerrainRule[] freeElec = new[] { new TerrainRule(Terrain.TerrainType.FreeElectron, true, 1) };

    public static readonly Structure HYDROGEN = new Structure(new TerrainRule[,][] {
        { null, molc, null },
        { molc, hydr, molc },
        { molc, hydr, molc },
        { null, molc, null }
    }, "hydrogen");

    public static readonly Structure HELIUM = new Structure(new TerrainRule[,][] {
        { null, molc, null },
        { molc, heli, molc },
        { null, molc, null },
    }, "helium");
    
    public static readonly Structure HYDROGEN_IONIZED = new Structure(new TerrainRule[,][] {
        { null, molc, null },
        { molc, hydrIon, molc },
        { null, molc, null }
    }, "hydrogen_ionized");

    public static readonly Structure HELIUM_IONIZED = new Structure(new TerrainRule[,][] {
        { null, molc, null },
        { molc, heliIon, molc },
        { null, molc, null },
    }, "helium_ionized");
    
    public static readonly Structure FREE_ELECTRON = new Structure(new TerrainRule[,][] {
        { null, molc, null },
        { molc, freeElec, molc },
        { null, molc, null },
    }, "free_electron");

    public static readonly Structure WATER = new Structure(new TerrainRule[,][] {
        { molc, molc, molc, null },
        { molc, hydr, molc, molc },
        { molc, oxyg, hydr, molc },
        { molc, molc, molc, molc }, }, "water");
    public static readonly Structure HYDROXIDE = new Structure(new TerrainRule[,][] {
        { molc, molc, molc },
        { molc, hydr, molc },
        { molc, oxyg, molc },
        { molc, molc, molc }, }, "hydroxide");
    public static readonly Structure HYDRONIUM = new Structure(new TerrainRule[,][] {
        { null, molc, molc, molc, null },
        { molc, molc, hydr, molc, molc },
        { molc, hydr, oxyg, hydr, molc },
        { molc, molc, molc, molc, molc }, }, "hydronium");
    public static readonly Structure SILICA = new Structure(new TerrainRule[,][] {
        { silc, oxyg },
        { oxyg, molc }, }, "silica");
    public static readonly Structure ADENINE = new Structure(new TerrainRule[,][] {
        { hydr, null, null, null, hydr, null },
        { null, carb, nitr, null, nitr, hydr },
        { null, nitr, null, carb, carb, null }, //attatchment point 1,2
        { null, null, carb, null, null, nitr },
        { null, null, null, nitr, carb, null },
        { null, null, null, null, hydr, null } }, "adenine");

    public static readonly Structure GUANINE = new Structure(new TerrainRule[,][] {
        { hydr, null, null, null, null, null },
        { null, carb, nitr, null, null, oxyg },
        { null, nitr, null, carb, carb, null }, //attatchment point 1,2
        { null, null, carb, null, nitr, hydr },
        { null, null, null, nitr, carb, null },
        { null, null, null, null, nitr, hydr },
        { null, null, null, null, hydr, null } }, "guanine");
    public static readonly Structure THYMINE = new Structure(new TerrainRule[,][] {
        { null, hydr, hydr, null, null},
        { null, hydr, carb, null, oxyg},
        { hydr, null, carb, carb, null},
        { null, carb, null, nitr, hydr},
        { null, null, nitr, carb, null}, //attatchment point 2,4
        { null, null, null, oxyg, null} }, "thymine");
    public static readonly Structure CYTOSINE = new Structure(new TerrainRule[,][] {
        { null, null, null, hydr, null},
        { null, hydr, null, nitr, hydr},
        { hydr, null, carb, carb, null},
        { null, carb, null, null, nitr},
        { null, null, nitr, carb, null}, //attatchment point 2,4
        { null, null, null, null, oxyg} }, "cytosine");

    public static readonly Structure DNA_BACKBONE = new Structure(new TerrainRule[,][] {
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

    public static readonly Structure RNA_BACKBONE = new Structure(new TerrainRule[,][] {
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