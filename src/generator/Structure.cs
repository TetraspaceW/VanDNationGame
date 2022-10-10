using System.Collections.Generic;
using System.Linq;
class Structure
{
    public TerrainRule[,][] rules;
    public static Dictionary<string, Structure> structureDict = new Dictionary<string, Structure>();
    public string name;

    private static readonly TerrainRule[] hydr = new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
        {PropKey.AtomElement, Terrain.AtomElement.Hydrogen.ToString()}
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
    public static readonly Structure NULL = new Structure(new TerrainRule[,][] {
        { null }, }, "null");
    public static readonly Structure WATER = new Structure(new TerrainRule[,][] {
        { null, molc, null, null },
        { molc, hydr, molc, null },
        { molc, oxyg, hydr, molc },
        { null, molc, molc, null }, }, "water");
    public static readonly Structure HYDROXIDE = new Structure(new TerrainRule[,][] {
        { null, molc, null },
        { molc, hydr, molc },
        { molc, oxyg, molc },
        { null, molc, null }, }, "hydroxide");
    public static readonly Structure HYDRONIUM = new Structure(new TerrainRule[,][] {
        { null, null, molc, null, null },
        { null, molc, hydr, molc, null },
        { molc, hydr, oxyg, hydr, molc },
        { null, molc, molc, molc, null }, }, "hydronium");
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

    public Structure(TerrainRule[,][] rules, string name = null)
    {
        this.rules = rules;
        if (name != null)
        {
            structureDict.Add(name, this);
        }
        this.name = name;
    }
    public TileModel[,] AttemptPlace(TileModel parent, TileModel[,] tiles, int x, int y, int rotation = 0)
    {
        TileModel[,] tiles2 = (TileModel[,])tiles.Clone();

        int x2 = x;
        int y2 = y;
        if (rotation == 1)
        {
            x2 = y;
            y2 = tiles.GetLength(0) - 1 - x - rules.GetLength(0) + 1;
        }
        else if (rotation == 2)
        {
            x2 = tiles.GetLength(0) - 1 - x - rules.GetLength(1) + 1;
            y2 = tiles.GetLength(1) - 1 - y - rules.GetLength(0) + 1;
        }
        else if (rotation == 3)
        {
            x2 = tiles.GetLength(1) - 1 - y - rules.GetLength(1) + 1;
            y2 = x;
        }
        x = x2;
        y = y2;
        for (int i = 0; i < rules.GetLength(1); i++)
        {
            for (int j = 0; j < rules.GetLength(0); j++)
            {
                int iMid = x + i;
                int jMid = y + j;
                if (iMid < 0 || iMid >= tiles.GetLength(0))
                {
                    continue;
                }
                if (jMid < 0 || jMid >= tiles.GetLength(1))
                {
                    continue;
                }
                else if (tiles[iMid, jMid] != null && rules[j, i] != null)
                {

                    return tiles2;
                }
                else if (rules[j, i] != null)
                {
                    tiles[iMid, jMid] = TerrainGenRule.RandomTileFromRule(parent, rules[j, i]);
                }

            }
        }
        for (int i = 0; i < rules.GetLength(1); i++)
        {
            for (int j = 0; j < rules.GetLength(0); j++)
            {
                int iMid = x + i;
                int jMid = y + j;
                if (iMid < 0 || iMid >= tiles.GetLength(0))
                {
                    continue;
                }
                if (jMid < 0 || jMid >= tiles.GetLength(1))
                {
                    continue;
                }
                if (tiles[iMid, jMid] == null)
                {
                    continue;
                }

                if (tiles[iMid, jMid].terrain.terrainType == Terrain.TerrainType.StructureTile)
                {
                    Dictionary<PropKey, string> temp = tiles[iMid, jMid].terrain.props;
                    tiles[iMid, jMid] = null;
                    Structure structure = structureDict[temp[PropKey.StructureType]];
                    int rotate = int.Parse(temp[PropKey.Rotation]);
                    int posX = int.Parse(temp[PropKey.StructureShiftX]);
                    int posY = int.Parse(temp[PropKey.StructureShiftY]);
                    int posX2 = posX;
                    int posY2 = posY;
                    if (rotate == 1)
                    {
                        posX2 = posY;
                        posY2 = structure.rules.GetLength(1) - 1 - posX;
                    }
                    else if (rotate == 2)
                    {
                        posX2 = structure.rules.GetLength(1) - 1 - posX;
                        posY2 = structure.rules.GetLength(0) - 1 - posY;
                    }
                    else if (rotate == 3)
                    {
                        posX2 = structure.rules.GetLength(0) - 1 - posY;
                        posY2 = posX;
                    }
                    posX = posX2;
                    posY = posY2;
                    tiles = structure.Rotate(rotate).AttemptPlace(parent, tiles, iMid - posX, jMid - posY);
                }
            }
        }
        return tiles;
    }

    public Structure Rotate(int rot)
    {
        rot %= 4;
        if (rot == 1)
        {
            TerrainRule[,][] result = new TerrainRule[rules.GetLength(1), rules.GetLength(0)][];
            for (int i = 0; i < rules.GetLength(0); i++)
            {
                for (int j = 0; j < rules.GetLength(1); j++)
                {
                    if (rules[i, j] != null)
                    {
                        result[rules.GetLength(1) - 1 - j, i] = (TerrainRule[])rules[i, j].Clone();
                        for (int k = 0; k < result[rules.GetLength(1) - 1 - j, i].Length; k++)
                        {
                            if (result[rules.GetLength(1) - 1 - j, i][k].props.ContainsKey(PropKey.Rotation))
                            {
                                result[rules.GetLength(1) - 1 - j, i][k] = result[rules.GetLength(1) - 1 - j, i][k].rotate(rot);
                            }
                        }
                    }
                    else
                    {
                        result[rules.GetLength(1) - 1 - j, i] = null;
                    }
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
                    if (rules[i, j] != null)
                    {
                        result[rules.GetLength(0) - 1 - i, rules.GetLength(1) - 1 - j] = (TerrainRule[])rules[i, j].Clone();
                        for (int k = 0; k < result[rules.GetLength(0) - 1 - i, rules.GetLength(1) - 1 - j].Length; k++)
                        {
                            if (result[rules.GetLength(0) - 1 - i, rules.GetLength(1) - 1 - j][k].props.ContainsKey(PropKey.Rotation))
                            {
                                result[rules.GetLength(0) - 1 - i, rules.GetLength(1) - 1 - j][k] = result[rules.GetLength(0) - 1 - i, rules.GetLength(1) - 1 - j][k].rotate(rot);
                            }
                        }
                    }
                    else
                    {
                        result[rules.GetLength(0) - 1 - i, rules.GetLength(1) - 1 - j] = null;
                    }
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
                    if (rules[i, j] != null)
                    {
                        result[j, rules.GetLength(0) - 1 - i] = (TerrainRule[])rules[i, j].Clone();
                        for (int k = 0; k < result[j, rules.GetLength(0) - 1 - i].Length; k++)
                        {
                            if (result[j, rules.GetLength(0) - 1 - i][k].props.ContainsKey(PropKey.Rotation))
                            {
                                result[j, rules.GetLength(0) - 1 - i][k] = result[j, rules.GetLength(0) - 1 - i][k].rotate(rot);
                            }
                        }
                    }
                    else
                    {
                        result[j, rules.GetLength(0) - 1 - i] = null;
                    }
                }
            }
            return new Structure(result);
        }
        else
        {
            return this;
        }
    }

    public Structure AddAt(int x, int y, TerrainRule[] rule)
    {
        TerrainRule[,][] result = (TerrainRule[,][])this.rules.Clone();
        result[y, x] = rule;
        return new Structure(result);
    }
    public StructureRule[] RotateAll(double weight)
    {
        return new[] { new StructureRule(this, weight / 4), new StructureRule(Rotate(1), weight / 4), new StructureRule(Rotate(2), weight / 4), new StructureRule(Rotate(3), weight / 4) };
    }

    public static TerrainRule CreateStructureTile(string name, int shiftX, int shiftY, int rot=0, double weight=1)
    {
        return new TerrainRule(Terrain.TerrainType.StructureTile, true, weight, props: new Dictionary<PropKey, string>() {
            {PropKey.StructureType, name },
            {PropKey.StructureShiftX, shiftX.ToString() },
            {PropKey.StructureShiftY, shiftY.ToString() },
            {PropKey.Rotation, rot.ToString() }
        });
    }
}