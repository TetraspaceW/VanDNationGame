using System;
using System.Collections.Generic;
class TerrainGenRule
{
    static public (int, int) ArbitraryCenter(TileModel[,] tiles)
    {
        var (width, height) = Shape(tiles);

        var centerX = (width % 2 == 0) ? width / 2 - RND.Next(0, 2) : width / 2;
        var centerY = (height % 2 == 0) ? height / 2 - RND.Next(0, 2) : height / 2;

        return (centerX, centerY);
    }
    static public (int, int) AddCenter(TileModel parent, TileModel[,] tiles, TerrainRule[] rules)
    {
        var (centerX, centerY) = TerrainGenRule.ArbitraryCenter(tiles);
        tiles[centerX, centerY] = RandomTileFromRule(parent, rules);
        return (centerX, centerY);
    }

    static public void AddAtDistance(TileModel parent, TileModel[,] tiles, TerrainRule[] rules, (int, int) center, int radius, List<Terrain.TerrainType> mask)
    {
        var (width, height) = Shape(tiles);
        var (centerX, centerY) = center;
        List<(int, int)> possibleLocations = new List<(int, int)>();

        for (int x = Math.Max(centerX - radius - 1, 0); x < Math.Min(centerX + radius + 1, width); x++)
        {
            for (int y = Math.Max(centerY - radius - 1, 0); y < Math.Min(centerY + radius + 1, height); y++)
            {
                var distance = Math.Pow(((float)x - centerX), 2) + Math.Pow(((float)y - centerY), 2);
                if (distance <= Math.Pow(radius, 2) && distance > Math.Pow(radius - 1, 2))
                {
                    if (tiles[x, y] == null || !mask.Contains(tiles[x, y].terrain.terrainType))
                    {
                        possibleLocations.Add((x, y));
                    }
                }
            }
        }

        if (possibleLocations.Count > 0)
        {
            var (newX, newY) = possibleLocations[RND.Next(0, possibleLocations.Count)];
            tiles[newX, newY] = RandomTileFromRule(parent, rules);
        }
        else { AddAtDistance(parent, tiles, rules, center, radius + 1, mask); }
    }

    static public void AddOneRandomly(TileModel parent, TileModel[,] tiles, TerrainRule[] rules, List<Terrain.TerrainType> mask)
    {
        var (width, height) = Shape(tiles);
        List<(int, int)> possibleLocations = new List<(int, int)>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y] == null || !mask.Contains(tiles[x, y].terrain.terrainType))
                {
                    possibleLocations.Add((x, y));
                }
            }
        }

        var (newX, newY) = possibleLocations[RND.Next(0, possibleLocations.Count)];
        tiles[newX, newY] = RandomTileFromRule(parent, rules);
    }

    static public void Fill(TileModel parent, TileModel[,] tiles, TerrainRule[] rules)
    {
        var (width, height) = Shape(tiles);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = RandomTileFromRule(parent, rules);
            }
        }
    }

    static public void AddCircle(TileModel parent, TileModel[,] tiles, TerrainRule[] rules, (int, int) center, int radius, bool filled, (int, int)? mask = null)
    {
        var (width, height) = Shape(tiles);
        var (centerX, centerY) = center;

        for (int x = Math.Max(centerX - radius - 1, 0); x < Math.Min(centerX + radius + 1, width); x++)
        {
            for (int y = Math.Max(centerY - radius - 1, 0); y < Math.Min(centerY + radius + 1, height); y++)
            {
                var distance = Math.Pow(((float)x - centerX), 2) + Math.Pow(((float)y - centerY), 2);
                if (distance <= Math.Pow(radius, 2) && (filled || (distance > Math.Pow(radius - 1, 2))))
                {
                    if (!mask.HasValue || mask.Value != (x, y))
                    {
                        tiles[x, y] = RandomTileFromRule(parent, rules);
                    }
                }
            }
        }
    }

    static public TileModel RandomTileFromRule(TileModel parent, TerrainRule[] rules)
    {
        double weightSum = 0;
        foreach (var rule in rules)
        {
            weightSum += rule.weight;
        }

        var rand = RND.NextDouble() * weightSum;
        foreach (var rule in rules)
        {
            rand -= rule.weight;
            if (rand < 0)
            {
                return new TileModel(new Terrain(rule.terrainType, rule.props), parent, parent.scale - 1, rule.zoomable);
            }
        }

        return null;
    }
    static public Structure RandomStructureFromRule(TileModel parent, StructureRule[] rules, double chanceNone)
    {
        double weightSum = 0;
        foreach (var rule in rules)
        {
            weightSum += rule.weight;
        }

        var rand = RND.NextDouble() * weightSum / (1.0 - chanceNone);
        foreach (var rule in rules)
        {
            rand -= rule.weight;
            if (rand < 0)
            {
                return rule.structure;
            }
        }

        return null;
    }

    static public TileModel[,] StructureFill(TileModel parent, TileModel[,] tiles, StructureRule[] rules, double chanceNone, TerrainRule[] baseFill)
    {
        var (width, height) = Shape(tiles);
        tiles = new TileModel[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Structure thing = RandomStructureFromRule(parent, rules, chanceNone);
                if (thing != null)
                {
                    tiles = thing.AttemptPlace(parent, tiles, x - RND.Next(-thing.rules.GetLength(0), 0), y - RND.Next(-thing.rules.GetLength(1), 0));
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y] == null)
                {
                    tiles[x, y] = RandomTileFromRule(parent, baseFill);
                }
            }
        }
        return tiles;
    }


    static public TileModel[,] StructureTile(TileModel parent, TileModel[,] tiles, StructureRule[] rules, TerrainRule[] baseFill)
    {
        var (width, height) = Shape(tiles);
        tiles = new TileModel[width, height];

        int tileLength = rules[0].structure.rules.GetLength(0);
        int tileHeight = rules[0].structure.rules.GetLength(1);
        int tileStartX = -RND.Next(0, tileLength);
        int tileStartY = -RND.Next(0, tileHeight);

        for (int y = tileStartX; y < width + tileLength; y += tileLength)
        {
            for (int x = tileStartY; x < height + tileHeight; x += tileHeight)
            {
                Structure thing = RandomStructureFromRule(parent, rules, 0);
                if (thing != null)
                {
                    if (thing.rules.GetLength(0) != tileLength || thing.rules.GetLength(1) != tileHeight)
                    {
                        throw new ArgumentException("Rules must be the same dimensions");
                    }
                    else
                    {
                        tiles = thing.AttemptPlace(parent, tiles, x, y);
                    }
                }
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y] == null)
                {
                    tiles[x, y] = RandomTileFromRule(parent, baseFill);
                }
            }
        }
        return tiles;
    }
    static public TileModel[,] PlaceStructure(TileModel parent, TileModel[,] tiles, StructureRule[] rules, int initX, int initY, TerrainRule[] baseFill, int rotate)
    {
        var (width, height) = Shape(tiles);
        tiles = new TileModel[width, height];
        Structure thing = RandomStructureFromRule(parent, rules, 0);
        if (thing != null)
        {
            tiles = thing.AttemptPlace(parent, tiles, initX, initY, rotate);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (tiles[x, y] == null)
                {
                    tiles[x, y] = RandomTileFromRule(parent, baseFill);
                }
            }
        }
        return tiles;
    }
    static private (int, int) Shape<T>(T[,] array) => (array.GetLength(0), array.GetLength(1));
}