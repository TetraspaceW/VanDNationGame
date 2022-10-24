using System.Collections.Generic;
using System.Linq;
using System;
public class TileResources
{
    public Dictionary<string, double> resources = new Dictionary<string, double>();

    public string GetResourcesList()
    {
        string s = "Resources: ";
        resources.ToList().ForEach(r => { s += r.Value.ToString() + " " + r.Key.ToString() + ", "; });
        return s.TrimEnd(", ".ToCharArray());
    }

    public double GetAmount(string resource)
    {
        double amount;
        return resources.TryGetValue(resource, out amount) ? amount : 0;
    }

    public void SetAmount(string resource, double amount)
    {
        resources[resource] = amount;
    }

    public void AddAmount(string resource, double amount)
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