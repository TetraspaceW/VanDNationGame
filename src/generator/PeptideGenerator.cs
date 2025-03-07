using System.Linq;
using System.Collections.Generic;
using Godot;
using System.Drawing;
class PeptideGenerator
{
    readonly TileModel tile;
    readonly TileModel[,] Tiles;

    public PeptideGenerator(TileModel tile, TileModel[,] Tiles)
    {
        this.tile = tile;
        this.Tiles = Tiles;
    }
    public TileModel[,] GeneratePeptideChain()
    {
        _ = Chem.ARGININE; // lazy load static Chem variables if necessary
        TerrainRule[] HNSM = new[] { new TerrainRule(Terrain.TerrainType.Nucleosome, true) };

        TerrainRule hcChain = Structure.CreateStructureTile("peptide-chain", 0, 0);

        TerrainRule[] PCHN = new[] { hcChain };
        TerrainRule[] PCHI = PCHN.Select((it) => it.Reflect(1)).ToArray();

        string[] aminoAcids = new[] {
            "arginine", "histidine", "lysine", "aspartic-acid", "glutamic-acid",
            "serine", "threonine", "asparagine", "glutamine",
            "cysteine", "selenocysteine", "glycine", "proline",
            "alanine", "valine", "leucine", "isoleucine", "methionine", "phenylalanine", "tyrosine", "tryptophan"
        };
        TerrainRule[] RGRP = aminoAcids.Select((it) => Structure.CreateStructureTile(it, 0, 0)).ToArray();

        TerrainRule[] oxyg = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, props: new Dictionary<PropKey, string>() {
            { PropKey.AtomElement, Terrain.AtomElement.Oxygen.ToString() }
        }) };
        TerrainRule[] carb = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, props: new Dictionary<PropKey, string>() {
            { PropKey.AtomElement, Terrain.AtomElement.Carbon.ToString() }
        }) };
        TerrainRule[] nitr = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, props: new Dictionary<PropKey, string>() {
            { PropKey.AtomElement, Terrain.AtomElement.Nitrogen.ToString() }
        }) };
        TerrainRule[] hydr = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, props: new Dictionary<PropKey, string>() {
            { PropKey.AtomElement, Terrain.AtomElement.Hydrogen.ToString() }
        }) };

        if (!Structure.structureDict.ContainsKey("peptide-chain"))
        {
            _ = new Structure(new TerrainRule[,][] {
                { null, hydr, null, null },
                { carb, nitr, null, null },
                { oxyg, null, carb, PCHI },
                { null, null, null, null }
            }, "peptide-chain");
        }

        Structure peptideChainStart = new(new TerrainRule[,][] {
            { PCHN },
        });

        return StructureFill(Tiles,
            new StructureRule[] { new(peptideChainStart, 1), },
            0.99,
            baseFill: new[] { new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
        });
    }

    private TileModel[,] StructureFill(TileModel[,] tiles, StructureRule[] rules, double chanceNone, TerrainRule[] baseFill, Terrain.TerrainType[] replace = null)
    {
        return TerrainGenRule.StructureFill(parent: tile, tiles, rules, chanceNone, baseFill, replace);
    }
}