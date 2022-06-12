using System;
using System.Collections.Generic;

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
        var terrain = tile.terrain;

        var Tiles = new TileModel[10, 10];

        switch (terrain.terrainType)
        {
            case Terrain.TerrainType.InteruniversalSpace:
                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InteruniversalSpace) });
                break;
            case Terrain.TerrainType.Universe:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.Space, zoomable: true),
                    new TerrainRule(Terrain.TerrainType.Void)
                    });
                AddBorder(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Energy) });
                break;
            case Terrain.TerrainType.Space:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.GalaxySupercluster, zoomable: true),
                    new TerrainRule(Terrain.TerrainType.IntersuperclusterVoid),
                });
                break;
            case Terrain.TerrainType.GalaxySupercluster:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.GalaxyCluster, zoomable: true),
                    new TerrainRule(Terrain.TerrainType.InterclusterSpace),
                });
                break;
            case Terrain.TerrainType.GalaxyCluster:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.GalaxyGroup, zoomable: true),
                    new TerrainRule(Terrain.TerrainType.IntergroupSpace),
                });
                break;
            case Terrain.TerrainType.GalaxyGroup:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.Galaxy, zoomable: true, 2),
                    new TerrainRule(Terrain.TerrainType.DwarfGalaxy, weight: 5),
                    new TerrainRule(Terrain.TerrainType.IntergroupSpace, weight: 75),
                });
                break;
            case Terrain.TerrainType.Galaxy:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.GalacticHalo, zoomable: false),
                });
                var core = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.GalacticCore) });
                AddCircle(Tiles, new[] { new TerrainRule(Terrain.TerrainType.SpiralArm, zoomable: true) }, core, 5, true, core);
                break;
            case Terrain.TerrainType.SpiralArm:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.StellarBelt, zoomable: true),
                });
                break;
            case Terrain.TerrainType.StellarBelt:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.StellarBubble, zoomable: true),
                });
                break;
            case Terrain.TerrainType.StellarBubble:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.StellarCloud, zoomable: true),
                });
                break;
            case Terrain.TerrainType.StellarCloud:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.SolarSystem, zoomable: true, weight: 0.00001, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpectralClass, Terrain.StarSpectralClass.O.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.SolarSystem, zoomable: true, weight: 0.1, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpectralClass, Terrain.StarSpectralClass.B.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.SolarSystem, zoomable: true, weight: 0.7, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpectralClass, Terrain.StarSpectralClass.A.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.SolarSystem, zoomable: true, weight: 2, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpectralClass, Terrain.StarSpectralClass.F.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.SolarSystem, zoomable: true, weight: 3.5, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpectralClass, Terrain.StarSpectralClass.G.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.SolarSystem, zoomable: true, weight: 8, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpectralClass, Terrain.StarSpectralClass.K.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.SolarSystem, zoomable: true, weight: 80, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpectralClass, Terrain.StarSpectralClass.M.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.SolarSystem, weight: 5, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpectralClass, Terrain.StarSpectralClass.D.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InterstellarSpace, weight: 1895)
                });
                break;
            case Terrain.TerrainType.SolarSystem:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.InterstellarSpace)
                });
                var system = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.HillsCloud, zoomable: true, props: terrain.props) });
                AddCircle(Tiles, new[] { new TerrainRule(Terrain.TerrainType.OortCloudBodies) }, system, 5, true, system);
                break;
            case Terrain.TerrainType.HillsCloud:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.HillsCloudBodies)
                });
                AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.ScatteredDisk, zoomable: true, props: terrain.props) });
                break;
            case Terrain.TerrainType.ScatteredDisk:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.ScatteredDiskBodies)
                });
                AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.OuterSolarSystem, zoomable: true, props: terrain.props) });
                break;
            case Terrain.TerrainType.OuterSolarSystem:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.OuterSystemOrbit, weight: 75),
                    new TerrainRule(Terrain.TerrainType.OuterSystemBody, weight: 2)
                });
                AddBorder(Tiles, new[] { new TerrainRule(Terrain.TerrainType.KuiperBeltBodies) });
                AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerSolarSystem, zoomable: true, props: terrain.props) });
                break;
            case Terrain.TerrainType.InnerSolarSystem:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.InnerSystemOrbit, weight: 75),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: 2)
                });
                var centralStar = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Star, props: terrain.props) });
                AddCircle(Tiles, new[] { new TerrainRule(Terrain.TerrainType.AsteroidBeltBodies) }, centralStar, 5, false);
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

    private (int, int) AddCenter(TileModel[,] tiles, TerrainRule[] rules)
    {
        var (width, height) = Shape(tiles);

        var centerX = (width % 2 == 0) ? width / 2 - _random.Next(0, 2) : width / 2;
        var centerY = (height % 2 == 0) ? height / 2 - _random.Next(0, 2) : height / 2;

        tiles[centerX, centerY] = RandomTileFromRule(rules);
        return (centerX, centerY);
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

    private void AddCircle(TileModel[,] tiles, TerrainRule[] rules, (int, int) center, int radius, bool filled, (int, int)? mask = null)
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
                        tiles[x, y] = RandomTileFromRule(rules);
                    }
                }
            }
        }
    }

    private TileModel RandomTileFromRule(TerrainRule[] rules)
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
                return new TileModel(new Terrain(rule.terrainType, rule.props), tile, tile.scale - 1, rule.zoomable);
            }
        }

        return null;
    }

    private (int, int) Shape<T>(T[,] array) => (array.GetLength(0), array.GetLength(1));


    private class TerrainRule
    {
        public double weight;
        public Terrain.TerrainType terrainType;
        public bool zoomable;
        public Dictionary<PropKey, string> props;
        public TerrainRule(Terrain.TerrainType terrainType, bool zoomable = false, double weight = 1, Dictionary<PropKey, string> props = null)
        {
            this.terrainType = terrainType;
            this.zoomable = zoomable;
            this.weight = weight;
            this.props = props;
        }
    }
}