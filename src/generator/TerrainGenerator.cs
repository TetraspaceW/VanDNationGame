using System;
using System.Collections.Generic;
using System.Linq;

class TerrainGenerator
{
    readonly TileModel tile;
    public TerrainGenerator(TileModel insideTile)
    {
        tile = insideTile;
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
                _ = AddCenter(Tiles, new[] {
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
                _ = tile.scale switch
                {
                    -6 => AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.OuterLunarSystem, zoomable: true, props: terrain.props) }),
                    -7 => AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerLunarSystem, zoomable: true, props: terrain.props) }),
                    _ => AddCenter(Tiles, new[] { new TerrainRule(terrain.terrainType, zoomable: true, props: terrain.props) }),
                };
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
                _ = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerLunarSystem, zoomable: true, props: terrain.props) });
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
                    _ = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.TerrestrialPlanet, zoomable: true, props: terrain.props) });
                }
                else
                {
                    _ = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.GasGiant, true, props: terrain.props) });
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
                        zoomable: true,
                        weight: oceanWeight,
                        props: new Dictionary<PropKey, string>() {
                            { PropKey.PlanetIsLifeBearing, planetIsLifeBearing.ToString() }
                        }),
                    new TerrainRule(planetIsLifeBearing ? Terrain.TerrainType.VerdantTerrain : Terrain.TerrainType.BarrenTerrain, true, weight: 100 - oceanWeight,
                        props: new Dictionary<PropKey, string>() {
                            { PropKey.PlanetIsLifeBearing, planetIsLifeBearing.ToString() },
                            { PropKey.PlanetHydrosphereType, hydrosphere.ToString() }
                        })
                }, planetaryCenter, planetaryTileSize < 5 ? planetaryTileSize : 10, true);
                break;
            case Terrain.TerrainType.GasGiant:
                var gasGiantCenter = TerrainGenRule.ArbitraryCenter(Tiles);

                double gasGiantRadius = double.Parse(terrain.props[PropKey.PlanetRadius]);
                var gasGiantTileSize = (int)Math.Round(gasGiantRadius / 1000);

                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.LunarOrbit) });
                AddCircle(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.GasGiantTerrain, true)
                }, gasGiantCenter, gasGiantTileSize < 5 ? gasGiantTileSize : 10, true);
                break;
            case Terrain.TerrainType.StellarTerrain:
                switch (tile.scale)
                {
                    case -25:
                        Tiles = StarFill(Tiles);
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(terrain.terrainType, true)
                        });
                        break;
                }
                break;
            case Terrain.TerrainType.GasGiantTerrain:
                switch (tile.scale)
                {
                    case -25:
                        Tiles = NonMetalFill(Tiles);
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(terrain.terrainType, true)
                        });
                        break;
                }
                break;
            case Terrain.TerrainType.BarrenTerrain:
                switch (tile.scale)
                {
                    case -18:
                        SolarSystemGenerator.Hydrosphere barrenTerrainParentHydrosphereType = (SolarSystemGenerator.Hydrosphere)Enum.Parse(typeof(SolarSystemGenerator.Hydrosphere), terrain.props[PropKey.PlanetHydrosphereType]);
                        if (barrenTerrainParentHydrosphereType == SolarSystemGenerator.Hydrosphere.Crustal)
                        {
                            Fill(Tiles, new[] {
                                new TerrainRule(Terrain.TerrainType.Mineral, true, 1, props: new Dictionary<PropKey, string> {
                                    { PropKey.Mineral, Terrain.Mineral.Ice.ToString() }
                                }),
                                new TerrainRule(Terrain.TerrainType.Mineral, true, 1, props: new Dictionary<PropKey, string> {
                                    { PropKey.Mineral, Terrain.Mineral.Silica.ToString() }
                                }),
                            });
                        }
                        else
                        {
                            Fill(Tiles, new[] {
                                new TerrainRule(Terrain.TerrainType.Mineral, true, 80, props: new Dictionary<PropKey, string> {
                                    { PropKey.Mineral, Terrain.Mineral.Anorthite.ToString() }
                                }),
                                new TerrainRule(Terrain.TerrainType.Mineral, true, 10/3, props: new Dictionary<PropKey, string> {
                                    { PropKey.Mineral, Terrain.Mineral.Wallastonite.ToString() }
                                }),
                                new TerrainRule(Terrain.TerrainType.Mineral, true, 10/3, props: new Dictionary<PropKey, string> {
                                    { PropKey.Mineral, Terrain.Mineral.Enstatite.ToString() }
                                }),
                                new TerrainRule(Terrain.TerrainType.Mineral, true, 10/3, props: new Dictionary<PropKey, string> {
                                    { PropKey.Mineral, Terrain.Mineral.Ferrosilite.ToString() }
                                }),
                                new TerrainRule(Terrain.TerrainType.Mineral, true, 3/2, props: new Dictionary<PropKey, string> {
                                    { PropKey.Mineral, Terrain.Mineral.Forsterite.ToString() }
                                }),
                                new TerrainRule(Terrain.TerrainType.Mineral, true, 3/2, props: new Dictionary<PropKey, string> {
                                    { PropKey.Mineral, Terrain.Mineral.Fayalite.ToString() }
                                }),
                                new TerrainRule(Terrain.TerrainType.Mineral, true, 2.5, props: new Dictionary<PropKey, string> {
                                    { PropKey.Mineral, Terrain.Mineral.Ilmenite.ToString() }
                                })
                            });
                        }
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.BarrenTerrain, true, props: terrain.props)
                        });
                        break;
                }
                break;
            case Terrain.TerrainType.VerdantTerrain:
                switch (tile.scale)
                {
                    case -18:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.Mineral, true, props: new Dictionary<PropKey, string> {
                                { PropKey.Mineral, Terrain.Mineral.Silica.ToString() }
                            })
                        });
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.VerdantTerrain, true, 99, props: terrain.props)
                        }.Concat(GetLifeForTerrain(terrain)).ToArray());
                        break;
                }
                break;
            case Terrain.TerrainType.IceSheet:
                switch (tile.scale)
                {
                    case -18:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.Mineral, true, props: new Dictionary<PropKey, string> {
                                { PropKey.Mineral, Terrain.Mineral.Ice.ToString() }
                            })
                        });
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.IceSheet, true, props: terrain.props)
                        });
                        break;
                }
                break;
            case Terrain.TerrainType.Ocean:
                switch (tile.scale)
                {
                    case -25:
                        Tiles = WaterFill(Tiles);
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.Ocean, true, 99, props: tile.terrain.props)
                        }.Concat(GetLifeForTerrain(terrain)).ToArray());
                        break;
                }
                break;
            case Terrain.TerrainType.IntermolecularFluid:
                switch (tile.scale)
                {
                    case -25:
                        Tiles = WaterFill(Tiles);
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.IntermolecularFluid, true, props: tile.terrain.props)
                        });
                        break;
                }
                break;
            case Terrain.TerrainType.Mineral:
                Terrain.Mineral mineralType = (Terrain.Mineral)Enum.Parse(typeof(Terrain.Mineral), terrain.props[PropKey.Mineral]);
                switch (tile.scale)
                {
                    case -25:
                        switch (mineralType)
                        {
                            case Terrain.Mineral.Ice:
                                Tiles = StructureTile(Tiles, new[] { new StructureRule(Chem.ICE, 1) }
                                    , new[] {
                                    new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                                });
                                break;
                            case Terrain.Mineral.Silica:
                                Tiles = StructureTile(Tiles, new[] { new StructureRule(Chem.SILICA, 1) }
                                    , new[] {
                                    new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                                });
                                break;
                            case Terrain.Mineral.Anorthite:
                                Tiles = StructureTile(Tiles, new[] { new StructureRule(Chem.ANORTHITE, 1) }
                                    , new[] {
                                    new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                                });
                                break;
                            case Terrain.Mineral.Wallastonite:
                                Tiles = StructureTile(Tiles, new[] { new StructureRule(Chem.WOLLASTONITE, 1) }
                                    , new[] {
                                    new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                                });
                                break;
                            case Terrain.Mineral.Enstatite:
                                Tiles = StructureTile(Tiles, new[] { new StructureRule(Chem.ENSTATITE, 1) }
                                    , new[] {
                                    new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                                });
                                break;
                            case Terrain.Mineral.Ferrosilite:
                                Tiles = StructureTile(Tiles, new[] { new StructureRule(Chem.FERROSILITE, 1) }
                                    , new[] {
                                    new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                                });
                                break;
                            case Terrain.Mineral.Forsterite:
                                Tiles = StructureTile(Tiles, new[] { new StructureRule(Chem.FORSTERITE, 1) }
                                    , new[] {
                                    new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                                });
                                break;
                            case Terrain.Mineral.Fayalite:
                                Tiles = StructureTile(Tiles, new[] { new StructureRule(Chem.FAYALITE, 1) }
                                    , new[] {
                                    new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                                });
                                break;
                            case Terrain.Mineral.Ilmenite:
                                Tiles = StructureTile(Tiles, new[] { new StructureRule(Chem.ILMENITE, 1) }
                                    , new[] {
                                    new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                                });
                                break;
                            default:
                                Fill(Tiles, new[] {
                                    new TerrainRule(Terrain.TerrainType.Mineral, true, props: tile.terrain.props)
                                });
                                break;
                        }
                        break;
                    default:
                        Fill(Tiles, new[] {
                            new TerrainRule(Terrain.TerrainType.Mineral, true, props: tile.terrain.props)
                        });
                        break;
                }
                break;
            case Terrain.TerrainType.Dinosaur:
            case Terrain.TerrainType.Cetacean:
            case Terrain.TerrainType.Mammal:
            case Terrain.TerrainType.Carnifern:
            case Terrain.TerrainType.Bird:
            case Terrain.TerrainType.Amphibian:
            case Terrain.TerrainType.Arthropod:
            case Terrain.TerrainType.Fish:
            case Terrain.TerrainType.Reptile:
            case Terrain.TerrainType.Radiate:
            case Terrain.TerrainType.Mollusk:
            case Terrain.TerrainType.Trichordate:
            case Terrain.TerrainType.Insect:
                var lifeformSurroundingTerrain = new Terrain(
                    (Terrain.TerrainType)Enum.Parse(typeof(Terrain.TerrainType), terrain.props[PropKey.Habitat]),
                    tile.terrain.props
                );
                var lifeformSurroundingsTerrainRule = new[] {
                    new TerrainRule (lifeformSurroundingTerrain.terrainType, true, 99, lifeformSurroundingTerrain.props)
                }.Concat(GetLifeForTerrain(lifeformSurroundingTerrain)).ToArray();
                Fill(Tiles, lifeformSurroundingsTerrainRule);
                var organismCenter = TerrainGenRule.ArbitraryCenter(Tiles);
                AddCircle(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.Skin, true)
                }, organismCenter, 4, true);
                break;
            case Terrain.TerrainType.Skin:
                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Tissue, true) });
                break;
            case Terrain.TerrainType.Tissue:
                switch (tile.scale)
                {
                    case -20: Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Cell, true) }); break;
                    default: Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Tissue, true) }); break;
                }
                break;
            case Terrain.TerrainType.Cell:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.Cytoplasm, false),
                    new TerrainRule(Terrain.TerrainType.Mitochrondrion, false, 0.2),
                    new TerrainRule(Terrain.TerrainType.Vesicle, false, 0.1),
                    new TerrainRule(Terrain.TerrainType.Vacuole, false, 0.1),
                    new TerrainRule(Terrain.TerrainType.Lysosome, false, 0.1),
                    new TerrainRule(Terrain.TerrainType.Centrosome, false, 0.1)
                 });
                var center = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Nucleolus, false) });
                AddCircle(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.Nucleoplasm, true),
                    new TerrainRule(Terrain.TerrainType.EuchromatinDomain, true, 0.5)
                }, center, 3, true, center);
                AddCircle(Tiles, new[] { new TerrainRule(Terrain.TerrainType.HeterochromatinDomain, true) }, center, 3, false);
                break;
            case Terrain.TerrainType.HeterochromatinDomain:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.Nucleoplasm, true),
                    new TerrainRule(Terrain.TerrainType.Heterochromatin, true, 0.5),
                });
                break;
            case Terrain.TerrainType.EuchromatinDomain:
                Fill(Tiles, new[] {
                    new TerrainRule(Terrain.TerrainType.Nucleoplasm, true),
                    new TerrainRule(Terrain.TerrainType.Euchromatin, true, 0.5),
                });
                break;
            case Terrain.TerrainType.Euchromatin:
                Tiles = new ChromatinGenerator(tile, Tiles).GenerateEuchromatin();
                break;
            case Terrain.TerrainType.Heterochromatin:
                Tiles = new ChromatinGenerator(tile, Tiles).GenerateHeterochromatin();
                break;
            case Terrain.TerrainType.Nucleoplasm:
                switch (tile.scale)
                {
                    case -24:
                        Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.IntermolecularFluid, true) });
                        break;
                    default:
                        Fill(Tiles, new[] { new TerrainRule(terrain.terrainType, true) });
                        break;
                }
                break;
            case Terrain.TerrainType.LinkerDNA:
            case Terrain.TerrainType.Nucleosome:
                Tiles = new ChromatinGenerator(tile, Tiles).GenerateDNA(terrain.terrainType == Terrain.TerrainType.Nucleosome ? 0.5 : 0.99);

                break;
            case Terrain.TerrainType.Nucleotide:
                int rotation = int.Parse(terrain.props[PropKey.Rotation]);
                TerrainRule[] nucleobase = terrain.props[PropKey.Nucleobase] switch
                {
                    "adenine" => new[] { Structure.CreateStructureTile("adenine", 1, 2, 0) },
                    "guanine" => new[] { Structure.CreateStructureTile("guanine", 1, 2, 0) },
                    "thymine" => new[] { Structure.CreateStructureTile("thymine", 2, 4, 0) },
                    "cytosine" => new[] { Structure.CreateStructureTile("cytosine", 2, 4, 0) },
                    _ => new[] { new TerrainRule(Terrain.TerrainType.Atom, true, 1, props: new Dictionary<PropKey, string>() {
                            { PropKey.AtomElement, Terrain.AtomElement.Hydrogen.ToString() }
                        }) },
                };
                Structure backbone = terrain.props[PropKey.NucleicBackbone] switch
                {
                    "RNA" => Chem.RNA_BACKBONE.AddAt(5, 4, nucleobase).Rotate(rotation),
                    _ => Chem.DNA_BACKBONE.AddAt(5, 4, nucleobase).Rotate(rotation),
                };
                Tiles = PlaceStructure(Tiles, new[] { new StructureRule(backbone) },
                           0, 0, new[] {
                            new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                       }, rotation);
                Tiles = WaterFill(Tiles);
                break;
            case Terrain.TerrainType.Atom:
                // Check if the atom is ionized
                bool isIonized = terrain.props.ContainsKey(PropKey.AtomIsIonized) &&
                    bool.Parse(terrain.props[PropKey.AtomIsIonized]);

                if (isIonized)
                {
                    Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false) });
                    _ = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Nucleus, true, props: terrain.props) });
                }
                else
                {
                    Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.ElectronCloud, false) });
                    _ = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Nucleus, true, props: terrain.props) });
                }
                break;
            case Terrain.TerrainType.FreeElectron:
                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false) });
                _ = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.ElectronCloud, false) });
                break;

            case Terrain.TerrainType.Nucleus:
                bool parentIsIonized = terrain.props.ContainsKey(PropKey.AtomIsIonized) &&
                    bool.Parse(terrain.props[PropKey.AtomIsIonized]);

                if (parentIsIonized)
                {
                    Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false) });
                }
                else
                {
                    Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.ElectronCloud, false) });
                }

                switch (tile.scale)
                {
                    case -30:
                        var nucleusCenter = parentIsIonized
                            ? AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false) })
                            : AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.ElectronCloud, false) });

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

                    default: _ = AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Nucleus, true, props: terrain.props) }); break;
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
                _ = tile.scale switch
                {
                    -34 => AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Quark, false, props: terrain.props) }),
                    _ => AddCenter(Tiles, new[] { new TerrainRule(Terrain.TerrainType.ValenceQuark, true, props: terrain.props) }),
                };
                break;
            case Terrain.TerrainType.Sandbox:
                Fill(Tiles, new[] { new TerrainRule(Terrain.TerrainType.Nucleoplasm, true), new TerrainRule(Terrain.TerrainType.Nucleosome, true, 0.1) });


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
        var (width, height) = Shape3D(tiles);
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
                var x1 = counterclockwise ? -j * Math.Cos(ang) : j * Math.Cos(ang);
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
        var (width, height) = Shape3D(tiles);

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

    private TileModel[,] StructureFill(TileModel[,] tiles, StructureRule[] rules, double chanceNone, TerrainRule[] baseFill, Terrain.TerrainType[] replace = null)
    {
        return TerrainGenRule.StructureFill(parent: tile, tiles, rules, chanceNone, baseFill, replace);
    }


    private TileModel[,] WaterFill(TileModel[,] tiles)
    {
        return StructureFill(tiles, Chem.WATER.RotateAll(1).Concat(Chem.HYDROXIDE.RotateAll(0.0000001)).Concat(Chem.HYDRONIUM.RotateAll(0.0000001)).ToArray()
                            , 0.5, new[] {
                            new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                        }, new[] { Terrain.TerrainType.IntermolecularSpace });
    }
    private TileModel[,] StarFill(TileModel[,] tiles)
    {
        return StructureFill(tiles,
            Chem.HYDROGEN_IONIZED.RotateAll(73.46 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Hydrogen))
            .Concat(Chem.HELIUM_IONIZED.RotateAll(24.85 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Helium)))
            .Concat(Chem.OXYGEN_IONIZED.RotateAll(0.77 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Oxygen)))
            .Concat(Chem.CARBON_IONIZED.RotateAll(0.29 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Carbon)))
            .Concat(Chem.IRON_IONIZED.RotateAll(0.16 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Iron)))
            .Concat(Chem.NEON_IONIZED.RotateAll(0.12 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Neon)))
            .Concat(Chem.NITROGEN_IONIZED.RotateAll(0.09 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Nitrogen)))
            .Concat(Chem.SILICON_IONIZED.RotateAll(0.07 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Silicon)))
            .Concat(Chem.MAGNESIUM_IONIZED.RotateAll(0.05 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Magnesium)))
            .Concat(Chem.SULFUR_IONIZED.RotateAll(0.04 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Sulfur)))
            .Concat(Chem.FREE_ELECTRON.RotateAll(
                73.46 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Hydrogen)
                + 24.85 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Helium) * 2
                + 0.77 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Oxygen) * 8
                + 0.29 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Carbon) * 6
                + 0.16 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Iron) * 26
                + 0.12 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Neon) * 10
                + 0.09 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Nitrogen) * 7
                + 0.07 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Silicon) * 14
                + 0.05 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Magnesium) * 12
                + 0.04 / AtomGenerator.ElementMassNumber(Terrain.AtomElement.Sulfur) * 16
            ))
            .ToArray()
            , 0, new[] {
                    new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
            }, new[] { Terrain.TerrainType.IntermolecularSpace });
    }

    private TileModel[,] NonMetalFill(TileModel[,] tiles)
    {
        return StructureFill(tiles, Chem.HYDROGEN.RotateAll(3).Concat(Chem.HELIUM.RotateAll(0.25)).ToArray()
                            , 0, new[] {
                                new TerrainRule(Terrain.TerrainType.IntermolecularSpace, false)
                        }, new[] { Terrain.TerrainType.IntermolecularSpace });
    }

    private TileModel[,] StructureTile(TileModel[,] tiles, StructureRule[] rules, TerrainRule[] baseFill)
    {
        return TerrainGenRule.StructureTile(parent: tile, tiles, rules, baseFill);
    }

    private TileModel[,] PlaceStructure(TileModel[,] tiles, StructureRule[] rules, int initX, int initY, TerrainRule[] baseFill, int rotate = 0, Terrain.TerrainType[] replace = null)
    {
        return TerrainGenRule.PlaceStructure(parent: tile, tiles, rules, initX, initY, baseFill, rotate, replace);
    }

    private void AddCircle(TileModel[,] tiles, TerrainRule[] rules, (int, int) center, int radius, bool filled, (int, int)? mask = null)
    {
        TerrainGenRule.AddCircle(parent: tile, tiles, rules, center, radius, filled, mask);
    }

    private TileModel RandomTileFromRule(TerrainRule[] rules)
    {
        return TerrainGenRule.RandomTileFromRule(parent: tile, rules);
    }

    public static (int, int) Shape3D<T>(T[,] array) => (array.GetLength(0), array.GetLength(1));

    private static bool PlanetIsTerrestrial(string planetType)
    {
        return Enum.Parse(typeof(Terrain.PlanetType), planetType) switch
        {
            Terrain.PlanetType.Terrestrial or Terrain.PlanetType.Chunk => true,
            _ => false,
        };
    }

    private TerrainRule[] GetLifeForTerrain(Terrain terrain)
    {
        if (bool.Parse(terrain.props[PropKey.PlanetIsLifeBearing]))
        {
            var props = new Dictionary<PropKey, string>() {
                { PropKey.PlanetIsLifeBearing, true.ToString() },
                { PropKey.Habitat, terrain.terrainType.ToString() }
            };
            switch (terrain.terrainType)
            {
                case Terrain.TerrainType.VerdantTerrain:
                    switch (tile.scale)
                    {
                        case -14:
                            return new[] {
                                new TerrainRule(Terrain.TerrainType.Dinosaur, true, props: props)
                            };
                        case -15:
                            return new[] {
                                new TerrainRule(Terrain.TerrainType.Mammal, true, props: props),
                            };
                        case -16:
                            return new[] {
                                new TerrainRule(Terrain.TerrainType.Bird, true, 0.3, props: props),
                                new TerrainRule(Terrain.TerrainType.Amphibian, true, 0.15, props: props),
                                new TerrainRule(Terrain.TerrainType.Reptile, true, 0.3, props: props),
                                new TerrainRule(Terrain.TerrainType.Trichordate, true, 0.3, props: props)
                            };
                        case -17:
                            return new[] {
                                new TerrainRule(Terrain.TerrainType.Insect, true, props: props)
                            };
                    }
                    break;
                case Terrain.TerrainType.Ocean:
                    switch (tile.scale)
                    {
                        case -14:
                            return new[] {
                                new TerrainRule(Terrain.TerrainType.Cetacean, true, props: props)
                            };
                        case -16:
                            return new[] {
                                new TerrainRule(Terrain.TerrainType.Amphibian, true, 0.1, props: props),
                                new TerrainRule(Terrain.TerrainType.Arthropod, true, 0.2, props: props),
                                new TerrainRule(Terrain.TerrainType.Fish, true, 0.2, props: props),
                                new TerrainRule(Terrain.TerrainType.Radiate, true, 0.2, props: props),
                                new TerrainRule(Terrain.TerrainType.Mollusk, true, 0.2, props: props),
                            };

                        case -19:
                            return new[] {
                                new TerrainRule(Terrain.TerrainType.Eukaryote, false, props: props)
                            };
                        case -21:
                            return new[] {
                                new TerrainRule(Terrain.TerrainType.Prokaryote, false, props: props)
                            };
                    }
                    break;
            }
        }

        return Array.Empty<TerrainRule>();
    }
}
