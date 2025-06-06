using System;
class AtomGenerator
{
    public static double GetMassNumber(Terrain terrain)
    {
        return ElementMassNumber((Terrain.AtomElement)Enum.Parse(typeof(Terrain.AtomElement), terrain.props[PropKey.AtomElement]));
    }

    public static double ElementMassNumber(Terrain.AtomElement nucleusElement)
    {
        var massNumber = nucleusElement switch
        {
            Terrain.AtomElement.Hydrogen => 1,
            Terrain.AtomElement.Deuterium => 2,
            Terrain.AtomElement.Tritium => 3,
            Terrain.AtomElement.Helium => 4.0026,
            Terrain.AtomElement.Lithium => 6.94,
            Terrain.AtomElement.Beryllium => 9.0122,
            Terrain.AtomElement.Boron => 10.81,
            Terrain.AtomElement.Carbon => 12,
            Terrain.AtomElement.Carbon13 => 13,
            Terrain.AtomElement.Carbon14 => 14,
            Terrain.AtomElement.Nitrogen => 14,
            Terrain.AtomElement.Nitrogen15 => 15,
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
            Terrain.AtomElement.Potassium => 39.098,
            Terrain.AtomElement.Calcium => 40.078,
            Terrain.AtomElement.Titanium => 47.87,
            Terrain.AtomElement.Iron => 55.85,
            Terrain.AtomElement.Bromine => 79.904,
            Terrain.AtomElement.Strontium => 87.62,
            _ => 1.75,
        };
        return massNumber;
    }
}