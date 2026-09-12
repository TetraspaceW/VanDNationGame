using System.Collections.Generic;
class TerrainRule(Terrain.TerrainType terrainType, bool zoomable = false, double weight = 1, Dictionary<PropKey, string> props = null)
{
    public double weight = weight;
    public Terrain.TerrainType terrainType = terrainType;
    public bool zoomable = zoomable;
    public Dictionary<PropKey, string> props = props;

    public TerrainRule Rotate(int rot)
    {
        TerrainRule rot0 = new(terrainType, zoomable, weight, new Dictionary<PropKey, string>(props));
        if (props.TryGetValue(PropKey.Rotation, out string value))
        {
            rot0.props[PropKey.Rotation] = (int.Parse(value) + rot) % 4 + "";
        }
        else
        {
            rot0.props[PropKey.Rotation] = rot + "";
        }
        return rot0;
    }

    public TerrainRule[] RotateAll()
    {
        TerrainRule rot0 = new(terrainType, zoomable, weight, new Dictionary<PropKey, string>(props));
        TerrainRule rot1 = new(terrainType, zoomable, weight, new Dictionary<PropKey, string>(props));
        TerrainRule rot2 = new(terrainType, zoomable, weight, new Dictionary<PropKey, string>(props));
        TerrainRule rot3 = new(terrainType, zoomable, weight, new Dictionary<PropKey, string>(props));
        rot0.props[PropKey.Rotation] = "0";
        rot1.props[PropKey.Rotation] = "1";
        rot2.props[PropKey.Rotation] = "2";
        rot3.props[PropKey.Rotation] = "3";
        return [rot0, rot1, rot2, rot3];
    }
}