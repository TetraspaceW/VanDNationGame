using System.Linq;
using System.Collections.Generic;
class DNAGenerator
{
    readonly TileModel tile;
    TileModel[,] Tiles;

    public DNAGenerator(TileModel tile, TileModel[,] Tiles)
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

    public static Structure CreateNucleotideStructure(TerrainRule base1, TerrainRule base2, TerrainRule[] continuations, string name)
    {
        return new Structure(new TerrainRule[,][] {
            { new[] { base1 }, new[] { base2.Rotate(2) } },
            { continuations, null },
        }, name);
    }

    public static Structure CreateNTNucleotideStructure(TerrainRule base1, TerrainRule base2, string name)
    {
        return new Structure(new TerrainRule[,][] {
            { new[] { base1 }, new[] { base2.Rotate(2) } },
        }, name);
    }

    public Dictionary<string, TerrainRule> InitialiseDNAStructures()
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
        TerrainRule adenineThymineNT = Structure.CreateStructureTile("adenine-thymine-nt", 0, 0, 0);
        TerrainRule guanineCytosineNT = Structure.CreateStructureTile("guanine-cytosine-nt", 0, 0, 0);
        TerrainRule thymineAdenineNT = Structure.CreateStructureTile("thymine-adenine-nt", 0, 0, 0);
        TerrainRule cytosineGuanineNT = Structure.CreateStructureTile("cytosine-guanine-nt", 0, 0, 0);
        TerrainRule dnaLeft = Structure.CreateStructureTile("dna-turn-left", 1, 0, 0, 0.5);
        TerrainRule dnaRight = Structure.CreateStructureTile("dna-turn-right", 0, 0, 0, 0.5);
        TerrainRule dnaTurn = Structure.CreateStructureTile("dna-turn", 0, 0, 0);

        TerrainRule[] nucleotideStructures = new[] { adenineThymine, guanineCytosine, thymineAdenine, cytosineGuanine, dnaLeft, dnaRight };
        TerrainRule[] nucleotideStructuresLeft = new[] { adenineThymine.Rotate(3), guanineCytosine.Rotate(3), thymineAdenine.Rotate(3), cytosineGuanine.Rotate(3), dnaLeft.Rotate(3), dnaRight.Rotate(3) };
        TerrainRule[] nucleotideStructuresRight = new[] { adenineThymine.Rotate(1), guanineCytosine.Rotate(1), thymineAdenine.Rotate(1), cytosineGuanine.Rotate(1), dnaLeft.Rotate(1), dnaRight.Rotate(1) };

        if (!Structure.structureDict.ContainsKey("adenine-thymine"))
        {
            _ = CreateNucleotideStructure(adenine, thymine, nucleotideStructures, "adenine-thymine");
            _ = CreateNucleotideStructure(guanine, cytosine, nucleotideStructures, "guanine-cytosine");
            _ = CreateNucleotideStructure(thymine, adenine, nucleotideStructures, "thymine-adenine");
            _ = CreateNucleotideStructure(cytosine, guanine, nucleotideStructures, "cytosine-guanine");
            _ = CreateNTNucleotideStructure(adenine, thymine, "adenine-thymine-nt");
            _ = CreateNTNucleotideStructure(guanine, cytosine, "guanine-cytosine-nt");
            _ = CreateNTNucleotideStructure(thymine, adenine, "thymine-adenine-nt");
            _ = CreateNTNucleotideStructure(cytosine, guanine, "cytosine-guanine-nt");
            _ = new Structure(new TerrainRule[,][] {
                    { nucleotideStructuresLeft, new[] { turnInner.Rotate(3) }, new[] { blank.Rotate(2) } },
                    { null,  new[] { blank.Rotate(1) } , new[] { turnOuter.Rotate(3) } },
                }, "dna-turn-left");
            _ = new Structure(new TerrainRule[,][] {
                    { new[] { blank } , new[] { turnInner.Rotate(2) }, null },
                    { new[] { turnOuter.Rotate(2) } , new[] { blank.Rotate(1) }, nucleotideStructuresRight },
                }, "dna-turn-right");
            _ = new Structure(new TerrainRule[,][] {
                    { new[] { turnInner.Rotate(3) }, new[] { blank.Rotate(2) } },
                    { new[] { blank.Rotate(1) } , new[] { turnOuter.Rotate(3) } },
                }, "dna-turn");
        }

        return new Dictionary<string, TerrainRule>() {
            { "adenine", adenine },
            { "guanine", guanine },
            { "thymine", thymine },
            { "cytosine", cytosine },
            { "blank", blank },
            { "turnInner", turnInner },
            { "turnOuter", turnOuter },
            { "adenine-thymine", adenineThymine },
            { "guanine-cytosine", guanineCytosine },
            { "thymine-adenine", thymineAdenine },
            { "cytosine-guanine", cytosineGuanine },
            { "dna-turn-left", dnaLeft },
            { "dna-turn-right", dnaRight },
            { "dna-turn", dnaTurn },
            { "adenine-thymine-nt", adenineThymineNT },
            { "guanine-cytosine-nt", guanineCytosineNT },
            { "thymine-adenine-nt", thymineAdenineNT },
            { "cytosine-guanine-nt", cytosineGuanineNT },
        };
    }

    public TileModel[,] GenerateDNA(double density)
    {
        Dictionary<string, TerrainRule> dnaStructures = InitialiseDNAStructures();
        TerrainRule adenineThymine = dnaStructures["adenine-thymine"];
        TerrainRule guanineCytosine = dnaStructures["guanine-cytosine"];
        TerrainRule thymineAdenine = dnaStructures["thymine-adenine"];
        TerrainRule cytosineGuanine = dnaStructures["cytosine-guanine"];
        TerrainRule dnaLeft = dnaStructures["dna-turn-left"];
        TerrainRule dnaRight = dnaStructures["dna-turn-right"];
        TerrainRule[] nucleotideStructuresLeft = new[] { adenineThymine.Rotate(3), guanineCytosine.Rotate(3), thymineAdenine.Rotate(3), cytosineGuanine.Rotate(3), dnaLeft.Rotate(3), dnaRight.Rotate(3) };
        TerrainRule[] nucleotideStructuresRight = new[] { adenineThymine.Rotate(1), guanineCytosine.Rotate(1), thymineAdenine.Rotate(1), cytosineGuanine.Rotate(1), dnaLeft.Rotate(1), dnaRight.Rotate(1) };

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

    public TileModel[,] GenerateHistone()
    {
        Dictionary<string, TerrainRule> dnaStructures = InitialiseDNAStructures();
        TerrainRule adenineThymineNT = dnaStructures["adenine-thymine-nt"];
        TerrainRule guanineCytosineNT = dnaStructures["guanine-cytosine-nt"];
        TerrainRule thymineAdenineNT = dnaStructures["thymine-adenine-nt"];
        TerrainRule cytosineGuanineNT = dnaStructures["cytosine-guanine-nt"];
        TerrainRule blank = dnaStructures["blank"];
        TerrainRule dnaTurn = dnaStructures["dna-turn"];

        TerrainRule[] nucleotideStructures = new[] { adenineThymineNT, guanineCytosineNT, thymineAdenineNT, cytosineGuanineNT };

        TerrainRule[] DNAH = nucleotideStructures.Select((it) => it.Rotate(3)).ToArray();
        TerrainRule[] GNTL = new[] { dnaTurn.Rotate(2) };
        TerrainRule[] GNTR = new[] { dnaTurn.Rotate(1) };
        TerrainRule[] GNBL = new[] { dnaTurn.Rotate(3) };
        TerrainRule[] DNAV = nucleotideStructures;

        Structure histone = new(new TerrainRule[,][] {
            { null, null, DNAH, DNAH, DNAH, DNAH, DNAH, DNAH, null, null },
            { null, GNTL, null, null, null, null, null, null, GNTR, null },
            { DNAV, null, null, null, null, null, null, null, DNAV, null },
            { DNAV, null, null, null, null, null, null, null, DNAV, null },
            { DNAV, null, null, null, null, null, null, null, DNAV, null },
            { DNAV, null, null, null, null, null, null, null, DNAV, null },
            { DNAV, null, null, null, null, null, null, null, DNAV, null },
            { DNAV, null, null, null, null, null, null, null, DNAV, null },
            { null, GNBL, DNAH, DNAH, DNAH, DNAH, DNAH, DNAH, DNAH, DNAH },
            { null, null, null, null, null, null, null, null, null, null },
        });

        Tiles = PlaceStructure(Tiles, new[] { new StructureRule(histone) }, 0, 0, new[] { new TerrainRule(Terrain.TerrainType.AlphaHelix, true) });
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