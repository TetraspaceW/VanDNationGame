using System.Collections.Generic;
using System.Linq;
using System;
using System.Runtime.CompilerServices;

public class TileResources
{
    public Dictionary<string, decimal> resources = new Dictionary<string, decimal>();

    public string GetResourcesList()
    {
        string s = "Resources: ";
        resources.ToList().ForEach(r => { s += r.Value.ToString() + " " + r.Key.ToString() + ", "; });
        return s.TrimEnd(", ".ToCharArray());
    }

    public decimal GetAmount(string resource)
    {
        decimal amount;
        return resources.TryGetValue(resource, out amount) ? amount : 0;
    }

    public void SetAmount(string resource, decimal amount)
    {
        resources[resource] = amount;
    }

    public void AddAmount(string resource, decimal amount)
    {
        resources[resource] = GetAmount(resource) + amount;
    }

    public void AddTileResources(TileResources amounts)
    {
        amounts.resources.ToList().ForEach((resourceAmount) =>
        {
            AddAmount(resourceAmount.Key, resourceAmount.Value);
        });
    }

    public static TileResources SubtractTileResources(TileResources amounts, TileResources amounts2)
    {
        var result = new TileResources();
        amounts.resources.ToList().ForEach((resourceAmount) =>
        {
            result.AddAmount(resourceAmount.Key, resourceAmount.Value);
        });
        amounts2.resources.ToList().ForEach((resourceAmount) =>
        {
            result.AddAmount(resourceAmount.Key, -resourceAmount.Value);
        });
        return result;
    }

}