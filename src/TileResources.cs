using System.Collections.Generic;
using System.Linq;
using System;
public class TileResources
{
    public Dictionary<Resource, double> resources = new Dictionary<Resource, double>();

    public static Resource GetResource(string resource)
    {
        return (Resource)Enum.Parse(typeof(Resource), new string(resource.Where(c => c != ' ').ToArray()));
    }

    public string GetResourcesList()
    {
        string s = "Resources: ";
        resources.ToList().ForEach(r => { s += r.Value.ToString() + " " + r.Key.ToString() + ", "; });
        return s.TrimEnd(", ".ToCharArray());
    }

    public double GetAmount(Resource resource)
    {
        double amount;
        return resources.TryGetValue(resource, out amount) ? amount : 0;
    }

    public void SetAmount(Resource resource, double amount)
    {
        resources[resource] = amount;
    }

    public void AddAmount(Resource resource, double amount)
    {
        resources[resource] = GetAmount(resource) + amount;
    }

    public void AddTileResources(TileResources amounts)
    {
        resources.ToList().ForEach((resourceAmount) =>
        {
            AddAmount(resourceAmount.Key, resourceAmount.Value);
        });
    }

}