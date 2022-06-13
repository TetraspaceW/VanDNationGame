using System;
using System.Collections.Generic;
class TerrainGenRule
{
    static private readonly Random _random = new Random();
    static public (int, int) AddCenter(TileModel parent, TileModel[,] tiles, TerrainRule[] rules)
    {
        var (width, height) = Shape(tiles);

        var centerX = (width % 2 == 0) ? width / 2 - _random.Next(0, 2) : width / 2;
        var centerY = (height % 2 == 0) ? height / 2 - _random.Next(0, 2) : height / 2;

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

        var (newX, newY) = possibleLocations[_random.Next(0, possibleLocations.Count)];
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

        var rand = _random.NextDouble() * weightSum;
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

    static private (int, int) Shape<T>(T[,] array) => (array.GetLength(0), array.GetLength(1));
}