using System;
using System.Collections.Generic;
class CelestialGenerator { }
class SolarSystemGenerator : CelestialGenerator
{
    SpectralClass spectralClass;
    TileModel tile;
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
        Star star = StarData(spectralClass);
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
        switch (bodyType)
        {
            case Body.Type.AsteroidBelt:
                return false;
            default:
                return true;
        }
    }

    public TileModel[,] EpiepiepistellarSystemMap() // size -7, engulfed by G star
    {
        // Closest a planet can be to its star is 374 Mm / 3 tiles at this distance
        // you might be thinking isn't this basically kissing the M class star?
        // and yep, it totally is! M class stars are tepid
        // you could probably touch one and you'd be fine

        return null;
    }
    public TileModel[,] EpiepistellarSystemMap() // size -6, engulfed by O star
    {
        return null;
    }

    public TileModel[,] EpistellarSystemMap() // size -5
    {
        return null;
    }

    public TileModel[,] InnerSystemMap(TileModel parent) // size -4
    {
        var Tiles = new TileModel[10, 10];
        TerrainGenRule.Fill(parent, Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerSystemOrbit) });
        var center = TerrainGenRule.AddCenter(parent, Tiles, new[] {
            new TerrainRule(Terrain.TerrainType.Star, props: new Dictionary<PropKey, string>() {
                {PropKey.SpectralClass, spectralClass.ToString()}
            })
        });

        var i = 0;
        while (i < orbits.Length && orbits[i].distance < 6)
        {
            if (orbits[i].body != null && orbits[i].distance >= 0.6)
            {
                var radius = (int)(orbits[i].distance / 0.6);
                PlaceWorld(parent, orbits[i].body, radius, true, Tiles, center);
            }
            i++;
        }
        return Tiles;
    }

    private void PlaceWorld(TileModel parent, Body body, int distance, bool inner, TileModel[,] Tiles, (int, int) center)
    {
        Terrain.TerrainType terrainType;
        if (!IsSingleBody(body.bodyType))
        {
            terrainType = inner ? Terrain.TerrainType.AsteroidBeltBodies : Terrain.TerrainType.KuiperBeltBodies;
            TerrainGenRule.AddCircle(parent, Tiles,
            rules: new[] {
                new TerrainRule(Terrain.TerrainType.AsteroidBeltBodies)
            }, center, distance, false);
        }
        else
        {
            terrainType = inner ? Terrain.TerrainType.InnerSystemBody : Terrain.TerrainType.OuterSystemBody;
            Terrain.PlanetType planetType = Terrain.PlanetType.Rockball;
            switch (body.bodyType)
            {
                case Body.Type.Chunk:
                    planetType = Terrain.PlanetType.Rockball; break;
                case Body.Type.Terrestrial:
                    planetType = Terrain.PlanetType.Arid; break;
                case Body.Type.GasGiant:
                    planetType = Terrain.PlanetType.Jovian; break;
                case Body.Type.Superjovian:
                    planetType = Terrain.PlanetType.Jovian; break;
            }

            TerrainGenRule.AddAtDistance(parent, Tiles,
            rules: new[] {
                new TerrainRule(Terrain.TerrainType.InnerSystemBody, inner, props: new Dictionary<PropKey, string>() {
                    { PropKey.PlanetType, planetType.ToString() }
                })
            },
            center,
            distance,
            mask: new List<Terrain.TerrainType> {
                Terrain.TerrainType.InnerSystemBody, Terrain.TerrainType.OuterSystemBody,
                Terrain.TerrainType.Star, Terrain.TerrainType.InnerSolarSystem
            });
        }
    }

    public TileModel[,] OuterSystemMap() // size -3
    {
        var Tiles = new TileModel[10, 10];
        TerrainGenRule.Fill(parent: tile, Tiles, new[] { new TerrainRule(Terrain.TerrainType.OuterSystemOrbit) });
        var center = TerrainGenRule.AddCenter(parent: tile, Tiles, new[] {
            new TerrainRule(Terrain.TerrainType.InnerSolarSystem, true, props: new Dictionary<PropKey, string>() {
                {PropKey.SpectralClass, spectralClass.ToString()}
            })
        });
        var centerTile = Tiles[center.Item1, center.Item2];
        centerTile.internalMap = new MapModel(centerTile, InnerSystemMap(centerTile));

        var i = 0;
        while (i < orbits.Length && orbits[i].distance < 60)
        {
            if (orbits[i].body != null && orbits[i].distance >= 6)
            {
                var radius = (int)(orbits[i].distance / 6);
                PlaceWorld(tile, orbits[i].body, radius, false, Tiles, center);
            }
            i++;
        }
        return Tiles;
    }

    TileModel[,] ScatteredDiskMap() // size -2
    {
        var i = 0;
        while (i < orbits.Length && orbits[i].distance >= 60 && orbits[i].distance < 600)
        {

        }
        return null;
    }
    TileModel[,] HillsCloudMap() // size -1
    {
        var i = 0;
        while (i < orbits.Length && orbits[i].distance >= 600 && orbits[i].distance < 6000)
        {

        }
        return null;
    }
    TileModel[,] SolarSystemMap() // size 0
    {
        var i = 0;
        while (i < orbits.Length && orbits[i].distance >= 6000 && orbits[i].distance < 60000)
        {

        }
        return null;
    }

    private Star StarData(SpectralClass spectralClass)
    {
        switch (spectralClass)
        {
            case SpectralClass.O: return new Star(L: 100000.0, M: 50.0, age: 0.005);
            case SpectralClass.B: return new Star(L: 1000.0, M: 10.0, age: 0.05);
            case SpectralClass.A: return new Star(L: 20.0, M: 2.0, age: 1);
            case SpectralClass.F: return new Star(L: 4.0, M: 1.5, age: 2);
            case SpectralClass.G: return new Star(L: 1.0, M: 1.0, age: 5);
            case SpectralClass.K: return new Star(L: 0.20, M: 0.7, age: 5);
            case SpectralClass.M: return new Star(L: 0.01, M: 0.2, age: 5);
            case SpectralClass.D: return new Star(L: 0.01, M: 1.0, age: 5);
            default: return new Star(L: 1.0, M: 1.0, age: 5.0);
        }
    }

    public enum SpectralClass
    {
        O, B, A, F, G, K, M, D
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
        public Body(Type bodyType, double T, Orbit orbit)
        {
            this.bodyType = bodyType;
            temperature = T;

            var sizeRoll = _random.Next(1, 11);
            var innerSizeRoll = _random.Next(1, 11);
            var densityRoll = Math.Min(Math.Max(_random.Next(1, 11) + orbit.star.abundance, 1), 11);
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

            orbit = null;
        }

        public enum Type
        {
            AsteroidBelt, Chunk, Terrestrial, GasGiant, Superjovian
        }
    }

    class Star
    {
        private readonly Random _random = new Random();
        public double luminosity;
        public double mass;
        public int age;
        public int abundance;
        public Star(double L, double M, double age)
        {
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

class PlanetGenerator
{
    PlanetGenerator(PlanetType planetType)
    {
    }

    public enum PlanetType
    {
        // Dwarf Terrestrial
        Rockball, Arean, Meltball, Hebean, Promethean, Snowball,
        // Terrestrial
        Telluric, Arid, Tectonic, Oceanic,
        // Helian
        Helian, Panthallasic,
        // Jovian
        Jovian
    }
}