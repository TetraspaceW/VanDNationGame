using System.Collections.Generic;
public class TileResources
{
    Dictionary<Resource, double> _resources;

    public double GetAmount(Resource resource)
    {
        double amount;
        return _resources.TryGetValue(resource, out amount) ? amount : 0;
    }

    public void SetAmount(Resource resource, double amount)
    {
        _resources.Add(resource, amount);
    }
}