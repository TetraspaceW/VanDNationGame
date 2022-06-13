using System;
class CelestialGenerator { }
class SolarSystemGenerator : CelestialGenerator
{
    private readonly Random _random = new Random();
    SolarSystemGenerator(TileModel tile, SpectralClass spectralClass)
    {
        // Object       | Size      | S     | Comparable map object
        // -------------+-----------+-------+--------------------
        // G-Class      | 1 Gm      | -7    |   
        // O-Class      | 10 Gm     | -6    | 
        //              | 100 Gm    | -5    | Inner system map tile
        // Supergiant   | 500 Gm    | -5×5  | 5 inner system tiles

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

        Orbit[] orbits = new Orbit[numOrbits];
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

    TileModel[,] InnerSystemMap() // size -4
    {
        return null;
    }

    TileModel[,] OuterSystemMap() // size -3
    {
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
        public Type bodyType;

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