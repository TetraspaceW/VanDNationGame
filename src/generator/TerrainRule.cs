using System.Collections.Generic;
class TerrainRule
{
    public double weight;
    public Terrain.TerrainType terrainType;
    public bool zoomable;
    public Dictionary<PropKey, string> props;
    public TerrainRule(Terrain.TerrainType terrainType, bool zoomable = false, double weight = 1, Dictionary<PropKey, string> props = null)
    {
        this.terrainType = terrainType;
        this.zoomable = zoomable;
        this.weight = weight;
        this.props = props;
    }
}