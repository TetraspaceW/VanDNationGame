using System.Collections.Generic;
using System.Linq;
using System;
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

}