using System.Collections.Generic;
using System.Linq;
class Structure
{
    public TerrainRule[,][] rules;
    public static Dictionary<string, Structure> structureDict = new Dictionary<string, Structure>();
    public string name;

    public static readonly Structure NULL = new Structure(new TerrainRule[,][] {
        { null }, }, "null");

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
                            if (result[rules.GetLength(1) - 1 - j, i][k].props != null && result[rules.GetLength(1) - 1 - j, i][k].props.ContainsKey(PropKey.Rotation))
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
                            if (result[rules.GetLength(0) - 1 - i, rules.GetLength(1) - 1 - j][k].props != null && result[rules.GetLength(0) - 1 - i, rules.GetLength(1) - 1 - j][k].props.ContainsKey(PropKey.Rotation))
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
                            if (result[j, rules.GetLength(0) - 1 - i][k].props != null && result[j, rules.GetLength(0) - 1 - i][k].props.ContainsKey(PropKey.Rotation))
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