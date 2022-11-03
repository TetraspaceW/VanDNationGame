using System.Collections.Generic;
class SpeciesGenerator
{
    string habitat;
    string thermoregulation;
    string intelligence;
    public SpeciesGenerator(string intelligence = "NerveNet", string habitat = "Aquatic", string thermoregulation = "Cold")
    {
        this.habitat = habitat;
        this.thermoregulation = thermoregulation;
        this.intelligence = intelligence;
    }

    public TerrainRule GetTerrainRule(double weight = 1)
    {
        return new TerrainRule(Terrain.TerrainType.Animal, false, weight, new Dictionary<PropKey, string>() {
            { PropKey.AnimalHabitat, habitat },
            { PropKey.AnimalIntelligence, intelligence },
            { PropKey.AnimalThermoregulation, thermoregulation }
        });
    }
}