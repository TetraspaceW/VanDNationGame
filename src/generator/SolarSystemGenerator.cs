using System;
using System.Collections.Generic;
class CelestialGenerator { }
class SolarSystemGenerator : CelestialGenerator
{
    readonly SpectralClass spectralClass;
    readonly TileModel tile;
    readonly Star star;
    readonly Orbit[] orbits;
    public SolarSystemGenerator(TileModel tile, SpectralClass spectralClass)
    {
        // Object       | Size      | S     | Comparable map object
        // -------------+-----------+-------+----------------------
        // Ω-class      | 3 km      | -12.5 |
        // n-Class      | 10 km     | -12   |
        // D-Class      | 10 Mm     | -9    |
        // M-Class      | 300 Mm    | -7.5  |
        // G-Class      | 1 Gm      | -7    | 
        // O-Class      | 10 Gm     | -6    | 
        //              | 100 Gm    | -5    | Inner system map tile
        // Supergiant   | 500 Gm    | -4.5  | 5 inner system tiles

        this.tile = tile;
        this.spectralClass = spectralClass;
        star = StarData(spectralClass);
        int orbitsTableRoll = RND.d(10) + ((spectralClass == SpectralClass.K) ? 1 : 0) + ((spectralClass == SpectralClass.M) ? 3 : 0);
        int numOrbits = 0;
        if (orbitsTableRoll <= 1)
        {
            numOrbits = RND.d(10) + 10;
        }
        else if (orbitsTableRoll <= 5)
        {
            numOrbits = RND.d(10) + 5;
        }
        else if (orbitsTableRoll <= 7)
        {
            numOrbits = RND.d(10);
        }
        else if (orbitsTableRoll <= 9)
        {
            numOrbits = RND.d(5);
        }

        orbits = new Orbit[numOrbits];
        for (int i = 0; i < numOrbits; i++)
        {
            double R;
            if (i == 0)
            {
                R = Math.Pow(star.mass, 2) * 0.05 * RND.d(10);
                orbits[i] = new Orbit(R, star);
            }
            else
            {
                R = orbits[i - 1].distance * (1.1 + RND.d(10) * 0.1) + 0.1;
                orbits[i] = new Orbit(R, star);
            }
        }
    }

    public TileModel[,] SolarSystemMap() // size 0
    {
        return SystemAreaMap(tile);
    }

    private static bool IsSingleBody(World.Type bodyType)
    {
        return bodyType != World.Type.AsteroidBelt;
    }

    private static bool IsPlanet(World.Type bodyType)
    {
        return IsSingleBody(bodyType) && (bodyType != World.Type.Chunk);
    }

    private static bool IsTerrestrial(World.Type bodyType)
    {
        return IsSingleBody(bodyType) && (bodyType != World.Type.GasGiant) && (bodyType != World.Type.Superjovian);
    }

    TileModel[,] SystemAreaMap(TileModel parent)
    {
        Terrain.TerrainType systemArea = parent.terrain.terrainType;
        Terrain.TerrainType fillMaterial = Terrain.TerrainType.SystemOrbit;
        Terrain.TerrainType smallBodiesMaterial = Terrain.TerrainType.SystemOrbit;
        Terrain.TerrainType centerPieceMaterial;
        bool centerIsZoomable = true;

        double innerRadiusAU = Math.Pow(10, parent.scale + 4) * 6.324 / 10 / 2;
        double centralBodyRadius = star.radius * 0.00465;

        switch (systemArea)
        {
            case Terrain.TerrainType.SolarSystem:
                fillMaterial = Terrain.TerrainType.InterstellarSpace;
                centerPieceMaterial = Terrain.TerrainType.HillsCloud;
                smallBodiesMaterial = Terrain.TerrainType.InterstellarSpace;
                break;
            case Terrain.TerrainType.HillsCloud:
                centerPieceMaterial = Terrain.TerrainType.ScatteredDisk;
                smallBodiesMaterial = Terrain.TerrainType.HillsCloudBodies;
                break;
            case Terrain.TerrainType.ScatteredDisk:
                centerPieceMaterial = Terrain.TerrainType.OuterSolarSystem;
                smallBodiesMaterial = Terrain.TerrainType.ScatteredDiskBodies;
                break;
            case Terrain.TerrainType.OuterSolarSystem:
                centerPieceMaterial = Terrain.TerrainType.InnerSolarSystem;
                smallBodiesMaterial = Terrain.TerrainType.KuiperBeltBodies;
                break;
            case Terrain.TerrainType.InnerSolarSystem:
                centerPieceMaterial = Terrain.TerrainType.InnerSolarSystem;
                break;
            default:
                throw new Exception();
        }

        if (centralBodyRadius > innerRadiusAU)
        {
            parent.SetTerrainType(Terrain.TerrainType.Star);
            return GenerateEpiSystem(parent, systemArea, fillMaterial, smallBodiesMaterial, centralBodyRadius, innerRadiusAU);
        }
        else
        {
            return GenerateStandardSystem(parent, systemArea, fillMaterial, smallBodiesMaterial, centerPieceMaterial, centerIsZoomable, innerRadiusAU);
        }

    }

    TileModel[,] GenerateEpiSystem(TileModel parent, Terrain.TerrainType systemArea, Terrain.TerrainType fillMaterial, Terrain.TerrainType smallBodiesMaterial, double stellarRadius, double innerRadiusAU)
    {
        double outermostPlanetDistance = OutermostPlanetDistance();

        var Tiles = new TileModel[10, 10];

        TerrainGenRule.Fill(parent, Tiles, new[] { new TerrainRule(smallBodiesMaterial) });
        var center = TerrainGenRule.ArbitraryCenter(Tiles);
        TerrainGenRule.AddCircle(parent, Tiles, new[] { new TerrainRule(fillMaterial) }, center, (int)Math.Round(outermostPlanetDistance / (innerRadiusAU * 2)), true);

        TerrainGenRule.AddCircle(parent, Tiles, new[] {
            new TerrainRule(star.spectralClass == SpectralClass.D ? Terrain.TerrainType.WhiteDwarfTerrain : Terrain.TerrainType.StellarTerrain, true, props: new Dictionary<PropKey, string>() {
                {PropKey.SpectralClass, spectralClass.ToString()}
            })
        }, center, (int)Math.Round(stellarRadius / (innerRadiusAU * 2)), true);

        PlaceWorlds(Tiles, parent, innerRadiusAU, systemArea, center);

        return Tiles;
    }

    TileModel[,] GenerateStandardSystem(TileModel parent, Terrain.TerrainType systemArea, Terrain.TerrainType fillMaterial, Terrain.TerrainType smallBodiesMaterial, Terrain.TerrainType centerPieceMaterial, bool centerIsZoomable, double innerRadiusAU)
    {
        bool isWholeSystem = systemArea == Terrain.TerrainType.SolarSystem;
        double outermostPlanetDistance = OutermostPlanetDistance();

        var Tiles = new TileModel[10, 10];

        // fill the solar system up with small bodies dust
        TerrainGenRule.Fill(parent, Tiles, new[] { new TerrainRule(smallBodiesMaterial) });

        // add the central tile of the solar system containing the next step in
        var center = TerrainGenRule.AddCenter(parent, Tiles, new[] {
            new TerrainRule(centerPieceMaterial, centerIsZoomable, props: new Dictionary<PropKey, string>() {
                {PropKey.SpectralClass, spectralClass.ToString()}
            })
        });

        // if it's the whole solar system view, add an Oort cloud with r = 5 tiles / 0.5 ly
        if (isWholeSystem)
        {
            TerrainGenRule.AddCircle(parent, Tiles, new[] { new TerrainRule(Terrain.TerrainType.OortCloudBodies) }, center, 5, true, center);
        }

        // remove the small bodies dust from any orbits that are cleared by outer planets
        TerrainGenRule.AddCircle(parent, Tiles, new[] { new TerrainRule(fillMaterial) }, center, (int)Math.Round(outermostPlanetDistance / (innerRadiusAU * 2)), true, center);

        if (centerIsZoomable)
        {
            var centerTile = Tiles[center.Item1, center.Item2];
            centerTile.internalMap = new MapModel(centerTile, SystemAreaMap(centerTile));
        }

        PlaceWorlds(Tiles, parent, innerRadiusAU, systemArea, center);

        return Tiles;
    }

    void PlaceWorlds(TileModel[,] Tiles, TileModel parent, double innerRadiusAU, Terrain.TerrainType systemArea, (int, int) center)
    {
        var i = 0;
        while (i < orbits.Length && orbits[i].distance <= innerRadiusAU * 10)
        {
            if (orbits[i].body != null && orbits[i].distance > innerRadiusAU)
            {
                var radius = (int)Math.Round(orbits[i].distance / (innerRadiusAU * 2));
                PlaceWorld(parent, orbits[i].body, radius, systemArea, Tiles, center);
            }
            i++;
        }
    }

    private static void PlaceWorld(TileModel parent, World body, int distance, Terrain.TerrainType systemDistance, TileModel[,] Tiles, (int, int) center)
    {
        Terrain.TerrainType terrainType;
        if (!IsSingleBody(body.bodyType))
        {
            terrainType = systemDistance switch
            {
                Terrain.TerrainType.SolarSystem => Terrain.TerrainType.OortCloudBodies,
                Terrain.TerrainType.HillsCloud => Terrain.TerrainType.HillsCloudBodies,
                Terrain.TerrainType.ScatteredDisk => Terrain.TerrainType.ScatteredDiskBodies,
                Terrain.TerrainType.OuterSolarSystem => Terrain.TerrainType.KuiperBeltBodies,
                Terrain.TerrainType.InnerSolarSystem => Terrain.TerrainType.AsteroidBeltBodies,
                _ => Terrain.TerrainType.AsteroidBeltBodies,
            };
            TerrainGenRule.AddCircle(parent, Tiles,
            rules: new[] {
                new TerrainRule(terrainType)
            }, center, distance, false);
        }
        else
        {
            terrainType = systemDistance switch
            {
                Terrain.TerrainType.SolarSystem => Terrain.TerrainType.FarfarfarSystemBody,
                Terrain.TerrainType.HillsCloud => Terrain.TerrainType.FarfarSystemBody,
                Terrain.TerrainType.ScatteredDisk => Terrain.TerrainType.FarSystemBody,
                Terrain.TerrainType.OuterSolarSystem => Terrain.TerrainType.OuterSystemBody,
                Terrain.TerrainType.InnerSolarSystem => Terrain.TerrainType.InnerSystemBody,
                _ => Terrain.TerrainType.InnerSystemBody,
            };
            Terrain.PlanetType planetType = Terrain.PlanetType.Terrestrial;
            switch (body.bodyType)
            {
                case World.Type.Chunk: case World.Type.Terrestrial: planetType = Terrain.PlanetType.Terrestrial; break;
                case World.Type.GasGiant: case World.Type.Superjovian: planetType = Terrain.PlanetType.Jovian; break;
            }

            TerrainGenRule.AddAtDistance(parent, Tiles,
            rules: new[] {
                new TerrainRule(terrainType, true, props: new Dictionary<PropKey, string>() {
                    { PropKey.PlanetType, planetType.ToString() },
                    { PropKey.PlanetIsLifeBearing, body.hasLife.ToString() },
                    { PropKey.PlanetHydrosphereType, body.hydrosphere.ToString() },
                    { PropKey.PlanetHydrosphereCoverage, body.hydrosphereCoverage.ToString() },
                    { PropKey.PlanetRadius, body.radius.ToString() },
                    { PropKey.PlanetTemperature, (body.temperature - 273.15).ToString() },
                    { PropKey.OrbitalPeriod, body.orbit.orbitalPeriod.ToString() }
                })
            },
            center,
            distance,
            mask: new List<Terrain.TerrainType> {
                Terrain.TerrainType.InnerSystemBody, Terrain.TerrainType.OuterSystemBody, Terrain.TerrainType.FarfarfarSystemBody, Terrain.TerrainType.FarfarSystemBody, Terrain.TerrainType.FarSystemBody,
                Terrain.TerrainType.Star, Terrain.TerrainType.InnerSolarSystem, Terrain.TerrainType.OuterSolarSystem, Terrain.TerrainType.ScatteredDisk, Terrain.TerrainType.HillsCloud,
                Terrain.TerrainType.StellarTerrain
            });
        }
    }

    private static Star StarData(SpectralClass spectralClass)
    {
        return spectralClass switch
        {
            SpectralClass.O => new Star(R: 10, L: 100000.0, M: 50.0, age: 0.005, spectralClass: SpectralClass.O),
            SpectralClass.B => new Star(R: 5, L: 1000.0, M: 10.0, age: 0.05, spectralClass: SpectralClass.B),
            SpectralClass.A => new Star(R: 1.7, L: 20.0, M: 2.0, age: 1, spectralClass: SpectralClass.A),
            SpectralClass.F => new Star(R: 1.3, L: 4.0, M: 1.5, age: 2, spectralClass: SpectralClass.F),
            SpectralClass.G => new Star(R: 1, L: 1.0, M: 1.0, age: 5, spectralClass: SpectralClass.G),
            SpectralClass.K => new Star(R: 0.8, L: 0.20, M: 0.7, age: 5, spectralClass: SpectralClass.K),
            SpectralClass.M => new Star(R: 0.3, L: 0.01, M: 0.2, age: 5, spectralClass: SpectralClass.M),
            SpectralClass.D => new Star(R: 0.01, L: 0.01, M: 1.0, age: 5, spectralClass: SpectralClass.D),
            SpectralClass.MIII => new Star(R: 50, L: 1000, M: 1.0, age: 10, spectralClass: SpectralClass.M),
            SpectralClass.KI => new Star(R: 500, L: 30000, M: 10.0, age: 1, spectralClass: SpectralClass.K),
            _ => new Star(R: 1, L: 1, M: 1, age: 5, spectralClass: SpectralClass.G),
        };
    }

    private double OutermostPlanetDistance()
    {
        if (orbits.Length > 0)
        {
            for (int i = orbits.Length - 1; i >= 0; i--)
            {
                if (orbits[i].body != null && IsPlanet(orbits[i].body.bodyType))
                {
                    return orbits[i].distance;
                }
            }
        }
        return 0;
    }

    public enum SpectralClass
    {
        O, B, A, F, G, K, M, D,
        MIII, KI
    }

    public enum Hydrosphere
    {
        Liquid, IceSheet, Crustal, None
    }

    class Atmosphere
    {
        readonly List<G> gasesPresent = new();
        public double pressure = 0;

        public Atmosphere(double temperature, double mass, double radius)
        {
            var gravity = mass / Math.Pow(radius / 6380, 2);
            var escapeVelocity = Math.Sqrt(19600 * gravity * radius) / 11200;

            var mwr = 0.02783 * temperature / Math.Pow(escapeVelocity, 2);

            var atmosphereRoll = RND.d(9);
            if (temperature <= 50)
            {
                switch (atmosphereRoll)
                {
                    case 1: case 2: case 3: case 4: gasesPresent = new List<G> { G.H2 }; break;
                    case 5: case 6: gasesPresent = new List<G> { G.He }; break;
                    case 7: case 8: gasesPresent = new List<G> { G.H2, G.He }; break;
                    case 9: gasesPresent = new List<G> { G.Ne }; break;
                }
            }
            else if (temperature > 50 && temperature <= 150)
            {
                switch (atmosphereRoll)
                {
                    case 1: case 2: case 3: case 4: gasesPresent = new List<G> { G.N2, G.CH4 }; break;
                    case 5: case 6: gasesPresent = new List<G> { G.H2, G.He, G.N2 }; break;
                    case 7: case 8: gasesPresent = new List<G> { G.N2, G.CO }; break;
                    case 9: gasesPresent = new List<G> { G.H2, G.He }; break;
                }
            }
            else if (temperature > 150 && temperature <= 400)
            {
                switch (atmosphereRoll)
                {
                    case 1: case 2: case 3: case 4: gasesPresent = new List<G> { G.N2, G.CO2 }; break;
                    case 5: case 6: gasesPresent = new List<G> { G.CO2 }; break;
                    case 7: case 8: gasesPresent = new List<G> { G.N2, G.CH4 }; break;
                    case 9: gasesPresent = (temperature > 240) ? new List<G> { G.CO2, G.CH4, G.NH3 } : new List<G> { G.H2, G.He }; break;
                }
            }
            else if (temperature > 400)
            {
                switch (atmosphereRoll)
                {
                    case 1: case 2: case 3: case 4: gasesPresent = new List<G> { G.N2, G.CO2 }; break;
                    case 5: case 6: gasesPresent = new List<G> { G.CO2 }; break;
                    case 7: case 8: gasesPresent = new List<G> { G.NO2, G.SO2 }; break;
                    case 9: gasesPresent = new List<G> { G.SO2 }; break;
                }
            }

            var gasesRemoved = 0;
            gasesPresent = gasesPresent.FindAll((gas) =>
            {
                bool gasRemains = MolecularWeight(gas) >= mwr;
                gasesRemoved += gasRemains ? 0 : 1;
                return gasRemains;
            });

            if (gasesPresent.Count >= 0)
            {
                pressure = mass * RND.d(10);
                switch (RND.d(10) + (gasesRemoved > 0 ? -1 : 0))
                {
                    case 0: case 1: case 2: pressure *= 0.01; break;
                    case 3: case 4: pressure *= 0.1; break;
                    case 5: case 6: case 7: pressure *= 0.2; break;
                    case 8: pressure *= 0.5; break;
                    case 9: pressure *= 2; break;
                    case 10: pressure *= 20; break;
                }
            }
            else
            {
                pressure = 0;
            }
        }

        public bool HasCO2()
        {
            return gasesPresent.Exists((gas) => { return gas == G.CO2; });
        }

        private enum G
        {
            H2, He,
            CH4, NH3,
            Ne, N2, CO,
            CO2, NO2,
            SO2
        }

        static double MolecularWeight(G gas)
        {
            var H = AtomGenerator.ElementMassNumber(Terrain.AtomElement.Hydrogen);
            var He = AtomGenerator.ElementMassNumber(Terrain.AtomElement.Helium);
            var C = AtomGenerator.ElementMassNumber(Terrain.AtomElement.Carbon);
            var Ne = AtomGenerator.ElementMassNumber(Terrain.AtomElement.Neon);
            var N = AtomGenerator.ElementMassNumber(Terrain.AtomElement.Nitrogen);
            var O = AtomGenerator.ElementMassNumber(Terrain.AtomElement.Oxygen);
            var S = AtomGenerator.ElementMassNumber(Terrain.AtomElement.Sulfur);
            return gas switch
            {
                G.H2 => H * 2,
                G.He => He,
                G.CH4 => C + H * 4,
                G.NH3 => N + H * 3,
                G.Ne => Ne,
                G.N2 => N,
                G.CO => C + O,
                G.CO2 => C + O * 2,
                G.NO2 => N + O * 2,
                G.SO2 => S + O * 2,
                _ => 0,
            };
        }
    }

    class Orbit
    {
        public World body;
        public double distance;
        public bool inner = false;
        public Star star;
        public double orbitalPeriod;

        public Orbit(double R, Star star)
        {
            this.star = star;
            distance = R;

            orbitalPeriod = Math.Sqrt(Math.Pow(distance, 3) / star.mass);

            var vaporised = R <= Math.Sqrt(star.luminosity) * 0.025 || star.spectralClass == SpectralClass.D && R <= Math.Sqrt(1000) * 0.025;
            inner = R <= Math.Sqrt(star.luminosity) * 4 || star.spectralClass == SpectralClass.D && R <= 4;

            var orbitRoll = RND.d(95);
            var T = 255.0 / Math.Sqrt(distance / Math.Sqrt(star.luminosity));
            if (inner)
            {
                if (orbitRoll <= 18)
                {
                    body = new World(World.Type.AsteroidBelt, T, this);
                }
                else if (orbitRoll <= 62)
                {
                    body = new World(World.Type.Terrestrial, T, this);
                }
                else if (orbitRoll <= 71)
                {
                    body = new World(World.Type.Chunk, T, this);
                }
                else if (orbitRoll <= 82)
                {
                    body = new World(World.Type.GasGiant, T, this);
                }
                else if (orbitRoll <= 87)
                {
                    body = new World(World.Type.Superjovian, T, this);
                }
            }
            else
            {
                if (orbitRoll <= 15)
                {
                    body = new World(World.Type.AsteroidBelt, T, this);
                }
                else if (orbitRoll <= 23)
                {
                    body = new World(World.Type.Terrestrial, T, this);
                }
                else if (orbitRoll <= 35)
                {
                    body = new World(World.Type.Chunk, T, this);
                }
                else if (orbitRoll <= 74)
                {
                    body = new World(World.Type.GasGiant, T, this);
                }
                else if (orbitRoll <= 84)
                {
                    body = new World(World.Type.Superjovian, T, this);
                }
            }

            body = vaporised ? null : body;
        }

        public Orbit(double R, World body)
        {
            this.body = body;
            distance = R;
        }
    }

    class World
    {

        public double temperature;
        public double radius;
        public double density;
        public double mass;
        public Orbit[] moons;
        public Orbit orbit; // Store reference to the orbit
        public Type bodyType;
        public Atmosphere atmosphere;
        public Hydrosphere hydrosphere;
        public int hydrosphereCoverage;
        public bool hasLife;
        public World(Type bodyType, double T, Orbit orbit)
        {
            this.bodyType = bodyType;
            temperature = T;
            this.orbit = orbit; // Store the orbit reference

            var sizeRoll = d(10);
            var innerSizeRoll = d(10);
            var densityRoll = Math.Min(Math.Max(d(10) + orbit.star.abundance, 1), 11);
            switch (bodyType)
            {
                case Type.Chunk:
                    radius = 200 * innerSizeRoll;
                    density = orbit.inner ? (densityRoll * 0.1 + 0.3) : (densityRoll * 0.05 + 0.1);
                    break;
                case Type.Terrestrial:
                    sizeRoll += orbit.star.abundance;
                    sizeRoll = Math.Min(Math.Max(sizeRoll, 1), 10);
                    switch (sizeRoll)
                    {
                        case 1: case 2: radius = innerSizeRoll * 100 + 2000; break;
                        case 3: case 4: radius = innerSizeRoll * 100 + 3000; break;
                        case 5: case 6: case 7: case 8: radius = innerSizeRoll * 100 + (sizeRoll - 1) * 1000; break;
                        case 9: radius = innerSizeRoll * 200 + (sizeRoll - 1) * 1000; break;
                        case 10: radius = innerSizeRoll * 500 + 10000; break;
                    }
                    density = orbit.inner ? (densityRoll * 0.1 + 0.3) : (densityRoll * 0.05 + 0.1);
                    break;
                case Type.GasGiant:
                    switch (sizeRoll <= 5)
                    {
                        case true: radius = innerSizeRoll * 300 + (sizeRoll + 4) * 3000; break;
                        case false: radius = innerSizeRoll * 1000 + (sizeRoll - 3) * 10000; break;
                    }
                    density = orbit.inner ? (densityRoll * 0.1 + 0.3) : (densityRoll * 0.05 + 0.1);
                    break;
                case Type.Superjovian:
                    radius = (sizeRoll - 0.5 * orbit.star.age) * 2000 + 60000;
                    switch (d(10))
                    {
                        case 1: case 2: case 3: case 4: mass = d(10) * 50 + 500; break;
                        case 5: case 6: case 7: mass = d(10) * 100 + 1000; break;
                        case 8: case 9: mass = d(10) * 100 + 2000; break;
                        case 10: mass = d(10) * 100 + 300; break;
                    }
                    density = mass / Math.Pow(radius / 6380, 3);
                    break;
            }

            mass = Math.Pow(radius / 6380, 3) * density;

            if (IsTerrestrial(bodyType))
            {
                (hydrosphere, hydrosphereCoverage) = GenerateHydrosphere(orbit.inner, IsTerrestrial(bodyType));

                atmosphere = new Atmosphere(temperature, mass, radius);
                hydrosphereCoverage = atmosphere.pressure <= 0.006 ? 0 : hydrosphereCoverage;
                hydrosphere = (hydrosphereCoverage == 0 && hydrosphere != Hydrosphere.Crustal) ? Hydrosphere.None : hydrosphere;


                temperature = UpdateTemperature(temperature, atmosphere, hydrosphere, hydrosphereCoverage, orbit);
                hydrosphere = HydrosphereFromTemperature(temperature, orbit.inner);
                hydrosphereCoverage = hydrosphere != Hydrosphere.None ? hydrosphereCoverage : 0;

                hasLife = hydrosphere == Hydrosphere.Liquid && atmosphere.HasCO2() && d(10) <= 3;
            }
            else
            {
                (hydrosphere, hydrosphereCoverage) = (Hydrosphere.None, 0);
            }

            moons = GenerateMoons(orbit.inner, bodyType);
        }

        public World(double R, double T, bool inner)
        {
            radius = R;
            temperature = T;
            (hydrosphere, hydrosphereCoverage) = GenerateHydrosphere(inner, true);
            hasLife = hydrosphere == Hydrosphere.Liquid && d(10) <= 3;
        }

        // Save the orbit reference when creating Orbit for a moon
        public void SetOrbit(Orbit orbit)
        {
            this.orbit = orbit;
        }

        static Hydrosphere HydrosphereFromTemperature(double temperature, bool inner)
        {
            if (inner)
            {
                if (temperature <= 273.15) { return Hydrosphere.IceSheet; }
                else if (temperature <= 373.15) { return Hydrosphere.Liquid; }
                else { return Hydrosphere.None; }
            }
            else { return Hydrosphere.Crustal; }
        }

        (Hydrosphere, int hydrosphereCoverage) GenerateHydrosphere(bool inner, bool terrestrial)
        {
            Hydrosphere hydrosphere = Hydrosphere.None;
            int hydrosphereCoverage = 0;
            if (inner && terrestrial)
            {
                if (temperature <= 273.15) { hydrosphere = Hydrosphere.IceSheet; }
                else if (temperature <= 373.15) { hydrosphere = Hydrosphere.Liquid; }
            }
            else if (terrestrial) { hydrosphere = Hydrosphere.Crustal; }

            if (hydrosphere != Hydrosphere.None)
            {
                var hydroRoll = d(10);
                if (radius <= 2000)
                {
                    switch (hydroRoll)
                    {
                        case 1: case 2: case 3: case 4: case 5: break;
                        case 6: case 7: hydrosphereCoverage += d(10); break;
                        case 8: hydrosphereCoverage += d(10) + 10; break;
                        case 9: case 10: hydrosphereCoverage += Math.Min(d(10, N: (hydroRoll - 8) * 5) + (hydroRoll - 9) * 10, 100); break;
                    }
                }
                else if (radius <= 4000)
                {
                    switch (hydroRoll)
                    {
                        case 1: case 2: break;
                        case 3: case 4: hydrosphereCoverage += d(10); break;
                        case 5: case 6: case 7: case 8: case 9: hydrosphereCoverage += d(10) + (hydroRoll - 4) * 10; break;
                        case 10: hydrosphereCoverage += Math.Min(d(10, N: 10) + 10, 100); break;
                    }
                }
                else if (radius <= 7000)
                {
                    switch (hydroRoll)
                    {
                        case 1: break;
                        case 2: hydrosphereCoverage += d(10, N: 2); break;
                        case 3: case 4: case 5: case 6: case 7: case 8: hydrosphereCoverage += d(10) + (hydroRoll - 1) * 10; break;
                        case 9: hydrosphereCoverage += d(10, N: 2) + 80; break;
                        case 10: hydrosphereCoverage = 100; break;
                    }
                }
                else
                {
                    switch (hydroRoll)
                    {
                        case 1: break;
                        case 2: hydrosphereCoverage += d(10, N: 2); break;
                        case 3: case 4: hydrosphereCoverage += d(10, N: 2) + (hydroRoll - 2) * 20; break;
                        case 5: case 6: case 7: case 8: hydrosphereCoverage += d(10) + (hydroRoll + 1) * 10; break;
                        case 9: case 10: hydrosphereCoverage = 100; break;
                    }
                }
            }

            hydrosphere = (hydrosphereCoverage == 0 && hydrosphere != Hydrosphere.Crustal) ? Hydrosphere.None : hydrosphere;

            return (hydrosphere, hydrosphereCoverage);
        }

        Orbit[] GenerateMoons(bool inner, Type parentType)
        {
            var numberOfMoons = 0;
            var moonsRoll = d(10) + (inner ? 0 : 5);

            switch (parentType)
            {
                case Type.Chunk:
                    numberOfMoons = (moonsRoll >= 10) ? 1 : 0; break;
                case Type.Terrestrial:
                    switch (moonsRoll)
                    {
                        case 6: case 7: numberOfMoons = 1; break;
                        case 8: case 9: numberOfMoons = d(2); break;
                        case 10: case 11: case 12: case 13: numberOfMoons = d(5); break;
                        case 14: case 15: numberOfMoons = d(10); break;
                    }
                    break;
                case Type.GasGiant:
                case Type.Superjovian:
                    switch (moonsRoll)
                    {
                        case 1: case 2: case 3: case 4: case 5: numberOfMoons = d(5); break;
                        case 6: case 7: numberOfMoons = d(10); break;
                        case 8: case 9: numberOfMoons = d(10) + 5; break;
                        case 10: case 11: case 12: case 13: numberOfMoons = d(10) + 10; break;
                        case 14: case 15: numberOfMoons = d(10) + 20; break;
                    }
                    break;
            }

            var orbits = new Orbit[numberOfMoons];
            for (int i = 0; i < numberOfMoons; i++)
            {
                var distanceRoll = d(9);
                double distance = 0;
                switch (distanceRoll)
                {
                    case 1: case 2: case 3: case 4: distance = d(10) * 0.5 + 1; break;
                    case 5: case 6: distance = d(10) + 6; break;
                    case 7: case 8: distance = d(10) * 3 + 16; break;
                    case 9: distance = d(100) * 3 + 45; break;
                }
                orbits[i] = new Orbit(distance, this);

                var lunarRadius = 0;
                switch (parentType)
                {
                    case Type.Chunk: lunarRadius = d(10) * 10; break;
                    case Type.Terrestrial:
                        var terrestrialMoonRoll = d(94);
                        if (1 <= terrestrialMoonRoll && terrestrialMoonRoll <= 64)
                        {
                            lunarRadius = d(10) * 10;
                        }
                        else if (65 <= terrestrialMoonRoll && terrestrialMoonRoll <= 84)
                        {
                            lunarRadius = d(10) * 100;
                        }
                        else if (85 <= terrestrialMoonRoll && terrestrialMoonRoll <= 94)
                        {
                            lunarRadius = d(10) * 100 + 1000;
                        }
                        break;
                    case Type.GasGiant:
                    case Type.Superjovian:
                        var ggMoonRoll = d(100);
                        if (1 <= ggMoonRoll && ggMoonRoll <= 64)
                        {
                            lunarRadius = d(10) * 10;
                        }
                        else if (65 <= ggMoonRoll && ggMoonRoll <= 84)
                        {
                            lunarRadius = d(10) * 100;
                        }
                        else if (85 <= ggMoonRoll && ggMoonRoll <= 94)
                        {
                            lunarRadius = d(10) * 100 + 1000;
                        }
                        else if (95 <= ggMoonRoll && ggMoonRoll <= 99)
                        {
                            lunarRadius = d(10) * 200 + 2000;
                        }
                        else if (ggMoonRoll == 100)
                        {
                            lunarRadius = d(10) * 400 + 4000;
                        }
                        break;
                }

                var moonWorld = new World(lunarRadius, temperature, inner);
                orbits[i].body = moonWorld;
                moonWorld.SetOrbit(orbits[i]);
            }

            return orbits;
        }

        double UpdateTemperature(double temperature, Atmosphere atmosphere, Hydrosphere hydrosphere, int hydrosphereCoverage, Orbit orbit)
        {
            int outerAlbedoRoll = d(10) +
                (orbit.inner ? (
                (atmosphere.pressure == 0 ? -2 : 0)
                + (atmosphere.pressure >= 5 ? 2 : (atmosphere.pressure >= 50 ? 4 : 0))
                + (hydrosphere == Hydrosphere.IceSheet ? (hydrosphereCoverage >= 50 ? -2 : (hydrosphereCoverage >= 90 ? -4 : 0)) : 0))
                : (atmosphere.pressure >= 1 ? 1 : 0));
            int innerAlbedoRoll = d(10);

            double albedoFactor = 1;

            switch (outerAlbedoRoll)
            {
                case -7: case -6: case -5: case -4: case -3: case -2: case -1: case 0: case 1: albedoFactor = innerAlbedoRoll * 0.01 + 0.75; break;
                case 2: case 3: albedoFactor = innerAlbedoRoll * 0.01 + (orbit.inner ? 0.85 : 0.75); break;
                case 4: case 5: albedoFactor = innerAlbedoRoll * 0.01 + (orbit.inner ? 0.95 : 0.85); break;
                case 6: albedoFactor = innerAlbedoRoll * 0.01 + 0.95; break;
                case 7: albedoFactor = innerAlbedoRoll * 0.01 + (orbit.inner ? 1.05 : 0.95); break;
                case 8: case 9: albedoFactor = innerAlbedoRoll * 0.01 + 1.05; break;
                case 10: case 11: case 12: albedoFactor = innerAlbedoRoll * 0.01 + 1.15; break;
            }

            double vapourFactor = Math.Max((temperature - 240) / 100 * hydrosphereCoverage / 100 * d(10), 0);
            return temperature * albedoFactor * (1 + (Math.Pow(atmosphere.pressure, 0.5) * 0.01 * d(10)) + vapourFactor * 0.1);
        }

        static int d(int n, int N = 1)
        {
            return RND.d(n, N);
        }

        public enum Type
        {
            AsteroidBelt, Chunk, Terrestrial, GasGiant, Superjovian
        }
    }

    class Star
    {
        public SpectralClass spectralClass;
        public double radius;
        public double luminosity;
        public double mass;
        public int age;
        public int abundance;
        public Star(double R, double L, double M, double age, SpectralClass spectralClass)
        {
            radius = R;
            luminosity = L;
            mass = M;
            this.spectralClass = spectralClass;
            this.age = (int)age;

            var abundanceRoll = RND.d(10, N: 2) + this.age;
            if (abundanceRoll <= 9)
            {
                abundance = 2;
            }
            else if (abundanceRoll <= 12)
            {
                abundance = 1;
            }
            else if (abundanceRoll <= 18)
            {
                abundance = 0;
            }
            else if (abundanceRoll <= 21)
            {
                abundance = -1;
            }
            else
            {
                abundance = -3;
            }
        }
    }
}