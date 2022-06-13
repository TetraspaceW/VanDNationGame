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
        // -------------+-----------+-------+--------------------
        // G-Class      | 1 Gm      | -7    |   
        // O-Class      | 10 Gm     | -6    | 
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

        Godot.GD.Print("Generating ", numOrbits, " orbits around this star.");
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

    public TileModel[,] InnerSystemMap() // size -4
    {
        var Tiles = new TileModel[10, 10];
        TerrainGenRule.Fill(parent: tile, Tiles, new[] { new TerrainRule(Terrain.TerrainType.InnerSystemOrbit) });
        var center = TerrainGenRule.AddCenter(parent: tile, Tiles, new[] {
            new TerrainRule(Terrain.TerrainType.Star, props: new Dictionary<PropKey, string>() {
                {PropKey.SpectralClass, spectralClass.ToString()}
            })
        });

        Godot.GD.Print(orbits.Length, " orbits generated around this star.");
        foreach (Orbit orbit in orbits)
        {
            Godot.GD.Print("Distance ", orbit.distance, "AU; Type ", (orbit.body != null) ? orbit.body.bodyType.ToString() : "Empty");
        }

        var i = 0;
        while (i < orbits.Length && orbits[i].distance < 6)
        {
            if (orbits[i].body != null && orbits[i].distance >= 0.6)
            {
                var radius = (int)(orbits[i].distance / 0.6);
                if (!IsSingleBody(orbits[i].body.bodyType))
                {
                    TerrainGenRule.AddCircle(parent: tile, Tiles,
                    rules: new[] {
                        new TerrainRule(Terrain.TerrainType.AsteroidBeltBodies)
                    }, center, radius, false);
                }
                else
                {
                    Terrain.PlanetType planetType = Terrain.PlanetType.Rockball;
                    switch (orbits[i].body.bodyType)
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

                    TerrainGenRule.AddAtDistance(parent: tile, Tiles,
                    rules: new[] {
                    new TerrainRule(Terrain.TerrainType.InnerSystemBody, true, props: new Dictionary<PropKey, string>() {
                        { PropKey.PlanetType, planetType.ToString() }
                    })},
                    center,
                    radius,
                    mask: new List<Terrain.TerrainType> {
                        Terrain.TerrainType.InnerSystemBody, Terrain.TerrainType.Star
                    });
                }
            }
            i++;
        }
        return Tiles;
    }

    TileModel[,] OuterSystemMap() // size -3
    {
        var i = 0;
        while (i < orbits.Length && orbits[i].distance >= 6 && orbits[i].distance < 60)
        {

        }
        return null;
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
            case SpectralClass.O: return new Star(L: 200000.0, M: 40.0, age: 0.0055);
            case SpectralClass.B: return new Star(L: 1000.0, M: 8.2, age: 0.0451);
            case SpectralClass.A: return new Star(L: 23.0, M: 2.2, age: 0.75);
            case SpectralClass.F: return new Star(L: 2.9, M: 1.31, age: 2.75);
            case SpectralClass.G: return new Star(L: 0.79, M: 0.94, age: 5.5);
            case SpectralClass.K: return new Star(L: 0.19, M: 0.66, age: 5.5);
            case SpectralClass.M: return new Star(L: 0.003, M: 0.18, age: 5.5);
            case SpectralClass.D: return new Star(L: 0.012, M: 0.575, age: 5.5);
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

            inner = (R <= star.luminosity * 4);

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