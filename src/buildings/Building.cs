using Godot;
using System;
using System.Diagnostics;

public class Building
{
    public (int, int) coords;
    public BuildingTemplate template;
    public bool active;
    public bool removed;
    public TileResources storage;
    public TileResources capacity;

    public Building((int, int) coords, BuildingTemplate template)
    {
        this.coords = coords;
        this.template = template;
        active = true;
        storage = new TileResources();
        capacity = new TileResources();
        if (template.transport != null)
        {
            for (int i = 0; i < template.transport.resources.Length; i++)
            {
                capacity.AddAmount(template.transport.resources[i], template.transport.amount);
            }
        }
    }

    public bool TryMaintenance(TileModel tile)
    {
        var maintenance = template.maintenance;

        // maintenance succeeds if there is none
        if (maintenance == null) { return true; }

        if (tile.totalChildResources.GetAmount(maintenance.resource) >= maintenance.amount)
        {
            tile.SubtractResource(maintenance.resource, maintenance.amount);
            return true;
        }
        else { 
            switch(template.name)
            {
                case "Satellite":
                    removed = true;
                    break;
            }
            return false;
        }
    }

    public void SubtractResourceFromStorage(string resource, decimal amount)
    {
        var localChange = Math.Min(amount, storage.GetAmount(resource));
        amount -= localChange;
        storage.AddAmount(resource, -localChange);
    }

    public void AddResourceToStorage(string resource, decimal amount)
    {
        var localChange = Math.Min(amount, capacity.GetAmount(resource) - storage.GetAmount(resource));
        amount -= localChange;
        storage.AddAmount(resource, localChange);
    }

    public override string ToString()
    {
        return template.name;
    }
}