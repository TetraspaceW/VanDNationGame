using System;
using System.Collections.Generic;
class CelestialGenerator { }
class SolarSystemGenerator : CelestialGenerator
{
    SpectralClass spectralClass;
    TileModel tile;
    Star star;
    Orbit[] orbits;
    private readonly Random _random = new Random();
    public SolarSystemGenerator(TileModel tile, SpectralClass spectralClass)
    {
        // Object       | Size      | S     | Comparable map object
        // -------------+-----------+-------+----------------------
        // M-Class      | 300 Mm    | -8×5  | 5 epiepiepistellar system tiles
        // G-Class      | 1 Gm      | -7    | Epiepistellar system tile
        // O-Class      | 10 Gm     | -6    | Epistellar system tile 
        //              | 100 Gm    | -5    | Inner system map tile
        // Supergiant   | 500 Gm    | -5×5  | 5 inner system tiles

        this.tile = tile;
        this.spectralClass = spectralClass;
        this.star = StarData(spectralClass);
        int orbitsTableRoll = _random.Next(1, 11) + ((spectralClass == SpectralClass.K) ? 1 : 0) + ((spectralClass == SpectralClass.M) ? 3 : 0);
        int numOrbits = 0;
        if (orbitsTableRoll <= 1)
        {
            numOrbits = _random.Next(1, 11) + 10;
        }
        else if (orbitsTableRoll <= 5)
        {
            numOrbits = _random.Next(1, 11) + 5;
        }
        else if (orbitsTableRoll <= 7)
        {
            numOrbits = _random.Next(1, 11);
        }
        else if (orbitsTableRoll <= 9)
        {
            numOrbits = _random.Next(1, 6);
        }

        orbits = new Orbit[numOrbits];
        for (int i = 0; i < numOrbits; i++)
        {
            double R;
            if (i == 0)
            {
                R = Math.Pow(star.mass, 2) * 0.05 * _random.Next(1, 11);
                orbits[i] = new Orbit(R, star);
            }
            else
            {
                R = orbits[i - 1].distance * (1.1 + _random.Next(1, 11) * 0.1) + 0.1;
                orbits[i] = new Orbit(R, star);
            }
        }
    }

    private bool IsSingleBody(Body.Type bodyType)
    {
        return (bodyType != Body.Type.AsteroidBelt);
    }

    private bool IsPlanet(Body.Type bodyType)
    {
        return IsSingleBody(bodyType) && (bodyType != Body.Type.Chunk);
    }

    TileModel[,] SystemAreaMap(TileModel parent)
    {
        Terrain.TerrainType systemArea = parent.terrain.terrainType;
        Terrain.TerrainType fillMaterial = Terrain.TerrainType.SystemOrbit;
        Terrain.TerrainType smallBodiesMaterial = Terrain.TerrainType.SystemOrbit;
        Terrain.TerrainType centerPieceMaterial;
        bool centerIsZoomable = true;

        double innerRadiusAU = (Math.Pow(10, parent.scale + 4) * 6.324) / 10 / 2;
        double stellarRadius = star.radius * 0.00465;

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
                centerPieceMaterial = Terrain.TerrainType.EpistellarSolarSystem;
                break;
            case Terrain.TerrainType.EpistellarSolarSystem:
                centerPieceMaterial = Terrain.TerrainType.EpiepistellarSolarSystem;
                break;
            case Terrain.TerrainType.EpiepistellarSolarSystem:
                centerPieceMaterial = Terrain.TerrainType.EpiepiepistellarSolarSystem;
                break;
            case Terrain.TerrainType.EpiepiepistellarSolarSystem:
                centerPieceMaterial = Terrain.TerrainType.Star;
                break;
            default:
                throw new Exception();
        }

        if (stellarRadius > innerRadiusAU)
        {
            parent.SetTerrainType(Terrain.TerrainType.Star);
            return GenerateEpiSystem(parent, systemArea, fillMaterial, smallBodiesMaterial, stellarRadius, innerRadiusAU);
        }
        else
        {
            return GenerateStandardSystem(parent, systemArea, fillMaterial, smallBodiesMaterial, centerPieceMaterial, centerIsZoomable, innerRadiusAU);
        }

    }

    TileModel[,] GenerateEpiSystem(TileModel parent, Terrain.TerrainType systemArea, Terrain.TerrainType fillMaterial, Terrain.TerrainType smallBodiesMaterial, double stellarRadius, double innerRadiusAU)
    {
        bool isWholeSystem = (systemArea == Terrain.TerrainType.SolarSystem);
        double outermostPlanetDistance = OutermostPlanetDistance();

        var Tiles = new TileModel[10, 10];

        TerrainGenRule.Fill(parent, Tiles, new[] { new TerrainRule(smallBodiesMaterial) });
        var center = TerrainGenRule.ArbitraryCenter(Tiles);
        TerrainGenRule.AddCircle(parent, Tiles, new[] { new TerrainRule(fillMaterial) }, center, (int)Math.Round(outermostPlanetDistance / (innerRadiusAU * 2)), true);

        TerrainGenRule.AddCircle(parent, Tiles, new[] {
            new TerrainRule(Terrain.TerrainType.StarSurface, false, props: new Dictionary<PropKey, string>() {
                {PropKey.SpectralClass, spectralClass.ToString()}
            })
        }, center, (int)Math.Round(stellarRadius / (innerRadiusAU * 2)), true);

        PlaceWorlds(Tiles, parent, innerRadiusAU, systemArea, center);

        return Tiles;
    }

    TileModel[,] GenerateStandardSystem(TileModel parent, Terrain.TerrainType systemArea, Terrain.TerrainType fillMaterial, Terrain.TerrainType smallBodiesMaterial, Terrain.TerrainType centerPieceMaterial, bool centerIsZoomable, double innerRadiusAU)
    {
        bool isWholeSystem = (systemArea == Terrain.TerrainType.SolarSystem);
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

    public TileModel[,] SolarSystemMap() // size 0
    {
        return SystemAreaMap(tile);
    }

    private void PlaceWorld(TileModel parent, Body body, int distance, Terrain.TerrainType systemDistance, TileModel[,] Tiles, (int, int) center)
    {
        Terrain.TerrainType terrainType;
        if (!IsSingleBody(body.bodyType))
        {
            switch (systemDistance)
            {
                case Terrain.TerrainType.SolarSystem: terrainType = Terrain.TerrainType.OortCloudBodies; break;
                case Terrain.TerrainType.HillsCloud: terrainType = Terrain.TerrainType.HillsCloudBodies; break;
                case Terrain.TerrainType.ScatteredDisk: terrainType = Terrain.TerrainType.ScatteredDiskBodies; break;
                case Terrain.TerrainType.OuterSolarSystem: terrainType = Terrain.TerrainType.KuiperBeltBodies; break;
                case Terrain.TerrainType.InnerSolarSystem: terrainType = Terrain.TerrainType.AsteroidBeltBodies; break;
                default: terrainType = Terrain.TerrainType.AsteroidBeltBodies; break;
            }
            TerrainGenRule.AddCircle(parent, Tiles,
            rules: new[] {
                new TerrainRule(terrainType)
            }, center, distance, false);
        }
        else
        {
            switch (systemDistance)
            {
                case Terrain.TerrainType.SolarSystem: terrainType = Terrain.TerrainType.FarfarfarSystemBody; break;
                case Terrain.TerrainType.HillsCloud: terrainType = Terrain.TerrainType.FarfarSystemBody; break;
                case Terrain.TerrainType.ScatteredDisk: terrainType = Terrain.TerrainType.FarSystemBody; break;
                case Terrain.TerrainType.OuterSolarSystem: terrainType = Terrain.TerrainType.OuterSystemBody; break;
                case Terrain.TerrainType.InnerSolarSystem: terrainType = Terrain.TerrainType.InnerSystemBody; break;
                default: terrainType = Terrain.TerrainType.InnerSystemBody; break;
            }
            Terrain.PlanetType planetType = Terrain.PlanetType.Terrestrial;
            switch (body.bodyType)
            {
                case Body.Type.Chunk: case Body.Type.Terrestrial: planetType = Terrain.PlanetType.Terrestrial; break;
                case Body.Type.GasGiant: case Body.Type.Superjovian: planetType = Terrain.PlanetType.Jovian; break;
            }

            TerrainGenRule.AddAtDistance(parent, Tiles,
            rules: new[] {
                new TerrainRule(terrainType, true, props: new Dictionary<PropKey, string>() {
                    { PropKey.PlanetType, planetType.ToString() },
                    { PropKey.PlanetIsLifeBearing, body.hasLife.ToString() },
                    { PropKey.PlanetHydrosphereType, body.hydrosphere.ToString() },
                    { PropKey.PlanetHydrosphereCoverage, body.hydrosphereCoverage.ToString() },
                    { PropKey.PlanetRadius, body.radius.ToString() }
                })
            },
            center,
            distance,
            mask: new List<Terrain.TerrainType> {
                Terrain.TerrainType.InnerSystemBody, Terrain.TerrainType.OuterSystemBody, Terrain.TerrainType.FarfarfarSystemBody, Terrain.TerrainType.FarfarSystemBody, Terrain.TerrainType.FarSystemBody,
                Terrain.TerrainType.Star, Terrain.TerrainType.InnerSolarSystem, Terrain.TerrainType.OuterSolarSystem, Terrain.TerrainType.ScatteredDisk, Terrain.TerrainType.HillsCloud,
                Terrain.TerrainType.StarSurface
            });
        }
    }

    private Star StarData(SpectralClass spectralClass)
    {
        switch (spectralClass)
        {
            case SpectralClass.O: return new Star(R: 10, L: 100000.0, M: 50.0, age: 0.005);
            case SpectralClass.B: return new Star(R: 5, L: 1000.0, M: 10.0, age: 0.05);
            case SpectralClass.A: return new Star(R: 1.7, L: 20.0, M: 2.0, age: 1);
            case SpectralClass.F: return new Star(R: 1.3, L: 4.0, M: 1.5, age: 2);
            case SpectralClass.G: return new Star(R: 1, L: 1.0, M: 1.0, age: 5);
            case SpectralClass.K: return new Star(R: 0.8, L: 0.20, M: 0.7, age: 5);
            case SpectralClass.M: return new Star(R: 0.3, L: 0.01, M: 0.2, age: 5);
            case SpectralClass.D: return new Star(R: 0.01, L: 0.01, M: 1.0, age: 5);
            case SpectralClass.MIII: return new Star(R: 50, L: 1000, M: 1.0, age: 10);
            case SpectralClass.KI: return new Star(R: 500, L: 30000, M: 10.0, age: 1);
            default: return new Star(R: 1, L: 1, M: 1, age: 5);
        }
    }

    private double OutermostPlanetDistance()
    {
        if (orbits.Length > 0)
        {
            for (int i = orbits.Length - 1; i >= 0; i--)
            {
                if (orbits[i].body != null && orbits[i].body.bodyType != Body.Type.AsteroidBelt && orbits[i].body.bodyType != Body.Type.Chunk)
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
        Liquid, IceSheet, None
    }

    class Orbit
    {
        public Body body;
        public double distance;
        public bool inner = false;
        private readonly Random _random = new Random();
        public Star star;
        public Orbit(double R, Star star)
        {
            this.star = star;
            this.distance = R;

            var molten = (R <= Math.Sqrt(star.luminosity) * 0.025);
            inner = (R <= Math.Sqrt(star.luminosity) * 4);

            var orbitRoll = _random.Next(1, 96);
            var T = 255.0 / Math.Sqrt(distance / Math.Sqrt(star.luminosity));
            if (inner)
            {
                if (orbitRoll <= 18)
                {
                    body = new Body(Body.Type.AsteroidBelt, T, this);
                }
                else if (orbitRoll <= 62)
                {
                    body = new Body(Body.Type.Terrestrial, T, this);
                }
                else if (orbitRoll <= 71)
                {
                    body = new Body(Body.Type.Chunk, T, this);
                }
                else if (orbitRoll <= 82)
                {
                    body = new Body(Body.Type.GasGiant, T, this);
                }
                else if (orbitRoll <= 87)
                {
                    body = new Body(Body.Type.Superjovian, T, this);
                }
            }
            else
            {
                if (orbitRoll <= 15)
                {
                    body = new Body(Body.Type.AsteroidBelt, T, this);
                }
                else if (orbitRoll <= 23)
                {
                    body = new Body(Body.Type.Terrestrial, T, this);
                }
                else if (orbitRoll <= 35)
                {
                    body = new Body(Body.Type.Chunk, T, this);
                }
                else if (orbitRoll <= 74)
                {
                    body = new Body(Body.Type.GasGiant, T, this);
                }
                else if (orbitRoll <= 84)
                {
                    body = new Body(Body.Type.Superjovian, T, this);
                }
            }

            body = molten ? null : body;

            star = null;
        }
    }

    class Body
    {

        public double temperature;
        public double radius;
        public double density;
        public double mass;
        public Orbit[] moons;
        private readonly Random _random = new Random();
        public Type bodyType;
        public Hydrosphere hydrosphere;
        public int hydrosphereCoverage;
        public bool hasLife;
        public Body(Type bodyType, double T, Orbit orbit)
        {
            this.bodyType = bodyType;
            temperature = T;

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
                    sizeRoll = Math.Min(Math.Max(sizeRoll, 0), 10);
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
                case Type.Superjovian:
                    switch (sizeRoll <= 5)
                    {
                        case true: radius = innerSizeRoll * 300 + (sizeRoll + 4) * 3000; break;
                        case false: radius = innerSizeRoll * 1000 + (sizeRoll - 3) * 10000; break;
                    }
                    density = orbit.inner ? (densityRoll * 0.1 + 0.3) : (densityRoll * 0.05 + 0.1);
                    break;
            }

            (hydrosphere, hydrosphereCoverage) = GenerateHydrosphere(orbit.inner, T);

            hasLife = hydrosphere == Hydrosphere.Liquid && d(10) <= 3;

            orbit = null;
        }

        (Hydrosphere, int hydrosphereCoverage) GenerateHydrosphere(bool inner, double T)
        {
            Hydrosphere hydrosphere = Hydrosphere.None;
            int hydrosphereCoverage = 0;
            if (inner)
            {
                if (T <= 245)
                {
                    hydrosphere = Hydrosphere.IceSheet;
                }
                else if (T <= 370)
                {
                    hydrosphere = Hydrosphere.Liquid;
                }
            }

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
                        case 9:
                        case 10:
                            hydrosphereCoverage += d(10, N: (hydroRoll - 8) * 5);
                            hydrosphereCoverage += (hydroRoll - 9) * 10;
                            hydrosphereCoverage = Math.Min(hydrosphereCoverage, 100);
                            break;
                    }
                }
                else if (radius <= 4000)
                {
                    switch (hydroRoll)
                    {
                        case 1: case 2: break;
                        case 3: case 4: hydrosphereCoverage += d(10); break;
                        case 5: case 6: case 7: case 8: case 9: hydrosphereCoverage += d(10) + (hydroRoll - 4) * 10; break;
                        case 10:
                            hydrosphereCoverage += d(10, N: 10) + 10;
                            hydrosphereCoverage = Math.Min(hydrosphereCoverage, 100);
                            break;
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

            return (hydrosphere, hydrosphereCoverage);
        }

        int d(int n, int N = 1)
        {
            var c = 0;
            for (int i = 0; i < N; i++)
            {
                c += _random.Next(1, n + 1);
            }
            return c;
        }

        public enum Type
        {
            AsteroidBelt, Chunk, Terrestrial, GasGiant, Superjovian
        }
    }

    class Star
    {
        private readonly Random _random = new Random();
        public double radius;
        public double luminosity;
        public double mass;
        public int age;
        public int abundance;
        public Star(double R, double L, double M, double age)
        {
            this.radius = R;
            this.luminosity = L;
            this.mass = M;
            this.age = (int)age;

            var abundanceRoll = _random.Next(1, 11) + _random.Next(1, 11) + this.age;
            if (abundanceRoll <= 9)
            {
                this.abundance = 2;
            }
            else if (abundanceRoll <= 12)
            {
                this.abundance = 1;
            }
            else if (abundanceRoll <= 18)
            {
                this.abundance = 0;
            }
            else if (abundanceRoll <= 21)
            {
                this.abundance = -1;
            }
            else
            {
                this.abundance = -3;
            }
        }
    }
}