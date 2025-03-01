using System;
class AtomGenerator
{
    public static double GetMassNumber(Terrain terrain)
    {
        return AtomGenerator.ElementMassNumber((Terrain.AtomElement)Enum.Parse(typeof(Terrain.AtomElement), terrain.props[PropKey.AtomElement]));
    }

    public static double ElementMassNumber(Terrain.AtomElement nucleusElement)
    {
        var massNumber = nucleusElement switch
        {
            Terrain.AtomElement.Hydrogen => 1.008,
            Terrain.AtomElement.Helium => 4.0026,
            Terrain.AtomElement.Lithium => 6.94,
            Terrain.AtomElement.Beryllium => 9.0122,
            Terrain.AtomElement.Boron => 10.81,
            Terrain.AtomElement.Carbon => 12.011,
            Terrain.AtomElement.Nitrogen => 14.007,
            Terrain.AtomElement.Oxygen => 16.0,
            Terrain.AtomElement.Fluorine => 18.998,
            Terrain.AtomElement.Neon => 20.18,
            Terrain.AtomElement.Sodium => 22.99,
            Terrain.AtomElement.Magnesium => 24.305,
            Terrain.AtomElement.Aluminium => 26.982,
            Terrain.AtomElement.Silicon => 28.085,
            Terrain.AtomElement.Phosphorus => 30.974,
            Terrain.AtomElement.Sulfur => 32.06,
            Terrain.AtomElement.Chlorine => 35.45,
            Terrain.AtomElement.Argon => 39.95,
            Terrain.AtomElement.Iron => 55.85,
            _ => 1.0,
        };
        return massNumber;
    }
}