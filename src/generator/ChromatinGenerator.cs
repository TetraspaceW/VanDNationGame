using System.Linq;
using System.Collections.Generic;
class ChromatinGenerator
{
    readonly TileModel tile;
    TileModel[,] Tiles;

    public ChromatinGenerator(TileModel tile, TileModel[,] Tiles)
    {
        this.tile = tile;
        this.Tiles = Tiles;
    }
    public TileModel[,] GenerateHeterochromatin()
    {
        TerrainRule[] HNSM = new[] { new TerrainRule(Terrain.TerrainType.Nucleosome, true) };

        TerrainRule hcChain = Structure.CreateStructureTile("30nm-chain", 0, 0, weight: 2);
        TerrainRule hcLeft = Structure.CreateStructureTile("30nm-turn-left", 0, 0);
        TerrainRule hcRight = Structure.CreateStructureTile("30nm-turn-right", 0, 0);

        TerrainRule[] HCHN = new[] { hcChain, hcLeft, hcRight };
        TerrainRule[] HCHL = HCHN.Select((it) => it.Rotate(3)).ToArray();
        TerrainRule[] HCHR = HCHN.Select((it) => it.Rotate(1)).ToArray();

        if (!Structure.structureDict.ContainsKey("30nm-chain"))
        {
            _ = new Structure(new TerrainRule[,][] {
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { HCHN, null, null, null, null, null, null },
                { null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null },
            }, "30nm-chain");

            _ = new Structure(new TerrainRule[,][] {
                { null, HCHL, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null },
            }, "30nm-turn-left");

            _ = new Structure(new TerrainRule[,][] {
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, null, null, null, null, null },
                { null, null, null, null, null, HCHR, null },
            }, "30nm-turn-right");
        }

        Structure hcChainStart = new(new TerrainRule[,][] {
            { HCHL, null },
            { null, null },
            { null, null },
            { null, null },
            { null, null },
            { null, null },
            { null, HCHR },
        });

        return StructureFill(Tiles,
            new StructureRule[] { new(hcChainStart, 1), },
            0.5,
            baseFill: new[] { new TerrainRule(Terrain.TerrainType.Nucleoplasm, true)
        });
    }

    public TileModel[,] GenerateEuchromatin()
    {
        TerrainRule[] ENSM = new[] { new TerrainRule(Terrain.TerrainType.Nucleosome, true) };
        TerrainRule[] LNKV = new[] { new TerrainRule(Terrain.TerrainType.LinkerDNA, true, props: new Dictionary<PropKey, string>() {
            { PropKey.Rotation, "0" }
        }) };
        TerrainRule[] LNKH = LNKV.Select((it) => it.Rotate(1)).ToArray();

        TerrainRule ecChain = Structure.CreateStructureTile("10nm-chain", 0, 0, weight: 2);
        TerrainRule ecLeft = Structure.CreateStructureTile("10nm-turn-left", 0, 0);
        TerrainRule ecRight = Structure.CreateStructureTile("10nm-turn-right", 0, 0);

        TerrainRule[] ECHN = new[] { ecChain, ecLeft, ecRight };
        TerrainRule[] ECHL = ECHN.Select((it) => it.Rotate(3)).ToArray();
        TerrainRule[] ECHR = ECHN.Select((it) => it.Rotate(1)).ToArray();

        if (!Structure.structureDict.ContainsKey("10nm-chain"))
        {
            _ = new Structure(new TerrainRule[,][] {
                { null, LNKV, null },
                { null, ENSM, null },
                { ECHN, null, null }
            }, "10nm-chain");

            _ = new Structure(new TerrainRule[,][] {
                { ECHL, LNKV, null },
                { null, ENSM, null },
                { null, null, null }
            }, "10nm-turn-left");

            _ = new Structure(new TerrainRule[,][] {
                { null, LNKV, null },
                { null, ENSM, null },
                { null, null, ECHR }
            }, "10nm-turn-right");
        }

        Structure ecChainStart = new(new TerrainRule[,][] {
            { ECHL, null },
            { null, null },
            { null, ECHR },
        });

        return StructureFill(Tiles,
            new StructureRule[] { new(ecChainStart, 1), },
            0.5,
            baseFill: new[] { new TerrainRule(Terrain.TerrainType.Nucleoplasm, true)
        });
    }

    public TileModel[,] GenerateDNA(double density)
    {
        TerrainRule adenine = new(Terrain.TerrainType.Nucleotide, true, props: new Dictionary<PropKey, string>() {
                        {PropKey.Nucleobase, "adenine"},
                        {PropKey.NucleicBackbone, "DNA"},
                        {PropKey.Rotation, "0"}
                    });
        TerrainRule guanine = new(Terrain.TerrainType.Nucleotide, true, props: new Dictionary<PropKey, string>() {
                        {PropKey.Nucleobase, "guanine"},
                        {PropKey.NucleicBackbone, "DNA"},
                        {PropKey.Rotation, "0"}
                    });
        TerrainRule thymine = new(Terrain.TerrainType.Nucleotide, true, props: new Dictionary<PropKey, string>() {
                        {PropKey.Nucleobase, "thymine"},
                        {PropKey.NucleicBackbone, "DNA"},
                        {PropKey.Rotation, "0"}
                    });
        TerrainRule cytosine = new(Terrain.TerrainType.Nucleotide, true, props: new Dictionary<PropKey, string>() {
                        {PropKey.Nucleobase, "cytosine"},
                        {PropKey.NucleicBackbone, "DNA"},
                        {PropKey.Rotation, "0"}
                    });
        TerrainRule blank = new(Terrain.TerrainType.NucleotideBlank, props: new Dictionary<PropKey, string>() {
                        {PropKey.NucleicBackbone, "DNA"},
                        {PropKey.Rotation, "0"}
                    });
        TerrainRule turnInner = new(Terrain.TerrainType.NucleotideTurnInner, props: new Dictionary<PropKey, string>() {
                        {PropKey.NucleicBackbone, "DNA"},
                        {PropKey.Rotation, "0"}
                    });
        TerrainRule turnOuter = new(Terrain.TerrainType.NucleotideTurnOuter, props: new Dictionary<PropKey, string>() {
                        {PropKey.NucleicBackbone, "DNA"},
                        {PropKey.Rotation, "0"}
                    });

        TerrainRule adenineThymine = Structure.CreateStructureTile("adenine-thymine", 0, 0, 0);
        TerrainRule guanineCytosine = Structure.CreateStructureTile("guanine-cytosine", 0, 0, 0);
        TerrainRule thymineAdenine = Structure.CreateStructureTile("thymine-adenine", 0, 0, 0);
        TerrainRule cytosineGuanine = Structure.CreateStructureTile("cytosine-guanine", 0, 0, 0);
        TerrainRule dnaLeft = Structure.CreateStructureTile("dna-turn-left", 1, 0, 0, 0.5);
        TerrainRule dnaRight = Structure.CreateStructureTile("dna-turn-right", 0, 0, 0, 0.5);

        TerrainRule[] nucleotideStructures = new[] { adenineThymine, guanineCytosine, thymineAdenine, cytosineGuanine, dnaLeft, dnaRight };
        TerrainRule[] nucleotideStructuresLeft = new[] { adenineThymine.Rotate(3), guanineCytosine.Rotate(3), thymineAdenine.Rotate(3), cytosineGuanine.Rotate(3), dnaLeft.Rotate(3), dnaRight.Rotate(3) };
        TerrainRule[] nucleotideStructuresRight = new[] { adenineThymine.Rotate(1), guanineCytosine.Rotate(1), thymineAdenine.Rotate(1), cytosineGuanine.Rotate(1), dnaLeft.Rotate(1), dnaRight.Rotate(1) };

        if (!Structure.structureDict.ContainsKey("adenine-thymine"))
        {
            _ = new Structure(new TerrainRule[,][] {
                        { new[] { adenine }, new[] { thymine.Rotate(2) } },
                        { nucleotideStructures, null },
                    }, "adenine-thymine");
            _ = new Structure(new TerrainRule[,][] {
                        { new[] { thymine }, new[] { adenine.Rotate(2) } },
                        { nucleotideStructures, null },
                    }, "thymine-adenine");
            _ = new Structure(new TerrainRule[,][] {
                        { new[] { guanine }, new[] { cytosine.Rotate(2) } },
                        { nucleotideStructures, null },
                    }, "guanine-cytosine");
            _ = new Structure(new TerrainRule[,][] {
                        { new[] { cytosine }, new[] { guanine.Rotate(2) } },
                        { nucleotideStructures, null },
                    }, "cytosine-guanine");
            _ = new Structure(new TerrainRule[,][] {
                        { nucleotideStructuresLeft, new[] { turnInner.Rotate(3) }, new[] { blank.Rotate(2) } },
                        { null,  new[] { blank.Rotate(1) } , new[] { turnOuter.Rotate(3) } },
                    }, "dna-turn-left");
            _ = new Structure(new TerrainRule[,][] {
                        { new[] { blank } , new[] { turnInner.Rotate(2) }, null },
                        { new[] { turnOuter.Rotate(2) } , new[] { blank.Rotate(1) }, nucleotideStructuresRight },
                    }, "dna-turn-right");
        }

        Structure dnaStart = new(new TerrainRule[,][] {
                        { nucleotideStructuresLeft , null },
                        { null , nucleotideStructuresRight },
                    });

        Tiles = StructureFill(Tiles, new StructureRule[] {
                    new(dnaStart, 1),
                    new(dnaStart.Rotate(1), 1),
                    },
            density, new[] { new TerrainRule(Terrain.TerrainType.IntermolecularFluid, true)
            });
        Tiles = PlaceStructure(Tiles, new[] { new StructureRule(dnaStart, 1) }, RND.Next(0, 10), RND.Next(0, 10), new[] { new TerrainRule(Terrain.TerrainType.IntermolecularFluid, true) }, 0, new[] { Terrain.TerrainType.IntermolecularFluid });
        return Tiles;
    }

    private TileModel[,] StructureFill(TileModel[,] tiles, StructureRule[] rules, double chanceNone, TerrainRule[] baseFill, Terrain.TerrainType[] replace = null)
    {
        return TerrainGenRule.StructureFill(parent: tile, tiles, rules, chanceNone, baseFill, replace);
    }
    private TileModel[,] PlaceStructure(TileModel[,] tiles, StructureRule[] rules, int initX, int initY, TerrainRule[] baseFill, int rotate = 0, Terrain.TerrainType[] replace = null)
    {
        return TerrainGenRule.PlaceStructure(parent: tile, tiles, rules, initX, initY, baseFill, rotate, replace);
    }
}