using System.Collections.Generic;
class Structure
{
    public TerrainRule[,][] rules;

    private static readonly TerrainRule[] hydr = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Hydrogen.ToString()}
    })};
    private static readonly TerrainRule[] oxyg = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Oxygen.ToString()}
    })};
    private static readonly TerrainRule[] silc = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Silicon.ToString()}
    })};
    private static readonly TerrainRule[] molc = new[] { new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false, 1) };
    public static readonly Structure WATER = new Structure(new TerrainRule[,][] {
        { null, molc, null, null },
        { molc, hydr, molc, null },
        { molc, oxyg, hydr, molc },
        { null, molc, molc, null }, });
    public static readonly Structure HYDROXIDE = new Structure(new TerrainRule[,][] {
        { null, molc, null },
        { molc, hydr, molc },
        { molc, oxyg, molc },
        { null, molc, null }, });
    public static readonly Structure HYDRONIUM = new Structure(new TerrainRule[,][] {
        { null, null, molc, null, null },
        { null, molc, hydr, molc, null },
        { molc, hydr, oxyg, hydr, molc },
        { null, molc, molc, molc, null }, });
    public static readonly Structure SILICA = new Structure(new TerrainRule[,][] {
        { silc, oxyg },
        { oxyg, molc }, });

    public Structure(TerrainRule[,][] rules)
    {
        this.rules = rules;
    }
    public TileModel[,] AttemptPlace(TileModel parent, TileModel[,] tiles, int x, int y)
    {
        TileModel[,] tiles2 = (TileModel[,])tiles.Clone();
        for (int i = 0; i < rules.GetLength(0); i++)
        {
            for (int j = 0; j < rules.GetLength(1); j++)
            {
                int iMid = x + i - rules.GetLength(0) / 2;
                int jMid = y + j - rules.GetLength(1) / 2;
                if (iMid < 0 || iMid >= tiles.GetLength(0))
                {
                    continue;
                }
                if (jMid < 0 || jMid >= tiles.GetLength(1))
                {
                    continue;
                }
                else if (tiles[iMid, jMid] != null)
                {
                    return tiles2;
                }
                else if (rules[i, j] != null)
                {
                    tiles[iMid, jMid] = TerrainGenRule.RandomTileFromRule(parent, rules[i, j]);
                }

            }
        }
        return tiles;
    }

    public Structure Rotate(int rot)
    {
        if (rot == 1)
        {
            TerrainRule[,][] result = new TerrainRule[rules.GetLength(1), rules.GetLength(0)][];
            for (int i = 0; i < rules.GetLength(0); i++)
            {
                for (int j = 0; j < rules.GetLength(1); j++)
                {
                    result[rules.GetLength(1) - 1 - j, i] = rules[i, j];
                }
            }
            return new Structure(result);
        }
        else if (rot == 2)
        {
            TerrainRule[,][] result = new TerrainRule[rules.GetLength(0), rules.GetLength(1)][];
            for (int i = 0; i < rules.GetLength(0); i++)
            {
                for (int j = 0; j < rules.GetLength(1); j++)
                {
                    result[rules.GetLength(0) - 1 - i, rules.GetLength(1) - 1 - j] = rules[i, j];
                }
            }
            return new Structure(result);
        }
        else if (rot == 3)
        {
            TerrainRule[,][] result = new TerrainRule[rules.GetLength(1), rules.GetLength(0)][];
            for (int i = 0; i < rules.GetLength(0); i++)
            {
                for (int j = 0; j < rules.GetLength(1); j++)
                {
                    result[j, rules.GetLength(0) - 1 - i] = rules[i, j];
                }
            }
            return new Structure(result);
        }
        else
        {
            return this;
        }
    }
    public StructureRule[] RotateAll(double weight)
    {
        return new[] { new StructureRule(this, weight / 4), new StructureRule(Rotate(1), weight / 4), new StructureRule(Rotate(2), weight / 4), new StructureRule(Rotate(3), weight / 4) };
    }
}