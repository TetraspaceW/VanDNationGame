using System;
using System.Collections.Generic;
using System.Linq;

class TerrainGenerator
{
    TileModel tile;
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
                if (scale == 11)
                {
                    AddOneRandomly(Tiles, new[] {
                        new TerrainRule(Terrain.TerrainType.Universe, zoomable: true)
                    }, new List<Terrain.TerrainType> { });
                }
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
                    new TerrainRule(Terrain.TerrainType.DwarfGalaxy, zoomable: true, weight: 5),
                    new TerrainRule(Terrain.TerrainType.IntergalacticSpace, weight: 75),
                });
                break;
            case Terrain.TerrainType.DwarfGalaxy:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.GalacticHalo, zoomable: false),
                });
                AddCenter(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.SpiralArm, zoomable: true)
                });
                break;
            case Terrain.TerrainType.Galaxy:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.GalacticHalo, zoomable: false),
                });

                switch (Enum.Parse(typeof(Terrain.GalaxyType), terrain.props[PropKey.GalaxyType]))
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
                    new TerrainRule(Terrain.TerrainType.StellarBubble, zoomable: true, weight: 0.05, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpecialStar, Terrain.StarSpectralClass.KI.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.StellarBubble, zoomable: true, weight: 0.005, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpecialStar, Terrain.StarSpectralClass.O.ToString()}
                    }),
                });
                break;
            case Terrain.TerrainType.StellarBubble:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.StellarCloud, zoomable: true),
                    new TerrainRule(Terrain.TerrainType.StellarCloud, zoomable: true, weight: 0.005, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpecialStar, Terrain.StarSpectralClass.B.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.StellarCloud, zoomable: true, weight: 0.035, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpecialStar, Terrain.StarSpectralClass.A.ToString()}
                    }),
                    new TerrainRule(Terrain.TerrainType.StellarCloud, zoomable: true, weight: 0.02, props: new Dictionary<PropKey, string>() {
                        {PropKey.SpecialStar, Terrain.StarSpectralClass.MIII.ToString()}
                    }),
                });
                if (terrain.props.ContainsKey(PropKey.SpecialStar))
                {
                    AddOneRandomly(Tiles, new[] {
                        new TerrainRule(Terrain.TerrainType.StellarCloud, zoomable: true, weight: 1, props: new Dictionary<PropKey, string>() {
                            {PropKey.SpecialStar, terrain.props[PropKey.SpecialStar]}
                        }),
                    }, new List<Terrain.TerrainType>());
                }
                break;
            case Terrain.TerrainType.StellarCloud:
                Fill(Tiles, new[] {
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
                if (terrain.props.ContainsKey(PropKey.SpecialStar))
                {
                    AddOneRandomly(Tiles, new[] {
                        new TerrainRule(Terrain.TerrainType.SolarSystem, zoomable: true, weight: 1, props: new Dictionary<PropKey, string>() {
                            {PropKey.SpectralClass, terrain.props[PropKey.SpecialStar]}
                        }),
                    }, new List<Terrain.TerrainType>());
                }

                break;
            case Terrain.TerrainType.SolarSystem:
                var solarSystemGenerator = new SolarSystemGenerator(tile, (SolarSystemGenerator.SpectralClass)Enum.Parse(typeof(SolarSystemGenerator.SpectralClass), terrain.props[PropKey.SpectralClass]));
                Tiles = solarSystemGenerator.SolarSystemMap();
                break;
            case Terrain.TerrainType.FarfarfarSystemBody:
            case Terrain.TerrainType.FarfarSystemBody:
            case Terrain.TerrainType.FarSystemBody:
            case Terrain.TerrainType.OuterSystemBody:
            case Terrain.TerrainType.InnerSystemBody:
                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.SystemOrbit) });
                switch (tile.scale)
                {
                    case -6: AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.OuterLunarSystem, zoomable: true, props: terrain.props) }); break;
                    case -7: AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerLunarSystem, zoomable: true, props: terrain.props) }); break;
                    default: AddCenter(Tiles, new[] { new TerrainRule(terrain.terrainType, zoomable: true, props: terrain.props) }); break;
                }
                break;
            case Terrain.TerrainType.OuterLunarSystem:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.LunarOrbit, weight: 98),
                    new TerrainRule(Terrain.TerrainType.LunarBody, weight: 1, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Chunk.ToString()},
                        {PropKey.PlanetIsLifeBearing, false.ToString()},
                        {PropKey.PlanetHydrosphereType, SolarSystemGenerator.Hydrosphere.None.ToString()}
                    })
                });
                AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerLunarSystem, zoomable: true, props: terrain.props) });
                break;
            case Terrain.TerrainType.InnerLunarSystem:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.LunarOrbit, weight: 98),
                    new TerrainRule(Terrain.TerrainType.LunarBody, weight: 1, props: new Dictionary<PropKey, string>() {
                        {PropKey.PlanetType, Terrain.PlanetType.Chunk.ToString()},
                        {PropKey.PlanetIsLifeBearing, false.ToString()},
                        {PropKey.PlanetHydrosphereType, SolarSystemGenerator.Hydrosphere.None.ToString()}
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
                int oceanWeight = int.Parse(terrain.props[PropKey.PlanetHydrosphereCoverage]);

                SolarSystemGenerator.Hydrosphere hydrosphere = (SolarSystemGenerator.Hydrosphere)Enum.Parse(typeof(SolarSystemGenerator.Hydrosphere), terrain.props[PropKey.PlanetHydrosphereType]);

                bool planetIsLifeBearing = bool.Parse(terrain.props[PropKey.PlanetIsLifeBearing]);

                double planetaryRadius = double.Parse(terrain.props[PropKey.PlanetRadius]);
                var planetaryTileSize = (int)Math.Round(planetaryRadius / 1000);

                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.LunarOrbit) });
                var planetaryCenter = TerrainGenRule.ArbitraryCenter(Tiles);
                AddCircle(Tiles, new[] {
                    new TerrainRule(
                        terrainType: hydrosphere == SolarSystemGenerator.Hydrosphere.Liquid ? Terrain.TerrainType.Ocean : Terrain.TerrainType.IceSheet,
                        zoomable: hydrosphere == SolarSystemGenerator.Hydrosphere.Liquid,
                        weight: oceanWeight,
                        props: new Dictionary<PropKey, string>() {
                            { PropKey.PlanetIsLifeBearing, planetIsLifeBearing.ToString() }
                        }),
                    new TerrainRule(planetIsLifeBearing ? Terrain.TerrainType.VerdantTerrain : Terrain.TerrainType.BarrenTerrain, true, weight: 100 - oceanWeight)
                }, planetaryCenter, planetaryTileSize < 5 ? planetaryTileSize : 10, true);
                break;

            case Terrain.TerrainType.BarrenTerrain:
                switch (tile.scale)
                {
                    case -25:
                        Tiles = StructureTile(Tiles, new[] { new StructureRule(Structure.SILICA, 1) }
                            , new[] {
                            new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                        });
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.BarrenTerrain, true)
                        });
                        break;
                }
                break;
            case Terrain.TerrainType.VerdantTerrain:
                TerrainRule[] landLife = new TerrainRule[] { };

                switch (tile.scale)
                {
                    case -14:
                        landLife = new[] {
                                new TerrainRule(Terrain.TerrainType.Dinosaur, false)
                            };
                        break;
                    case -15:
                        landLife = new[] {
                                new TerrainRule(Terrain.TerrainType.Mammal, false),
                            };
                        break;
                    case -16:
                        landLife = new[] {
                                new TerrainRule(Terrain.TerrainType.Bird, false, 0.3),
                                new TerrainRule(Terrain.TerrainType.Amphibian, false, 0.15),
                                new TerrainRule(Terrain.TerrainType.Reptile, false, 0.3),
                                new TerrainRule(Terrain.TerrainType.Trichordate, false, 0.3)
                            };
                        break;
                    case -17:
                        landLife = new[] {
                                new TerrainRule(Terrain.TerrainType.Insect, false)
                            };
                        break;
                }

                switch (tile.scale)
                {
                    case -25:
                        Tiles = StructureTile(Tiles, new[] { new StructureRule(Structure.SILICA, 1) }
                            , new[] {
                            new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                        });
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.VerdantTerrain, true, 99)
                        }.Concat(landLife).ToArray());
                        break;
                }
                break;
            case Terrain.TerrainType.Ocean:
                TerrainRule[] oceanLife = new TerrainRule[] { };

                if (bool.Parse(terrain.props[PropKey.PlanetIsLifeBearing]))
                {
                    switch (tile.scale)
                    {
                        case -14:
                            oceanLife = new[] {
                                new TerrainRule(Terrain.TerrainType.Cetacean, false)
                            };
                            break;
                        case -16:
                            oceanLife = new[] {
                                new TerrainRule(Terrain.TerrainType.Amphibian, false, 0.1),
                                new TerrainRule(Terrain.TerrainType.Arthropod, false, 0.2),
                                new TerrainRule(Terrain.TerrainType.Fish, false, 0.2),
                                new TerrainRule(Terrain.TerrainType.Radiate, false, 0.2),
                                new TerrainRule(Terrain.TerrainType.Mollusk, false, 0.2),
                            };
                            break;
                        case -19:
                            oceanLife = new[] {
                                new TerrainRule(Terrain.TerrainType.Eukaryote, false)
                            };
                            break;
                        case -21:
                            oceanLife = new[] {
                                new TerrainRule(Terrain.TerrainType.Prokaryote, false)
                            };
                            break;
                    }
                }

                switch (tile.scale)
                {
                    case -25:
                        Tiles = StructureFill(Tiles, Structure.WATER.RotateAll(1).Concat(Structure.HYDROXIDE.RotateAll(0.0000001)).Concat(Structure.HYDRONIUM.RotateAll(0.0000001)).ToArray()
                            , 0.5, new[] {
                            new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                        });
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.Ocean, true, 99, props: tile.terrain.props)
                        }.Concat(oceanLife).ToArray());
                        break;
                }
                break;
            case Terrain.TerrainType.Atom:
                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.ElectronCloud, false) });
                AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Nucleus, true, props: terrain.props) });
                break;
            case Terrain.TerrainType.Nucleus:
                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.ElectronCloud, false) });
                switch (tile.scale)
                {
                    case -30:
                        var nucleusCenter = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.ElectronCloud, false) });

                        double massNumber = AtomGenerator.GetMassNumber(terrain);

                        AddCircle(Tiles, new[] {
                                new TerrainRule(Terrain.TerrainType.Proton, true),
                                new TerrainRule(Terrain.TerrainType.Neutron, true)
                            },
                            nucleusCenter,
                            (int)(Math.Pow(massNumber, 1.0 / 3.0) * 0.6),
                            true
                        );
                        break;

                    default: AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Nucleus, true, props: terrain.props) }); break;
                }
                break;
            case Terrain.TerrainType.Proton:
                Tiles = new NucleonGenerator(tile, Terrain.QuarkFlavour.Up, Terrain.QuarkFlavour.Up, Terrain.QuarkFlavour.Down).GenerateNucleon();
                break;
            case Terrain.TerrainType.Neutron:
                Tiles = new NucleonGenerator(tile, Terrain.QuarkFlavour.Up, Terrain.QuarkFlavour.Down, Terrain.QuarkFlavour.Down).GenerateNucleon();
                break;
            case Terrain.TerrainType.ValenceQuark:
                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.GluonSea) });
                switch (tile.scale)
                {
                    case -34:
                        AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Quark, false, props: terrain.props) }); break;
                    default:
                        AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.ValenceQuark, true, props: terrain.props) }); break;
                }
                break;
        }

        return Tiles;
    }


    private void AddGalaxy(TileModel[,] tiles, TerrainRule[] rules, int radius, int hasCore = 0, bool hasArms = false)
    {
        var core = AddCenter(tiles, new[] { new TerrainRule(Terrain.TerrainType.GalacticCore) });
        if (hasArms)
        {
            AddArms(tiles, rules, core, radius, RND.NextDouble() > 0.5, (hasCore == 2) ? 2 : RND.Next(4, 7), hasCore == 2, (hasCore != 0) ? core : ((int, int)?)null);
        }
        else
        {
            AddCircle(tiles, rules, core, radius, true, (hasCore != 0) ? core : ((int, int)?)null);
        }
    }
    private void AddArms(TileModel[,] tiles, TerrainRule[] rules, (int, int) center, int radius, bool counterclockwise, int numArms, bool barred, (int, int)? mask = null)
    {
        var (width, height) = Shape(tiles);
        var (centerX, centerY) = center;
        var turn = RND.NextDouble() * Math.PI * 2;
        for (double j = 0; j < numArms; j++)
        {
            for (double i = 0; i < Math.PI * 2; i += Math.PI / 100)
            {
                double r = Math.Pow(2, i);
                if (barred)
                {
                    r += 2;
                }
                double ang = i + 2 * Math.PI / numArms * j + turn;
                var x = (int)Math.Round(centerX + (counterclockwise ? -r * Math.Cos(ang) : r * Math.Cos(ang)));
                var y = (int)Math.Round(centerY + r * Math.Sin(ang));
                if (r > radius || x >= width || x < 0 || y >= height || y < 0)
                {
                    break;
                }
                if (!mask.HasValue || mask.Value != (x, y))
                {
                    tiles[x, y] = RandomTileFromRule(rules);
                }
            }
        }
        if (barred)
        {
            for (double j = -1; j <= 1; j++)
            {
                double ang = turn + Math.PI / 2;
                var x1 = (counterclockwise ? -j * Math.Cos(ang) : j * Math.Cos(ang));
                var y1 = j * Math.Sin(ang);
                for (double i = -2; i < 2; i += 0.01)
                {
                    double r = i;
                    double ang2 = turn;
                    var x2 = centerX + (counterclockwise ? -r * Math.Cos(ang2) : r * Math.Cos(ang2));
                    var y2 = centerY + r * Math.Sin(ang2);
                    var x = (int)Math.Round(x1 + x2);
                    var y = (int)Math.Round(y1 + y2);
                    if (r > radius || x >= width || x < 0 || y >= height || y < 0)
                    {
                        break;
                    }
                    if (!mask.HasValue || mask.Value != (x, y))
                    {
                        tiles[x, y] = RandomTileFromRule(rules);
                    }
                }
            }
        }
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
        return TerrainGenRule.AddCenter(parent: tile, tiles, rules);
    }

    private void Fill(TileModel[,] tiles, TerrainRule[] rules)
    {
        TerrainGenRule.Fill(parent: tile, tiles, rules);
    }
    private void AddOneRandomly(TileModel[,] tiles, TerrainRule[] rules, List<Terrain.TerrainType> mask)
    {
        TerrainGenRule.AddOneRandomly(parent: tile, tiles, rules, mask);
    }

    private TileModel[,] StructureFill(TileModel[,] tiles, StructureRule[] rules, double chanceNone, TerrainRule[] baseFill)
    {
        return TerrainGenRule.StructureFill(parent: tile, tiles, rules, chanceNone, baseFill);
    }
    private TileModel[,] StructureTile(TileModel[,] tiles, StructureRule[] rules, TerrainRule[] baseFill)
    {
        return TerrainGenRule.StructureTile(parent: tile, tiles, rules, baseFill);
    }

    private void AddCircle(TileModel[,] tiles, TerrainRule[] rules, (int, int) center, int radius, bool filled, (int, int)? mask = null)
    {
        TerrainGenRule.AddCircle(parent: tile, tiles, rules, center, radius, filled, mask);
    }

    private TileModel RandomTileFromRule(TerrainRule[] rules)
    {
        return TerrainGenRule.RandomTileFromRule(parent: tile, rules);
    }

    public static (int, int) Shape<T>(T[,] array) => (array.GetLength(0), array.GetLength(1));

    private bool PlanetIsTerrestrial(string planetType)
    {
        switch (Enum.Parse(typeof(Terrain.PlanetType), planetType))
        {
            default: case Terrain.PlanetType.Jovian: return false;
            case Terrain.PlanetType.Terrestrial: case Terrain.PlanetType.Chunk: return true;
        }
    }
}