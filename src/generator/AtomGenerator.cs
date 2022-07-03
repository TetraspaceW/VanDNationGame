using System;
class AtomGenerator
{
    public static double GetMassNumber(Terrain terrain)
    {
        double massNumber;
        Terrain.AtomElement nucleusElement;
        Enum.TryParse<Terrain.AtomElement>(terrain.props[PropKey.AtomElement], out nucleusElement);

        switch (nucleusElement)
        {
            case Terrain.AtomElement.Hydrogen: massNumber = 1.008; break;
            case Terrain.AtomElement.Helium: massNumber = 4.0026; break;
            case Terrain.AtomElement.Lithium: massNumber = 6.94; break;
            case Terrain.AtomElement.Beryllium: massNumber = 9.0122; break;
            case Terrain.AtomElement.Boron: massNumber = 10.81; break;
            case Terrain.AtomElement.Carbon: massNumber = 12.011; break;
            case Terrain.AtomElement.Nitrogen: massNumber = 14.007; break;
            case Terrain.AtomElement.Oxygen: massNumber = 16.0; break;
            case Terrain.AtomElement.Fluorine: massNumber = 18.998; break;
            case Terrain.AtomElement.Neon: massNumber = 20.18; break;
            default: massNumber = 1.0; break;
        }
        return massNumber;
    }
}