using System.Collections.Generic;
using System.Linq;
using System;
public class TileResources
{
    public Dictionary<Resource, double> resources;

    public static Resource GetResource(string resource)
    {
        return (Resource)Enum.Parse(typeof(Resource), resource);
    }

    public double GetAmount(Resource resource)
    {
        double amount;
        return resources.TryGetValue(resource, out amount) ? amount : 0;
    }

    public void SetAmount(Resource resource, double amount)
    {
        resources.Add(resource, amount);
    }

    public void AddAmount(Resource resource, double amount)
    {
        resources.Add(resource, GetAmount(resource) + amount);
    }

    public void AddTileResources(TileResources amounts)
    {
        resources.ToList().ForEach((resourceAmount) =>
        {
            AddAmount(resourceAmount.Key, resourceAmount.Value);
        });
    }

}