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
                SolarSystemGenerator.SpectralClass ossStarType;
                Enum.TryParse<SolarSystemGenerator.SpectralClass>(terrain.props[PropKey.SpectralClass], out ossStarType);
                var solarSystemGenerator = new SolarSystemGenerator(tile, ossStarType);
                Tiles = solarSystemGenerator.OuterSystemMap();
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
        return TerrainGenRule.AddCenter(parent: tile, tiles, rules);
    }

    private void Fill(TileModel[,] tiles, TerrainRule[] rules)
    {
        TerrainGenRule.Fill(parent: tile, tiles, rules);
    }

    private void AddCircle(TileModel[,] tiles, TerrainRule[] rules, (int, int) center, int radius, bool filled, (int, int)? mask = null)
    {
        TerrainGenRule.AddCircle(parent: tile, tiles, rules, center, radius, filled, mask);
    }

    private TileModel RandomTileFromRule(TerrainRule[] rules)
    {
        return TerrainGenRule.RandomTileFromRule(parent: tile, rules);
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

}