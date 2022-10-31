using System.Linq;
using System.Collections.Generic;
class ChromatinGenerator
{
    TileModel tile;
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
        TerrainRule[] HCHL = HCHN.Select((it) => it.rotate(3)).ToArray();
        TerrainRule[] HCHR = HCHN.Select((it) => it.rotate(1)).ToArray();

        if (!Structure.structureDict.ContainsKey("30nm-chain"))
        {
            new Structure(new TerrainRule[,][] {
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { HCHN, null, null, null, null, null, null },
                { null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null },
            }, "30nm-chain");

            new Structure(new TerrainRule[,][] {
                { null, HCHL, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, null, null, null, null, null },
                { null, null, null, null, null, null, null },
            }, "30nm-turn-left");

            new Structure(new TerrainRule[,][] {
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, HNSM, HNSM, HNSM, null, null },
                { null, null, null, null, null, null, null },
                { null, null, null, null, null, HCHR, null },
            }, "30nm-turn-right");
        }

        Structure hcChainStart = new Structure(new TerrainRule[,][] {
            { HCHL, null },
            { null, null },
            { null, null },
            { null, null },
            { null, null },
            { null, null },
            { null, HCHR },
        });

        return StructureFill(Tiles,
            new StructureRule[] { new StructureRule(hcChainStart, 1), },
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
        TerrainRule[] LNKH = LNKV.Select((it) => it.rotate(1)).ToArray();

        TerrainRule ecChain = Structure.CreateStructureTile("10nm-chain", 0, 0, weight: 2);
        TerrainRule ecLeft = Structure.CreateStructureTile("10nm-turn-left", 0, 0);
        TerrainRule ecRight = Structure.CreateStructureTile("10nm-turn-right", 0, 0);

        TerrainRule[] ECHN = new[] { ecChain, ecLeft, ecRight };
        TerrainRule[] ECHL = ECHN.Select((it) => it.rotate(3)).ToArray();
        TerrainRule[] ECHR = ECHN.Select((it) => it.rotate(1)).ToArray();

        if (!Structure.structureDict.ContainsKey("10nm-chain"))
        {
            new Structure(new TerrainRule[,][] {
                { null, LNKV, null },
                { null, ENSM, null },
                { ECHN, null, null }
            }, "10nm-chain");

            new Structure(new TerrainRule[,][] {
                { ECHL, LNKV, null },
                { null, ENSM, null },
                { null, null, null }
            }, "10nm-turn-left");

            new Structure(new TerrainRule[,][] {
                { null, LNKV, null },
                { null, ENSM, null },
                { null, null, ECHR }
            }, "10nm-turn-right");
        }

        Structure ecChainStart = new Structure(new TerrainRule[,][] {
            { ECHL, null },
            { null, null },
            { null, ECHR },
        });

        return StructureFill(Tiles,
            new StructureRule[] { new StructureRule(ecChainStart, 1), },
            0.5,
            baseFill: new[] { new TerrainRule(Terrain.TerrainType.Nucleoplasm, true)
        });
    }

    private TileModel[,] StructureFill(TileModel[,] tiles, StructureRule[] rules, double chanceNone, TerrainRule[] baseFill, Terrain.TerrainType[] replace = null)
    {
        return TerrainGenRule.StructureFill(parent: tile, tiles, rules, chanceNone, baseFill, replace);
    }
}