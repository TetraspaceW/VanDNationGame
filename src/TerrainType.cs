class TerrainType
{
    string texture;

    class Space : TerrainType
    {
        Space()
        {
            texture = "space";
        }
    }

    class Void : TerrainType
    {
        public Void()
        {
            texture = "void";
        }
    }

    class IntersuperclusterVoid : Void { }
    class InterclusterSpace : Void { }
    class IntergroupSpace : Void { }
    class IntergalacticSpace : Void { }
    class GalacticHalo : Void { }
    class InterstellarSpace : Void { }
    class OuterSystemOrbit : Void { }
    class InnerSystemOrbit : Void { }

    class Comets : TerrainType
    {
        public Comets()
        {
            texture = "kuiper";
        }
    }
}