using System.Collections.Generic;
using System.Linq;

class NucleonGenerator
{
    TileModel parent;
    List<Terrain.QuarkFlavour> quarks;

    public NucleonGenerator(TileModel tile, Terrain.QuarkFlavour q1, Terrain.QuarkFlavour q2, Terrain.QuarkFlavour q3)
    {
        parent = tile;
        quarks = new List<Terrain.QuarkFlavour> { q1, q2, q3 };
    }

    public TileModel[,] GenerateNucleon()
    {
        var nucleonMap = new TileModel[10, 10];

        TerrainGenRule.Fill(parent, nucleonMap, new[] { new TerrainRule(Terrain.TerrainType.GluonSea) });

        _ = new List<Terrain.QuarkColour> { Terrain.QuarkColour.Red, Terrain.QuarkColour.Green, Terrain.QuarkColour.Blue }
        .OrderBy(a => RND.Next())
        .Zip(quarks, (colour, flavour) =>
            {
                TerrainGenRule.AddOneRandomly(parent, nucleonMap, new[] { new TerrainRule(Terrain.TerrainType.ValenceQuark, true, props: new Dictionary<PropKey, string>() {
                    {PropKey.QuarkColour, colour.ToString()},
                    {PropKey.QuarkFlavour, flavour.ToString()}
                }) }, new List<Terrain.TerrainType> { Terrain.TerrainType.ValenceQuark });
                return true;
            }
        ).ToList();

        return nucleonMap;
    }
}