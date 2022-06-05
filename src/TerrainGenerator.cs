using System;

class TerrainGenerator
{
    TileModel tile;
    private readonly Random _random = new Random();
    public TerrainGenerator(TileModel insideTile)
    {
        this.tile = insideTile;
    }

    public TileModel[,] GenerateTerrain()
    {
        var scale = tile.scale;
        var terrainType = tile.terrain;

        var Tiles = new TileModel[32, 32];

        switch (terrainType)
        {
            case TileModel.TerrainType.InteruniversalSpace:
                Fill(Tiles, new[] { new TerrainRule(TileModel.TerrainType.InteruniversalSpace) });
                break;
            case TileModel.TerrainType.Universe:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.Space, zoomable: true),
                    new TerrainRule(TileModel.TerrainType.Void)
                    });
                AddBorder(Tiles, new[] { new TerrainRule(TileModel.TerrainType.Energy) });
                break;
            case TileModel.TerrainType.Space:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.GalaxySupercluster, zoomable: true),
                    new TerrainRule(TileModel.TerrainType.IntersuperclusterVoid),
                });
                break;
            case TileModel.TerrainType.GalaxySupercluster:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.GalaxyCluster, zoomable: true),
                    new TerrainRule(TileModel.TerrainType.InterclusterSpace),
                });
                break;
            case TileModel.TerrainType.GalaxyCluster:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.GalaxyGroup, zoomable: true),
                    new TerrainRule(TileModel.TerrainType.IntergroupSpace),
                });
                break;
            case TileModel.TerrainType.GalaxyGroup:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.Galaxy, zoomable: true, 2),
                    new TerrainRule(TileModel.TerrainType.DwarfGalaxy, weight: 5),
                    new TerrainRule(TileModel.TerrainType.IntergroupSpace, weight: 75),
                });
                break;
            case TileModel.TerrainType.Galaxy:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.SpiralArm, zoomable: true),
                });
                AddBorder(Tiles, new[] { new TerrainRule(TileModel.TerrainType.GalacticHalo) });
                AddCenter(Tiles, new[] { new TerrainRule(TileModel.TerrainType.GalacticCore) });
                break;
            case TileModel.TerrainType.SpiralArm:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.StellarBelt, zoomable: true),
                });
                break;
            case TileModel.TerrainType.StellarBelt:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.StellarBubble, zoomable: true),
                });
                break;
            case TileModel.TerrainType.StellarBubble:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.StellarCloud, zoomable: true),
                });
                break;
            case TileModel.TerrainType.StellarCloud:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.SolarSystem, zoomable: true),
                    new TerrainRule(TileModel.TerrainType.InterstellarSpace, weight: 6)
                });
                break;
            case TileModel.TerrainType.SolarSystem:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.OortCloudBodies)
                });
                AddCenter(Tiles, new[] { new TerrainRule(TileModel.TerrainType.HillsCloud, zoomable: true) });
                break;
            case TileModel.TerrainType.HillsCloud:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.HillsCloudBodies)
                });
                AddCenter(Tiles, new[] { new TerrainRule(TileModel.TerrainType.ScatteredDisk, zoomable: true) });
                break;
            case TileModel.TerrainType.ScatteredDisk:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.ScatteredDiskBodies)
                });
                AddCenter(Tiles, new[] { new TerrainRule(TileModel.TerrainType.OuterSolarSystem, zoomable: true) });
                break;
            case TileModel.TerrainType.OuterSolarSystem:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.OuterSystemOrbit, weight: 75),
                    new TerrainRule(TileModel.TerrainType.OuterSystemBody, weight: 2)
                });
                AddBorder(Tiles, new[] { new TerrainRule(TileModel.TerrainType.KuiperBeltBodies) });
                AddCenter(Tiles, new[] { new TerrainRule(TileModel.TerrainType.InnerSolarSystem, zoomable: true) });
                break;
            case TileModel.TerrainType.InnerSolarSystem:
                Fill(Tiles, new[] {
                    new TerrainRule(TileModel.TerrainType.InnerSystemOrbit, weight: 75),
                    new TerrainRule(TileModel.TerrainType.InnerSystemBody, weight: 2)
                });
                AddBorder(Tiles, new[] { new TerrainRule(TileModel.TerrainType.AsteroidBeltBodies) });
                AddCenter(Tiles, new[] { new TerrainRule(TileModel.TerrainType.Star) });
                break;
        }

        return Tiles;
    }

    private void AddBorder(TileModel[,] tiles, TerrainRule[] rules)
    {
        var (width, height) = Shape(tiles);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || y == 0 || x == width - 1 || y == height - 1)
                {
                    tiles[x, y] = RandomTileFromRule(rules);
                }
            }
        }
    }

    private void AddCenter(TileModel[,] tiles, TerrainRule[] rules)
    {
        var (width, height) = Shape(tiles);

        var centerX = (width % 2 == 0) ? width / 2 - _random.Next(0, 2) : width / 2;
        var centerY = (height % 2 == 0) ? height / 2 - _random.Next(0, 2) : height / 2;

        tiles[centerX, centerY] = RandomTileFromRule(rules);
    }

    private void Fill(TileModel[,] tiles, TerrainRule[] rules)
    {
        var (width, height) = Shape(tiles);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tiles[x, y] = RandomTileFromRule(rules);
            }
        }
    }

    private TileModel RandomTileFromRule(TerrainRule[] rules)
    {
        int weightSum = 0;
        foreach (var rule in rules)
        {
            weightSum += rule.weight;
        }

        var rand = _random.Next(0, weightSum);
        foreach (var rule in rules)
        {
            rand -= rule.weight;
            if (rand < 0)
            {
                return new TileModel(rule.terrainType, tile, tile.scale - 1, rule.zoomable);
            }
        }

        return null;
    }

    private (int, int) Shape<T>(T[,] array) => (array.GetLength(0), array.GetLength(1));


    private class TerrainRule
    {
        public int weight;
        public TileModel.TerrainType terrainType;
        public bool zoomable;
        public TerrainRule(TileModel.TerrainType terrainType, bool zoomable = false, int weight = 1)
        {
            this.terrainType = terrainType;
            this.zoomable = zoomable;
            this.weight = weight;
        }
    }
}