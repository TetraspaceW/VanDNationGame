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
                    new TerrainRule(Terrain.TerrainType.Filament, zoomable: true),
                    new TerrainRule(Terrain.TerrainType.Void)
                    });
                AddBorder(Tiles, new[] { new TerrainRule(Terrain.TerrainType.CMB) });
                break;
            case Terrain.TerrainType.Filament:
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
                    new TerrainRule(Terrain.TerrainType.Galaxy, zoomable: true, 0.1, props: new Dictionary<PropKey, string>() {
                        {PropKey.GalaxyType, Terrain.GalaxyType.S0.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.Galaxy, zoomable: true, 0.7, props: new Dictionary<PropKey, string>() {
                        {PropKey.GalaxyType, Terrain.GalaxyType.S.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.Galaxy, zoomable: true, 0.7, props: new Dictionary<PropKey, string>() {
                        {PropKey.GalaxyType, Terrain.GalaxyType.SB.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.Galaxy, zoomable: true, 0.4, props: new Dictionary<PropKey, string>() {
                        {PropKey.GalaxyType, Terrain.GalaxyType.E.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.Galaxy, zoomable: true, 0.1, props: new Dictionary<PropKey, string>() {
                        {PropKey.GalaxyType, Terrain.GalaxyType.Irr.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.DwarfGalaxy, weight: 5),
                    new TerrainRule(Terrain.TerrainType.IntergroupSpace, weight: 75),
                });
                break;
            case Terrain.TerrainType.DwarfGalaxy:
            case Terrain.TerrainType.Galaxy:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.GalacticHalo, zoomable: false),
                });

                Terrain.GalaxyType galaxyType;
                Enum.TryParse<Terrain.GalaxyType>(terrain.props[PropKey.GalaxyType], out galaxyType);
                switch (galaxyType)
                {
                    case Terrain.GalaxyType.E:
                        AddGalaxy(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.SpiralArm, zoomable: true)
                        }, 5);
                        break;
                    case Terrain.GalaxyType.S0:
                        AddGalaxy(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.SpiralArm, zoomable: true)
                        }, 5, hasCore: 1);
                        break;
                    case Terrain.GalaxyType.S:
                        AddGalaxy(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.SpiralArm, zoomable: true)
                        }, 5, hasCore: 1, hasArms: true);
                        break;
                    case Terrain.GalaxyType.SB:
                        AddGalaxy(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.SpiralArm, zoomable: true)
                        }, 5, hasCore: 2, hasArms: true);
                        break;
                    case Terrain.GalaxyType.Irr:
                        AddGalaxy(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.SpiralArm, zoomable: true),
                            new TerrainRule(Terrain.TerrainType.GalacticHalo)
                        }, 5);
                        break;
                }
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
                    new TerrainRule(Terrain.TerrainType.OuterSystemOrbit, weight: 99 - 2.5),
                    new TerrainRule(Terrain.TerrainType.OuterSystemBody, weight: 2.5)
                });
                AddBorder(Tiles, new[] { new TerrainRule(Terrain.TerrainType.KuiperBeltBodies) });
                AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerSolarSystem, zoomable: true, props: terrain.props) });
                break;
            case Terrain.TerrainType.InnerSolarSystem:
                var innerSystemWeight = (terrain.props[PropKey.SpectralClass] == Terrain.StarSpectralClass.M.ToString()) ? 5f / 3f : 5f / 2f;
                var asteroidsWeight = innerSystemWeight / 6f;
                innerSystemWeight *= 5f / 6f;
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.InnerSystemOrbit, weight: 99 - innerSystemWeight),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 1f/5f * 3f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Rockball.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 1f/5f * 2f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Meltball.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 1f/5f * 1f/6f * 4f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Hebean.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 1f/5f * 1f/6f * 2f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Promethean.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 1f/5f * 9f/36f * 2f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Telluric.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 1f/5f * 9f/36f * 2f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Arid.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 1f/5f * 9f/36f * 2f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Tectonic.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 1f/5f * 9f/36f * 2f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Oceanic.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 1f/5f * 4f/6f * 2f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Helian.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 1f/5f * 2f/6f * 2f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Panthallasic.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, weight: innerSystemWeight * 2f/5f * 6f/6f * 2f/6f, zoomable: true, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Jovian.ToString()}
                    })
                });
                var centralStar = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Star, props: terrain.props) });
                if (_random.NextDouble() < (double)asteroidsWeight)
                {
                    AddCircle(Tiles, new[] { new TerrainRule(Terrain.TerrainType.AsteroidBeltBodies) }, centralStar, 5, false);
                }
                break;
            case Terrain.TerrainType.InnerSystemBody:
                switch (tile.scale)
                {
                    case -5:
                        Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerSystemOrbit) });
                        AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerSystemBody, zoomable: true, props: terrain.props) });
                        break;
                    case -6:
                        Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerSystemOrbit) });
                        AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.OuterLunarSystem, zoomable: true, props: terrain.props) });
                        break;
                }
                break;
            case Terrain.TerrainType.OuterLunarSystem:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.OuterLunarOrbit, weight: 98),
                    new TerrainRule(Terrain.TerrainType.LunarBody, weight: 1, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Rockball.ToString()}
                    })
                });
                AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerLunarSystem, zoomable: true, props: terrain.props) });
                break;
            case Terrain.TerrainType.InnerLunarSystem:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.InnerLunarOrbit, weight: 98),
                    new TerrainRule(Terrain.TerrainType.LunarBody, weight: 1, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Rockball.ToString()}
                    })
                });
                if (PlanetIsTerrestrial(terrain.props[PropKey.PlanetType]))
                {
                    AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.TerrestrialPlanet, zoomable: true, props: terrain.props) });
                }
                else
                {
                    AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.GasGiant, props: terrain.props) });
                }
                break;
            case Terrain.TerrainType.TerrestrialPlanet:
                var (oceanWeight, landWeight) = (0, 1);
                Terrain.PlanetType terrestrialPlanetType;
                Enum.TryParse<Terrain.PlanetType>(terrain.props[PropKey.PlanetType], out terrestrialPlanetType);
                switch (terrestrialPlanetType)
                {
                    case Terrain.PlanetType.Oceanic:
                    case Terrain.PlanetType.Panthallasic:
                        oceanWeight = 1;
                        landWeight = 0;
                        break;
                    case Terrain.PlanetType.Tectonic:
                    case Terrain.PlanetType.Promethean:
                        oceanWeight = 3;
                        landWeight = 1;
                        break;
                    case Terrain.PlanetType.Hebean:
                    case Terrain.PlanetType.Arid:
                        oceanWeight = 1;
                        landWeight = 3;
                        break;
                }
                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerLunarOrbit) });
                var planetaryCenter = AddCenter(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.InnerLunarOrbit)
                });
                AddCircle(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.Ocean, weight: oceanWeight),
                    new TerrainRule(Terrain.TerrainType.Land, weight: landWeight)
                }, planetaryCenter, 100, true);
                break;
        }

        return Tiles;
    }

    private void AddGalaxy(TileModel[,] tiles, TerrainRule[] rules, int radius, int hasCore = 0, bool hasArms = false)
    {
        var core = AddCenter(tiles, new[] { new TerrainRule(Terrain.TerrainType.GalacticCore) });
        AddCircle(tiles, rules, core, radius, true, (hasCore != 0) ? core : ((int, int)?)null);
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

    private bool PlanetIsTerrestrial(string planetType)
    {
        Terrain.PlanetType output;
        Enum.TryParse<Terrain.PlanetType>(planetType, out output);
        switch (output)
        {
            case Terrain.PlanetType.Jovian:
            case Terrain.PlanetType.Helian:
            case Terrain.PlanetType.Panthallasic:
                return false;
            case Terrain.PlanetType.Rockball:
            case Terrain.PlanetType.Meltball:
            case Terrain.PlanetType.Hebean:
            case Terrain.PlanetType.Promethean:
            case Terrain.PlanetType.Snowball:
            case Terrain.PlanetType.Telluric:
            case Terrain.PlanetType.Arid:
            case Terrain.PlanetType.Tectonic:
            case Terrain.PlanetType.Oceanic:
                return true;
        }
        return false;
    }
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